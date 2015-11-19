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
    /// A logger that wraps another one.
    /// </summary>
    public class LoggerWrapper : LoggerBase
    {
        #region Fields (1)

        private readonly LoggerProvider _PROVIDER;

        #endregion Fields (1)

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerWrapper" /> class.
        /// </summary>
        /// <param name="baseLogger">The value for the <see cref="LoggerWrapper.BaseLogger" /> property.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseLogger" /> is <see langword="null" />.
        /// </exception>
        public LoggerWrapper(ILogger baseLogger, object syncRoot = null)
            : this(provider: CreateProvider(baseLogger),
                   syncRoot: syncRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerWrapper" /> class.
        /// </summary>
        /// <param name="provider">The function that provides the value for the <see cref="LoggerWrapper.BaseLogger" /> property.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        public LoggerWrapper(LoggerProvider provider, object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            _PROVIDER = provider;
        }

        #endregion Constructors (2)

        #region Delegates (1)

        /// <summary>
        /// A function that provides the value for an <see cref="LoggerWrapper.BaseLogger" /> property.
        /// </summary>
        /// <param name="logger">The parent logger.</param>
        /// <returns>The wrapped logger.</returns>
        public delegate ILogger LoggerProvider(LoggerWrapper logger);

        #endregion Delegates (1)

        #region Properties (1)

        /// <summary>
        /// Gets the wrapped logger.
        /// </summary>
        public ILogger BaseLogger
        {
            get { return _PROVIDER(this); }
        }

        #endregion Properties (1)

        #region Methods (2)

        private static LoggerProvider CreateProvider(ILogger baseLogger)
        {
            if (baseLogger == null)
            {
                throw new ArgumentNullException("baseLogger");
            }

            return (logger) => baseLogger;
        }

        /// <inheriteddoc />
        protected override void OnLog(ILogMessage msg, ref bool success)
        {
            success = BaseLogger.Log(msg: msg.Message,
                                     category: msg.Category, prio: msg.Priority,
                                     tag: msg.Tag);
        }

        #endregion Methods (2)
    }
}