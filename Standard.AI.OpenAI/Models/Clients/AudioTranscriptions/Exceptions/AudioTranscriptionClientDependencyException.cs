﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using Xeptions;

namespace Standard.AI.OpenAI.Models.Clients.AudioTranscriptions.Exceptions
{
    public class AudioTranscriptionClientDependencyException : Xeption
    {
        public AudioTranscriptionClientDependencyException(Xeption innerException)
            : base(
                message: "Audio transcription dependency error occurred, contact support.",
                innerException)
        { }
    }
}