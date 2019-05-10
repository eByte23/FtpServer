// <copyright file="FtpCommandContext.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Threading.Channels;

using JetBrains.Annotations;

namespace FubarDev.FtpServer.Commands
{
    /// <summary>
    /// The context in which the command gets executed.
    /// </summary>
    public class FtpCommandContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpCommandContext"/> class.
        /// </summary>
        /// <param name="command">The FTP command.</param>
        /// <param name="responseWriter">The FTP response writer.</param>
        /// <param name="connection">The FTP connection.</param>
        public FtpCommandContext(
            [NotNull] FtpCommand command,
            [NotNull] ChannelWriter<IFtpResponse> responseWriter,
            [NotNull] IFtpConnection connection)
        {
            Command = command;
            ResponseWriter = responseWriter;
            Connection = connection;
        }

        /// <summary>
        /// Gets the FTP command to be executed.
        /// </summary>
        [NotNull]
        public FtpCommand Command { get; }

        /// <summary>
        /// Gets the FTP connection.
        /// </summary>
        [NotNull]
        public IFtpConnection Connection { get; }

        /// <summary>
        /// Gets the response writer.
        /// </summary>
        [NotNull]
        public ChannelWriter<IFtpResponse> ResponseWriter { get; }
    }
}