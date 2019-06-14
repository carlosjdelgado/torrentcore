// This file is part of TorrentCore.
//   https://torrentcore.org
// Copyright (c) Samuel Fisher.
//
// Licensed under the GNU Lesser General Public License, version 3. See the
// LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TorrentCore.Transport.Utp;

namespace TorrentCore.Test.Utp
{
    [TestFixture]
    public class UtpStreamTest
    {
        [Test]
        public async Task SendData()
        {
            var listener = new UtpListener(IPAddress.Any, 8080);
            listener.Start();

            UtpClient receiverClient = new UtpClient();
            var listeningTask = Task.Run(async () =>
            {
                receiverClient = await listener.AcceptUtpClientAsync();
                return Task.CompletedTask;
            });

            var senderClient = new UtpClient();
            await senderClient.ConnectAsync(IPAddress.Loopback, 8080);

            // Create a 256kB of random data
            var data = Enumerable.Repeat(Enumerable.Range(0, 255).Select(x => (byte)x), 1000)
                                     .SelectMany(x => x).ToArray();

            var senderStream = senderClient.GetStream();

            senderStream.Write(data, 0, data.Length);

            var receiverStream = receiverClient.GetStream();
            var receivedData = new byte[256000];
            receiverStream.Read(receivedData, 0, receivedData.Length);

            Assert.That(receivedData.ToArray().SequenceEqual(data), "Downloaded data is not correct.");
        }
    }
}
