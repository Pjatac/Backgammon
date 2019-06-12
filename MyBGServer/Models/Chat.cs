using System;
using System.Collections.Generic;
using System.Text;

namespace MyBGServer.Models
{
	public class Chat
	{
		public List<string> Messages { get; set; }
		public string User1 { get; set; }
		public string User2 { get; set; }
	}
}
