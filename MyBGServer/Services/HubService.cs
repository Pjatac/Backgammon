using MyBGServer.Infra;
using MyBGServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyBGServer.Services
{
	public class HubService : IHubService
	{
		public static Dictionary<string, User> userConnections = new Dictionary<string, User>();
		public static Dictionary<string, string> userNames = new Dictionary<string, string>();
		public static List<Chat> Chats = new List<Chat>();
		public static List<Board> Boards = new List<Board>();
		public string Move(string connectionId, string userName, Cell[] board)
		{
			Board gameBoard = Boards.FirstOrDefault(b => (b.User1 == userConnections[connectionId].Name && b.User2 == userName) || (b.User1 == userName && b.User2 == userConnections[connectionId].Name));
			gameBoard.MyCells = board;
			return userNames[userName];
		}
		public int Priority(int num, string connectionId, string userName)
		{
			Board board = Boards.FirstOrDefault(b => (b.User1 == userConnections[connectionId].Name && b.User2 == userName) || (b.User1 == userName && b.User2 == userConnections[connectionId].Name));
			if (board.User1 == userConnections[connectionId].Name)
			{
				board.User1FirstThrow = num;
				if (board.User2FirstThrow > 0)
				{
					if (board.User1FirstThrow > board.User2FirstThrow)
						return 1;
					else
						return 2;
				}
			}
			if (board.User2 == userConnections[connectionId].Name)
			{
				board.User2FirstThrow = num;
				if (board.User1FirstThrow > 0)
					if (board.User1FirstThrow > board.User2FirstThrow)
						return 2;
					else
						return 1;
			}
			return 0;
		}
		public string ForwardMessage(string connectionId, string message, string recipient)
		{
			Chat chat = Chats.FirstOrDefault(c => (c.User1 == userConnections[connectionId].Name && c.User2 == recipient) || (c.User2 == userConnections[connectionId].Name && c.User1 == recipient));
			if (chat.Messages == null)
			{
				chat.Messages = new List<string>();
			}
			chat.Messages.Add(message);
			return userNames[recipient];
		}

		public Chat GetChatConnectionResponse(string connectionId, string userName, string res)
		{
			if (res == "Ok")
			{
				Chat chat = new Chat();
				if (Chats.Count > 0)
				{
					chat = Chats.FirstOrDefault(c => (c.User1 == userConnections[connectionId].Name && c.User2 == userName) || (c.User2 == userConnections[connectionId].Name && c.User1 == userName));
				}
				if (chat == null)
				{
					chat = new Chat { User1 = userName, User2 = userConnections[connectionId].Name };
					Chats.Add(chat);
				}
				else if (chat.User1 == null)
				{
					chat = new Chat { User1 = userName, User2 = userConnections[connectionId].Name };
					Chats.Add(chat);
				}
				return chat;
			}
			else
			{
				return null;
			}
		}

		public string GetConnIdByName(string name)
		{
			return userNames[name];
		}

		public Board GetGameConnectionResponse(string connectionId, string userName, string res)
		{
			if (res == "Ok")
			{
				Board board = new Board { User1 = userName, User2 = userConnections[connectionId].Name };
				Boards.Add(board);
				return board;
			}
			else
			{
				return null;
			}
		}

		public string GetNameByConnId(string connectionId)
		{
			return userConnections[connectionId].Name;
		}

		public IEnumerable<User> GetOnlineUsers(string connectionId)
		{
			User user = userConnections[connectionId];
			return userConnections.Values.Where(k => k != user);
		}

		public void OnDisconnectedAsync(string connectionId)
		{
			userNames.Remove(userConnections[connectionId].Name);
			userConnections.Remove(connectionId);
		}

		public void UserConnected(User user, string connectionId)
		{
			userConnections.Add(connectionId, user);
			userNames.Add(user.Name, connectionId);
		}
	}
}
