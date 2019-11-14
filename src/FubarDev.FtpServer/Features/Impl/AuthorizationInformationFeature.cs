// <copyright file="AuthorizationInformationFeature.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Security.Claims;

using Microsoft.AspNetCore.Connections.Features;

namespace FubarDev.FtpServer.Features.Impl
{
    /// <summary>
    /// Default implementation of <see cref="IConnectionUserFeature"/>.
    /// </summary>
    internal class AuthorizationInformationFeature
        : IConnectionUserFeature,
#pragma warning disable 618
            IAuthorizationInformationFeature
#pragma warning restore 618
    {
        private ClaimsPrincipal? _user;

        /// <inheritdoc />
        ClaimsPrincipal? IAuthorizationInformationFeature.FtpUser
        {
            get => _user;
            set => _user = value;
        }

        /// <inheritdoc />
#nullable disable
        public ClaimsPrincipal User
        {
            get => _user;
            set => _user = value;
        }
#nullable restore
    }
}
