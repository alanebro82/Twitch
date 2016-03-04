using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Twitch.Services;

namespace Twitch.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider( () => SimpleIoc.Default );

            SetUpServices();
            SetUpViewModels();
        }

        private static void SetUpServices()
        {
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<ITwitchQueryService, TwitchQueryService>();
        }

        private static void SetUpViewModels()
        {
            SimpleIoc.Default.Register<GameResultsViewModel>();
            SimpleIoc.Default.Register<PlayerViewModel>();
            SimpleIoc.Default.Register<StreamResultsViewModel>();
        }

        public PlayerViewModel Player => ServiceLocator.Current.GetInstance<PlayerViewModel>();
        public GameResultsViewModel GameResults => ServiceLocator.Current.GetInstance<GameResultsViewModel>();
        public StreamResultsViewModel StreamResults => ServiceLocator.Current.GetInstance<StreamResultsViewModel>();
    }
}
