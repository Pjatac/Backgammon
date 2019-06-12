using MyBGServer.Models;
using System.Collections.Generic;

namespace MyBGServer.Infra
{
	public interface IHubService
	{
		IEnumerable<User> GetOnlineUsers(string connectionId);
		void OnDisconnectedAsync(string connectionId);
		void UserConnected(User user, string connectionId);
		string GetNameByConnId(string connectionId);
		string GetConnIdByName(string name);
		Chat GetChatConnectionResponse(string connectionId, string userName, string res);
		Board GetGameConnectionResponse(string connectionId, string userName, string res);
		string ForwardMessage(string connectionId, string message, string recipient);
		int Priority(int num, string connectionId, string userName);
		string Move(string connectionId, string userName, Cell[] board);
	}
}
