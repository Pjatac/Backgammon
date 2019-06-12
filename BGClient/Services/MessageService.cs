using BGClient.Infra;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace BGClient.Services
{
	public class MessageService: IMessageService
	{
		public ContentDialog CD = new ContentDialog();
		public async Task Show(string msg)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
			MessageDialog myDialog = new MessageDialog(msg);
			await myDialog.ShowAsync();
			});
		}
		public async Task ShowContent(string msg)
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CD.Content = msg;
				await CD.ShowAsync();
			});
		}
		public async Task HideContent()
		{
			await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			async () =>
			{
				CD.Content = string.Empty;
				CD.Hide();
			});
		}
	}
}
