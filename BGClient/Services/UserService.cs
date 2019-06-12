using BGClient.Infra;
using BGClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Services
{
	public class UserService : IUserService
	{
		public async Task<IList<User>> GetExistingUsers()
		{
			try
			{
				var url = "https://localhost:44320/api/home";
				HttpClient client = new HttpClient();
				var resp = await client.GetAsync(url);
				var users = JsonConvert.DeserializeObject<List<User>>(await resp.Content.ReadAsStringAsync());
				return users;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
		public async Task<string> Login(User logingUser)
		{
			HttpClient httpClient = new HttpClient();
			string userInfo = JsonConvert.SerializeObject(logingUser);
			var response = await httpClient.PostAsync($"https://localhost:44320/api/home", new StringContent(userInfo, Encoding.UTF8, "application/json"));
			var code = response.StatusCode.ToString();
			if (code == "NotFound" || code == "BadRequest")
			{
				return code;
			}
			else
			{
				var result = await response.Content.ReadAsStringAsync();
				return result;
			}
		}

		public async Task<string> CheckExistData(User logingUser)
		{
			if (logingUser.Name == null)
			{
				return "Name can't be empty";
			}
			if (logingUser.Password == null)
			{
				return "Password can't be empty";
			}
			var existingUsers = await GetExistingUsers();
			bool isExist = false;
			foreach (var user in existingUsers)
			{
				if (user.Name == logingUser.Name)
				{
					isExist = true;
					var selectedUser = await Login(logingUser);
					if (selectedUser == "NotFound")
					{
						return "Wrong password";
					}
					else if (selectedUser == "BadRequest")
					{
						return "BadRequest";
					}
					else
					{
						return selectedUser;
					}
				}
			}
			if (!isExist)
			{
				return "Wrong email or you are not registred user";
			}
			else
			{
				return "Something wrong";
			}
		}
		public async Task<string> CheckNewUserData(User newUser)
		{	
		if (newUser.Name == null)
			{
				return "Name can't be empty";
			}
			else if (newUser.Password == null || newUser.Password == string.Empty)
			{
				return "Password can't be empty";
			}
			else if (newUser.PassConfirm != newUser.Password)
			{
				return "Missing right password confirmation";
			}
			var existingUsers = await GetExistingUsers();
			bool isExist = false;
			foreach (var user in existingUsers)
			{
				if (user.Name == newUser.Name)
				{
					isExist = true;
					return "User with such email already exist in service";
				}
			}
			if (!isExist)
			{
				var selectedUser = await AddNewUser(newUser);
				if (selectedUser == "NotFound")
				{
					return "NotFound";
				}
				else if (selectedUser == "BadRequest")
				{
					return "BadRequest";
				}
				else
				{
					return selectedUser;
				}
			}
			else
			{
				return "Something wrong";
			}
		}
		public async Task<string> AddNewUser(User logingUser)
		{
			HttpClient httpClient = new HttpClient();
			string userInfo = JsonConvert.SerializeObject(logingUser);
			var response = await httpClient.PostAsync($"https://localhost:44320/api/home/add", new StringContent(userInfo, Encoding.UTF8, "application/json"));
			var code = response.StatusCode.ToString();
			if (code == "NotFound" || code == "BadRequest")
			{
				return code;
			}
			else
			{
				var result = await response.Content.ReadAsStringAsync();
				return result;
			}
		}
	}
}
