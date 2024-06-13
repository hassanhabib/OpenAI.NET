﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using Standard.AI.OpenAI.Models.Clients.FineTunes.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.FineTunes;
using Standard.AI.OpenAI.Models.Services.Foundations.FineTunes.Exceptions;
using Standard.AI.OpenAI.Services.Foundations.FineTunes;
using Xeptions;

namespace Standard.AI.OpenAI.Clients.FineTunes
{
    internal class FineTuneClient : IFineTuneClient
    {
        private readonly IFineTuneService fineTuneService;

        public FineTuneClient(IFineTuneService fineTuneService) =>
            this.fineTuneService = fineTuneService;

        public async ValueTask<FineTune> SubmitFineTuneAsync(FineTune fineTune)
        {
            try
            {
                return await this.fineTuneService.SubmitFineTuneAsync(fineTune);
            }
            catch (FineTuneValidationException fineTuneValidationException)
            {
                throw CreateFineTuneClientValidationException(
                    fineTuneValidationException.InnerException as Xeption);
            }
            catch (FineTuneDependencyException fineTuneDependencyException)
            {
                throw CreateFineTuneClientDependencyException(
                    fineTuneDependencyException.InnerException as Xeption);
            }
            catch (FineTuneDependencyValidationException fineTuneDependencyValidationException)
            {
                throw CreateFineTuneClientValidationException(
                    fineTuneDependencyValidationException.InnerException as Xeption);
            }
            catch (FineTuneServiceException fineTuneServiceException)
            {
                throw CreateFineTuneClientServiceException(
                    fineTuneServiceException.InnerException as Xeption);
            }
        }

        private static FineTuneClientValidationException CreateFineTuneClientValidationException(
            Xeption innerException)
        {
            return new FineTuneClientValidationException(
                message: "Fine tune client validation error occurred, fix errors and try again.",
                innerException);
        }

        private static FineTuneClientDependencyException CreateFineTuneClientDependencyException(
            Xeption innerException)
        {
            return new FineTuneClientDependencyException(
                message: "Fine tune client dependency error occurred, contact support.",
                innerException);
        }

        private static FineTuneClientServiceException CreateFineTuneClientServiceException(
            Xeption innerException)
        {
            return new FineTuneClientServiceException(
                message: "Fine tune client service error occurred, contact support.",
                innerException);
        }
    }
}
