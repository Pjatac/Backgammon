using BGClient.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BGClient.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class RegisterPage : Page
	{
		public RegisterPage()
		{
			this.InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var navigableViewModel = this.DataContext as INavigable;
			if (navigableViewModel != null)
				navigableViewModel.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			var navigableViewModel = this.DataContext as INavigable;
			if (navigableViewModel != null)
				navigableViewModel.OnNavigatedFrom(e);
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			base.OnNavigatingFrom(e);

			var navigableViewModel = this.DataContext as INavigable;
			if (navigableViewModel != null)
				navigableViewModel.OnNavigatingFrom(e);
		}
	}
}