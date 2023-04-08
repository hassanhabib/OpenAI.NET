﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Standard.AI.OpenAI.Models.Services.Foundations.ExternalFiles;
using Standard.AI.OpenAI.Models.Services.Foundations.AIFiles;

namespace Standard.AI.OpenAI.Brokers.OpenAIs
{
    internal partial class OpenAIBroker
    {
        public ValueTask<ExternalAIFileResponse> PostFileFormAsync(
            ExternalAIFileRequest externalFileRequest)
        {
            throw new NotImplementedException();
        }
        
        public async ValueTask<ExternalFile> DeleteFileByIdAsync(string fileId) =>
            await DeleteAsync<ExternalFile>(relativeUrl: $"v1/files/{fileId}");
    }
}