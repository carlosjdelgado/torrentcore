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
    public class UtpListenerTest
    {
        [Test]
        public void AcceptUtpClient()
        {
            var sut = new UtpListener(IPAddress.Any, 8080);
            sut.Start();

            var listeningTask = Task.Run(async () =>
            {
                var client = await sut.AcceptUtpClientAsync();
                Assert.IsTrue(client is UtpClient);
                Assert.IsTrue(client.Connected);
                return Task.CompletedTask;
            });

            var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            socket.Connect(new IPEndPoint(IPAddress.Loopback, 8080));
            socket.Send(BuildTestSynPacket());

            listeningTask.Wait();
        }

        private byte[] BuildTestSynPacket()
        {
            var rnd = new Random();

            var synPacket = new List<byte>();

            synPacket.Add(0x41); // Type and version
            synPacket.Add(0x00); // Extension

            ushort connectionId = (ushort)rnd.Next();
            synPacket.AddRange(BitConverter.GetBytes(connectionId)); // Connection Id

            uint timestamp = (uint)DateTimeOffset.Now.ToUnixTimeMicroseconds();
            synPacket.AddRange(BitConverter.GetBytes(timestamp)); // Timestamp microseconds

            uint timestampDiff = 0U;
            synPacket.AddRange(BitConverter.GetBytes(timestampDiff)); // Timestamp difference microseconds

            uint windowSize = 20U;
            synPacket.AddRange(BitConverter.GetBytes(windowSize)); // Timestamp difference milliseconds

            ushort sequenceNumber = 1;
            synPacket.AddRange(BitConverter.GetBytes(sequenceNumber)); // sequence number

            ushort ackSequenceNumber = default(ushort);
            synPacket.AddRange(BitConverter.GetBytes(ackSequenceNumber)); // sequence number

            return synPacket.ToArray();
        }
    }
}
