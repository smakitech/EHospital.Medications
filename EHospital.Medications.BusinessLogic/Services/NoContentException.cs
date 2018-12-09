using System;

namespace EHospital.Medications.BusinessLogic.Services
{
    /// <summary>
    /// Represent exception situation
    /// when collection is empty and doesn't retrieves entities.
    /// </summary>
    /// <seealso cref="System.Exception"/>
    public class NoContentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoContentException"/> class.
        /// </summary>
        public NoContentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoContentException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NoContentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoContentException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner exception.</param>
        public NoContentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}