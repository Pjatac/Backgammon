using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Models
{
	public class Chat
	{
		public List<string> Messages { get; set; }
		public string User1 { get; set; }
		public string User2 { get; set; }
		public HubConnection hubConnection { get; set; }
	}
}
