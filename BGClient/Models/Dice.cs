using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Models
{
	public class Dice: INotifyPropertyChanged
	{
		private bool _active;
		private int _number;
		public bool Active
		{
			get { return _active; }
			set
			{
				_active = value;
				RaiseItemChanged(nameof(Active));
			}
		}
		public int Number
		{
			get { return _number; }
			set
			{
				_number = value;
				RaiseItemChanged(nameof(Number));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaiseItemChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
	}
}
