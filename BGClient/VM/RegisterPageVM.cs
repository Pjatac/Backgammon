using BGClient.Infra;
using BGClient.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace BGClient.VM
{
	public class RegisterPageVM : ViewModelBase
	{
		public User BGUser { get; set; }
		public RelayCommand NavigateToHomeCommand { get; private set; }
		public RelayCommand AddNewUserCommand { get; private set; }
		private readonly INavigationService _navigationService;
		private readonly IMessageService _messageService;
		private readonly IUserService _userService;
		public RegisterPageVM(INavigationService navigationService, IMessageService messageService, IUserService userService)
		{
			_messageService = messageService;
			_userService = userService;
			_navigationService = navigationService;
			BGUser = new User();
			InitCommand();
		}
		private void InitCommand()
		{
			NavigateToHomeCommand = new RelayCommand(NavigateToHomeCommandAction);
			AddNewUserCommand = new RelayCommand(async () => {
				string res = await _userService.CheckNewUserData(BGUser);
				if (!res.Contains("UserId"))
					await _messageService.Show(res);
				else
				{
					BGUser = new User
					{
						Password = JToken.Parse(res)["Password"].ToString(),
						UserID = int.Parse(JToken.Parse(res)["UserId"].ToString()),
						Name = JToken.Parse(res)["Name"].ToString()
					};
					NavigateToHomeCommandAction();
				}
			});
		}
		private void NavigateToHomeCommandAction()
		{
			_navigationService.NavigateTo("HomePage", BGUser);
		}
	}
}
