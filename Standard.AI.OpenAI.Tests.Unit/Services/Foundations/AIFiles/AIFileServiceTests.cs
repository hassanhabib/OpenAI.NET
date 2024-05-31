﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using KellermanSoftware.CompareNetObjects;
using Moq;
using RESTFulSense.Exceptions;
using Standard.AI.OpenAI.Brokers.DateTimes;
using Standard.AI.OpenAI.Brokers.OpenAIs;
using Standard.AI.OpenAI.Models.Services.Foundations.AIFiles;
using Standard.AI.OpenAI.Models.Services.Foundations.AIFiles.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.ExternalAIFiles;
using Standard.AI.OpenAI.Services.Foundations.AIFiles;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace Standard.AI.OpenAI.Tests.Unit.Services.Foundations.AIFiles
{
    public partial class AIFileServiceTests
    {
        private readonly Mock<IOpenAIBroker> openAIBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly ICompareLogic compareLogic;
        private readonly IAIFileService aiFileService;

        public AIFileServiceTests()
        {
            this.openAIBrokerMock = new Mock<IOpenAIBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.compareLogic = new CompareLogic();

            this.aiFileService = new AIFileService(
                openAIBroker: this.openAIBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }

        public static TheoryData UnauthorizedExceptions()
        {
            return new TheoryData<HttpResponseException>
            {
                new HttpResponseUnauthorizedException(),
                new HttpResponseForbiddenException()
            };
        }

        private List<dynamic> CreateRandomFilesPropertiesList()
        {
            return Enumerable.Range(start: 0, count: GetRandomNumber())
                .Select(item =>
                {
                    return CreateRandomFileProperties();
                }).ToList();
        }

        public dynamic CreateRandomFileProperties()
        {
            int randomCreated = GetRandomDateNumber();
            DateTimeOffset randomCreatedDate = CreateRandomDateTimeOffset();

            dynamic randomFileProperties = CreateRandomFileProperties(
                created: randomCreated,
                createdDate: randomCreatedDate);

            return randomFileProperties;
        }

        public dynamic CreateRandomFileProperties(int created, DateTimeOffset createdDate)
        {
            Stream randomStream = CreateRandomStream();
            string randomFileName = CreateRandomString();
            string randomFileType = CreateRandomString();
            int randomBytesSize = GetRandomNumber();
            AIFileStatus randomAIFileStatus = GetRandomAIFileStatus();
            string randomExternalStatus = Enum.GetName(randomAIFileStatus);

            return new
            {
                ExternalFile = randomStream,
                Content = randomStream,
                FileName = randomFileName,
                Name = randomFileName,
                Type = randomFileType,
                Object = randomFileType,
                Purpose = CreateRandomString(),
                Id = CreateRandomString(),
                Size = randomBytesSize,
                Bytes = randomBytesSize,
                Created = created,
                CreatedDate = createdDate,
                Deleted = GetRandomBoolean(),
                ExternalStatus = randomExternalStatus,
                Status = randomAIFileStatus,
                StatusDetails = CreateRandomString()
            };
        }

        private Expression<Func<ExternalAIFileRequest, bool>> SameExternalAIFileRequestAs(
            ExternalAIFileRequest expectedExternalAIFileRequest)
        {
            return actualExternalAIFileRequest =>
                this.compareLogic.Compare(
                    expectedExternalAIFileRequest,
                    actualExternalAIFileRequest)
                        .AreEqual;
        }

        private static AIFileStatus GetRandomAIFileStatus() =>
            new RandomListItem<AIFileStatus>(Enum.GetValues<AIFileStatus>()).GetValue();

        private static bool GetRandomBoolean() =>
            Randomizer<bool>.Create();

        private static DateTimeOffset CreateRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string CreateRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomDateNumber() =>
            new Random((int)Stopwatch.GetTimestamp()).Next(int.MinValue, int.MaxValue);

        private Stream CreateRandomStream()
        {
            var mockStream = new Mock<MemoryStream>();

            mockStream.SetupGet(stream =>
                stream.ReadTimeout)
                    .Returns(0);

            mockStream.SetupGet(stream =>
                stream.WriteTimeout)
                    .Returns(0);

            return mockStream.Object;
        }

        private AIFile CreateRandomAIFile() =>
            CreateAIFileFiller().Create();

        private Filler<AIFile> CreateAIFileFiller()
        {
            var filler = new Filler<AIFile>();

            filler.Setup()
                .OnType<DateTimeOffset>().IgnoreIt()
                .OnType<Stream>().Use(CreateRandomStream);

            return filler;
        }

        private static AIFileDependencyException createAIFileDependencyException(Xeption innerException)
        {
            return new AIFileDependencyException(
                message: "AI file dependency error occurred, contact support.",
                innerException);
        }

        private static AIFileDependencyValidationException createAIFileDependencyValidationException(Xeption innerException)
        {
            return new AIFileDependencyValidationException(
                message: "AI file dependency validation error occurred, contact support.",
                innerException);
        }

        private static AIFileServiceException createAIFileServiceException(Xeption innerException)
        {
            return new AIFileServiceException(
                message: "AI file service error occurred, contact support.",
                innerException);
        }

        private static ExcessiveCallAIFileException createExcessiveCallAIFileException(Xeption innerException)
        {
            return new ExcessiveCallAIFileException(
                message: "Excessive call error occurred, limit your calls.",
                innerException);
        }

        private static FailedAIFileServiceException createFailedAIFileServiceException(Exception innerException)
        {
            return new FailedAIFileServiceException(
                message: "Failed AI file service error occurred, contact support.",
                innerException);
        }

        private static FailedServerAIFileException createFailedServerAIFileException(Exception innerException)
        {
            return new FailedServerAIFileException(
                message: "Failed AI file server error occurred, contact support.",
                innerException);
        }

        private static InvalidAIFileException createInvalidAIFileException(string message)
        {
            return new InvalidAIFileException(message);
        }

        private static InvalidAIFileException createInvalidAIFileException(string message, Exception innerException)
        {
            return new InvalidAIFileException(
                message,
                innerException);
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

        private static InvalidConfigurationAIFileException createInvalidConfigurationAIFileException(Exception innerException)
        {
            return new InvalidConfigurationAIFileException(
                message: "Invalid AI file configuration error occurred, contact support.",
                innerException);
        }

        private static UnauthorizedAIFileException createUnauthorizedAIFileException(Exception innerException)
        {
            return new UnauthorizedAIFileException(
                message: "Unauthorized AI file request, fix errors and try again.",
                innerException);
        }

    }
}
