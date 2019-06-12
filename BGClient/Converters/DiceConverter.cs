using BGClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace BGClient.Converters
{
	public class DiceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			int number = (int)value;
			switch (number)
			{
				case 1:
					return "/Assets/dice1.png";
				case 2:
					return "/Assets/dice2.png";
				case 3:
					return "/Assets/dice3.png";
				case 4:
					return "/Assets/dice4.png";
				case 5:
					return "/Assets/dice5.png";
				case 6:
					return "/Assets/dice6.png";
				default:
					return "/Assets/black.png";
			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}