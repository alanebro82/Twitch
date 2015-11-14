using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace Twitch.Model
{
    //==========================================================================
    public class GameSearchResults
    {
        //----------------------------------------------------------------------
        public GameSearchResults( JsonObject aJsonObject )
        {
            Total = Convert.ToInt32( aJsonObject.GetNamedNumber( scTotalString ) );
            Links = new Links( aJsonObject.GetNamedObject( scLinksString ) );

            var theGameList = new List<Game>();
            var theTop = aJsonObject.GetNamedArray( scTopString );
            foreach( var theGame in theTop )
            {
                theGameList.Add( new Game( theGame.GetObject() ) );
            }

            GamesList = theGameList;
        }

        //----------------------------------------------------------------------
        public int Total
        {
            get;
        }

        //----------------------------------------------------------------------
        public Links Links
        {
            get;
        }

        //----------------------------------------------------------------------
        public IEnumerable<Game> GamesList
        {
            get;
        }

        private const string scTotalString = "_total";
        private const string scLinksString = "_links";
        private const string scTopString = "top";
    }

    //==========================================================================
    public class Links
    {
        //----------------------------------------------------------------------
        public Links( JsonObject aJsonObject )
        {
            Self = aJsonObject.GetNamedString( scSelfString, string.Empty );
            Next = aJsonObject.GetNamedString( scNextString, string.Empty );
            Prev = aJsonObject.GetNamedString( scPrevString, string.Empty );
        }


        //----------------------------------------------------------------------
        public string Self
        {
            get;
        }

        //----------------------------------------------------------------------
        public string Next
        {
            get;
        }

        //----------------------------------------------------------------------
        public string Prev
        {
            get;
        }

        private const string scSelfString = "self";
        private const string scNextString = "next";
        private const string scPrevString = "prev";
    }

    public class Game
    {
        //----------------------------------------------------------------------
        public Game( JsonObject aJsonObject )
        {
            Viewers = Convert.ToInt32( aJsonObject.GetNamedNumber( scViewersString ) );
            Channels = Convert.ToInt32( aJsonObject.GetNamedNumber( scChannelsString ) );

            var theGameObject = aJsonObject.GetNamedObject( scGameString );
            Name = theGameObject.GetNamedString( scNameString );
            ImageUrl = theGameObject.GetNamedObject( scImageUrlString ).GetNamedString( scLargeImageUrlString );
        }

        //----------------------------------------------------------------------
        public override bool Equals( object aOther )
        {
            if( aOther is Game )
            {
                return ( (Game)aOther ).Name == Name;
            }

            return false;
        }

        //----------------------------------------------------------------------
        public override int GetHashCode()
        {
            return 17 * Name.GetHashCode();
        }

        //----------------------------------------------------------------------
        public int Viewers
        {
            get;
        }

        //----------------------------------------------------------------------
        public int Channels
        {
            get;
        }

        //----------------------------------------------------------------------
        public string Name
        {
            get;
        }

        //----------------------------------------------------------------------
        public string ImageUrl
        {
            get;
        }

        private const string scViewersString = "viewers";
        private const string scChannelsString = "channels";
        private const string scGameString = "game";
        private const string scNameString = "name";
        private const string scImageUrlString = "box";
        private const string scLargeImageUrlString = "large";

    }
}
