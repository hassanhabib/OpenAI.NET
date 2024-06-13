﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using Xeptions;

namespace Standard.AI.OpenAI.Models.Clients.FineTunes.Exceptions
{
    /// <summary>
    /// This exception is thrown when a validation error occurs while using the File Tune client.
    /// For example, if required data is missing or invalid
    /// </summary>
    public class FineTuneClientValidationException : Xeption
    {
        public FineTuneClientValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
