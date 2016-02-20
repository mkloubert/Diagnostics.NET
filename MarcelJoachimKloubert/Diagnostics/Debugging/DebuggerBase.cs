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

namespace MarcelJoachimKloubert.Diagnostics.Debugging
{
    /// <summary>
    /// A basic debugger.
    /// </summary>
    abstract public class DebuggerBase : IDebugger
    {
        #region Fields

        /// <summary>
        /// Stores the object for thread safe operations.
        /// </summary>
        protected readonly object _SYNC;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DebuggerBase" /> class.
        /// </summary>
        /// <param name="sync">The custom object for the <see cref="DebuggerBase._SYNC" /> field.</param>
        protected DebuggerBase(object sync = null)
        {
            _SYNC = sync ?? new object();
        }

        #endregion Constructors

        #region Events

        /// <inheriteddoc />
        public event EventHandler<DebugMessageReceivedEventArgs> DebugMessageReceived;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public object SyncRoot => _SYNC;

        /// <summary>
        /// Gets or sets an object that should be linked with that instance.
        /// </summary>
        public virtual object Tag { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the <see cref="DebuggerBase.DebugMessageReceived" /> event.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>Event was raised or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="msg" /> is <see langword="null" />.
        /// </exception>
        protected bool RaiseDebugMessageReceived(IDebugMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException("msg");
            }

            var handler = DebugMessageReceived;
            if (handler == null)
            {
                return false;
            }

            handler(this, new DebugMessageReceivedEventArgs(msg));
            return true;
        }

        #endregion Methods
    }
}