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
    /// A cached monitor.
    /// </summary>
    public class CachedMonitor : MonitorWrapper
    {
        #region Fields (4)

        private IMonitorInfo _lastInfo;
        private DateTimeOffset? _lastUpdate;
        private readonly TimeProvider _TIME_PROVIDER;
        private readonly TimeSpan _UPDATE_INTERVAL;

        #endregion Fields (4)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedMonitor" /> class.
        /// </summary>
        /// <param name="baseMonitor">The monitor to wrap.</param>
        /// <param name="sec">The update interval in seconds.</param>
        /// <param name="timeProvider">The custom time provider to use.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseMonitor" /> is <see langword="null" />.
        /// </exception>
        public CachedMonitor(IMonitor baseMonitor, double sec, TimeProvider timeProvider = null)
            : this(baseMonitor: baseMonitor,
                   updateInterval: TimeSpan.FromSeconds(sec),
                   timeProvider: timeProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedMonitor" /> class.
        /// </summary>
        /// <param name="baseMonitor">The monitor to wrap.</param>
        /// <param name="updateInterval">The update interval.</param>
        /// <param name="timeProvider">The custom time provider to use.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseMonitor" /> is <see langword="null" />.
        /// </exception>
        public CachedMonitor(IMonitor baseMonitor, TimeSpan updateInterval, TimeProvider timeProvider = null)
            : base(baseMonitor: baseMonitor)
        {
            _TIME_PROVIDER = timeProvider ?? GetNow;
            _UPDATE_INTERVAL = updateInterval;
        }

        #endregion Constructors (1)

        #region Delegates (1)

        /// <summary>
        /// Provides the current time.
        /// </summary>
        /// <returns>The current time.</returns>
        public delegate DateTimeOffset TimeProvider(CachedMonitor monitor);

        #endregion Delegates (1)

        #region Methods (3)

        private static DateTimeOffset GetNow(CachedMonitor monitor)
        {
            return DateTimeOffset.Now;
        }

        /// <inheriteddoc />
        protected override void OnGetInfo(CultureInfo lang,
                                          ref MonitorState state, StringBuilder summary, StringBuilder desc, ref object value, ref DateTimeOffset lastUpdate)
        {
            lock (SyncRoot)
            {
                var now = _TIME_PROVIDER(this);

                var lastMonitorUpdate = _lastUpdate;
                var doUpdate = true;
                if (lastMonitorUpdate.HasValue)
                {
                    doUpdate = false;

                    var interval = now - lastMonitorUpdate.Value;
                    if (interval >= _UPDATE_INTERVAL ||
                        _UPDATE_INTERVAL <= TimeSpan.Zero)
                    {
                        doUpdate = true;
                    }
                }

                IMonitorInfo info;
                if (doUpdate)
                {
                    _lastInfo = info = BaseMonitor.GetInfo(lang);
                    _lastUpdate = now;
                }
                else
                {
                    info = _lastInfo;
                }

                if (info == null)
                {
                    return;
                }

                state = info.State;
                summary.Append(info.Summary);
                desc.Append(info.Description);
                value = info.Value;
                lastUpdate = info.LastUpdate;
            }
        }

        /// <summary>
        /// Resets the state.
        /// </summary>
        public void Reset()
        {
            lock (SyncRoot)
            {
                _lastUpdate = null;
                _lastInfo = null;
            }
        }

        #endregion Methods (3)
    }
}