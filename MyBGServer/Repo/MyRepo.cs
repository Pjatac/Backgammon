using MyBGServer.DL;
using MyBGServer.Infra;
using MyBGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyBGServer.Repo
{
	public class MyRepo: IRepo
	{
		private static Context _dbContext;
		public MyRepo(Context dbContext)
		{
			_dbContext = dbContext;
			if (!_dbContext.Users.Any())
			{
				_dbContext.Users.Add(new User()
				{
					Name = "t1",
					Password = HashMD5("1").Result
				});
				_dbContext.Users.Add(new User()
				{
					Name = "t2",
					Password = HashMD5("2").Result
				});
				_dbContext.SaveChanges();
			}
		}
		public async Task RemoveUser(User userToRemove)
		{
			_dbContext.Users.Remove(userToRemove);
			_dbContext.SaveChanges();
		}
		public async Task<User> AddNewUser(User newUser)
		{
			newUser.Password = HashMD5(newUser.Password).Result;
			_dbContext.Users.Add(newUser);
			_dbContext.SaveChanges();
			newUser = (from user in _dbContext.Users
					   where user.Name == newUser.Name
					   select user).SingleOrDefault();
			return newUser;
		}
		public async Task<bool> VerifyMD5(string password, string originalHashedPassword)
		{
			string hashOfInput = await HashMD5(password);
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;
			if (0 == comparer.Compare(hashOfInput, originalHashedPassword))
				return true;
			else
				return false;
		}
		public async Task<User> Login(User existingUser)
		{
			User loginUser;
			loginUser = (from user in _dbContext.Users
						 where VerifyMD5(existingUser.Password, user.Password).Result
						 select new User
						 {
							 Name = user.Name,
							 Password = user.Password,
							 UserId = user.UserId
						 }).SingleOrDefault();
			return loginUser;
		}
		public async Task<IList<User>> GetUsers()
		{
			List<User> users = new List<User>();
			foreach (var user in _dbContext.Users)
			{
				users.Add(user);
			}
			return users;
		}
		public async Task<string> HashMD5(string password)
		{
			MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
				sBuilder.Append(data[i].ToString("x2"));

			return sBuilder.ToString();
		}
		public async Task<User> GetUserByName(string userName)
		{
			User searchingUser = new User();
			searchingUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
			return searchingUser;
		}
	}
}
