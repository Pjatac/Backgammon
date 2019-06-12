using BGClient.Infra;
using BGClient.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace BGClient.VM
{
	public class StartPageVM: ViewModelBase, INavigable
	{
		private readonly INavigationService _navigationService;
		private readonly IUserService _userService;
		private readonly IMessageService _messageService;
		public RelayCommand NavigateToRegCommand { get; private set; }
		public RelayCommand NavigateToHomeCommand { get; private set; }
		public RelayCommand LoginCommand { get; private set; }
		public User BGUser { get; set; }
		public StartPageVM(INavigationService navigationService, IUserService userService, IMessageService messageService)
		{
			_messageService = messageService;
			_userService = userService;
			_navigationService = navigationService;
			InitCommand();
		}

		private void InitCommand()
		{
			NavigateToRegCommand = new RelayCommand(() => {
				_navigationService.NavigateTo("RegisterPage");
			});
			NavigateToHomeCommand = new RelayCommand(NavigateToHomeCommandAction);
			LoginCommand = new RelayCommand(async () => {
				string res = await _userService.CheckExistData(BGUser);
				if (!res.Contains("userId"))
					await _messageService.Show(res);
				else
				{
					BGUser = JsonConvert.DeserializeObject<User>(res);
					NavigateToHomeCommandAction();
				}
			});
		}
		private void NavigateToHomeCommandAction()
		{
			_navigationService.NavigateTo("HomePage", BGUser);
		}
		public void OnNavigatedTo(NavigationEventArgs e)
		{
			BGUser = new User();
			RaisePropertyChanged(() => BGUser);
		}

		public void OnNavigatedFrom(NavigationEventArgs e)
		{
		}

		public void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
		}

		public bool AllowGoBack()
		{
			throw new NotImplementedException();
		}
	}
}
