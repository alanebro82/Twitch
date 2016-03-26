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
    public class IncrementalLoadingCollection<I> : ObservableCollection<I>, ISupportIncrementalLoading, IDisposable where I : IUniqueId
    {
        //----------------------------------------------------------------------
        // PUBLIC METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        public IncrementalLoadingCollection( Func<uint, uint, Task<IEnumerable<I>>> aLoadingFunction )
            :
            this( aLoadingFunction, TimeSpan.Zero )
        {
        }

        //----------------------------------------------------------------------
        public IncrementalLoadingCollection( Func<uint, uint, Task<IEnumerable<I>>> aLoadingFunction, TimeSpan aUpdateFrequency )
        {
            if( aUpdateFrequency != TimeSpan.Zero )
            {
                mLoadingFunction = aLoadingFunction;
                mUpdateTimer.Interval = aUpdateFrequency;
                mUpdateTimer.Tick += HandleUpdateTimerTick;
                mUpdateTimer.Start();
            }
        }

        //----------------------------------------------------------------------
        public void Dispose()
        {
            mUpdateTimer.Stop();
            mUpdateTimer.Tick -= HandleUpdateTimerTick;
        }

        //----------------------------------------------------------------------
        private async void HandleUpdateTimerTick( object sender, object e )
        {
            mUpdateTimer.Stop();

            uint theLoadedItemCount;
            lock ( mLock )
            {
                theLoadedItemCount = mItemsLoaded;
            }

            var theResults = await mLoadingFunction( 0, theLoadedItemCount );

            lock ( mLock )
            {
                for( int i = 0; i < theResults.Count(); ++i )
                {
                    this[i] = theResults.ElementAt( i );
                }
            }

            mUpdateTimer.Start();
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
                uint theLoadedItemCount;
                lock ( mLock )
                {
                    theLoadedItemCount = mItemsLoaded;
                }

                var theResult = await mLoadingFunction( theLoadedItemCount, aCount );

                uint theResultCount = 0;

                if( theResult == null || !theResult.Any() )
                {
                    HasMoreItems = false;
                }
                else
                {
                    theResultCount = (uint)theResult.Count();
                    lock( mLock )
                    {
                        mItemsLoaded += theResultCount;
                    }

                    await theDispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () =>
                        {
                            lock ( mLock )
                            {
                                var theKeys = this.Select( ( aItem ) => aItem.Key );
                                foreach( I item in theResult )
                                {
                                    if( !theKeys.Contains( item.Key ) )
                                    {
                                        Add( item );
                                    }
                                }
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
        private readonly DispatcherTimer mUpdateTimer = new DispatcherTimer();
        private readonly object mLock = new object();

    }
}
