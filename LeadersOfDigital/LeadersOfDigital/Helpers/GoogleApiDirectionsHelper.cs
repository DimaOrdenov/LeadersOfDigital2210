using System;
using System.Collections.Generic;
using Business.Definitions.Models;
using Business.Definitions.Models.GoogleDirectionsApi;

namespace LeadersOfDigital.Helpers
{
    public static class GoogleApiDirectionsHelper
    {
        public static IEnumerable<Position> DecodePolylineIntoPoints(this Polyline polyline)
        {
            if (string.IsNullOrEmpty(polyline?.Points))
            {
                throw new ArgumentNullException(nameof(Polyline.Points));
            }

            char[] polylineChars = polyline.Points.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylineChars.Length)
            {
                sum = 0;
                shifter = 0;

                do
                {
                    next5bits = polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                {
                    break;
                }

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                sum = 0;
                shifter = 0;

                do
                {
                    next5bits = polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32)
                {
                    break;
                }

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                yield return new Position(Convert.ToDouble(currentLat) / 1E5, Convert.ToDouble(currentLng) / 1E5);
            }
        }
    }
}
