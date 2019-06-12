using BGClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BGClient.Converters
{
	public class BoardConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			ObservableCollection<Cell> board = (ObservableCollection<Cell>)value;
			for (int i = 12, j = 23; i < 18; i++, j--)
			{
				Cell tmp = board[i];
				board[i] = board[j];
				board[j] = tmp;
			}
			return board;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			ObservableCollection<Cell> board = (ObservableCollection<Cell>)value;
			for (int i = 12, j = 23; i < 18; i++, j--)
			{
				Cell tmp = board[i];
				board[i] = board[j];
				board[j] = tmp;
			}
			return board;
		}
	}
}