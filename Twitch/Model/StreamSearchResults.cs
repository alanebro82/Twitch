using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace Twitch.Model
{
    public class StreamSearchResults
    {
        //----------------------------------------------------------------------
        public StreamSearchResults( JsonObject aJsonObject )
        {
            Total = Convert.ToInt32( aJsonObject.GetNamedNumber( scTotalString ) );
            Links = new Links( aJsonObject.GetNamedObject( scLinksString ) );

            var theStreamList = new List<Stream>();
            var theStreams = aJsonObject.GetNamedArray( scStreamsString );
            foreach( var theStream in theStreams )
            {
                theStreamList.Add( new Stream( theStream.GetObject() ) );
            }

            StreamsList = theStreamList;
        }

        public int Total
        {
            get;
        }

        public Links Links
        {
            get;
        }

        public IEnumerable<Stream> StreamsList
        {
            get;
        }

        private const string scTotalString = "_total";
        private const string scLinksString = "_links";
        private const string scStreamsString = "streams";
    }

    public class Stream
    {
        public Stream( JsonObject aJsonObject )
        {
            Id = Convert.ToInt64( aJsonObject.GetNamedNumber( scIdString ) );
            Game = aJsonObject.GetNamedString( scGameString );
            Viewers = Convert.ToInt32( aJsonObject.GetNamedNumber( scViewersString ) );
            VideoHeight = Convert.ToInt32( aJsonObject.GetNamedNumber( scVideoHeightString ) );
            AverageFps = Convert.ToInt32( aJsonObject.GetNamedNumber( scAverageFpsString ) );
            Delay = Convert.ToInt32( aJsonObject.GetNamedNumber( scDelayString ) );
            CreatedAt = DateTime.Parse( aJsonObject.GetNamedString( scCreatedAtString ) );
            IsPlaylist = aJsonObject.GetNamedBoolean( scIsPlaylistString );
            // Preview    = aJsonObject.GetNamedString( scGameString );
            Links = new Links( aJsonObject.GetNamedObject( scLinksString ) );
            Channel = new Channel( aJsonObject.GetNamedObject( scChannelString ) );
        }

        public Int64 Id
        {
            get;
        }

        public string Game
        {
            get;
        }

        public int Viewers
        {
            get;
        }
        public int VideoHeight
        {
            get;
        }
        public int AverageFps
        {
            get;
        }
        public double Delay
        {
            get;
        }
        public DateTime CreatedAt
        {
            get;
        }
        public bool IsPlaylist
        {
            get;
        }
        public string Preview
        {
            get;
        }
        public Links Links
        {
            get;
        }
        public Channel Channel
        {
            get;
        }

        private const string scIdString = "_id";
        private const string scGameString = "game";
        private const string scViewersString = "viewers";
        private const string scVideoHeightString = "video_height";
        private const string scAverageFpsString = "average_fps";
        private const string scDelayString = "delay";
        private const string scCreatedAtString = "created_at";
        private const string scIsPlaylistString = "is_playlist";
        private const string scPreviewString = "preview";
        private const string scLinksString = "_links";
        private const string scChannelString = "channel";
    }

    public class Channel
    {
        public Channel( JsonObject aJsonObject )
        {
            IsMature = aJsonObject.GetNamedBoolean( scMatureString );
            Status = aJsonObject.GetNamedString( scStatusString );
            BroadcasterLanguage = aJsonObject.GetNamedString( scBroadcasterLanguageString );
            DisplayName = aJsonObject.GetNamedString( scDisplayNameString );
            Game = aJsonObject.GetNamedString( scGameString );
            Delay = Convert.ToInt32( aJsonObject.GetNamedNumber( scDelayString ) );
            Language = aJsonObject.GetNamedString( scLanguageString );
            Id = Convert.ToInt64( aJsonObject.GetNamedNumber( scIdString ) );
            Name = aJsonObject.GetNamedString( scNameString );
            CreatedAt = DateTime.Parse( aJsonObject.GetNamedString( scCreatedAtString ) );
            UpdatedAt = DateTime.Parse( aJsonObject.GetNamedString( scUpdatedAtString ) );
            Logo = aJsonObject.GetNamedString( scLogoString );

            var theValue = aJsonObject.GetNamedValue( scBannerString );
            if( theValue.ValueType == JsonValueType.String )
            {
                Banner = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scVideoBannerString );
            if( theValue.ValueType == JsonValueType.String )
            {
                VideoBanner = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scBackgroundString );
            if( theValue.ValueType == JsonValueType.String )
            {
                Background = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scProfileBannerString );
            if( theValue.ValueType == JsonValueType.String )
            {
                ProfileBanner = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scProfileBannerBackgroundColorString );
            if( theValue.ValueType == JsonValueType.String )
            {
                ProfileBannerBackgroundColor = theValue.GetString();
            }

            IsPartner = aJsonObject.GetNamedBoolean( scPartnerString );
            Url = aJsonObject.GetNamedString( scUrlString );
            Views = Convert.ToInt64( aJsonObject.GetNamedNumber( scViewsString ) );
            Followers = Convert.ToInt64( aJsonObject.GetNamedNumber( scFollowersString ) );
        }

        public bool IsMature
        {
            get;
        }

        public string Status
        {
            get;
        }

        public string BroadcasterLanguage
        {
            get;
        }

        public string DisplayName
        {
            get;
        }

        public string Game
        {
            get;
        }

        public int Delay
        {
            get;
        }

        public string Language
        {
            get;
        }

        public Int64 Id
        {
            get;
        }

        public string Name
        {
            get;
        }

        public DateTime CreatedAt
        {
            get;
        }

        public DateTime UpdatedAt
        {
            get;
        }

        public string Logo
        {
            get;
        }

        public string Banner
        {
            get;
        } = string.Empty;

        public string VideoBanner
        {
            get;
        } = string.Empty;

        public string Background
        {
            get;
        } = string.Empty;

        public string ProfileBanner
        {
            get;
        } = string.Empty;

        public string ProfileBannerBackgroundColor
        {
            get;
        } = string.Empty;

        public bool IsPartner
        {
            get;
        }

        public string Url
        {
            get;
        } = string.Empty;

        public Int64 Views
        {
            get;
        }

        public Int64 Followers
        {
            get;
        }

        private const string scMatureString = "mature";
        private const string scStatusString = "status";
        private const string scBroadcasterLanguageString = "broadcaster_language";
        private const string scDisplayNameString = "display_name";
        private const string scGameString = "game";
        private const string scDelayString = "delay";
        private const string scLanguageString = "language";
        private const string scIdString = "_id";
        private const string scNameString = "name";
        private const string scCreatedAtString = "created_at";
        private const string scUpdatedAtString = "updated_at";
        private const string scLogoString = "logo";
        private const string scBannerString = "banner";
        private const string scVideoBannerString = "video_banner";
        private const string scBackgroundString = "background";
        private const string scProfileBannerString = "profile_banner";
        private const string scProfileBannerBackgroundColorString = "profile_banner_background_color";
        private const string scPartnerString = "partner";
        private const string scUrlString = "url";
        private const string scViewsString = "views";
        private const string scFollowersString = "followers";
    }
}