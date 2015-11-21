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

        #region Methods (2)

        /// <inheriteddoc />
        protected sealed override void OnLog(ILogMessage msg, ref bool success)
        {
            Task.Factory.StartNew(OnLogTask,
                                  state: msg);
        }

        private void OnLogTask(object state)
        {
            try
            {
                var success = true;
                base.OnLog((ILogMessage)state, ref success);
            }
            catch
            {
                // ignore
            }
        }

        #endregion Methods (2)
    }
}