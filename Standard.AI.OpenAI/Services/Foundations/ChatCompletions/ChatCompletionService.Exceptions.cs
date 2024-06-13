﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using RESTFulSense.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions.Exceptions;
using Xeptions;

namespace Standard.AI.OpenAI.Services.Foundations.ChatCompletions
{
    internal partial class ChatCompletionService
    {
        private delegate ValueTask<ChatCompletion> ReturningChatCompletionFunction();

        private async ValueTask<ChatCompletion> TryCatch(
            ReturningChatCompletionFunction returningChatCompletionFunction)
        {
            try
            {
                return await returningChatCompletionFunction();
            }
            catch (NullChatCompletionException nullChatCompletionException)
            {
                throw CreateChatCompletionValidationException(
                    nullChatCompletionException);
            }
            catch (InvalidChatCompletionException invalidChatCompletionException)
            {
                throw CreateChatCompletionValidationException( 
                    invalidChatCompletionException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var invalidConfigurationChatCompletionException =
                    new InvalidConfigurationChatCompletionException(
                        message: "Invalid chat completion configuration error occurred, contact support.", 
                        httpResponseUrlNotFoundException);

                throw CreateChatCompletionDependencyException(
                    invalidConfigurationChatCompletionException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var unauthorizedChatCompletionException =
                    new UnauthorizedChatCompletionException(
                        message: "Unauthorized chat completion request, fix errors and try again.", 
                        httpResponseUnauthorizedException);

                throw CreateChatCompletionDependencyException(
                    unauthorizedChatCompletionException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var unauthorizedChatCompletionException =
                    new UnauthorizedChatCompletionException(
                        message: "Unauthorized chat completion request, fix errors and try again.", 
                        httpResponseForbiddenException);

                throw CreateChatCompletionDependencyException(
                    unauthorizedChatCompletionException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var notFoundChatCompletionException =
                    new NotFoundChatCompletionException(
                        message: "Chat completion not found.", 
                        httpResponseNotFoundException);

                throw CreateChatCompletionDependencyValidationException(
                    notFoundChatCompletionException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidChatCompletionException =
                    new InvalidChatCompletionException(
                        message: "Chat completion is invalid.", 
                        httpResponseBadRequestException);

                throw CreateChatCompletionDependencyValidationException(
                    invalidChatCompletionException);
            }
            catch (HttpResponseTooManyRequestsException httpResponseTooManyRequestsException)
            {
                var excessiveCallChatCompletionException =
                    new ExcessiveCallChatCompletionException(
                        message: "Excessive call error occurred, limit your calls.", 
                        httpResponseTooManyRequestsException);

                throw CreateChatCompletionDependencyValidationException(
                    excessiveCallChatCompletionException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedServerChatCompletionException =
                    new FailedServerChatCompletionException(
                        message: "Failed chat completion server error occurred, contact support.", 
                        httpResponseException);

                throw CreateChatCompletionDependencyException(
                    failedServerChatCompletionException);
            }
            catch (Exception exception)
            {
                var failedChatCompletionServiceException =
                    new FailedChatCompletionServiceException(
                        message: "Failed Chat Completion Service Exception occurred, " +
                        "please contact support for assistance.", 
                        exception);

                throw CreateChatCompletionServiceException(
                    failedChatCompletionServiceException);
            }
        }

        private static ChatCompletionValidationException CreateChatCompletionValidationException(
            Xeption innerException)
        {
            return new ChatCompletionValidationException(
                message: "Chat completion validation error occurred, fix errors and try again.",
                innerException);
        }

        private static ChatCompletionDependencyException CreateChatCompletionDependencyException(
            Xeption innerException)
        {
            return new ChatCompletionDependencyException(
                message: "Chat completion dependency error occurred, contact support.",
                innerException);
        }

        private static ChatCompletionDependencyValidationException CreateChatCompletionDependencyValidationException(
            Xeption innerException)
        {
            return new ChatCompletionDependencyValidationException(
                "Chat completion dependency validation error occurred, fix errors and try again.",
                innerException);
        }

        private static ChatCompletionServiceException CreateChatCompletionServiceException(
            Xeption innerException)
        {
            return new ChatCompletionServiceException(
                message: "Chat completion service error occurred, contact support.",
                innerException);
        }

        private static NullChatCompletionException CreateNullChatCompletionException()
        {
            return new NullChatCompletionException(
                message: "Chat completion is null.");
        }
    }
}