using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyBGServer.Helpers;
using MyBGServer.Infra;
using MyBGServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyBGServer.Services
{
	public class UserService : IUserService
	{
		private readonly IRepo _repo;
		private readonly AppSettings _appSettings;

		public UserService(IRepo repo, IOptions<AppSettings> appSettings)
		{
			_repo = repo;
			_appSettings = appSettings.Value;
		}
		public async Task<string> AddNewUser(User newUser)
		{
			string validation = await CheckValidity(newUser.Name, newUser.Password);
			if (validation != "ok")
				return validation;
			else
			{
				await _repo.AddNewUser(newUser);
				var res = JsonConvert.SerializeObject(newUser);
				return res;
			}
		}
		public async Task<string> CheckValidity(string name, string pass)
		{
			string result = "ok";
			string namePattern = @"([A-Z]{1}[a-z]{1,30}[- ]{0,1}|[A-Z]{1}[- \']{1}[A-Z]{0,1}[a-z]{1,30}[- ]{0,1}|[a-z]{1,2}[ -\']{1}[A-Z]{1}[a-z]{1,30}){2,5}";
			if (Regex.IsMatch(name, namePattern, RegexOptions.IgnoreCase) == false)
			{
				result = "Incorrect name";
				return result;
			}
			if (await IsUserExist(name))
			{
				result = "User with name '" + name + "' is already exist";
			}
			if (pass.Length < 6)
			{
				result = "Too short password(<6 characters)";
				return result;
			}
			return result;
		}
		private async Task<bool> IsUserExist(string name)
		{
			IList<User> ex = await GetUsers();
			foreach (var user in ex)
				if (name == user.Name)
					return true;
			return false;
		}
		public async Task<IList<User>> GetUsers()
		{
			IList<User> ExistingUsers = await _repo.GetUsers();
			return ExistingUsers;
		}

		public async Task<User> Login(User loginUser)
		{
			User user = await _repo.Login(loginUser);
			if (user == null)
				return null;
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name,user.UserId.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);
			return user;
		}

		public async Task Remove(User user)
		{
			var toRemove = await _repo.GetUserByName(user.Name);
			await _repo.RemoveUser(toRemove);
		}
	}
}
