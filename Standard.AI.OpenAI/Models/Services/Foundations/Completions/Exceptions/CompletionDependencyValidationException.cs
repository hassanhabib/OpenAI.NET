﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using Xeptions;

namespace Standard.AI.OpenAI.Models.Services.Foundations.Completions.Exceptions
{
    public class CompletionDependencyValidationException : Xeption
    {
        public CompletionDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}