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
    /// A logger that uses a delegate to log.
    /// </summary>
    public class DelegateLogger : LoggerBase
    {
        #region Fields (1)

        private readonly LogAction _ACTION;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBase" /> class.
        /// </summary>
        /// <param name="action">The action to use.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public DelegateLogger(LogAction action, object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            _ACTION = action;
        }

        #endregion Constructors (1)

        #region Delegates (1)

        /// <summary>
        /// Describes a log action.
        /// </summary>
        /// <param name="logger">The base logger.</param>
        /// <param name="msg">The log message.</param>
        /// <param name="success">The variable that stores if operation was successful or not.</param>
        public delegate void LogAction(DelegateLogger logger, ILogMessage msg, ref bool success);

        #endregion Delegates (1)

        #region Methods (2)

        /// <inheriteddoc />
        protected override void OnLog(ILogMessage msg, ref bool success)
        {
            _ACTION(this, msg, ref success);
        }

        /// <summary>
        /// Converts an action that only requires a <see cref="ILogMessage" /> argument
        /// to a <see cref="LogAction" /> instance.
        /// </summary>
        /// <param name="action">The input value.</param>
        /// <returns>The output value.</returns>
        /// <remarks>Returns <see langword="null" /> if <paramref name="action" /> is <see langword="null" />.</remarks>
        public static LogAction ToAction(Action<ILogMessage> action)
        {
            if (action == null)
            {
                return null;
            }

            return delegate(DelegateLogger logger, ILogMessage msg, ref bool success)
            {
                action(msg);
            };
        }

        #endregion Methods (1)
    }
}