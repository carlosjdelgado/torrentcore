﻿// This file is part of TorrentCore.
//     https://torrentcore.org
// Copyright (c) 2017 Samuel Fisher.
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
using System.Text;
using TorrentCore.Application.BitTorrent;
using TorrentCore.Data;
using TorrentCore.Data.Pieces;
using TorrentCore.Transport;

namespace TorrentCore.Modularity
{
    public interface ITorrentContext
    {
        /// <summary>
        /// Gets the metainfo for the torrent.
        /// </summary>
        Metainfo Metainfo { get; }

        /// <summary>
        /// Gets the collection of currently connected peers.
        /// </summary>
        IReadOnlyCollection<PeerConnection> Peers { get; }

        /// <summary>
        /// Provides access to downloaded pieces and block data.
        /// </summary>
        IPieceDataHandler DataHandler { get; }

        /// <summary>
        /// Tracks outstanding block requests.
        /// </summary>
        IBlockRequests BlockRequests { get; }

        /// <summary>
        /// Notifies that new peers are available to connect to.
        /// </summary>
        void PeersAvailable(IEnumerable<ITransportStream> peers);
    }
}
