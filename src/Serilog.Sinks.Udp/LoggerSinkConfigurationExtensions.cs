﻿// Copyright 2015 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Net;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Udp;

namespace Serilog
{
    /// <summary>
    /// Extends <see cref="LoggerConfiguration"/> to add UDP sink capabilities.
    /// </summary>
    public static class LoggerSinkConfigurationExtensions
    {
        private const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";

        /// <summary>
        /// Send log events as UDP packages over the network.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="remoteAddress">
        /// The <see cref="IPAddress"/> of the remote host or multicast group to which the UDP
        /// client should sent the logging event.
        /// </param>
        /// <param name="remotePort">
        /// The TCP port of the remote host or multicast group to which the UDP client should sent
        /// the logging event.
        /// </param>
        /// <param name="localPort">
        /// The TCP port from which the UDP client will communicate. The default is 0 and will
        /// cause the UDP client not to bind to a local port.
        /// </param>
        /// <param name="restrictedToMinimumLevel">
        /// The minimum level for events passed through the sink. The default is
        /// <see cref="LevelAlias.Minimum"/>.
        /// </param>
        /// <param name="outputTemplate">
        /// A message template describing the format used to write to the sink. The default is
        /// "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}".
        /// </param>
        /// <param name="formatProvider">
        /// Supplies culture-specific formatting information, or null.
        /// </param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfiguration Udp(
            this LoggerSinkConfiguration sinkConfiguration,
            IPAddress remoteAddress,
            int remotePort,
            int localPort = 0,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException("sinkConfiguration");

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            var sink = new UdpSink(localPort, remoteAddress, remotePort, formatter);
            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel);
        }
    }
}