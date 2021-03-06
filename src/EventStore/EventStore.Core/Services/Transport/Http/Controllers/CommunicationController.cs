﻿// Copyright (c) 2012, Event Store LLP
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
// 
// Redistributions of source code must retain the above copyright notice,
// this list of conditions and the following disclaimer.
// Redistributions in binary form must reproduce the above copyright
// notice, this list of conditions and the following disclaimer in the
// documentation and/or other materials provided with the distribution.
// Neither the name of the Event Store LLP nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
using EventStore.Common.Log;
using EventStore.Common.Utils;
using EventStore.Core.Bus;
using EventStore.Core.Messaging;
using EventStore.Transport.Http;
using EventStore.Transport.Http.Client;
using EventStore.Transport.Http.EntityManagement;

namespace EventStore.Core.Services.Transport.Http.Controllers
{
    public abstract class CommunicationController : IController
    {
        private static readonly ILogger Log = LogManager.GetLoggerFor<CommunicationController>();

        private readonly IPublisher _publisher;
        protected readonly HttpAsyncClient Client;

        protected CommunicationController(IPublisher publisher)
        {
            Ensure.NotNull(publisher, "publisher");

            _publisher = publisher;
            Client = new HttpAsyncClient();
        }

        public void Publish(Message message)
        {
            Ensure.NotNull(message, "message");
            _publisher.Publish(message);
        }

        public void Subscribe(IHttpService service, HttpMessagePipe pipe)
        {
            Ensure.NotNull(service, "service");
            Ensure.NotNull(pipe, "pipe");

            SubscribeCore(service, pipe);
        }

        protected abstract void SubscribeCore(IHttpService service, HttpMessagePipe pipe);

        protected void SendBadRequest(HttpEntity entity, string reason)
        {
            entity.Manager.ReplyStatus(HttpStatusCode.BadRequest,
                                       reason,
                                       e => Log.ErrorException(e, "Error while closing http connection (bad request)"));
        }

        protected void SendOk(HttpEntity entity)
        {
            entity.Manager.ReplyStatus(HttpStatusCode.OK,
                                       "OK",
                                       e => Log.ErrorException(e, "Error while closing http connection (ok)"));
        }
    }
}