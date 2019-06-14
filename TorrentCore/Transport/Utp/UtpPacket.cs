// This file is part of TorrentCore.
//   https://torrentcore.org
// Copyright (c) Samuel Fisher.
//
// Licensed under the GNU Lesser General Public License, version 3. See the
// LICENSE file in the project root for full license information.

using System.IO;

namespace TorrentCore.Transport.Utp
{
    public class UtpPacket
    {
        public UtpPacketType Type { get; set; }

        public ushort ConnectionId { get; set; }

        public uint Timestamp { get; set; }

        public uint TimestampDifference { get; set; }

        public uint WindowSize { get; set; }

        public ushort SequenceNumber { get; set; }

        public ushort AckSequenceNumber { get; set; }

        public byte[] Data { get; set; }

        public UtpPacket FromBuffer(byte[] buffer)
        {
            // TODO: check wellformed buffer
            using (var ms = new MemoryStream(buffer))
            {
                BinaryReader binaryReader = new BinaryReader(ms);

                Type = (UtpPacketType)(binaryReader.Read() >> 4);
                binaryReader.Read(); // Extensions: Doesn't matters at the moment
                ConnectionId = binaryReader.ReadUInt16();
                Timestamp = binaryReader.ReadUInt32();
                TimestampDifference = binaryReader.ReadUInt32();
                WindowSize = binaryReader.ReadUInt32();
                SequenceNumber = binaryReader.ReadUInt16();
                AckSequenceNumber = binaryReader.ReadUInt16();
                Data = binaryReader.ReadBytes((int)(ms.Length - ms.Position));
            }

            return this;
        }

        public byte[] ToBuffer()
        {
            using (var ms = new MemoryStream())
            {
                BinaryWriter binaryWriter = new BinaryWriter(ms);
                binaryWriter.Write(unchecked((byte)((byte)Type << 4 | 0x01)));
                binaryWriter.Write((byte)0); // Extensions
                binaryWriter.Write(ConnectionId);
                binaryWriter.Write(Timestamp);
                binaryWriter.Write(TimestampDifference);
                binaryWriter.Write(WindowSize);
                binaryWriter.Write(SequenceNumber);
                binaryWriter.Write(AckSequenceNumber);
                binaryWriter.Write(Data);

                return ms.ToArray();
            }
        }
    }
}
