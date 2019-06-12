using Windows.UI.Xaml.Navigation;

namespace BGClient.Infra
{
	public interface INavigable
	{
		void OnNavigatedTo(NavigationEventArgs e);
		void OnNavigatedFrom(NavigationEventArgs e);
		void OnNavigatingFrom(NavigatingCancelEventArgs e);
		bool AllowGoBack();
	}
}
