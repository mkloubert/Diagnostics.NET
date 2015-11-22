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
    /// A logger that uses a delegate.
    /// </summary>
    public class DelegateMonitor : MonitorBase
    {
        #region Fields (1)

        private readonly StateProvider _PROVIDER;

        #endregion Fields (1)

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateMonitor" /> class.
        /// </summary>
        /// <param name="provider">The provider to use.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        public DelegateMonitor(StateProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _PROVIDER = provider;
        }

        #endregion Constructors (1)

        #region Delegates (1)

        /// <summary>
        /// Provides a monitor state object.
        /// </summary>
        /// <param name="monitor">The parent monitor.</param>
        /// <param name="lang">The language to use.</param>
        /// <param name="state">The variable where to write the state to.</param>
        /// <param name="summary">The <see cref="StringBuilder" /> for building the summary.</param>
        /// <param name="desc">The <see cref="StringBuilder" /> for building the description.</param>
        /// <param name="value">The variable where to write the value to.</param>
        /// <param name="lastUpdate">The variable where to write the last update timestamp to.</param>
        public delegate void StateProvider(DelegateMonitor monitor, CultureInfo lang,
                                           ref MonitorState state, StringBuilder summary, StringBuilder desc, ref object value, ref DateTimeOffset lastUpdate);

        #endregion Delegates (1)

        #region Methods (1)

        /// <inheriteddoc />
        protected override void OnGetInfo(CultureInfo lang,
                                          ref MonitorState state, StringBuilder summary, StringBuilder desc, ref object value, ref DateTimeOffset lastUpdate)
        {
            _PROVIDER(this, lang,
                      ref state, summary, desc, ref value, ref lastUpdate);
        }

        #endregion Methods (1)
    }
}