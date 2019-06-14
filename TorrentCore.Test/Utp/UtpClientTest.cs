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
using System.Threading.Tasks;
using NUnit.Framework;
using TorrentCore.Transport.Utp;
using TorrentCore.Transport.Utp.Extensions;

namespace TorrentCore.Test.Utp
{
    [TestFixture]
    public class UtpClientTest
    {
        [Test]
        public async Task Connect()
        {
            var client = new UdpClient(new IPEndPoint(IPAddress.Any, 8080));
            var listeningTask = Task.Run(async () =>
            {
                var message = await client.ReceiveAsync();
                var response = BuildTestStatePacket();
                client.Send(response, response.Length, message.RemoteEndPoint);
                return Task.CompletedTask;
            });

            var sut = new UtpClient();
            await sut.ConnectAsync(IPAddress.Loopback, 8080);

            Assert.IsTrue(sut.Connected);
        }

        private byte[] BuildTestStatePacket()
        {
            var rnd = new Random();

            var statePacket = new List<byte>();

            statePacket.Add(0x21); // Type and version
            statePacket.Add(0x00); // Extension

            ushort connectionId = (ushort)rnd.Next();
            statePacket.AddRange(BitConverter.GetBytes(connectionId)); // Connection Id

            uint timestamp = (uint)DateTimeOffset.Now.ToUnixTimeMicroseconds();
            statePacket.AddRange(BitConverter.GetBytes(timestamp)); // Timestamp microseconds

            uint timestampDiff = 5000U;
            statePacket.AddRange(BitConverter.GetBytes(timestampDiff)); // Timestamp difference microseconds

            uint windowSize = 20U;
            statePacket.AddRange(BitConverter.GetBytes(windowSize)); // Timestamp difference milliseconds

            ushort sequenceNumber = 1;
            statePacket.AddRange(BitConverter.GetBytes(sequenceNumber)); // sequence number

            ushort ackSequenceNumber = default(ushort);
            statePacket.AddRange(BitConverter.GetBytes(ackSequenceNumber)); // sequence number

            return statePacket.ToArray();
        }
    }
}
