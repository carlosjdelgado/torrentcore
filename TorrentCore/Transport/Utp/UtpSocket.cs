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
using TorrentCore.Transport.Utp.Extensions;

namespace TorrentCore.Transport.Utp
{
    public class UtpSocket
    {
        private ushort _sequenceNumber;
        private ushort _ackSequenceNumber;
        private ushort _connectionIdReceive;
        private ushort _connectionIdSend;
        private uint _windowSize;
        private uint _replyMicroseconds;
        private UtpConnectionState _connectionState;

        private IPEndPoint _localEndpoint;
        private IPEndPoint _remoteEndpoint;
        private UdpClient _udpClient;

        public UtpSocket()
        {
            _udpClient = new UdpClient();
            _sequenceNumber = 0;
            _ackSequenceNumber = 0;
            _connectionIdReceive = 0;
            _connectionIdSend = 0;
            _windowSize = 0;
            _replyMicroseconds = 0;
        }

        public bool Connected => _connectionState == UtpConnectionState.CS_CONNECTED;

        public void Send(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void Bind(EndPoint localEP)
        {
            _udpClient = new UdpClient((IPEndPoint)localEP);
            _localEndpoint = (IPEndPoint)localEP;
        }

        public async Task ConnectAsync(EndPoint endPoint)
        {
            _remoteEndpoint = (IPEndPoint)endPoint;
            _udpClient.Connect(_remoteEndpoint);

            // send SYN packet
            var synBuffer = CreatePacket(UtpPacketType.ST_SYN).ToBuffer();
            await _udpClient.SendAsync(synBuffer, synBuffer.Length);
            _connectionState = UtpConnectionState.CS_SYN_SENT;

            // receive ACK
            var message = await _udpClient.ReceiveAsync();
            var packet = new UtpPacket().FromBuffer(message.Buffer);

            UpdateState(packet);
            _connectionState = UtpConnectionState.CS_CONNECTED;
        }

        public async Task<UtpSocket> AcceptAsync()
        {
            var message = await _udpClient.ReceiveAsync();
            _remoteEndpoint = message.RemoteEndPoint;

            // receive SYN packet
            var packet = new UtpPacket().FromBuffer(message.Buffer);

            // Analyze packet
            if (packet.Type != UtpPacketType.ST_SYN)
                throw new Exception();

            UpdateState(packet);
            _connectionState = UtpConnectionState.CS_SYN_RECV;

            // Send ACK
            var ackBuffer = CreatePacket(UtpPacketType.ST_STATE).ToBuffer();
            await _udpClient.SendAsync(ackBuffer, ackBuffer.Length, _remoteEndpoint);
            _connectionState = UtpConnectionState.CS_CONNECTED;

            return this;
        }

        public void UpdateState(UtpPacket packet)
        {
            var rnd = new Random();

            _replyMicroseconds = (uint)DateTimeOffset.Now.ToUnixTimeMicroseconds() - packet.Timestamp;
            _connectionIdReceive = (ushort)(packet.ConnectionId + 1);
            _connectionIdSend = packet.ConnectionId;
            _ackSequenceNumber = packet.SequenceNumber;

            if (_sequenceNumber == 0)
            {
                _sequenceNumber = (ushort)rnd.Next();
            }
            else
            {
                _sequenceNumber++;
            }
        }

        public UtpPacket CreatePacket(UtpPacketType type, byte[] data = null)
        {
            var packet = new UtpPacket
            {
                Type = type,
                ConnectionId = type == UtpPacketType.ST_SYN ? _connectionIdReceive : _connectionIdSend,
                Timestamp = (uint)DateTimeOffset.Now.ToUnixTimeMicroseconds(),
                TimestampDifference = _replyMicroseconds,
                WindowSize = _windowSize,
                SequenceNumber = _sequenceNumber,
                AckSequenceNumber = _ackSequenceNumber,
                Data = data ?? new byte[0],
            };

            return packet;
        }
    }
}
