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
            Preview    = aJsonObject.GetNamedObject( scPreviewString ).GetNamedString( "large" );
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
            var theValue = aJsonObject.GetNamedValue( scMatureString );
            if( theValue.ValueType == JsonValueType.Boolean )
            {
                IsMature = theValue.GetBoolean();
            }

            theValue = aJsonObject.GetNamedValue( scStatusString );
            if( theValue.ValueType == JsonValueType.String )
            {
                Status = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scBroadcasterLanguageString );
            if( theValue.ValueType == JsonValueType.String )
            {
                BroadcasterLanguage = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scDisplayNameString );
            if( theValue.ValueType == JsonValueType.String )
            {
                DisplayName = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scGameString );
            if( theValue.ValueType == JsonValueType.String )
            {
                Game = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scDelayString );
            if( theValue.ValueType == JsonValueType.Number )
            {
                Delay = Convert.ToInt32( theValue.GetNumber() );
            }

            theValue = aJsonObject.GetNamedValue( scLanguageString );
            if( theValue.ValueType == JsonValueType.String )
            {
                Language = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scIdString );
            if( theValue.ValueType == JsonValueType.Number )
            {
                Id = Convert.ToInt64( theValue.GetNumber() );
            }

            theValue = aJsonObject.GetNamedValue( scNameString );
            if( theValue.ValueType == JsonValueType.String )
            {
                Name = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scCreatedAtString );
            if( theValue.ValueType == JsonValueType.String )
            {
                CreatedAt = DateTime.Parse( theValue.GetString() );
            }

            theValue = aJsonObject.GetNamedValue( scUpdatedAtString );
            if( theValue.ValueType == JsonValueType.String )
            {
                UpdatedAt = DateTime.Parse( theValue.GetString() );
            }

            theValue = aJsonObject.GetNamedValue( scLogoString );
            if( theValue.ValueType == JsonValueType.String )
            {
                Logo = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scBannerString );
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

            theValue = aJsonObject.GetNamedValue( scIsPartnerString );
            if( theValue.ValueType == JsonValueType.Boolean )
            {
                IsPartner = theValue.GetBoolean();
            }

            theValue = aJsonObject.GetNamedValue( scUrlString );
            if( theValue.ValueType == JsonValueType.String )
            {
                Url = theValue.GetString();
            }

            theValue = aJsonObject.GetNamedValue( scViewsString );
            if( theValue.ValueType == JsonValueType.Number )
            {
                Views = Convert.ToInt64( theValue.GetNumber() );
            }

            theValue = aJsonObject.GetNamedValue( scFollowersString );
            if( theValue.ValueType == JsonValueType.Number )
            {
                Followers = Convert.ToInt64( theValue.GetNumber() );
            }
        }

        public bool IsMature
        {
            get;
        } = false;

        public string Status
        {
            get;
        } = string.Empty;

        public string BroadcasterLanguage
        {
            get;
        } = string.Empty;

        public string DisplayName
        {
            get;
        } = string.Empty;

        public string Game
        {
            get;
        } = string.Empty;

        public int Delay
        {
            get;
        } = 0;

        public string Language
        {
            get;
        } = string.Empty;

        public Int64 Id
        {
            get;
        } = 0;

        public string Name
        {
            get;
        } = string.Empty;

        public DateTime CreatedAt
        {
            get;
        } = DateTime.MinValue;

        public DateTime UpdatedAt
        {
            get;
        } = DateTime.MinValue;

        public string Logo
        {
            get;
        } = string.Empty;

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
        } = false;

        public string Url
        {
            get;
        } = string.Empty;

        public Int64 Views
        {
            get;
        } = 0;

        public Int64 Followers
        {
            get;
        } = 0;

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
        private const string scIsPartnerString = "partner";
        private const string scUrlString = "url";
        private const string scViewsString = "views";
        private const string scFollowersString = "followers";
    }
}
