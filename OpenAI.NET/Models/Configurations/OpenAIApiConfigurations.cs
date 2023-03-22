﻿// --------------------------------------------------------------- 
// Copyright (c) Coalition of the Good-Hearted Engineers 
// ---------------------------------------------------------------

namespace OpenAI.NET.Models.Configurations
{
    public class OpenAIApiConfigurations
    {
        public string ApiUrl { get; set; } = "https://api.openai.com/v1";
        public string ApiKey { get; set; }
        public string OrganizationId { get; set; }
        public HttpPolicySettings PolicySettings { get; set; }
    }
}
