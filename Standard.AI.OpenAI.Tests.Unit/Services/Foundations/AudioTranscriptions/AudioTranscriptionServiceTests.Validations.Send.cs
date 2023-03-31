// --------------------------------------------------------------- 
// Copyright (c) Coalition of the Good-Hearted Engineers 
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.AudioTranscriptions;
using Standard.AI.OpenAI.Models.Services.Foundations.AudioTranscriptions.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.ExternalAudioTranscriptions;
using Xunit;

namespace Standard.AI.OpenAI.Tests.Unit.Services.Foundations.AudioTranscriptions
{
    public partial class AudioTranscriptionServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendIfAudioTranscriptionIsNullAsync()
        {
            // given
            AudioTranscription audioTranscription = null;

            NullAudioTranscriptionException nullAudioTranscriptionException = new();

            AudioTranscriptionValidationException exceptedAudioTranscriptionValidationException =
                new(nullAudioTranscriptionException);

            // when
            ValueTask<AudioTranscription> sendAudioTranscriptionTask =
                this.audioTranscriptionService.SendAudioTranscriptionAsync(audioTranscription);

            AudioTranscriptionValidationException actualAudioTranscriptionValidationException =
                await Assert.ThrowsAsync<AudioTranscriptionValidationException>(
                    sendAudioTranscriptionTask.AsTask);

            // then
            actualAudioTranscriptionValidationException.Should()
                .BeEquivalentTo(exceptedAudioTranscriptionValidationException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostAudioTranscriptionRequestAsync(
                    It.IsAny<ExternalAudioTranscriptionRequest>()),
                        Times.Never);

            this.openAIBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendIfAudioTranscriptionRequestIsNullAsync()
        {
            // given
            AudioTranscription audioTranscription = new()
            {
                Request = null
            };

            InvalidAudioTranscriptionException invalidAudioTranscriptionException = new();

            invalidAudioTranscriptionException.AddData(
                key: nameof(AudioTranscription.Request),
                values: "Value is required");

            AudioTranscriptionValidationException exceptedAudioTranscriptionValidationException =
                new(invalidAudioTranscriptionException);

            // when
            ValueTask<AudioTranscription> sendAudioTranscriptionTask =
                this.audioTranscriptionService.SendAudioTranscriptionAsync(audioTranscription);

            AudioTranscriptionValidationException actualAudioTranscriptionValidationException =
                await Assert.ThrowsAsync<AudioTranscriptionValidationException>(
                    sendAudioTranscriptionTask.AsTask);

            // then
            actualAudioTranscriptionValidationException.Should()
                .BeEquivalentTo(exceptedAudioTranscriptionValidationException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostAudioTranscriptionRequestAsync(
                    It.IsAny<ExternalAudioTranscriptionRequest>()),
                        Times.Never);

            this.openAIBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnSendIfAudioTranscriptionRequestIsInvalidAsync(string invalidText)
        {
            // given
            AudioTranscription audioTranscription = new()
            {
                Request = new AudioTranscriptionRequest
                {
                    FilePath = invalidText,
                    Model = default
                }
            };

            InvalidAudioTranscriptionException invalidAudioTranscriptionException = new();

            invalidAudioTranscriptionException.AddData(
                key: nameof(AudioTranscriptionRequest.FilePath),
                values: "Value is required");

            invalidAudioTranscriptionException.AddData(
                key: nameof(AudioTranscriptionRequest.Model),
                values: "Value is required");

            AudioTranscriptionValidationException exceptedAudioTranscriptionValidationException =
                new(invalidAudioTranscriptionException);

            // when
            ValueTask<AudioTranscription> sendAudioTranscriptionTask =
                this.audioTranscriptionService.SendAudioTranscriptionAsync(audioTranscription);

            AudioTranscriptionValidationException actualAudioTranscriptionValidationException =
                await Assert.ThrowsAsync<AudioTranscriptionValidationException>(
                    sendAudioTranscriptionTask.AsTask);

            // then
            actualAudioTranscriptionValidationException.Should()
                .BeEquivalentTo(exceptedAudioTranscriptionValidationException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostAudioTranscriptionRequestAsync(
                    It.IsAny<ExternalAudioTranscriptionRequest>()),
                        Times.Never);

            this.openAIBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendIfAudioTranscriptionRequestFilePathIsNotFoundAsync()
        {
            // given
            const string notFoundFilePath = "notFoundFile.txt";

            AudioTranscription audioTranscription = new()
            {
                Request = new AudioTranscriptionRequest
                {
                    FilePath = notFoundFilePath,
                    Model = AudioTranscriptionModel.Create(CreateRandomString())
                }
            };

            InvalidAudioTranscriptionException invalidAudioTranscriptionException = new();

            invalidAudioTranscriptionException.AddData(
                key: nameof(AudioTranscriptionRequest.FilePath),
                values: "File not found");

            AudioTranscriptionValidationException exceptedAudioTranscriptionValidationException =
                new(invalidAudioTranscriptionException);

            // when
            ValueTask<AudioTranscription> sendAudioTranscriptionTask =
                this.audioTranscriptionService.SendAudioTranscriptionAsync(audioTranscription);

            AudioTranscriptionValidationException actualAudioTranscriptionValidationException =
                await Assert.ThrowsAsync<AudioTranscriptionValidationException>(
                    sendAudioTranscriptionTask.AsTask);

            // then
            actualAudioTranscriptionValidationException.Should()
                .BeEquivalentTo(exceptedAudioTranscriptionValidationException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostAudioTranscriptionRequestAsync(
                    It.IsAny<ExternalAudioTranscriptionRequest>()),
                        Times.Never);

            this.openAIBrokerMock.VerifyNoOtherCalls();
        }
    }
}
