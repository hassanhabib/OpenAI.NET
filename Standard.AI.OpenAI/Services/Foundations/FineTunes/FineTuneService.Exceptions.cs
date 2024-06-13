﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using RESTFulSense.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.FineTunes;
using Standard.AI.OpenAI.Models.Services.Foundations.FineTunes.Exceptions;
using Xeptions;

namespace Standard.AI.OpenAI.Services.Foundations.FineTunes
{
    internal partial class FineTuneService
    {
        private delegate ValueTask<FineTune> ReturningFineTuneFunction();

        private static async ValueTask<FineTune> TryCatch(ReturningFineTuneFunction returningFineTuneFunction)
        {
            try
            {
                return await returningFineTuneFunction();
            }
            catch (NullFineTuneException nullFineTuneException)
            {
                throw CreateFineTuneValidationException(
                    nullFineTuneException);
            }
            catch (InvalidFineTuneException invalidFineTuneException)
            {
                throw CreateFineTuneValidationException(
                    invalidFineTuneException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var invalidFineTuneConfigurationException =
                    new InvalidFineTuneConfigurationException(
                        message: "Invalid fine tune configuration error ocurred, contact support.", 
                        httpResponseUrlNotFoundException);

                throw CreateFineTuneDependencyException(
                    invalidFineTuneConfigurationException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var unauthorizedFineTuneException =
                    new UnauthorizedFineTuneException(
                        message: "Unauthorized fine tune request, fix errors and try again.",
                        httpResponseUnauthorizedException);

                throw CreateFineTuneDependencyException(
                    unauthorizedFineTuneException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var unauthorizedFineTuneException =
                    new UnauthorizedFineTuneException(
                        message: "Unauthorized fine tune request, fix errors and try again.",
                        httpResponseForbiddenException);

                throw CreateFineTuneDependencyException(
                    unauthorizedFineTuneException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidFineTuneException =
                    new InvalidFineTuneException(
                        message: "Fine tune is invalid.",
                        httpResponseBadRequestException);

                throw CreateFineTuneDependencyValidationException(
                    invalidFineTuneException);
            }
            catch (HttpResponseTooManyRequestsException httpResponseTooManyRequestsException)
            {
                var excessiveCallFineTuneException =
                    new ExcessiveCallFineTuneException(
                        message: "Excessive call error occurred, limit your calls.", 
                        httpResponseTooManyRequestsException);

                throw CreateFineTuneDependencyValidationException(
                    excessiveCallFineTuneException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedServerFineTuneException =
                    new FailedServerFineTuneException(
                        message: "Failed fine tune server error occurred, contact support.", 
                        httpResponseException);

                throw CreateFineTuneDependencyException(
                    failedServerFineTuneException);
            }
            catch (Exception exception)
            {
                var failedFineTuneServiceException =
                    new FailedFineTuneServiceException(
                        message: "Failed fine tune error occurred, contact support.", 
                        exception);

                throw CreateFineTuneServiceException(
                    failedFineTuneServiceException);
            }
        }

        private static FineTuneValidationException CreateFineTuneValidationException(Xeption innerException)
        {
            return new FineTuneValidationException(
                message: "Fine tune validation error occurred, fix errors and try again.",
                innerException);
        }

        private static FineTuneDependencyException CreateFineTuneDependencyException(Xeption innerException)
        {
            return new FineTuneDependencyException(
                message: "Fine tune dependency error ocurred, contact support.",
                innerException);
        }

        private static FineTuneDependencyValidationException CreateFineTuneDependencyValidationException(
            Xeption innerException)
        {
            return new FineTuneDependencyValidationException(
                message: "Fine tune dependency validation error occurred, fix errors and try again",
                innerException);
        }

        private static FineTuneServiceException CreateFineTuneServiceException(Xeption innerException)
        {
            return new FineTuneServiceException(
                message: "Fine tune error ocurred, contact support.", 
                innerException);
        }
    }
}
