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
    public class UtpClient : IDisposable
    {
        public UtpClient()
            : this(new UtpSocket())
        {
        }

        public UtpClient(UtpSocket socket)
        {
            Client = socket;
        }

        public UtpSocket Client { get; }

        public bool Connected => Client?.Connected ?? false;

        public UtpStream GetStream()
        {
            return new UtpStream(Client);
        }

        public async Task ConnectAsync(IPAddress address, int port)
        {
            await Client.ConnectAsync(new IPEndPoint(address, port));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
