using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Twitch.Services;

namespace Twitch.ViewModel
{
    public class ViewModelLocator
    {
        public const string SecondPageKey = "SecondPage";

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
            theNav.Configure( SecondPageKey, typeof( SecondPage ) );
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
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes." )]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
