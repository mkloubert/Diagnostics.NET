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
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Diagnostics.Logging
{
    /// <summary>
    /// An async logger.
    /// </summary>
    public class AsyncLogger : LoggerWrapper
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLogger" /> class.
        /// </summary>
        /// <param name="baseLogger">The value for the <see cref="LoggerWrapper.BaseLogger" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseLogger" /> is <see langword="null" />.
        /// </exception>
        public AsyncLogger(ILogger baseLogger)
            : base(baseLogger: baseLogger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLogger" /> class.
        /// </summary>
        /// <param name="provider">The function that provides the value for the <see cref="LoggerWrapper.BaseLogger" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        public AsyncLogger(LoggerProvider provider)
            : base(provider: provider)
        {
        }

        #endregion Constructors (2)

        #region Methods (4)

        /// <summary>
        /// Async operation of <see cref="LoggerBase.Log(object, LogCategory, LogPriority, string)" /> method.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="prio">The priority.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The started task.</returns>
        public async Task<bool> LogAsync(object msg,
                                         LogCategory category = LogCategory.Info, LogPriority prio = LogPriority.None,
                                         string tag = null)
        {
            var logMsg = CreateMessage(msg: msg,
                                       category: category, prio: prio,
                                       tag: tag);

            return await LogAsync(logMsg: logMsg);
        }

        private Task<bool> LogAsync(ILogMessage logMsg)
        {
            return Task.Factory.StartNew(function: (state) =>
                {
                    var args = (object[])state;

                    return ((AsyncLogger)args[0]).OnLogTask(logMsg: (ILogMessage)args[1]);
                }, state: new object[] { this, logMsg });
        }

        /// <inheriteddoc />
        protected sealed override void OnLog(ILogMessage msg, ref bool success)
        {
            LogAsync(logMsg: msg);
        }

        private bool OnLogTask(ILogMessage logMsg)
        {
            bool result;
            try
            {
                result = true;
                base.OnLog(logMsg, ref result);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        #endregion Methods (4)
    }
}