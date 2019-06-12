using Microsoft.AspNetCore.SignalR;
using MyBGServer.Infra;
using MyBGServer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MyBGServer.Hubs
{
	public class MyHub: Hub
	{
		private readonly IHubService _hubService;

		public MyHub(IHubService hubService)
		{
			_hubService = hubService;
		}
		public IEnumerable<User> GetOnlineUsers()
		{
			return _hubService.GetOnlineUsers(Context.ConnectionId);
		}

		public async Task Move(Cell[] board, string userName)
		{
			string user = _hubService.Move(Context.ConnectionId, userName, board);
			await this.Clients.Client(user).SendAsync("GetMove", board);
		}
		public override async Task OnDisconnectedAsync(Exception exception)
		{
			_hubService.OnDisconnectedAsync(Context.ConnectionId);
			await this.Clients.AllExcept(Context.ConnectionId).SendAsync("LogOutNotification");
		}
		public async Task UserConnected(User user)
		{
			_hubService.UserConnected(user, Context.ConnectionId);
			await this.Clients.AllExcept(Context.ConnectionId).SendAsync("LogInNotificated", user);
		}
		public async Task FirstThrow(int num, string userName)
		{
			int both = _hubService.Priority(num, Context.ConnectionId, userName);
			if (both == 1)
			{
				await this.Clients.Client(Context.ConnectionId).SendAsync("GetPriority", 1);
				await this.Clients.Client(_hubService.GetConnIdByName(userName)).SendAsync("GetPriority", 2);
			}
			else if (both == 2)
			{
				await this.Clients.Client(Context.ConnectionId).SendAsync("GetPriority", 2);
				await this.Clients.Client(_hubService.GetConnIdByName(userName)).SendAsync("GetPriority", 1);
			}
		}
		public async Task StartChatConnectingRequest(string caller)
		{
			await this.Clients.Client(_hubService.GetConnIdByName(caller)).SendAsync("GetChatConnectingRequest", _hubService.GetNameByConnId(Context.ConnectionId));
		}
		public async Task GetChatConnectionResponse(string userName, string res)
		{
			Chat chat = _hubService.GetChatConnectionResponse(Context.ConnectionId, userName, res);
			if (chat == null)
			{
				await this.Clients.Client(_hubService.GetConnIdByName(userName)).SendAsync("CatchCancelation");
			}
			else
			{
				await this.Clients.Client(_hubService.GetConnIdByName(userName)).SendAsync("JoinToChat", chat);
				await this.Clients.Client(Context.ConnectionId).SendAsync("JoinToChat", chat);
			}
		}
		public async Task StartGameConnectingRequest(string caller)
		{
			await this.Clients.Client(_hubService.GetConnIdByName(caller)).SendAsync("GetGameConnectingRequest", _hubService.GetNameByConnId(Context.ConnectionId));
		}
		public async Task GetGameConnectionResponse(string userName, string res)
		{
			Board board = _hubService.GetGameConnectionResponse(Context.ConnectionId, userName, res);
			if (board == null)
			{
				await this.Clients.Client(_hubService.GetConnIdByName(userName)).SendAsync("CatchCancelation");
			}
			else
			{
				await this.Clients.Client(_hubService.GetConnIdByName(userName)).SendAsync("JoinToGame", board);
				await this.Clients.Client(Context.ConnectionId).SendAsync("JoinToGame", board);
			}
		}
		public async Task ForwardMessage(string message, string recipient)
		{
			await this.Clients.Client(_hubService.ForwardMessage(Context.ConnectionId, message, recipient)).SendAsync("GetMessage", message);
		}
	}
}