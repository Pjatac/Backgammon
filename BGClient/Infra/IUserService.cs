using BGClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Infra
{
	public interface IUserService
	{
		Task<IList<User>> GetExistingUsers();
		Task<string> Login(User logingUser);
		Task<string> CheckExistData(User logingUser);
		Task<string> CheckNewUserData(User newUser);
		Task<string> AddNewUser(User logingUser);
	}
}
