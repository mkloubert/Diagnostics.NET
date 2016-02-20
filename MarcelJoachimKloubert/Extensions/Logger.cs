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
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Extensions
{
    static partial class MJKDiagnosticExtensionMethods
    {
        #region Methods

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Alert" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Alert(this ILogger logger,
                                 object msg, string tag = null,
                                 LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Alert, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Alert" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> AlertAsync(this ILogger logger,
                                            object msg, string tag = null,
                                            LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Alert, prio: prio,
                            tag: tag);
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Critical" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Crit(this ILogger logger,
                                object msg, string tag = null,
                                LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Critical, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Critical" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> CritAsync(this ILogger logger,
                                           object msg, string tag = null,
                                           LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Critical, prio: prio,
                            tag: tag);
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Debug" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Debug(this ILogger logger,
                                 object msg, string tag = null,
                                 LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Debug, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Debug" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> DebugAsync(this ILogger logger,
                                            object msg, string tag = null,
                                            LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Debug, prio: prio,
                            tag: tag);
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Emergency" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Emerg(this ILogger logger,
                                 object msg, string tag = null,
                                 LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Emergency, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Emergency" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> EmergAsync(this ILogger logger,
                                            object msg, string tag = null,
                                            LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Emergency, prio: prio,
                            tag: tag);
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Error" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Err(this ILogger logger,
                               object msg, string tag = null,
                               LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Error, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Error" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> ErrAsync(this ILogger logger,
                                          object msg, string tag = null,
                                          LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Error, prio: prio,
                            tag: tag);
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Info" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Info(this ILogger logger,
                                object msg, string tag = null,
                                LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Info, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Info" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> InfoAsync(this ILogger logger,
                                           object msg, string tag = null,
                                           LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Info, prio: prio,
                            tag: tag);
        }

        /// <summary>
        /// Logs a message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="category">The category.</param>
        /// <param name="prio">The priority.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The running task.</returns>
        public static Task<bool> LogAsync(this ILogger logger,
                                          object msg,
                                          LogCategory category = LogCategory.Info, LogPriority prio = LogPriority.None,
                                          string tag = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            var asyncLogger = logger as AsyncLogger;
            if (asyncLogger != null)
            {
                return asyncLogger.LogAsync(msg: msg,
                                            category: category, prio: prio,
                                            tag: tag);
            }

            return Task.Factory
                .StartNew((state) =>
                     {
                         try
                         {
                             var taskArgs = (object[])state;

                             return ((ILogger)taskArgs[0])
                                 .Log(msg: taskArgs[1],
                                      category: (LogCategory)taskArgs[2], prio: (LogPriority)taskArgs[3],
                                      tag: (string)taskArgs[4]);
                         }
                         catch (Exception)
                         {
                             return false;
                         }
                     }, state: new object[] { logger, msg, category, prio, tag });
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Notice" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Note(this ILogger logger,
                                 object msg, string tag = null,
                                 LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Notice, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Notice" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> NoteAsync(this ILogger logger,
                                           object msg, string tag = null,
                                           LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Notice, prio: prio,
                            tag: tag);
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Trace" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Trace(this ILogger logger,
                                 object msg, string tag = null,
                                 LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Trace, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Trace" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> TraceAsync(this ILogger logger,
                                            object msg, string tag = null,
                                            LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Trace, prio: prio,
                            tag: tag);
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Warning" /> message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>Operation was successful or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static bool Warn(this ILogger logger,
                                object msg, string tag = null,
                                LogPriority prio = LogPriority.None)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            try
            {
                return logger.Log(msg: msg,
                                  category: LogCategory.Warning, prio: prio,
                                  tag: tag);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value as <see cref="LogCategory.Warning" /> message async.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">The message value.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="prio">The priority.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> is <see langword="null" />.
        /// </exception>
        public static Task<bool> WarnAsync(this ILogger logger,
                                           object msg, string tag = null,
                                           LogPriority prio = LogPriority.None)
        {
            return LogAsync(logger: logger,
                            msg: msg,
                            category: LogCategory.Warning, prio: prio,
                            tag: tag);
        }

        #endregion Methods
    }
}