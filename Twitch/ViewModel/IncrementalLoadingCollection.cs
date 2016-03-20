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
    //==========================================================================
    public class IncrementalLoadingCollection<I> : ObservableCollection<I>, ISupportIncrementalLoading
    {
        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public IncrementalLoadingCollection( Func<uint, uint, Task<IEnumerable<I>>> aLoadingFunction )
        {
            mLoadingFunction = aLoadingFunction;
        }

        //----------------------------------------------------------------------
        // PUBLIC ISupportIncrementalLoading
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public bool HasMoreItems
        {
            get;
            private set;
        } = true;

        //----------------------------------------------------------------------
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync( uint aCount )
        {
            var theDispatcher = Window.Current.Dispatcher;

            return Task.Run( async () =>
            {
                var theResult = await mLoadingFunction( mItemsLoaded, aCount );

                uint theResultCount = 0;

                if( theResult == null || !theResult.Any() )
                {
                    HasMoreItems = false;
                }
                else
                {
                    theResultCount = (uint)theResult.Count();
                    mItemsLoaded += theResultCount;

                    await theDispatcher.RunAsync(
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

        //----------------------------------------------------------------------
        // PRIVATE FIELDS
        //----------------------------------------------------------------------

        private uint mItemsLoaded = 0;
        private readonly Func<uint, uint, Task<IEnumerable<I>>> mLoadingFunction;

    }
}
