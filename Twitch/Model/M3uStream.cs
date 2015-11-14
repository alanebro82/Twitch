using System;
using System.Collections.Generic;
using System.Linq;

namespace Twitch.Model
{
    //==========================================================================
    public class M3uStream
    {
        //----------------------------------------------------------------------
        public static IEnumerable<M3uStream> ParseM3uStreams( string aM3uString )
        {
            var theM3uList = new List<M3uStream>();

            var theLines = aM3uString.Split( new string[] { "\r\n", "\n" }, StringSplitOptions.None );
            foreach( var theLine in theLines )
            {
                if( theLine.StartsWith( "#EXT-X-MEDIA" ) )
                {
                    var theNewStream = new M3uStream();

                    var theSplitLine = theLine.Split( new string[] { "," }, StringSplitOptions.None );
                    foreach( var theSplit in theSplitLine )
                    {
                        if( theSplit.StartsWith( "NAME=" ) )
                        {
                            theNewStream.DisplayName = theSplit.Split( new string[] { "=" }, StringSplitOptions.None ).Last().Replace( "\"", "" );
                        }
                    }
                    theM3uList.Add( theNewStream );
                }
                else if( theLine.StartsWith( "#EXT-X-STREAM-INF" ) )
                {
                    var theCurrent = theM3uList.LastOrDefault();
                    if( theCurrent != null )
                    {
                        var theSplitLine = theLine.Split( new string[] { "," }, StringSplitOptions.None );
                        foreach( var theSplit in theSplitLine )
                        {
                            if( theSplit.StartsWith( "BANDWIDTH=" ) )
                            {
                                int theInt;
                                if( int.TryParse( theSplit.Split( new string[] { "=" }, StringSplitOptions.None ).Last(), out theInt ) )
                                {
                                    theCurrent.Bandwidth = theInt;
                                }
                            }
                            else if( theSplit.StartsWith( "RESOLUTION=" ) )
                            {
                                theCurrent.ResolutionString = theSplit.Split( new string[] { "=" }, StringSplitOptions.None ).Last();
                            }
                        }
                    }
                }
                else if( theLine.StartsWith( "http" ) )
                {
                    var theCurrent = theM3uList.LastOrDefault();
                    if( theCurrent != null )
                    {
                        theCurrent.Uri = new Uri( theLine );
                    }
                }
            }

            return theM3uList;
        }

        //----------------------------------------------------------------------
        private M3uStream()
        {

        }

        //----------------------------------------------------------------------
        public string DisplayName
        {
            get;
            private set;
        } = string.Empty;

        //----------------------------------------------------------------------
        public Uri Uri
        {
            get;
            private set;
        }

        //----------------------------------------------------------------------
        public int Bandwidth
        {
            get;
            private set;
        } = 0;

        //----------------------------------------------------------------------
        public string ResolutionString
        {
            get;
            private set;
        } = string.Empty;
    }
}
