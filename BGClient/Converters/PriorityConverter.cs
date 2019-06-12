using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BGClient.Converters
{
	public class PriorityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			int number = (int)value;
			switch (number)
			{
				case 1:
					return "/Assets/white.png";
				case 2:
					return "/Assets/black.png";
				default:
					return "";
			}

		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}