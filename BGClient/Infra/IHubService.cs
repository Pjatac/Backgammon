using BGClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Infra
{
	public interface IHubService
	{
		Task ConnectToHub(User user);
		Task<ObservableCollection<User>> GetOnlineUsers();
		void LogInNotification(User user);
		void CatchGameConnection(string userName);
		void CatchChatConnection(string userName);
		void CatchCancelation();
		void JoinToChat(Chat chat);
		void JoinToGame(Board game);
		Task SendResponseAboutGameConnection(string userName, bool res);
		Task StartChatConnectingRequest(string callerName);
		Task StartGameConnectingRequest(string callerName);
		Task SendResponseAboutChatConnection(string userName, bool res);
		Task CloseConnect();
		Task FirstThrow(int summ, string caller);
		Task Move(Cell[] cells, string userName);
		void SendMessage(string message, string recipient);
		void GetMessage(string message);
		void RegisterAction(string funcName, Action<object> action);
	}
}
