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
    /// Wraps another monitor.
    /// </summary>
    public class MonitorWrapper : MonitorBase
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorWrapper" /> class.
        /// </summary>
        /// <param name="baseMonitor">The monitor to wrap.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseMonitor" /> is <see langword="null" />.
        /// </exception>
        public MonitorWrapper(IMonitor baseMonitor)
        {
            if (baseMonitor == null)
            {
                throw new ArgumentNullException("baseMonitor");
            }

            BaseMonitor = baseMonitor;
        }

        #endregion Constructors (1)

        #region Events (1)

        /// <inheriteddoc />
        public override event EventHandler MonitorUpdated
        {
            add { BaseMonitor.MonitorUpdated += value; }

            remove { BaseMonitor.MonitorUpdated -= value; }
        }

        #endregion Events (1)

        #region Properties (1)

        /// <summary>
        /// Gets the wrapped monitor.
        /// </summary>
        public IMonitor BaseMonitor { get; private set; }

        #endregion Properties (1)

        #region Methods (1)

        /// <inheriteddoc />
        protected override void OnGetInfo(CultureInfo lang,
                                          ref MonitorState state, StringBuilder summary, StringBuilder desc, ref object value, ref DateTimeOffset lastUpdate)
        {
            var info = BaseMonitor.GetInfo(lang);
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

        #endregion Methods (1)
    }
}