using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Twitch.Services;
using Twitch.View;

namespace Twitch.ViewModel
{
    public class ViewModelLocator
    {
        public const string scMainPageKey = "MainPage";
        public const string scStreamResultsPageKey = "StreamResultsPage";
        public const string scPlayerPageKey = "PlayerPage";


        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );

            SetUpNavigation();
            SetUpServices();
            SetUpViewModels();
        }

        private static void SetUpNavigation()
        {
            var theNav = new NavigationService();
            theNav.Configure( scMainPageKey, typeof( MainPage ) );
            theNav.Configure( scStreamResultsPageKey, typeof( StreamResultsPage ) );
            theNav.Configure( scPlayerPageKey, typeof( PlayerPage ) );
            SimpleIoc.Default.Register<INavigationService>( () => theNav );
        }

        private static void SetUpServices()
        {
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<ITwitchQueryService, TwitchQueryService>();
        }

        private static void SetUpViewModels()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PlayerViewModel>();
            SimpleIoc.Default.Register<StreamResultsViewModel>();
        }

        public PlayerViewModel Player => ServiceLocator.Current.GetInstance<PlayerViewModel>();
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public StreamResultsViewModel StreamResults => ServiceLocator.Current.GetInstance<StreamResultsViewModel>();
    }
}
