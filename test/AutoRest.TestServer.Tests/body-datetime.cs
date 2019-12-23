﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using AutoRest.TestServer.Tests.Infrastructure;
using body_datetime;
using NUnit.Framework;

namespace AutoRest.TestServer.Tests
{
    public class BodyDateTimeTest: TestServerTestBase
    {
        public BodyDateTimeTest(TestServerVersion version) : base(version, "datetime") { }

        [Test]
        public Task GetDateTimeInvalid() => Test((host, pipeline) =>
        {
            Assert.ThrowsAsync<FormatException>(async () =>
                await DatetimeOperations.GetInvalidAsync(ClientDiagnostics, pipeline, host));
        });

        [Test]
        public Task GetDateTimeMinLocalNegativeOffset() => Test(async (host, pipeline) =>
        {
            var result = await DatetimeOperations.GetLocalNegativeOffsetMinDateTimeAsync(ClientDiagnostics, pipeline, host);
            Assert.AreEqual(DateTimeOffset.Parse("0001-01-01T00:00:00-14:00"), result.Value);
        });

        [Test]
        [Ignore("https://github.com/Azure/autorest.csharp/issues/346")]
        public Task GetDateTimeMinLocalPositiveOffset() => Test(async (host, pipeline) =>
        {
            var result = await DatetimeOperations.GetLocalPositiveOffsetMinDateTimeAsync(ClientDiagnostics, pipeline, host);
            Assert.AreEqual(DateTimeOffset.Parse("0001-01-01T00:00:00+14:00"), result.Value);
        });

        [Test]
        public Task GetDateTimeMinUtc() => Test(async (host, pipeline) =>
        {
            var result = await DatetimeOperations.GetUtcMinDateTimeAsync(ClientDiagnostics, pipeline, host);
            Assert.AreEqual(DateTimeOffset.Parse("0001-01-01T00:00:00Z"), result.Value);
        });

        [Test]
        [Ignore("https://github.com/Azure/autorest.csharp/issues/300")]
        public Task GetDateTimeNull() => Test(async (host, pipeline) =>
        {
            var result = await DatetimeOperations.GetNullAsync(ClientDiagnostics, pipeline, host);
            Assert.AreEqual(null, result.Value);
        });

        [Test]
        [Ignore("https://github.com/Azure/autorest.csharp/issues/346")]
        public Task GetDateTimeOverflow() => Test(async (host, pipeline) =>
        {
            var result = await DatetimeOperations.GetOverflowAsync(ClientDiagnostics, pipeline, host);
            Assert.AreEqual(DateTimeOffset.Parse("9999-12-31T23:59:59.9999999-14:00"), result.Value);
        });

        [Test]
        [Ignore("https://github.com/Azure/autorest.csharp/issues/346")]
        public Task GetDateTimeUnderflow() => Test(async (host, pipeline) =>
        {
            var result = await DatetimeOperations.GetUnderflowAsync(ClientDiagnostics, pipeline, host);
            Assert.AreEqual(DateTimeOffset.Parse("0000-00-00T00:00:00.0000000+00:00"), result.Value);
        });

        [Test]
        [Ignore("https://github.com/Azure/autorest.csharp/issues/346")]
        public Task PutDateTimeMaxLocalNegativeOffset() => TestStatus(async (host, pipeline) =>
        {
            var value = DateTimeOffset.Parse("9999-12-31T23:59:59.9999999-14:00");
            return await DatetimeOperations.PutLocalNegativeOffsetMaxDateTimeAsync(ClientDiagnostics, pipeline, value, host);
        });

        [Test]
        [IgnoreOnTestServer(TestServerVersion.V2, "Request not matched")]
        public Task PutDateTimeMaxLocalPositiveOffset() => TestStatus(async (host, pipeline) =>
        {
            var value = DateTimeOffset.Parse("9999-12-31T09:59:59.9999999Z");
            return await DatetimeOperations.PutLocalPositiveOffsetMaxDateTimeAsync(ClientDiagnostics, pipeline, value, host);
        });

        [Test]
        [IgnoreOnTestServer(TestServerVersion.V2, "Request not matched")]
        public Task PutDateTimeMaxUtc() => TestStatus(async (host, pipeline) =>
        {
            var value = DateTimeOffset.Parse("9999-12-31T23:59:59.9999999Z");
            return await DatetimeOperations.PutUtcMaxDateTimeAsync(ClientDiagnostics, pipeline, value, host);
        });

        [Test]
        [IgnoreOnTestServer(TestServerVersion.V2, "Request not matched")]
        public Task PutDateTimeMinLocalNegativeOffset() => TestStatus(async (host, pipeline) =>
        {
            var value = DateTimeOffset.Parse("0001-01-01T14:00:00Z");
            return await DatetimeOperations.PutLocalNegativeOffsetMinDateTimeAsync(ClientDiagnostics, pipeline, value, host);
        });

        [Test]
        [IgnoreOnTestServer(TestServerVersion.V2, "Request not matched")]
        public Task PutDateTimeMinLocalPositiveOffset() => TestStatus(async (host, pipeline) =>
        {
            var value = DateTimeOffset.Parse("0001-01-01T10:00:00Z");
            return await DatetimeOperations.PutLocalPositiveOffsetMinDateTimeAsync(ClientDiagnostics, pipeline, value, host);
        });

        [Test]
        [IgnoreOnTestServer(TestServerVersion.V2, "Request not matched")]
        public Task PutDateTimeMinUtc() => TestStatus(async (host, pipeline) =>
        {
            var value = DateTimeOffset.Parse("0001-01-01T00:00:00Z");
            return await DatetimeOperations.PutUtcMinDateTimeAsync(ClientDiagnostics, pipeline, value, host);
        });
    }
}