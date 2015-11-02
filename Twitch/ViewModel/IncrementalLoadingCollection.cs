using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Twitch.ViewModel
{
    public interface IIncrementalSource<T>
    {
        IEnumerable<T> GetPagedItems( int pageIndex, int pageSize );
    }

    public class IncrementalLoadingCollection<T, I> : ObservableCollection<I>, ISupportIncrementalLoading
         where T : IIncrementalSource<I>, new()
    {
        private T mSource = new T();
        private uint mItemsPerPage;
        private uint mCurrentPage;
        Func<uint, uint, Task<IEnumerable<I>>> mLoadingFunction;

        public IncrementalLoadingCollection( uint aItemsPerPage, Func<uint, uint, Task<IEnumerable<I>>> aLoadingFunction )
        {
            mItemsPerPage = aItemsPerPage;
            mLoadingFunction = aLoadingFunction;
        }

        public bool HasMoreItems
        {
            get;
            private set;
        } = true;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync( uint count )
        {
            var theDispatcher = Window.Current.Dispatcher;
            return mLoadingFunction( mItemsPerPage * mCurrentPage++, mItemsPerPage ).ContinueWith( ( aTask ) =>
            {
                uint theResultCount = 0;
                var theResult = aTask.Result;

                if( theResult == null || theResult.Count() == 0 )
                {
                    HasMoreItems = false;
                }
                else
                {
                    theResultCount = (uint)theResult.Count();

                    theDispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () =>
                        {
                            foreach( I item in theResult )
                            {
                                this.Add( item );
                            }
                        } );
                }

                return new LoadMoreItemsResult() { Count = theResultCount };
            } ).AsAsyncOperation();
        }
    }
}
