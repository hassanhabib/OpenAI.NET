﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.AIFiles;
using Standard.AI.OpenAI.Models.Services.Foundations.AIFiles.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.ExternalAIFiles;
using Xeptions;
using Xunit;

namespace Standard.AI.OpenAI.Tests.Unit.Services.Foundations.AIFiles
{
    public partial class AIFileServiceTests
    {
        [Fact]
        private async Task ShouldThrowValidationExceptionOnUploadIfAIFileIsNull()
        {
            // given
            AIFile nullAIFile = null;
            var nullAIFileException = new NullAIFileException(message: "Ai file is null.");

            var expectedAIFileValidationException =
                createAIFileValidationException(
                        innerException: nullAIFileException);

            // when
            ValueTask<AIFile> uploadFileTask = this.aiFileService.UploadFileAsync(nullAIFile);

            AIFileValidationException actualAIFileValidationException =
                await Assert.ThrowsAsync<AIFileValidationException>(uploadFileTask.AsTask);

            // then
            actualAIFileValidationException.Should().BeEquivalentTo(
                expectedAIFileValidationException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostFileFormAsync(It.IsAny<ExternalAIFileRequest>()),
                    Times.Never);

            this.openAIBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnUploadIfAIFileRequestIsNull()
        {
            // given
            var invalidAIFile = new AIFile();
            invalidAIFile.Request = null;

            var invalidAIFileException =
                createInvalidAIFileException();

            invalidAIFileException.AddData(
                key: nameof(AIFile.Request),
                values: "Value is required");

            var expectedAIFileValidationException =
                createAIFileValidationException(
                        innerException: invalidAIFileException);

            // when
            ValueTask<AIFile> uploadFileTask =
                this.aiFileService.UploadFileAsync(invalidAIFile);

            AIFileValidationException actualAIFileValidationException =
                await Assert.ThrowsAsync<AIFileValidationException>(uploadFileTask.AsTask);

            // then
            actualAIFileValidationException.Should().BeEquivalentTo(
                expectedAIFileValidationException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostFileFormAsync(It.IsAny<ExternalAIFileRequest>()),
                    Times.Never);

            this.openAIBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        private async Task ShouldThrowValidationExceptionOnUploadIfAIFileRequestIsInvalid(string invalidString)
        {
            // given
            var invalidAIFile = new AIFile();

            invalidAIFile.Request = new AIFileRequest
            {
                Name = invalidString,
                Purpose = invalidString,
                Content = null
            };

            var invalidAIFileException =
                createInvalidAIFileException();

            invalidAIFileException.AddData(
                key: nameof(AIFileRequest.Name),
                values: "Value is required");

            invalidAIFileException.AddData(
                key: nameof(AIFileRequest.Content),
                values: "Value is required");

            invalidAIFileException.AddData(
                key: nameof(AIFileRequest.Purpose),
                values: "Value is required");

            var expectedAIFileValidationException =
                createAIFileValidationException(
                        innerException: invalidAIFileException);

            // when
            ValueTask<AIFile> uploadFileTask =
                this.aiFileService.UploadFileAsync(invalidAIFile);

            AIFileValidationException actualAIFileValidationException =
                await Assert.ThrowsAsync<AIFileValidationException>(uploadFileTask.AsTask);

            // then
            actualAIFileValidationException.Should().BeEquivalentTo(
                expectedAIFileValidationException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostFileFormAsync(It.IsAny<ExternalAIFileRequest>()),
                    Times.Never);

            this.openAIBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        private static AIFileValidationException createAIFileValidationException(Xeption innerException)
        {
            return new AIFileValidationException(
                message: "AI file validation error occurred, fix errors and try again.",
                innerException);
        }

        private static InvalidAIFileException createInvalidAIFileException()
        {
            return new InvalidAIFileException(message: "Invalid AI file error occurred, fix errors and try again.");

        }
    }
}