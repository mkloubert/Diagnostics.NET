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

using MarcelJoachimKloubert.Diagnostics.Logging;
using System;

namespace MarcelJoachimKloubert.Diagnostics
{
    /// <summary>
    /// Provides global access to a logger.
    /// </summary>
    public static class Logger
    {
        #region Fields (1)

        private static LoggerProvider _provider;

        #endregion Fields (1)

        #region Delegates (1)

        /// <summary>
        /// Describes a function that provides a <see cref="ILogger" /> instance.
        /// </summary>
        /// <returns>The logger instance.</returns>
        public delegate ILogger LoggerProvider();

        #endregion Delegates (1)

        #region Properties (1)

        /// <summary>
        /// Gets the current logger.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// No provider defined.
        /// </exception>
        public static ILogger Current
        {
            get { return _provider(); }
        }

        #endregion Properties (1)

        #region Methods (3)

        /// <summary>
        /// Writes a log entry via the logger from <see cref="Logger.Current" />.
        /// </summary>
        /// <param name="msg">The message to write.</param>
        /// <param name="category">The category.</param>
        /// <param name="prio">The priority.</param>
        /// <param name="tag">The optional tag.</param>
        /// <returns>Message was written or not.</returns>
        public static bool Log(object msg,
                               LogCategory category = LogCategory.Info, LogPriority prio = LogPriority.None,
                               string tag = null)
        {
            return Current.Log(msg: msg,
                               category: category, prio: prio,
                               tag: tag);
        }

        /// <summary>
        /// Sets the value for <see cref="Logger.Current" /> property.
        /// </summary>
        /// <param name="logger">The new value.</param>
        public static void SetLogger(ILogger logger)
        {
            SetProvider(logger != null ? new LoggerProvider(() => logger)
                                       : null);
        }

        /// <summary>
        /// Sets the function that provides the value for <see cref="Logger.Current" /> property.
        /// </summary>
        /// <param name="provider">The new provider.</param>
        public static void SetProvider(LoggerProvider provider)
        {
            _provider = provider;
        }

        #endregion Methods (3)
    }
}