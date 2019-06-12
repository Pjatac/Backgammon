using MyBGServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBGServer.Infra
{
	public interface IRepo
	{
		Task<User> GetUserByName(string userName);
		Task<IList<User>> GetUsers();
		Task<string> HashMD5(string password);
		Task<bool> VerifyMD5(string password, string originalHashedPassword);
		Task<User> Login(User existingUser);
		Task<User> AddNewUser(User newUser);
		Task RemoveUser(User userToRemove);
	}
}
