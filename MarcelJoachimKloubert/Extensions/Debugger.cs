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

using MarcelJoachimKloubert.Diagnostics.Debugging;
using System;

namespace MarcelJoachimKloubert.Extensions
{
    static partial class MJKDiagnosticExtensionMethods
    {
        #region Methods

        /// <summary>
        /// Subscribes an action for receiving debug messages from a debugger.
        /// </summary>
        /// <param name="debugger">The debugger.</param>
        /// <param name="action">The action to subscribe.</param>
        /// <returns>The subscription context.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="debugger" /> and/or <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        public static IDebuggerSubscriptionContext Subscribe(this IDebugger debugger, Action<IDebugMessage> action)
        {
            if (debugger == null)
            {
                throw new ArgumentNullException(nameof(debugger));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new DebuggerSubscriptionContext(debugger, action);
        }

        #endregion Methods
    }
}