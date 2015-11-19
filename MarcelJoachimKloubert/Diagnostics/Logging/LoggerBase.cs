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

using System;

namespace MarcelJoachimKloubert.Diagnostics.Logging
{
    /// <summary>
    /// A basic logger.
    /// </summary>
    public abstract partial class LoggerBase : ILogger
    {
        #region Fields (1)

        private readonly object _SYNC_ROOT;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBase" /> class.
        /// </summary>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        protected LoggerBase(object syncRoot = null)
        {
            _SYNC_ROOT = syncRoot ?? new object();
        }

        #endregion Constructors (1)

        #region Methods (2)

        /// <inheriteddoc />
        public bool Log(object msg,
                        LogCategory category = LogCategory.Info, LogPriority prio = LogPriority.None,
                        string tag = null)
        {
            try
            {
                var now = DateTimeOffset.Now;

                var logMsg = new LogMessage()
                    {
                        Category = category,
                        Message = DBNull.Value.Equals(msg) ? null : msg,
                        Priority = prio,
                        Tag = string.IsNullOrWhiteSpace(tag) ? null : tag.ToUpper().Trim(),
                        Time = now,
                    };

                var result = true;
                OnLog(logMsg, ref result);

                return result;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The logic for the <see cref="LoggerBase.Log(object, LogCategory, LogPriority, string)" /> method.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="success">
        /// The variable that defines the result for <see cref="LoggerBase.Log(object, LogCategory, LogPriority, string)" /> method.
        /// Is <see langword="true" /> by default.
        /// </param>
        protected abstract void OnLog(ILogMessage msg, ref bool success);

        #endregion Methods (2)

        #region Properties (1)

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public object SyncRoot
        {
            get { return _SYNC_ROOT; }
        }

        #endregion Properties (1)
    }
}