using BGClient.Infra;
using BGClient.Services;
using BGClient.Views;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.VM
{
	class ViewModelLocator
	{
		public const string StartPageKey = "StartPage";
		public const string RegisterPageKey = "RegisterPage";
		public const string HomePageKey = "HomePage";
		public const string ChatPageKey = "ChatPage";
		public const string GamePageKey = "GamePage";
		public ViewModelLocator()
		{		
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			SimpleIoc.Default.Register<IHubService, HubService>();
			SimpleIoc.Default.Register<IMessageService, MessageService>();
			SimpleIoc.Default.Register<IUserService, UserService>();
			SimpleIoc.Default.Register<IGameService, GameService>();
			SimpleIoc.Default.Register<StartPageVM>();
			SimpleIoc.Default.Register<RegisterPageVM>();
			SimpleIoc.Default.Register<HomePageVM>();
			SimpleIoc.Default.Register<ChatPageVM>();
			SimpleIoc.Default.Register<GamePageVM>();

			var navService = new NavigationService();
			navService.Configure(StartPageKey, typeof(StartPage));
			navService.Configure(RegisterPageKey, typeof(RegisterPage));
			navService.Configure(HomePageKey, typeof(HomePage));
			navService.Configure(ChatPageKey, typeof(ChatPage));
			navService.Configure(GamePageKey, typeof(GamePage));
			SimpleIoc.Default.Register<INavigationService>(() => navService);
		}
		public StartPageVM StartPageInstance
		{
			get
			{
				return ServiceLocator.Current.GetInstance<StartPageVM>();
			}
		}
		public RegisterPageVM RegisterPageInstance
		{
			get
			{
				return ServiceLocator.Current.GetInstance<RegisterPageVM>();
			}
		}
		public HomePageVM HomePageInstance
		{
			get
			{
				return ServiceLocator.Current.GetInstance<HomePageVM>();
			}
		}
		public ChatPageVM ChatPageInstance
		{
			get
			{
				return ServiceLocator.Current.GetInstance<ChatPageVM>();
			}
		}
		public GamePageVM GamePageInstance
		{
			get
			{
				return ServiceLocator.Current.GetInstance<GamePageVM>();
			}
		}
		public static void Cleanup()
		{
		}
	}
}