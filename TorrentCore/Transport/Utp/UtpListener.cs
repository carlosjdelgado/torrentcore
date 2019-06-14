// This file is part of TorrentCore.
//   https://torrentcore.org
// Copyright (c) Samuel Fisher.
//
// Licensed under the GNU Lesser General Public License, version 3. See the
// LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TorrentCore.Transport.Utp
{
    public class UtpListener
    {
        private readonly UtpSocket _socket;

        public UtpListener(IPAddress localaddr, int port)
        {
            LocalEndpoint = new IPEndPoint(localaddr, port);
            _socket = new UtpSocket();
        }

        public EndPoint LocalEndpoint { get; }

        public void Start()
        {
            _socket.Bind(LocalEndpoint);
        }

        public async Task<UtpClient> AcceptUtpClientAsync()
        {
            var utpSocket = await _socket.AcceptAsync();
            return new UtpClient(utpSocket);
        }
    }
}
