using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Twitch.Model;
using Twitch.Services;

namespace Twitch.ViewModel
{
    public class ViewModelLocator
    {
        public const string SecondPageKey = "SecondPage";

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );

            var nav = new NavigationService();
            nav.Configure( SecondPageKey, typeof( SecondPage ) );
            SimpleIoc.Default.Register<INavigationService>( () => nav );

            SimpleIoc.Default.Register<IDialogService, DialogService>();

            SimpleIoc.Default.Register<ITwitchQueryService, TwitchQueryService>();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes." )]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}
