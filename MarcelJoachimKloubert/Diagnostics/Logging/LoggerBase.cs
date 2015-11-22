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
using System.Text;

namespace MarcelJoachimKloubert.Diagnostics.Logging
{
    /// <summary>
    /// A basic logger.
    /// </summary>
    public abstract partial class LoggerBase : ILogger
    {
        #region Fields (1)

        private readonly ICollection<MessageFilter> _FILTERS;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBase" /> class.
        /// </summary>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        protected LoggerBase(object syncRoot = null)
        {
            SyncRoot = syncRoot ?? new object();
            _FILTERS = CreateFilterStorage() ?? new List<MessageFilter>();
        }

        #endregion Constructors (1)

        #region Delegates (1)

        /// <summary>
        /// Describes a message filter.
        /// </summary>
        /// <param name="msg">The message to check.</param>
        /// <returns>Message is valid or not.</returns>
        public delegate bool MessageFilter(ILogMessage msg);

        #endregion Delegates (1)

        #region Methods (6)

        /// <summary>
        /// Adds a message filter.
        /// </summary>
        /// <param name="filter">The filter to add.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filter" /> is <see langword="null" />.
        /// </exception>
        public void AddFilter(MessageFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _FILTERS.Add(filter);
        }

        /// <summary>
        /// Checks a message.
        /// </summary>
        /// <param name="msg">The message to check.</param>
        /// <returns>Log the message or not.</returns>
        protected virtual bool CheckMessage(ILogMessage msg)
        {
#if !DEBUG
            if (msg.Category >= LogCategory.Debug)
            {
                return false;
            }

#endif
            using (var e = _FILTERS.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (!e.Current(msg))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Creates the collection for the message filters.
        /// </summary>
        /// <returns>The created storage.</returns>
        protected virtual ICollection<MessageFilter> CreateFilterStorage()
        {
            return null;
        }

        /// <summary>
        /// Creates a log message object.
        /// </summary>
        /// <param name="msg">The message value.</param>
        /// <param name="category">The category.</param>
        /// <param name="prio">The priority.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The created object.</returns>
        protected virtual ILogMessage CreateMessage(object msg,
                                                    LogCategory category, LogPriority prio,
                                                    string tag)
        {
            return new LogMessage()
	            {
	                Category = category,
	                Message = DBNull.Value.Equals(msg) ? null : msg,
	                Priority = prio,
	                Tag = string.IsNullOrWhiteSpace(tag) ? null : tag.ToUpper().Trim(),
	                Time = DateTimeOffset.Now,
	            };
        }

        /// <summary>
        /// Creates a common log string.
        /// </summary>
        /// <param name="msg">The message with the data.</param>
        /// <param name="str">The <see cref="StringBuilder" /> where to write the string to.</param>
        protected virtual void CreateString(ILogMessage msg, StringBuilder str)
        {
            var tag = msg.Tag;
            if (!string.IsNullOrWhiteSpace(tag))
            {
                tag = "[" + tag.Trim() + "] ";
            }

            var category = msg.Category.ToString();
            if (msg.Category < LogCategory.Notice)
            {
                category = category.ToUpper();
            }

            var prio = "";
            if (msg.Priority != LogPriority.None)
            {
                prio = msg.Priority.ToString();
                if (msg.Priority < LogPriority.Medium)
                {
                    prio = prio.ToUpper();
                }

                prio = " (" + prio + ")";
            }

            str.AppendFormat(@"[{0:yyyy-MM-dd HH:mm:ss.fffffff K}] {1}{2} :: {3}""{4}""", msg.Time
                                                                                        , category, prio
                                                                                        , tag
                                                                                        , msg.Message);
        }

        /// <inheriteddoc />
        public bool Log(object msg,
                        LogCategory category = LogCategory.Info, LogPriority prio = LogPriority.None,
                        string tag = null)
        {
            try
            {
                var logMsg = CreateMessage(msg: msg,
                                           category: category, prio: prio,
                                           tag: tag);

                var result = true;

                if (CheckMessage(logMsg))
                {
                    OnLog(logMsg, ref result);
                }

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

        #endregion Methods (6)

        #region Properties (2)

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public virtual object SyncRoot { get; }

        /// <summary>
        /// Gets or sets an object that should be linked with that instance.
        /// </summary>
        public virtual object Tag { get; set; }

        #endregion Properties (2)
    }
}