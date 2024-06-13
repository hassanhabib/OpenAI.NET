﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using Xeptions;

namespace Standard.AI.OpenAI.Models.Services.Foundations.LocalFiles.Exceptions
{
    public class LocalFileServiceException : Xeption
    {
        public LocalFileServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
