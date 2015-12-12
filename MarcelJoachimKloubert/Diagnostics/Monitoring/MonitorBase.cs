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
using System.Globalization;
using System.Text;

namespace MarcelJoachimKloubert.Diagnostics.Monitoring
{
    /// <summary>
    /// A basic monitor.
    /// </summary>
    public abstract partial class MonitorBase : IMonitor
    {
        #region Fields (1)

        private readonly object _SYNC_ROOT;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorBase" /> class.
        /// </summary>
        /// <param name="syncRoot"></param>
        protected MonitorBase(object syncRoot = null)
        {
            _SYNC_ROOT = syncRoot ?? new object();
        }

        #endregion Constructors (1)

        #region Events (1)

        /// <inheriteddoc />
        public virtual event EventHandler MonitorUpdated;

        #endregion Events (1)

        #region Properties (2)

        /// <summary>
        /// Gets or sets and object that should be linked with that instance.
        /// </summary>
        public virtual object Tag { get; set; }

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public object SyncRoot
        {
            get { return _SYNC_ROOT; }
        }

        #endregion Properties (2)

        #region Methods (3)

        /// <inheriteddoc />
        public IMonitorInfo GetInfo(CultureInfo lang = null)
        {
            try
            {
                var state = MonitorState.None;
                var summary = new StringBuilder();
                var desc = new StringBuilder();
                object value = null;
                var lastUpdate = DateTimeOffset.Now;

                OnGetInfo(lang ?? CultureInfo.CurrentCulture,
                          ref state, summary, desc, ref value, ref lastUpdate);

                return new MonitorInfo(this)
                {
                    Description = desc.Length < 1 ? null : desc.ToString(),
                    LastUpdate = lastUpdate,
                    State = state,
                    Summary = summary.Length < 1 ? null : summary.ToString(),
                    Value = value,
                };
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();

                return new MonitorInfo(this)
                {
                    Description = baseEx.ToString(),
                    LastUpdate = DateTimeOffset.Now,
                    State = MonitorState.Exception,
                    Summary = baseEx.GetType().FullName,
                    Value = ex,
                };
            }
        }

        /// <summary>
        /// The logic for the <see cref="MonitorBase.GetInfo(CultureInfo)" /> method.
        /// </summary>
        /// <param name="lang">The language to use.</param>
        /// <param name="state">The variable where to write the state to.</param>
        /// <param name="summary">The <see cref="StringBuilder" /> for building the summary.</param>
        /// <param name="desc">The <see cref="StringBuilder" /> for building the description.</param>
        /// <param name="value">The variable where to write the value to.</param>
        /// <param name="lastUpdate">The variable where to write the last update timestamp to.</param>
        protected abstract void OnGetInfo(CultureInfo lang,
                                          ref MonitorState state, StringBuilder summary, StringBuilder desc, ref object value, ref DateTimeOffset lastUpdate);

        /// <summary>
        /// Raises the <see cref="MonitorBase.MonitorUpdated" /> event.
        /// </summary>
        /// <returns>Event was raised or not.</returns>
        public bool RaiseMonitorUpdated()
        {
            var handler = MonitorUpdated;
            if (handler == null)
            {
                return false;
            }

            handler(this, EventArgs.Empty);
            return true;
        }

        #endregion Methods (3)
    }
}