using System;
using System.Runtime.Serialization;

namespace GeoIpProject.Clients.Interfaces.Exceptions
{
    [Serializable]
    public class FreeGeoIpClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeGeoIpClientException"/> class.
        /// </summary>
        public FreeGeoIpClientException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeGeoIpClientException"/> class with a specified error message and status code.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">The status code associated with the exception.</param>
        public FreeGeoIpClientException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeGeoIpClientException"/> class with a specified error message, status code, and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        /// <param name="statusCode">The status code associated with the exception.</param>
        public FreeGeoIpClientException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        #region //  Implement ISerializable  //

        /// <summary>
        /// Initializes a new instance of the <see cref="FreeGeoIpClientException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected FreeGeoIpClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/> object with the exception information.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
