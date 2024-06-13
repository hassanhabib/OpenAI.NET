﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using Xeptions;

namespace Standard.AI.OpenAI.Models.Services.Foundations.AIModels.Exceptions
{
    public class UnauthorizedAIModelException : Xeption
    { 
        public UnauthorizedAIModelException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
