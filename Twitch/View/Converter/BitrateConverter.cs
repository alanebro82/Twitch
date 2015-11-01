using System;
using Windows.UI.Xaml.Data;

namespace Twitch.View.Converter
{

    class BitrateConverter : IValueConverter
    {
        public object Convert( object aValue, Type aTargetType, object aParameter, string aLanguage )
        {
            var theValue = System.Convert.ToDouble( aValue );

            int thePrefixCount = 0;
            while( theValue > 1024 )
            {
                theValue /= 1024;
                ++thePrefixCount;
            }

            var theNumberString = theValue.ToString( "####.##" ) + " ";

            string theSuffix;
            switch( thePrefixCount )
            {
                case 0:
                    theSuffix = "bps";
                    break;
                case 1:
                    theSuffix = "Kbps";
                    break;
                case 2:
                    theSuffix = "Mbps";
                    break;
                case 3:
                    theSuffix = "Gbps";
                    break;
                case 4:
                    theSuffix = "Tbps";
                    break;
                default:
                    theSuffix = "LOTSAbps";
                    break;
            }

            return theNumberString + theSuffix;
        }

        public object ConvertBack( object value, Type targetType, object parameter, string language )
        {
            throw new NotImplementedException();
        }
    }
}
