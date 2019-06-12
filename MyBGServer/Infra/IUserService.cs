using MyBGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBGServer.Infra
{
	public interface IUserService
	{
		Task<IList<User>> GetUsers();
		Task<User> Login(User loginUser);
		Task<string> AddNewUser(User newUser);
		Task Remove(User user);
		Task<string> CheckValidity(string name, string password);
	}
}
