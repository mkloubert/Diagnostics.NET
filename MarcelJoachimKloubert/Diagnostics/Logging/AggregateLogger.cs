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
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Diagnostics.Logging
{
    /// <summary>
    /// A logger that writes to a list of loggers.
    /// </summary>
    public class AggregateLogger : LoggerBase
    {
        #region Fields (1)

        /// <summary>
        /// Stores the underlying loggers.
        /// </summary>
        protected readonly ICollection<ILogger> _LOGGERS;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateLogger" /> class.
        /// </summary>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        public AggregateLogger(object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            _LOGGERS = CreateLoggerStorage() ?? new List<ILogger>();
        }

        #endregion Constructors (1)

        #region Methods (3)

        /// <summary>
        /// Adds a new logger.
        /// </summary>
        /// <param name="logger">The logger to add.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public void AddLogger(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _LOGGERS.Add(logger);
        }

        /// <summary>
        /// Creates the storage for the underlying loggers.
        /// </summary>
        /// <returns>The new storage.</returns>
        protected virtual ICollection<ILogger> CreateLoggerStorage()
        {
            return null;  // default
        }

        /// <inheriteddoc />
        protected override void OnLog(ILogMessage msg, ref bool success)
        {
            bool? allFailed = null;

            using (var e = _LOGGERS.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    bool failed;
                    try
                    {
                        var logger = e.Current;

                        failed = !logger.Log(msg: msg.Message,
                                             category: msg.Category, prio: msg.Priority,
                                             tag: msg.Tag);
                    }
                    catch
                    {
                        failed = true;
                    }

                    if (failed && !allFailed.HasValue)
                    {
                        allFailed = true;
                    }
                    else if (!failed)
                    {
                        allFailed = false;
                    }
                }
            }

            if (allFailed.HasValue)
            {
                success = !allFailed.Value;
            }
        }

        #endregion Methods (3)
    }
}