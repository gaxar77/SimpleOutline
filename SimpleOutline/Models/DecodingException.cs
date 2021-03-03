using System;

namespace SimpleOutline.Models
{
    public class DecodingException : Exception
    {
        private const string DefaultMessage = "An error occured while trying to decode outline data.";
        public DecodingException()
            : base(DefaultMessage)
        {

        }

        public DecodingException(Exception innerException)
            : base(DefaultMessage, innerException)
        {

        }
    }
}
