using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Twitch.ViewModel
{
    //==========================================================================
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1063:ImplementIDisposableCorrectly" )]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1063:ImplementIDisposableCorrectly" )]
        public void Dispose()
        {
            mUpdateTimer.Stop();
            mUpdateTimer.Tick -= HandleUpdateTimerTick;
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
            return LoadMoreItemsAsyncImpl( aCount ).AsAsyncOperation();
        }

        //----------------------------------------------------------------------
        // PRIVATE METHODS
        //----------------------------------------------------------------------

        //----------------------------------------------------------------------
        private async Task<LoadMoreItemsResult> LoadMoreItemsAsyncImpl( uint aCount )
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
                lock ( mLock )
                {
                    mItemsLoaded += theResultCount;

                    var theKeys = this.Select( ( aItem ) => aItem.Key );
                    foreach( I item in theResult )
                    {
                        if( !theKeys.Contains( item.Key ) )
                        {
                            Add( item );
                        }
                    }
                }
            }

            return new LoadMoreItemsResult { Count = theResultCount };
        }

        //----------------------------------------------------------------------
#pragma warning disable S3168 // "async" methods should not return "void"
        private async void HandleUpdateTimerTick( object sender, object e )
#pragma warning restore S3168 // "async" methods should not return "void"
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
        // PRIVATE FIELDS
        //----------------------------------------------------------------------

        private uint mItemsLoaded = 0;
        private readonly Func<uint, uint, Task<IEnumerable<I>>> mLoadingFunction;
        private readonly DispatcherTimer mUpdateTimer = new DispatcherTimer();
        private readonly object mLock = new object();

    }
}
