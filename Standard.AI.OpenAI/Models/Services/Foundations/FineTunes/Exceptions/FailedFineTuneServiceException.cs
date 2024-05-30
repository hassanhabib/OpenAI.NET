﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using Xeptions;

namespace Standard.AI.OpenAI.Models.Services.Foundations.FineTunes.Exceptions
{
    public class FailedFineTuneServiceException : Xeption
    {
        public FailedFineTuneServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}