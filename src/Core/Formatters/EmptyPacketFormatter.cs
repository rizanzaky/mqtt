﻿using System.Net.Mqtt.Packets;

namespace System.Net.Mqtt.Formatters
{
	internal class EmptyPacketFormatter<T> : Formatter<T>
		where T : class, IPacket, new()
	{
		readonly PacketType packetType;

		public EmptyPacketFormatter (PacketType packetType)
		{
			this.packetType = packetType;
		}

		public override PacketType PacketType { get { return packetType; } }

		protected override T Read (byte[] bytes)
		{
			ValidateHeaderFlag (bytes, t => t == packetType, 0x00);

			return new T ();
		}

		protected override byte[] Write (T packet)
		{
			var flags = 0x00;
			var type = Convert.ToInt32(packetType) << 4;

			var fixedHeaderByte1 = Convert.ToByte(flags | type);
			var fixedHeaderByte2 = Convert.ToByte (0x00);

			return new byte[] { fixedHeaderByte1, fixedHeaderByte2 };
		}
	}
}