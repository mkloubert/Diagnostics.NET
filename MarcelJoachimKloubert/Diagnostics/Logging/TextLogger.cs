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
using System.IO;
using System.Text;

namespace MarcelJoachimKloubert.Diagnostics.Logging
{
    /// <summary>
    /// Logger that writes to a <see cref="TextWriter"/>
    /// </summary>
    public class TextLogger : LoggerBase
    {
        #region Fields (1)

        private readonly WriterProvider _PROVIDER;

        #endregion Fields (1)

        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBase" /> class.
        /// </summary>
        /// <param name="stream">The target stream.</param>
        /// <param name="enc">The custom encoding to use. Default is <see cref="Encoding.UTF8" />.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream" /> is <see langword="null" />.
        /// </exception>
        public TextLogger(Stream stream, Encoding enc = null, object syncRoot = null)
            : this(writer: CreateWriter(stream, enc),
                   syncRoot: syncRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBase" /> class.
        /// </summary>
        /// <param name="writer">The writer to use.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer" /> is <see langword="null" />.
        /// </exception>
        public TextLogger(TextWriter writer, object syncRoot = null)
            : this(provider: CreateProvider(writer),
                   syncRoot: syncRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBase" /> class.
        /// </summary>
        /// <param name="provider">The function that provides the value for the <see cref="TextLogger.Writer" /> property.</param>
        /// <param name="syncRoot">The custom object for thread safe operations.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="provider" /> is <see langword="null" />.
        /// </exception>
        public TextLogger(WriterProvider provider, object syncRoot = null)
            : base(syncRoot: syncRoot)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _PROVIDER = provider;
        }

        #endregion Constructors (3)

        #region Delegates (1)

        /// <summary>
        /// Describes a function that provides the writer to use.
        /// </summary>
        /// <param name="logger">The parent logger.</param>
        /// <returns>The writer to use.</returns>
        public delegate TextWriter WriterProvider(TextLogger logger);

        #endregion Delegates (1)

        #region Properties (1)

        /// <summary>
        /// Gets the writer to use.
        /// </summary>
        public TextWriter Writer => _PROVIDER(this);

        #endregion Properties (1)

        #region Methods (4)

        /// <summary>
        /// Creates a provider from a <see cref="TextWriter" /> instance.
        /// </summary>
        /// <param name="writer">The writer to return.</param>
        /// <returns>The created provider.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer" /> is <see langword="null" />.
        /// </exception>
        protected static WriterProvider CreateProvider(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            return (logger) => writer;
        }

        /// <summary>
        /// Creates a writer from a <see cref="Stream" /> instance.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="enc">The encoding to use. Default is <see cref="Encoding.UTF8" />.</param>
        /// <returns>The created provider.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream" /> is <see langword="null" />.
        /// </exception>
        protected static TextWriter CreateWriter(Stream stream, Encoding enc)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return new StreamWriter(stream, enc ?? Encoding.UTF8);
        }

        /// <inheriteddoc />
        protected override void OnLog(ILogMessage msg, ref bool success)
        {
            var writer = Writer;
            if (null == writer)
            {
                success = false;
                return;
            }

            writer.WriteLine();

            var sb = new StringBuilder();
            CreateString(msg, sb);

            writer.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Converts a delegate that only returns a <see cref="TextWriter" /> instance
        /// to a <see cref="WriterProvider" /> instance.
        /// </summary>
        /// <param name="func">The input value.</param>
        /// <returns>The output value.</returns>
        /// <remarks>Returns <see langword="null" /> if <paramref name="func" /> is <see langword="null" />.</remarks>
        public static WriterProvider ToProvider(Func<TextWriter> func)
        {
            if (func == null)
            {
                return null;
            }

            return (logger) => func();
        }

        #endregion Methods (4)
    }
}