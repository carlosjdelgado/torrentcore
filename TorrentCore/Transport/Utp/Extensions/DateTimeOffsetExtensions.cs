// This file is part of TorrentCore.
//   https://torrentcore.org
// Copyright (c) Samuel Fisher.
//
// Licensed under the GNU Lesser General Public License, version 3. See the
// LICENSE file in the project root for full license information.

using System;

namespace TorrentCore.Transport.Utp.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static long ToUnixTimeMicroseconds(this DateTimeOffset dateTimeOffset)
        {
            var ticks = dateTimeOffset.Ticks;
            var ticksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

            return ticks / ticksPerMicrosecond;
        }
    }
}
