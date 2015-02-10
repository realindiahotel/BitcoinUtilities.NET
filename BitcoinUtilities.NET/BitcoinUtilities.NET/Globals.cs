using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcoin.BitcoinUtilities
{
    public static class Globals
    {
        public static Byte[] ProdAddressVersion = {0};
        public static Byte[] TestAddressVersion = {111};
        public static Byte[] ProdDumpKeyVersion = {128};
        public static Byte[] TestDumpKeyVersion = {239};
		public const uint TestPacketMagic = 0xfabfb5da;
		public const uint ProdPacketMagic = 0xf9beb4d9;
		public const int TestP2PPort = 18333;
        public const int ProdP2PPort = 8333;
		public const uint ClientVersion = 70002;
		public const ulong SPVNodeNetwork = 0;
		public const ulong NodeNetwork = 1;
		public static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static ulong NotCryptoRandomNonce = Convert.ToUInt64(DateTime.UtcNow.Ticks);
		public const String LegoVersionString = "0.0.0";
		public const String LegoCodenameString = "Fuck Our Ordinary Lives";
		public const String UserAgentString = @"/Lego:"+LegoVersionString+ @"/"+LegoCodenameString+@"/";
		public const int RelayTransactionsAlways = 1;
		public const int RelayTransactionsOnDemand = 0;

		public enum NORM_FORM
        {
            NormalizationOther = 0,
            NormalizationC = 0x1,
            NormalizationD = 0x2,
            NormalizationKC = 0x5,
            NormalizationKD = 0x6
        };
    }
}
