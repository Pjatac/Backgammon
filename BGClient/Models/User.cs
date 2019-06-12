using BGClient.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Models
{
	public class User : INotifyPropertyChanged
	{
		public int UserID { get; set; }
		public string Token { get; set; }
		private string _pass;
		private string _name;
		private string _passConfirm;

		public string PassConfirm
		{
			get { return _passConfirm; }
			set
			{
				_passConfirm = value;
				RaiseItemChanged(nameof(PassConfirm));
			}
		}
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				RaiseItemChanged(nameof(Name));
			}
		}
		public string Password
		{
			get { return _pass; }
			set
			{
				_pass = value;
				RaiseItemChanged(nameof(Password));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaiseItemChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
	}
}
