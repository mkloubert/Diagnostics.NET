/**********************************************************************************************************************
 * Diagnostics.NET (https://github.com/mkloubert/Diagnostics.NET)                                                     *
 *                                                                                                                    *
 * Copyright (c) 2015, Marcel Joachim Kloubert <marcel.kloubert@gmx.net>                                              *
 * All rights reserved.                                                                                               *
 *                                                                                                                    *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the   *
 * following conditions are met:                                                                                      *
 *                                                                                                                    *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the          *
 *    following disclaimer.                                                                                           *
 *                                                                                                                    *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the       *
 *    following disclaimer in the documentation and/or other materials provided with the distribution.                *
 *                                                                                                                    *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote    *
 *    products derived from this software without specific prior written permission.                                  *
 *                                                                                                                    *
 *                                                                                                                    *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, *
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  *
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, *
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR    *
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,  *
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE   *
 * USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                                           *
 *                                                                                                                    *
 **********************************************************************************************************************/

using MarcelJoachimKloubert.Diagnostics.Http.Contracts;
using MarcelJoachimKloubert.Diagnostics.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Diagnostics.Http.Logging
{
    /// <summary>
    /// A HTTP logger.
    /// </summary>
    public class HttpLogger : LoggerBase
    {
        #region Fields

        /// <summary>
        /// Stores the target hosts.
        /// </summary>
        protected readonly ICollection<Uri> _HOSTS = new HashSet<Uri>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpLogger" /> class.
        /// </summary>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        public HttpLogger(object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds a host.
        /// </summary>
        /// <param name="addr">The address of the host.</param>
        /// <param name="port">The TCP port where the host is listening.</param>
        /// <returns>That instance.</returns>
        public HttpLogger AddHost(string addr, int port = HttpDebuggerHost.DEFAULT_PORT)
        {
            lock (SyncRoot)
            {
                if (string.IsNullOrWhiteSpace(addr))
                {
                    addr = "localhost";
                }

                _HOSTS.Add(new Uri(string.Format("http://{0}:{1}/", addr.Trim(), port)));
                return this;
            }
        }

        /// <inheriteddoc />
        protected override void OnLog(ILogMessage msg, ref bool success)
        {
            lock (SyncRoot)
            {
                var msgId = Guid.NewGuid();

                dynamic thread = null;
                try
                {
                    var curThread = Thread.CurrentThread;

                    thread = new ExpandoObject();
                    thread.id = GetValue(() => curThread.ManagedThreadId);

                    thread.user = null;

                    try
                    {
                        var curUser = Thread.CurrentPrincipal;
                        if (curUser != null)
                        {
                            thread.user = new ExpandoObject();

                            var curUserId = curUser.Identity;
                            if (curUserId != null)
                            {
                                thread.user.name = curUserId.Name;
                            }
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }
                catch
                {
                    // ignore
                }

                foreach (var host in _HOSTS)
                {
                    try
                    {
                        Task.Factory
                            .StartNew(SendDebugMessage,
                                      state: new object[] { msgId, host, msg, thread });
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }

        /// <summary>
        /// Prepares a message value for use as JSON data.
        /// </summary>
        /// <param name="msg">The input value.</param>
        /// <returns>The output value.</returns>
        protected virtual object PreparseMessage(object msg)
        {
            return msg == null ? null : msg.ToString();
        }

        private T GetValue<T>(Func<T> valueProvider, T defaultVal = default(T))
        {
            try
            {
                return valueProvider();
            }
            catch
            {
                return defaultVal;
            }
        }

        private void SendDebugMessage(object state)
        {
            try
            {
                var taskArgs = (IList<object>)state;

                var msgId = (Guid)taskArgs[0];
                var host = (Uri)taskArgs[1];
                var msg = (ILogMessage)taskArgs[2];
                dynamic thread = taskArgs[3];

                var httpReq = WebRequest.Create(host);
                httpReq.Method = "POST";

                var jsonMsg = new JsonDebugMessage()
                    {
                        id = msgId.ToString("N"),
                        cat = (int)msg.Category,
                        msg = PreparseMessage(msg.Message),
                        prio = (int)msg.Priority,
                        tag = msg.Tag,
                        time = msg.Time,
                    };

                if (thread != null)
                {
                    var jsonThread = new JsonDebugMessageThread();
                    jsonThread.id = GetValue(() => thread.id.ToString());

                    var jsonThreadUser = new JsonDebugMessageUser();
                    jsonThread.user = jsonThreadUser;
                    jsonThread.user.name = GetValue(() => thread.user.name.ToString());

                    jsonMsg.thread = jsonThread;
                }

                var json = JsonConvert.SerializeObject(jsonMsg);
                using (var reqStream = httpReq.GetRequestStream())
                {
                    using (var writer = new StreamWriter(reqStream, Encoding.UTF8))
                    {
                        writer.Write(json);
                        writer.Flush();
                    }
                }

                try
                {
                    var httpResp = httpReq.GetResponse();
                    using (var respStream = httpResp.GetResponseStream())
                    {
                        if (respStream != null)
                        {
                            using (var reader = new StreamReader(respStream, Encoding.UTF8))
                            {
                                reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    // ignore
                }
            }
            catch
            {
                // ignore
            }
        }

        #endregion Methods
    }
}