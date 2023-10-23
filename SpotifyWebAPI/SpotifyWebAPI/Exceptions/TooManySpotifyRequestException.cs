using System;

namespace SpotifyWebAPI.Exceptions
{
    public class TooManySpotifyRequestException : Exception
    {
        public TooManySpotifyRequestException(int retryAfter)
        {
            RetryAfter = retryAfter;
        }

        public int RetryAfter { get; set; }
    }
}
