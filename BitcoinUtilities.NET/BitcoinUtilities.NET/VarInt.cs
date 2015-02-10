using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.BitcoinUtilities
{
	public class VarInt
	{
		public ulong Value { get; private set; }

		public VarInt(ulong value)
		{
			Value = value;
		}

		// BitCoin has its own varint format, known in the C++ source as "compact size".
		public VarInt(byte[] buf, int offset)
		{
			var first = buf[offset];
			ulong val;
			if (first < 253)
			{
				// 8 bits.
				val = first;
			}
			else if (first == 253)
			{
				// 16 bits.
				val = (ushort)(buf[offset + 1] | (buf[offset + 2] << 8));
			}
			else if (first == 254)
			{
				// 32 bits.
				val = Utilities.ReadUint32(buf, offset + 1);
			}
			else
			{
				// 64 bits.
				val = Utilities.ReadUint32(buf, offset + 1) | (((ulong)Utilities.ReadUint32(buf, offset + 5)) << 32);
			}
			Value = val;
		}

		public int SizeInBytes
		{
			get
			{
				if (Value < 253)
					return 1;
				if (Value <= ushort.MaxValue)
					return 3; // 1 marker + 2 data bytes
				if (Value <= uint.MaxValue)
					return 5; // 1 marker + 4 data bytes
				return 9; // 1 marker + 8 data bytes
			}
		}

		public byte[] Encode()
		{
			return EncodeBe();
		}

		public byte[] EncodeBe()
		{
			if (Value < 253)
			{
				return new[] { (byte)Value };
			}

			if (Value <= ushort.MaxValue)
			{
				return new[] { (byte)253, (byte)Value, (byte)(Value >> 8) };
			}

			if (Value <= uint.MaxValue)
			{
				var bytes = new byte[5];
				bytes[0] = 254;
				Utilities.Uint32ToByteArrayLe((uint)Value, bytes, 1);
				return bytes;
			}
			else
			{
				var bytes = new byte[9];
				bytes[0] = 255;
				Utilities.Uint32ToByteArrayLe((uint)Value, bytes, 1);
				Utilities.Uint32ToByteArrayLe((uint)(Value >> 32), bytes, 5);
				return bytes;
			}
		}
	}
}
