using BGClient.Infra;
using BGClient.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace BGClient.VM
{
	public class ChatPageVM : ViewModelBase, INavigable
	{
		private readonly IMessageService _messageService;
		private readonly IHubService _hubService;
		private readonly INavigationService _navigationService;
		public ObservableCollection<string> Messages { get; set; }
		public Chat MyChat { get; set; }
		public string Message { get; set; }
		public RelayCommand SendMessageCommand { get; private set; }
		public RelayCommand BackCommand { get; private set; }
		public bool First = true;
		public ChatPageVM(IMessageService messageService, INavigationService navigationService, IHubService hubService)
		{
			_messageService = messageService;
			_navigationService = navigationService;
			_hubService = hubService;
			Messages = new ObservableCollection<string>();
			InitCommand();
		}
		private void InitCommand()
		{
			SendMessageCommand = new RelayCommand(async () =>
			{
				Messages.Add($"You at {DateTime.Now.ToString("G")}: {Message}");
				RaisePropertyChanged(() => Messages);
				 _hubService.SendMessage($"{MyChat.User1} at {DateTime.Now.ToString("G")}: {Message}", MyChat.User2);
				Message = string.Empty;
				RaisePropertyChanged(() => Message);
			});
			BackCommand = new RelayCommand(async () =>
			{
				Messages = new ObservableCollection<string>();
				_navigationService.NavigateTo("HomePage");
			});
		}
		public async Task GetMessage(string message)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				Messages.Add(message);
				RaisePropertyChanged(() => Messages);
			});
		}
		public void OnNavigatedTo(NavigationEventArgs e)
		{
			MyChat = e.Parameter as Chat;
			Messages = new ObservableCollection<string>();
			RaisePropertyChanged(() => MyChat);
			RaisePropertyChanged(() => Messages);
			if (First)
			{
				_hubService.RegisterAction("GetMessage", async (object message) => await GetMessage(message as string));
				First = false;
			}
			if (MyChat.Messages != null)
			{
				Messages = new ObservableCollection<string>(MyChat.Messages);
				RaisePropertyChanged(() => Messages);
			}
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
