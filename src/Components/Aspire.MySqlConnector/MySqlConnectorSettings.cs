// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Aspire.MySqlConnector;

/// <summary>
/// Provides the client configuration settings for connecting to a MySQL database using MySqlConnector.
/// </summary>
public sealed class MySqlConnectorSettings
{
    /// <summary>
    /// The connection string of the MySQL database to connect to.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// <para>Gets or sets a boolean value that indicates whether the database health check is enabled or not.</para>
    /// <value>
    /// The default value is <see langword="true"/>.
    /// </value>
    /// </summary>
    public bool HealthChecks { get; set; } = true;

    /// <summary>
    /// <para>Gets or sets a boolean value that indicates whether the Open Telemetry tracing is enabled or not.</para>
    /// <value>
    /// The default value is <see langword="true"/>.
    /// </value>
    /// </summary>
    public bool Tracing { get; set; } = true;

    /// <summary>
    /// <para>Gets or sets a boolean value that indicates whether the Open Telemetry metrics are enabled or not.</para>
    /// <value>
    /// The default value is <see langword="true"/>.
    /// </value>
    /// </summary>
    public bool Metrics { get; set; } = true;
}
