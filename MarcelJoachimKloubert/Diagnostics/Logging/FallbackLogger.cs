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
    /// A logger that uses a fallback logger is logging via base logger failes.
    /// </summary>
    public class FallbackLogger : LoggerWrapper
    {
        #region Fields (1)

        private readonly LoggerProvider _FALLBACK_PROVIDER;

        #endregion Fields (1)

        #region Constructors (4)

        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackLogger" /> class.
        /// </summary>
        /// <param name="baseLogger">The main / base logger.</param>
        /// <param name="fallbackLogger">The fallback logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseLogger" /> and/or <paramref name="fallbackLogger" /> is <see langword="null" />.
        /// </exception>
        public FallbackLogger(ILogger baseLogger, ILogger fallbackLogger)
            : this(baseLogger: baseLogger,
                   fallbackProvider: CreateProvider(fallbackLogger))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackLogger" /> class.
        /// </summary>
        /// <param name="baseLogger">The main / base logger.</param>
        /// <param name="fallbackProvider">The function that provides the fallback logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseLogger" /> and/or <paramref name="fallbackProvider" /> is <see langword="null" />.
        /// </exception>
        public FallbackLogger(ILogger baseLogger, LoggerProvider fallbackProvider)
            : this(baseProvider: CreateProvider(baseLogger),
                   fallbackProvider: fallbackProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackLogger" /> class.
        /// </summary>
        /// <param name="baseProvider">The function that provides the main / base logger.</param>
        /// <param name="fallbackLogger">The fallback logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseProvider" /> and/or <paramref name="fallbackLogger" /> is <see langword="null" />.
        /// </exception>
        public FallbackLogger(LoggerProvider baseProvider, ILogger fallbackLogger)
            : this(baseProvider: baseProvider,
                   fallbackProvider: CreateProvider(fallbackLogger))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FallbackLogger" /> class.
        /// </summary>
        /// <param name="baseProvider">The function that provides the main / base logger.</param>
        /// <param name="fallbackProvider">The function that provides the fallback logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseProvider" /> and/or <paramref name="fallbackProvider" /> is <see langword="null" />.
        /// </exception>
        public FallbackLogger(LoggerProvider baseProvider, LoggerProvider fallbackProvider)
            : base(provider: baseProvider)
        {
            if (fallbackProvider == null)
            {
                throw new ArgumentNullException("fallbackProvider");
            }

            _FALLBACK_PROVIDER = fallbackProvider;
        }

        #endregion Constructors (4)

        #region Properties (1)

        /// <summary>
        /// Gets the fallback logger.
        /// </summary>
        public ILogger Fallback
        {
            get { return _FALLBACK_PROVIDER(this); }
        }

        #endregion Properties (1)

        #region Methods (1)

        /// <inheriteddoc />
        protected override void OnLog(ILogMessage msg, ref bool success)
        {
            try
            {
                base.OnLog(msg, ref success);
            }
            catch
            {
                success = false;
            }

            if (!success)
            {
                var fb = Fallback;
                if (fb != null)
                {
                    success = fb.Log(msg: msg.Message,
                                     category: msg.Category, prio: msg.Priority,
                                     tag: msg.Tag);
                }
            }
        }

        #endregion Methods (1)
    }
}