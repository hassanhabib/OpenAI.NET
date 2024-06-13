﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using RESTFulSense.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.Completions;
using Standard.AI.OpenAI.Models.Services.Foundations.Completions.Exceptions;
using Xeptions;

namespace Standard.AI.OpenAI.Services.Foundations.Completions
{
    internal partial class CompletionService
    {
        private delegate ValueTask<Completion> ReturningCompletionFunction();

        private static async ValueTask<Completion> TryCatch(ReturningCompletionFunction returningCompletionFunction)
        {
            try
            {
                return await returningCompletionFunction();
            }
            catch (NullCompletionException nullCompletionException)
            {
                throw CreateCompletionValidationException(
                    nullCompletionException);
            }
            catch (InvalidCompletionException invalidCompletionException)
            {
                throw CreateCompletionValidationException(
                    invalidCompletionException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var invalidConfigurationCompletionException =
                    new InvalidConfigurationCompletionException(
                        message: "Invalid configuration error occurred, fix errors and try again.", 
                        httpResponseUrlNotFoundException);

                throw CreateCompletionDependencyException(
                    invalidConfigurationCompletionException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var unauthorizedCompletionException =
                    new UnauthorizedCompletionException(
                        message: "Unauthorized completion request, fix errors and try again.", 
                        httpResponseUnauthorizedException);

                throw CreateCompletionDependencyException(
                    unauthorizedCompletionException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var unauthorizedCompletionException =
                    new UnauthorizedCompletionException(
                        message: "Unauthorized completion request, fix errors and try again.", 
                        httpResponseForbiddenException);

                throw CreateCompletionDependencyException(
                    unauthorizedCompletionException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var notFoundCompletionException =
                    new NotFoundCompletionException(
                        message: "Not found completion error occurred, fix errors and try again.", 
                        httpResponseNotFoundException);

                throw CreateCompletionDependencyValidationException(
                    notFoundCompletionException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidCompletionException =
                    new InvalidCompletionException(
                        message: "Invalid completion error occurred, fix errors and try again.", 
                        httpResponseBadRequestException);

                throw CreateCompletionDependencyValidationException( 
                    invalidCompletionException);
            }

            catch (HttpResponseTooManyRequestsException httpResponseTooManyRequestsException)
            {
                var excessiveCallCompletionException =
                    new ExcessiveCallCompletionException(
                        message: "Excessive call error occurred, limit your calls.", 
                        httpResponseTooManyRequestsException);

                throw CreateCompletionDependencyValidationException(
                    excessiveCallCompletionException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedServerCompletionException =
                    new FailedServerCompletionException(
                        message: "Failed server completion error occurred, contact support.", 
                        httpResponseException);

                throw CreateCompletionDependencyException(
                    failedServerCompletionException);
            }
            catch (Exception exception)
            {
                var failedCompletionServiceException =
                    new FailedCompletionServiceException(
                        message: "Failed completion error occurred, contact support.", 
                        exception);

                throw CreateCompletionServiceException(
                    failedCompletionServiceException);
            }
        }

        private static CompletionValidationException CreateCompletionValidationException(Xeption innerException)
        {
            return new CompletionValidationException(
                message: "Completion validation error occurred, fix errors and try again.",
                innerException);
        }

        private static CompletionDependencyException CreateCompletionDependencyException(Xeption innerException)
        {
            return new CompletionDependencyException(
                message: "Completion dependency error occurred, contact support.",
                innerException);
        }
        
        private static CompletionDependencyValidationException CreateCompletionDependencyValidationException(
            Xeption innerException)
        {
            return new CompletionDependencyValidationException(
                message: "Completion dependency validation error occurred, fix errors and try again.", 
                innerException);
        }

        private static CompletionServiceException CreateCompletionServiceException(Xeption innerException)
        {
            return new CompletionServiceException(
                message: "Completion service error occurred, contact support.", 
                innerException);
        }
    }
}
