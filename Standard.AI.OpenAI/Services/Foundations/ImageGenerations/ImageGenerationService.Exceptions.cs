﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using RESTFulSense.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.ImageGenerations;
using Standard.AI.OpenAI.Models.Services.Foundations.ImageGenerations.Exceptions;
using Xeptions;

namespace Standard.AI.OpenAI.Services.Foundations.ImageGenerations
{
    internal partial class ImageGenerationService
    {
        private delegate ValueTask<ImageGeneration> ReturningImageGenerationFunction();

        private async ValueTask<ImageGeneration> TryCatch(ReturningImageGenerationFunction returningImageGenerationFunction)
        {
            try
            {
                return await returningImageGenerationFunction();
            }
            catch (NullImageGenerationException nullImageGenerationException)
            {
                throw CreateImageGenerationValidationException(
                    nullImageGenerationException);
            }
            catch (InvalidImageGenerationException invalidImageGenerationException)
            {
                throw CreateImageGenerationValidationException(
                    invalidImageGenerationException);
            }
            catch (HttpResponseUrlNotFoundException httpResponseUrlNotFoundException)
            {
                var invalidConfigurationImageGenerationException =
                    new InvalidConfigurationImageGenerationException(
                        message: "Invalid image generation configuration error occurred, contact support.", 
                        httpResponseUrlNotFoundException);

                throw CreateImageGenerationDependencyException(
                    invalidConfigurationImageGenerationException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizedException)
            {
                var unauthorizedImageGenerationException =
                    new UnauthorizedImageGenerationException(
                        message: "Unauthorized image generation request, fix errors and try again.", 
                        httpResponseUnauthorizedException);

                throw CreateImageGenerationDependencyException(
                    unauthorizedImageGenerationException);
            }
            catch (HttpResponseForbiddenException httpResponseForbiddenException)
            {
                var unauthorizedImageGenerationException =
                    new UnauthorizedImageGenerationException(
                        message: "Unauthorized image generation request, fix errors and try again.", 
                        httpResponseForbiddenException);

                throw CreateImageGenerationDependencyException(
                    unauthorizedImageGenerationException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var notFoundImageGenerationException =
                    new NotFoundImageGenerationException(
                        message: "Not found image generation error occurred, fix errors and try again.", 
                        httpResponseNotFoundException);

                throw CreateImageGenerationDependencyValidationException(
                    notFoundImageGenerationException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidImageGenerationException =
                    new InvalidImageGenerationException(
                        message: "Invalid image generation error occurred, fix errors and try again.", 
                        httpResponseBadRequestException);

                throw CreateImageGenerationDependencyValidationException(
                    invalidImageGenerationException);
            }
            catch (HttpResponseTooManyRequestsException httpResponseTooManyRequestsException)
            {
                var excessiveCallImageGenerationException =
                    new ExcessiveCallImageGenerationException(
                        message: "Excessive call error occurred, limit your calls.",
                        httpResponseTooManyRequestsException);

                throw CreateImageGenerationDependencyValidationException(
                    excessiveCallImageGenerationException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedServerImageGenerationException =
                    new FailedServerImageGenerationException(
                        message: "Failed image generation server error occurred, contact support.", 
                        httpResponseException);

                throw CreateImageGenerationDependencyException(
                    failedServerImageGenerationException);
            }
            catch (Exception exception)
            {
                var failedImageGenerationServiceException =
                    new FailedImageGenerationServiceException(
                        message: "Failed image generation service error occurred, contact support.", 
                        exception);

                throw CreateImageGenerationServiceException(failedImageGenerationServiceException);
            }
        }

        private static ImageGenerationValidationException CreateImageGenerationValidationException(Xeption innerException)
        {
            return new ImageGenerationValidationException(
                message: "Image generation validation error occurred, fix errors and try again.",
                innerException);
        }

        private static ImageGenerationDependencyException CreateImageGenerationDependencyException(Xeption innerException)
        {
            return new ImageGenerationDependencyException(
                message: "Image generation dependency error occurred, contact support.",
                innerException);
        }

        private static ImageGenerationDependencyValidationException CreateImageGenerationDependencyValidationException(Xeption innerException)
        {
            return new ImageGenerationDependencyValidationException(
                message: "Image generation dependency validation error occurred, fix errors and try again.",
                innerException);
        }

        private static ImageGenerationServiceException CreateImageGenerationServiceException(Exception innerException)
        {
            return new ImageGenerationServiceException(
                message: "Image generation service error occurred, contact support.", 
                innerException);
        }
    }
}