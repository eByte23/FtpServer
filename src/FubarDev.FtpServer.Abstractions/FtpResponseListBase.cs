// <copyright file="FtpResponseListBase.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FubarDev.FtpServer
{
    /// <summary>
    /// Base class for FTP response lists.
    /// </summary>
    public abstract class FtpResponseListBase : IFtpResponse
    {
        private readonly string _startMessage;
        private readonly string _endMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpResponseListBase"/> class.
        /// </summary>
        /// <param name="code">The status code.</param>
        /// <param name="startMessage">The message in the start line.</param>
        /// <param name="endMessage">The message in the end line.</param>
        protected FtpResponseListBase(
            int code,
            string startMessage,
            string endMessage)
        {
            _startMessage = startMessage;
            _endMessage = endMessage;
            Code = code;
        }

        /// <inheritdoc />
        public int Code { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            var lines = new[]
            {
                $"{Code}-{_startMessage.TrimEnd()}",
                " ... stripped ... async data",
                $"{Code} {_endMessage.TrimEnd()}",
            };

            return string.Join(Environment.NewLine, lines);
        }

        /// <inheritdoc />
        [Obsolete("Use GetLinesAsync instead.")]
        public async Task<FtpResponseLine> GetNextLineAsync(object? token, CancellationToken cancellationToken)
        {
            IAsyncEnumerator<string> linesEnumerator;
            if (token == null)
            {
                var lines = GetLinesAsync(cancellationToken);
                linesEnumerator = lines.GetAsyncEnumerator(cancellationToken);
            }
            else
            {
                linesEnumerator = (IAsyncEnumerator<string>)token;
            }

            if (await linesEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                return new FtpResponseLine(linesEnumerator.Current, linesEnumerator);
            }

            await linesEnumerator.DisposeAsync().ConfigureAwait(false);
            return new FtpResponseLine(null, null);
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<string> GetLinesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            yield return $"{Code}-{_startMessage.TrimEnd()}";

            await foreach (var line in GetDataLinesAsync(cancellationToken).ConfigureAwait(false))
            {
                yield return $" {line}";
            }

            yield return $"{Code} {_endMessage.TrimEnd()}";
        }

        /// <summary>
        /// Return all data lines that come between the start and end lines.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The data lines.</returns>
        protected abstract IAsyncEnumerable<string> GetDataLinesAsync(CancellationToken cancellationToken);
    }
}