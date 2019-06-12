using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Models
{
	public class Cell: INotifyPropertyChanged
	{
		public int _numOfChip;
		public int _chipColor;
		public int ChipColor
		{
			get { return _chipColor; }
			set
			{
				_chipColor = value;
				RaiseItemChanged(nameof(ChipColor));
			}
		}
		public int NumOfChip
		{
			get { return _numOfChip; }
			set
			{
				_numOfChip = value;
				RaiseItemChanged(nameof(NumOfChip));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaiseItemChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}
	}
}
