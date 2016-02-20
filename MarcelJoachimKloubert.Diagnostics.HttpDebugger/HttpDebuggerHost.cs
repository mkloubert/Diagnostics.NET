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

using MarcelJoachimKloubert.Diagnostics.Debugging;
using MarcelJoachimKloubert.Diagnostics.Http.Contracts;
using MarcelJoachimKloubert.Diagnostics.Http.Messages;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace MarcelJoachimKloubert.Diagnostics.Http
{
    public class HttpDebuggerHost : DebuggerBase, IDisposable
    {
        #region Fields

        /// <summary>
        /// The default TCP port.
        /// </summary>
        public const int DEFAULT_PORT = 5979;

        protected HttpListener _listener;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDebuggerHost" /> class.
        /// </summary>
        /// <param name="port">The TCP port.</param>
        /// <param name="sync"></param>
        public HttpDebuggerHost(int port = DEFAULT_PORT,
                                object sync = null)
            : base(sync: sync)
        {
            Port = port;
        }

        #endregion Constructors

        #region Destructors

        ~HttpDebuggerHost()
        {
            Dispose(false);
        }

        #endregion Destructors

        #region Properties

        /// <summary>
        /// Gets if the host is running or not.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                var l = _listener;

                return l != null &&
                       l.IsListening;
            }
        }

        /// <summary>
        /// Gets the TCP port to use.
        /// </summary>
        public int Port { get; private set; }

        #endregion Properties

        #region Methods

        /// <inheriteddoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Starts the host.
        /// </summary>
        public void Start()
        {
            lock (_SYNC)
            {
                if (IsRunning)
                {
                    return;
                }

                DisposeOldHost();

                try
                {
                    _listener = new HttpListener();
                    _listener.Prefixes.Add("http://+:" + Port + "/");
                    _listener.Prefixes.Add("http://*:" + Port + "/");

                    _listener.Start();
                    BeginListening(_listener);
                }
                catch
                {
                    DisposeOldHost();
                    throw;
                }
            }
        }

        /// <summary>
        /// Stops the host.
        /// </summary>
        public void Stop()
        {
            lock (_SYNC)
            {
                if (!IsRunning)
                {
                    return;
                }

                DisposeOldHost();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_SYNC)
            {
                DisposeOldHost();
            }
        }

        private void BeginListening(HttpListener listener)
        {
            if (listener == null)
            {
                return;
            }

            try
            {
                listener.BeginGetContext(HttpListener_EndGetContext, listener);
            }
            catch
            {
                // ignore
            }
        }

        private bool DisposeOldHost()
        {
            try
            {
                using (_listener)
                {
                    _listener = null;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

        private void HttpListener_EndGetContext(IAsyncResult ar)
        {
            var now = DateTimeOffset.Now;
            HttpListener listener = null;

            try
            {
                listener = (HttpListener)ar.AsyncState;

                var ctx = listener.EndGetContext(ar);
                BeginListening(listener);

                JsonDebugMessage jsonMsg = null;
                try
                {
                    using (var reader = new StreamReader(ctx.Request.InputStream, Encoding.UTF8))
                    {
                        var json = reader.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(json))
                        {
                            jsonMsg = JsonConvert.DeserializeObject<JsonDebugMessage>(json);
                        }
                    }
                }
                catch
                {
                    // ignore
                }

                // send response
                try
                {
                    using (var writer = new StreamWriter(ctx.Response.OutputStream, Encoding.UTF8))
                    {
                        writer.Close();
                    }
                }
                catch
                {
                    // throw;
                }

                if (jsonMsg == null)
                {
                    return;
                }

                var dbgMsg = new HttpDebugMessage();
                dbgMsg.Id = GetValue(() => Guid.Parse(jsonMsg.id), Guid.NewGuid());
                dbgMsg.Time = jsonMsg.time ?? now;
                dbgMsg.Message = jsonMsg.msg;
                dbgMsg.Priority = jsonMsg.prio;
                dbgMsg.Category = jsonMsg.cat;
                dbgMsg.Tag = jsonMsg.tag;

                if (jsonMsg.thread != null)
                {
                    var dbgMsgThread = new HttpDebugMessageThread();

                    if (jsonMsg.thread.user != null)
                    {
                        var dbgMsgThreadUser = new HttpDebugMessageUser();
                        dbgMsgThreadUser.Name = jsonMsg.thread.user.name;

                        dbgMsgThread.User = dbgMsgThreadUser;
                    }

                    dbgMsg.Thread = dbgMsgThread;
                }

                RaiseDebugMessageReceived(dbgMsg);
            }
            catch
            {
                // ignore
            }
        }

        #endregion Methods
    }
}