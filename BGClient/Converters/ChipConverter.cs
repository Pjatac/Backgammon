using BGClient.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace BGClient.Converters
{
	public class ChipConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			Cell cell = (Cell)value;
			

			ObservableCollection<Ellipse> col = new ObservableCollection<Ellipse>();

			for (int i = 0; i < cell.NumOfChip; i++)
			{
				Ellipse e = new Ellipse();
				e.Width = 20;
				e.Height = 20;
				e.Fill = new SolidColorBrush(cell.ChipColor == 1 ? Colors.White : Colors.Black);
				col.Add(e);
			}

			return col;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
