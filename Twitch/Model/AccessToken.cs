using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace Twitch.Model
{
    //==========================================================================
    class AccessToken
    {
        //----------------------------------------------------------------------
        public AccessToken( JsonObject aJsonObject )
        {
            var theToken = aJsonObject.GetNamedValue( scSignatureString );
            var theTokenString = theToken.GetString();
            Token = new Token( JsonObject.Parse( theTokenString ) );

            Signature = aJsonObject.GetNamedString( scTokenString );
            IsMobileRestricted = aJsonObject.GetNamedBoolean( scIsMobileRestrictedString );
        }

        //----------------------------------------------------------------------
        public JsonObject ToJsonObject()
        {
            JsonObject theJsonObject = new JsonObject();

            theJsonObject.SetNamedValue( scSignatureString, Token.ToJsonObject() );
            theJsonObject.SetNamedValue( scTokenString, JsonValue.CreateStringValue( Signature ) );
            theJsonObject.SetNamedValue( scIsMobileRestrictedString, JsonValue.CreateBooleanValue( IsMobileRestricted ) );

            return theJsonObject;
        }

        //----------------------------------------------------------------------
        public Token Token
        {
            get;
        }

        //----------------------------------------------------------------------
        public string Signature
        {
            get;
        }

        //----------------------------------------------------------------------
        public bool IsMobileRestricted
        {
            get;
        }

        private const string scSignatureString = "token";
        private const string scTokenString = "sig";
        private const string scIsMobileRestrictedString = "mobile_restricted";
    }

    //==========================================================================
    class Token
    {
        //----------------------------------------------------------------------
        public Token( JsonObject aJsonObject )
        {
            var theUserId = aJsonObject.GetNamedValue( scUserIdString );
            if( theUserId.ValueType == JsonValueType.Number )
            {
                UserId = Convert.ToInt32( theUserId.GetNumber() );
            }
            else
            {
                UserId = null;
            }

            Channel = aJsonObject.GetNamedString( scChannelString );
            Expires = aJsonObject.GetNamedNumber( scExpiresString );
            ChanSub = new ChanSub( aJsonObject.GetNamedValue( scChansubString ).GetObject() );
            Private = new Private( aJsonObject.GetNamedValue( scPrivateString ).GetObject() );
            Privileged = aJsonObject.GetNamedBoolean( scPrivilegedString );
            SourceRestricted = aJsonObject.GetNamedBoolean( scSourceRestrictedString );
        }

        //----------------------------------------------------------------------
        public JsonObject ToJsonObject()
        {
            JsonObject theJsonObject = new JsonObject();
            if( UserId.HasValue )
            {
                theJsonObject.SetNamedValue( scUserIdString, JsonValue.CreateNumberValue( UserId.Value ) );
            }
            else
            {
                theJsonObject.SetNamedValue( scUserIdString, JsonValue.CreateNullValue() );
            }

            theJsonObject.SetNamedValue( scChannelString, JsonValue.CreateStringValue( Channel ) );
            theJsonObject.SetNamedValue( scExpiresString, JsonValue.CreateNumberValue( Expires ) );
            theJsonObject.SetNamedValue( scChansubString, ChanSub.ToJsonObject() );
            theJsonObject.SetNamedValue( scPrivateString, Private.ToJsonObject() );
            theJsonObject.SetNamedValue( scPrivilegedString, JsonValue.CreateBooleanValue( Privileged ) );
            theJsonObject.SetNamedValue( scSourceRestrictedString, JsonValue.CreateBooleanValue( SourceRestricted ) );

            return theJsonObject;
        }

        //----------------------------------------------------------------------
        public Nullable<int> UserId
        {
            get;
            set;
        }

        //----------------------------------------------------------------------
        public string Channel
        {
            get;
            set;
        }

        //----------------------------------------------------------------------
        public double Expires
        {
            get;
            set;
        }

        //----------------------------------------------------------------------
        public ChanSub ChanSub
        {
            get;
            set;
        }

        //----------------------------------------------------------------------
        public Private Private
        {
            get;
            set;
        }

        //----------------------------------------------------------------------
        public bool Privileged
        {
            get;
            set;
        }

        //----------------------------------------------------------------------
        public bool SourceRestricted
        {
            get;
            set;
        }

        private const string scUserIdString = "user_id";
        private const string scChannelString = "channel";
        private const string scExpiresString = "expires";
        private const string scChansubString = "chansub";
        private const string scPrivateString = "private";
        private const string scPrivilegedString = "privileged";
        private const string scSourceRestrictedString = "source_restricted";

    }

    //==========================================================================
    class ChanSub
    {
        //----------------------------------------------------------------------
        public ChanSub( JsonObject aJsonObject )
        {
            ViewUntil = aJsonObject.GetNamedNumber( scViewUntilString );
            var theRestrictedBitrates = aJsonObject.GetNamedArray( scRestrictedBitratesString );

            foreach( var theBitrate in theRestrictedBitrates )
            {
                RestrictedBitrates.Add( theBitrate.GetNumber() );
            }
        }

        //----------------------------------------------------------------------
        public JsonObject ToJsonObject()
        {
            JsonObject theJsonObject = new JsonObject();
            theJsonObject.SetNamedValue( scViewUntilString, JsonValue.CreateNumberValue( ViewUntil ) );

            var theArray = new JsonArray();
            foreach( var theVal in RestrictedBitrates )
            {
                theArray.Add( JsonValue.CreateNumberValue( theVal ) );
            }

            theJsonObject.SetNamedValue( scRestrictedBitratesString, theArray );
            return theJsonObject;
        }

        //----------------------------------------------------------------------
        public double ViewUntil
        {
            get;
            set;
        }

        //----------------------------------------------------------------------
        public List<double> RestrictedBitrates
        {
            get;
            set;
        } = new List<double>();

        private const string scViewUntilString = "view_until";
        private const string scRestrictedBitratesString = "restricted_bitrates";

    }

    //==========================================================================
    class Private
    {
        //----------------------------------------------------------------------
        public Private( JsonObject aJsonObject )
        {
            AllowedToView = aJsonObject.GetNamedBoolean( scAllowedToViewString );
        }

        //----------------------------------------------------------------------
        public JsonObject ToJsonObject()
        {
            JsonObject theJsonObject = new JsonObject();
            theJsonObject.SetNamedValue( scAllowedToViewString, JsonValue.CreateBooleanValue( AllowedToView ) );
            return theJsonObject;
        }

        //----------------------------------------------------------------------
        public bool AllowedToView
        {
            get;
            set;
        }

        private const string scAllowedToViewString = "allowed_to_view";
    }
}
