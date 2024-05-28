﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RESTFulSense.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.AIFiles;
using Standard.AI.OpenAI.Models.Services.Foundations.AIFiles.Exceptions;

namespace Standard.AI.OpenAI.Services.Foundations.AIFiles
{
    internal partial class AIFileService
    {
        private delegate ValueTask<AIFile> ReturningAIFileFunction();
        private delegate ValueTask<IEnumerable<AIFileResponse>> ReturningAIFilesFunction();

        private async ValueTask<AIFile> TryCatch(ReturningAIFileFunction returningAIFileFunction)
        {
            try
            {
                return await returningAIFileFunction();
            }
            catch (NullAIFileException nullAIFileException)
            {
                throw new AIFileValidationException(nullAIFileException);
            }
            catch (InvalidAIFileException invalidFileException)
            {
                throw new AIFileValidationException(invalidFileException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var invalidConfigurationFileException =
                    new InvalidConfigurationAIFileException(httpResponseUrlNotFoundException);

                throw new AIFileDependencyException(
                    message: "AI file dependency error occurred, contact support.", 
                    invalidConfigurationFileException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var unauthorizedAIFileException =
                    new UnauthorizedAIFileException(httpResponseUnauthorizedException);

                throw new AIFileDependencyException(
                    message: "AI file dependency error occurred, contact support.",
                    unauthorizedAIFileException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var unauthorizedAIFileException =
                    new UnauthorizedAIFileException(httpResponseForbiddenException);

                throw new AIFileDependencyException(
                    message: "AI file dependency error occurred, contact support.", 
                    unauthorizedAIFileException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var notFoundAIFileException =
                    new NotFoundAIFileException(httpResponseNotFoundException);

                throw new AIFileDependencyValidationException(
                    message: "AI file dependency validation error occurred, contact support.",
                    notFoundAIFileException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidAIFileException =
                    new InvalidAIFileException(httpResponseBadRequestException);

                throw new AIFileDependencyValidationException(
                    message: "AI file dependency validation error occurred, contact support.",
                    invalidAIFileException);
            }
            catch (HttpResponseTooManyRequestsException httpResponseTooManyRequestsException)
            {
                var excessiveCallAIFileException =
                    new ExcessiveCallAIFileException(httpResponseTooManyRequestsException);

                throw new AIFileDependencyValidationException(
                    message: "AI file dependency validation error occurred, contact support.",
                    excessiveCallAIFileException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedServerAIFileException =
                    new FailedServerAIFileException(httpResponseException);

                throw new AIFileDependencyException(
                    message: "AI file dependency error occurred, contact support.", 
                    failedServerAIFileException);
            }
            catch (Exception exception)
            {
                var failedAIFileServiceException =
                    new FailedAIFileServiceException(exception);

                throw new AIFileServiceException(failedAIFileServiceException);
            }
        }

        private async ValueTask<IEnumerable<AIFileResponse>> TryCatch(ReturningAIFilesFunction returningAIFilesFunction)
        {
            try
            {
                return await returningAIFilesFunction();
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var invalidConfigurationAIFileException =
                    new InvalidConfigurationAIFileException(httpResponseUrlNotFoundException);

                throw new AIFileDependencyException(
                    message: "AI file dependency error occurred, contact support.", 
                    invalidConfigurationAIFileException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var unauthorizedAIFileException =
                    new UnauthorizedAIFileException(httpResponseUnauthorizedException);

                throw new AIFileDependencyException(
                    message: "AI file dependency error occurred, contact support.", 
                    unauthorizedAIFileException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var unauthorizedAIFileException =
                    new UnauthorizedAIFileException(httpResponseForbiddenException);

                throw new AIFileDependencyException(
                    message: "AI file dependency error occurred, contact support.", 
                    unauthorizedAIFileException);
            }
            catch (HttpResponseTooManyRequestsException httpResponseTooManyRequestsException)
            {
                var excessiveCallAIFileException =
                    new ExcessiveCallAIFileException(httpResponseTooManyRequestsException);

                throw new AIFileDependencyValidationException(
                    message: "AI file dependency validation error occurred, contact support.", 
                    excessiveCallAIFileException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedServerAIFileException =
                    new FailedServerAIFileException(httpResponseException);

                throw new AIFileDependencyException(
                    message: "AI file dependency error occurred, contact support.", 
                    failedServerAIFileException);
            }
            catch (Exception exception)
            {
                var failedAIFileServiceException =
                    new FailedAIFileServiceException(exception);

                throw new AIFileServiceException(failedAIFileServiceException);
            }
        }
    }
}