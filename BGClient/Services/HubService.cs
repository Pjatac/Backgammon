using BGClient.Infra;
using BGClient.Models;
using GalaSoft.MvvmLight.Threading;
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
using Windows.UI.Popups;

namespace BGClient.Services
{
	public class HubService: IHubService
	{
		private static HubConnection hubConnection;
		private static Dictionary<string, Action<object>> _actions;

		public HubService()
		{
			_actions = new Dictionary<string, Action<object>>();
		}
		public async Task StartChatConnectingRequest(string callerName)
		{
			await hubConnection.InvokeAsync("StartChatConnectingRequest", callerName);
		}
		public async Task StartGameConnectingRequest(string callerName)
		{
			await hubConnection.InvokeAsync("StartGameConnectingRequest", callerName);
		}
		public async Task CloseConnect()
		{
			await hubConnection.DisposeAsync();
		}
		public void RegisterAction(string funcName, Action<object> action)
		{
			if (!_actions.ContainsKey(funcName))
			{
				_actions.Add(funcName, action);
			}
		}
		public async Task ConnectToHub(User user)
		{
			hubConnection = new HubConnectionBuilder()
				.WithUrl("https://localhost:44320/hub/", options =>
				{
					options.AccessTokenProvider = () => Task.FromResult(user.Token);
				})
				.Build();
			hubConnection.On<User>("LogInNotificated", LogInNotification);
			hubConnection.On<string>("GetGameConnectingRequest", CatchGameConnection);
			hubConnection.On<string>("GetChatConnectingRequest", CatchChatConnection);
			hubConnection.On<Chat>("JoinToChat", JoinToChat);
			hubConnection.On<Board>("JoinToGame", JoinToGame);
			hubConnection.On("CatchCancelation", CatchCancelation);
			hubConnection.On<string>("GetMessage", GetMessage);
			hubConnection.On<Cell[]>("GetMove", GetMove);
			hubConnection.On<int>("GetPriority", GetPriority);
			await hubConnection.StartAsync();
			await hubConnection.InvokeAsync("UserConnected", user);

		}
		public async void GetMove(Cell[] cells)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CallAction(cells, "GetMove");
			});
		}
		public async void GetPriority(int priority)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CallAction(priority, "GetPriority");
			});
		}
		public async Task FirstThrow(int summ, string caller)
		{
			await hubConnection.InvokeAsync("FirstThrow", summ, caller);
		}
		public async Task Move(Cell[] cells, string userName)
		{
			await hubConnection.InvokeAsync("Move", cells, userName);
		}
		public async Task<ObservableCollection<User>> GetOnlineUsers()
		{
			return new ObservableCollection<User>(await hubConnection.InvokeAsync<IList<User>>("GetOnlineUsers"));
		}
		public async void LogInNotification(User user)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CallAction(user, "LogInNotification");
			});
		}

		private static void CallAction(object param, string actionName)
		{
			Action<object> action = _actions.ContainsKey(actionName) ? _actions[actionName] : null;

			if (action != null)
			{
				DispatcherHelper.CheckBeginInvokeOnUI(() => { action(param); });
			}
			else
			{
				throw new Exception($"{actionName} is not registered command");
			}
		}
		public async void GetMessage(string message)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CallAction(message, "GetMessage");
			});
		}
		public async void SendMessage(string message, string recipient)
		{
			await hubConnection.InvokeAsync("ForwardMessage", message, recipient);
		}
		public async void CatchGameConnection(string userName)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				MessageDialog accept = new MessageDialog($"{userName} want start game with you!");
				accept.Commands.Add(new UICommand("Cancel", null));
				accept.Commands.Add(new UICommand("Connect", null));
				var res = await accept.ShowAsync();
				if (res.Label == "Connect")
				{
					await hubConnection.InvokeAsync("GetGameConnectionResponse", userName, "Ok");
				}
				else
				{
					await hubConnection.InvokeAsync("GetGameConnectionResponse", userName, "Cancel");
				}
			});
		}
		public async void CatchChatConnection(string userName)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				MessageDialog accept = new MessageDialog($"{userName} want start chat with you!");
				accept.Commands.Add(new UICommand("Cancel", null));
				accept.Commands.Add(new UICommand("Connect", null));
				var res = await accept.ShowAsync();
				if (res.Label == "Connect")
				{
					await hubConnection.InvokeAsync("GetChatConnectionResponse", userName, "Ok");
				}
				else
				{
					await hubConnection.InvokeAsync("GetChatConnectionResponse", userName, "Cancel");
				}
			});
		}

		public async Task SendResponseAboutGameConnection(string userName, bool res)
		{
			if (res)
			{
				await hubConnection.InvokeAsync("GetGameConnectionResponse", userName, "Ok");
			}
			else
			{
				await hubConnection.InvokeAsync("GetGameConnectionResponse", userName, "Cancel");
			}
		}
		public async Task SendResponseAboutChatConnection(string userName, bool res)
		{
			if (res)
			{
				await hubConnection.InvokeAsync("GetChatConnectionResponse", userName, "Ok");
			}
			else
			{
				await hubConnection.InvokeAsync("GetChatConnectionResponse", userName, "Cancel");
			}
		}

		
		public async void CatchCancelation()
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CallAction(null, "CatchCancelation");
			});
		}

		public async void JoinToChat(Chat chat)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CallAction(chat, "JoinToChat");
			});
		}
		public async void JoinToGame(Board game)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CallAction(game, "JoinToGame");
			});
		}
	}
}
