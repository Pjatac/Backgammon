using BGClient.Infra;
using BGClient.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace BGClient.VM
{
	public class HomePageVM : ViewModelBase, INavigable
	{
		private readonly IMessageService _messageService;
		private readonly INavigationService _navigationService;
		private readonly IHubService _hubService;
		public ObservableCollection<User> OnlineUsers { get; set; }
		public ObservableCollection<User> OfflineUsers { get; set; }
		public RelayCommand LogoutCommand { get; private set; }
		public RelayCommand<User> ChatCommand { get; private set; }
		public RelayCommand<User> PlayCommand { get; private set; }
		public User ConnectedUser { get; set; }
		public bool First = true;
		public HomePageVM(IMessageService messageService, INavigationService navigationService, IHubService hubService)
		{
			_hubService = hubService;
			_messageService = messageService;
			_navigationService = navigationService;
			ConnectedUser = new User();
			OnlineUsers = new ObservableCollection<User>();
			OfflineUsers = new ObservableCollection<User>();
			InitCommand();
		}
		private void InitCommand()
		{
			LogoutCommand = new RelayCommand(async () => {
				await _hubService.CloseConnect();
				ConnectedUser = new User();
				_navigationService.NavigateTo("StartPage");
			});
			ChatCommand = new RelayCommand<User>(async (caller) => {
				await _messageService.ShowContent($"Sending request to " + caller.Name);
				await _hubService.StartChatConnectingRequest(caller.Name);
			});
			PlayCommand = new RelayCommand<User>(async (caller) => {
				await _messageService.ShowContent($"Sending request to " + caller.Name);
				await _hubService.StartGameConnectingRequest(caller.Name);
			});
		}
		public async Task GetOnlineUsers()
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				OnlineUsers = await _hubService.GetOnlineUsers();
				RaisePropertyChanged(() => OnlineUsers);
			});
		}
		public async Task AddOnlineUser(User user)
		{
			OnlineUsers.Add(user);
		}
		public async Task CatchGameConnection(string userName)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				MessageDialog accept = new MessageDialog($"{userName} want start game with you!");
				accept.Commands.Add(new UICommand("Cancel", null));
				accept.Commands.Add(new UICommand("Connect", null));
				var res = await accept.ShowAsync();
				bool response = res.Label == "Connect";
				await _hubService.SendResponseAboutGameConnection(userName, response);
			});
		}
		public async Task CatchChatConnection(string userName)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				MessageDialog accept = new MessageDialog($"{userName} want start chat with you!");
				accept.Commands.Add(new UICommand("Cancel", null));
				accept.Commands.Add(new UICommand("Connect", null));
				var res = await accept.ShowAsync();
				bool response = res.Label == "Connect";
				await _hubService.SendResponseAboutChatConnection(userName, response);
			});
		}
		public async Task CatchCancelation()
		{
			await _messageService.HideContent();
			await _messageService.Show("You get cancelation...");
		}

		public async Task JoinToChat(Chat chat)
		{
			if (chat.User1 == ConnectedUser.Name)
			{
				await _messageService.HideContent();
			}
			else
			{
				await _messageService.HideContent();
				string tmp = chat.User1;
				chat.User1 = ConnectedUser.Name;
				chat.User2 = tmp;
			}
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				_navigationService.NavigateTo("ChatPage", chat);
			});
		}
		public async Task JoinToGame(Board board)
		{			
			
			if (board.User1 == ConnectedUser.Name)
			{
				await _messageService.HideContent();
			}
			else
			{
				await _messageService.HideContent();
				string tmp = board.User1;
				board.User1 = ConnectedUser.Name;
				board.User2 = tmp;
			}
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				_navigationService.NavigateTo("GamePage", board);
			});
		}
		public async void OnNavigatedTo(NavigationEventArgs e)
		{
			var activeUser = e.Parameter as User;
			if (First)
			{
				ConnectedUser = activeUser;
				await _hubService.ConnectToHub(ConnectedUser);
				await _messageService.Show("Wellcome, " + ConnectedUser.Name);
				_hubService.RegisterAction("JoinToChat", async (object chat) => await JoinToChat(chat as Chat));
				_hubService.RegisterAction("JoinToGame", async (object board) => await JoinToGame(board as Board));
				_hubService.RegisterAction("LogInNotification", async (object user) => await AddOnlineUser(user as User));
				_hubService.RegisterAction("CatchCancelation", async (object empty) => await CatchCancelation());
				_hubService.RegisterAction("CatchChatConnection", async (object userName) => await CatchChatConnection(userName as string));
				_hubService.RegisterAction("CatchGameConnection", async (object userName) => await CatchGameConnection(userName as string));
				await GetOnlineUsers();
				First = false;
			}
		}

		private async Task InitPage(User activeUser)
		{
			
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
