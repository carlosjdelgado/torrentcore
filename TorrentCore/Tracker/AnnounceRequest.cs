﻿// This file is part of TorrentCore.
//     https://torrentcore.org
// Copyright (c) 2016 Sam Fisher.
// 
// TorrentCore is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, version 3.
// 
// TorrentCore is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with TorrentCore.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TorrentCore.Data;

namespace TorrentCore.Tracker
{
    /// <summary>
    /// Represents an announce request made to a tracker.
    /// </summary>
    public class AnnounceRequest
    {
        public AnnounceRequest(int listenPort,
                               long remaining,
                               Sha1Hash infoHash)
            : this(null, listenPort, remaining, infoHash)
        {
        }

        public AnnounceRequest(IPAddress listenAddress,
                               int listenPort,
                               long remaining,
                               Sha1Hash infoHash)
        {
            ListenAddress = listenAddress;
            ListenPort = listenPort;
            Remaining = remaining;
            InfoHash = infoHash;
        }

        /// <summary>
        /// Gets or sets the address the client is listening on for incoming connections from other peers.
        /// </summary>
        public IPAddress ListenAddress { get; }

        /// <summary>
        /// Gets or sets the port the client is listening on for incoming connections from other peers.
        /// </summary>
        public int ListenPort { get; }

        /// <summary>
        /// Gets or sets the number of bytes left to download.
        /// </summary>
        public long Remaining { get; }

        /// <summary>
        /// Gets or sets the infohash of the torrent being downloaded.
        /// </summary>
        public Sha1Hash InfoHash { get; }
    }
}
