// --------------------------------------------------------------- 
// Copyright (c) Coalition of the Good-Hearted Engineers 
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using Moq;
using RESTFulSense.Exceptions;
using Standard.AI.OpenAI.Brokers.OpenAIs;
using Standard.AI.OpenAI.Models.Services.Foundations.AudioTranscriptions;
using Standard.AI.OpenAI.Models.Services.Foundations.ExternalAudioTranscriptions;
using Standard.AI.OpenAI.Services.Foundations.AudioTranscriptions;
using Tynamix.ObjectFiller;
using Xunit;

namespace Standard.AI.OpenAI.Tests.Unit.Services.Foundations.AudioTranscriptions
{
    public partial class AudioTranscriptionServiceTests
    {
        private readonly Mock<IOpenAIBroker> openAIBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAudioTranscriptionService audioTranscriptionService;

        public AudioTranscriptionServiceTests()
        {
            this.openAIBrokerMock = new Mock<IOpenAIBroker>();
            this.compareLogic = new CompareLogic();

            this.audioTranscriptionService = new AudioTranscriptionService(
                openAIBroker: this.openAIBrokerMock.Object
            );
        }

        private static dynamic CreateRandomAudioTranscriptionProperties()
        {
            return new
            {
                FilePath = $"{CreateRandomString()}.txt",
                Model = AudioTranscriptionModel.Create(CreateRandomString()),
                ResponseFormat = CreateRandomString(),
                Prompt = CreateRandomString(),
                Temperature = CreateRandomDecimal(),
                Language = CreateRandomString(),
                Text = CreateRandomString()
            };
        }

        private Expression<Func<ExternalAudioTranscriptionRequest, bool>> SameExternalAudioTranscriptionRequestAs(
            ExternalAudioTranscriptionRequest expectedExternalAudioTranscriptionRequest)
        {
            return actualExternalAudioTranscriptionRequest =>
                this.compareLogic.Compare(
                    expectedExternalAudioTranscriptionRequest,
                    actualExternalAudioTranscriptionRequest)
                        .AreEqual;
        }

        private static decimal CreateRandomDecimal()
            => new SequenceGeneratorDecimal().GetValue();

        private static string CreateRandomString()
            => new MnemonicString().GetValue();

        public static TheoryData UnAuthorizationExceptions()
        {
            return new TheoryData<HttpResponseException>
            {
                new HttpResponseUnauthorizedException(),
                new HttpResponseForbiddenException()
            };
        }

        private static AudioTranscription CreateRandomAudioTranscription() =>
            AudioTranscriptionFiller().Create();

        private static Filler<AudioTranscription> AudioTranscriptionFiller()
        {
            var filler = new Filler<AudioTranscription>();

            filler.Setup()
                .OnType<object>().IgnoreIt();

            filler.Setup()
                .OnProperty(p => p.Request.FilePath)
                .Use(new string[]
                {
                    $"{CreateRandomString()}.mp3"
                });

            filler.Setup()
                .OnType<AudioTranscriptionModel>()
                .Use(new AudioTranscriptionModel[]
                {
                    AudioTranscriptionModel.Create(CreateRandomString()),
                    AudioTranscriptionModel.Create(CreateRandomString()),
                    AudioTranscriptionModel.Create(CreateRandomString())
                });

            return filler;
        }
    }
}
