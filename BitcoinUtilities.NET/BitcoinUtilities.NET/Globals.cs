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
		public static bool StrictVerackOutbound = true; //if true we force reciept of verack from peers we connect to outbound
		public const int TestP2PPort = 18333;
        public const int ProdP2PPort = 8333;
		public const int LocalP2PListeningPort = 8333; //if we want to accept messages from only private nodes we can change this port and listen outside the normal network
		public const uint ClientVersion = 70002;
		public const uint MinimumAcceptedClientVersion = 70001; //we don't connect to anything below this, AIDs.
		public enum Services :ulong {SPV_NODE_NETWORK=0, NODE_NETWORK=1};
		public enum Relay {RELAY_ON_DEMAND=0, RELAY_ALWAYS=1 };
		public static String[] DNSSeedHosts = { "seed.bitcoin.sipa.be", "dnsseed.bitcoin.dashjr.org", "bitseed.xf2.org", "dnsseed.bluematt.me" }; //in future lego will also have it's own dns responder
        public static int TCPMessageTimeout = 60000;	//60 sec timeout for TCP messages
		public static int HeartbeatTimeout = 1800000; //30 minute interval, heartbeat
		public static int AddrFartInterval = 86400000; //24 hour interval to transmit my addr
		public static bool HeartbeatKeepAlive = true; //if true then send heartbeat to keep connection open every 30 minutes-ish
		public static bool DeadIfNoHeartbeat = false; //if true then after the heartbeat timeout if we haven't had a message we kill the connection
		public static bool AllowP2PConnectToSelf = true; //Allows this client to connect to itself if it gets served its own address
		public static bool AdvertiseExternalIP = true; //If set to false we return local host instead of actual external IP
		public static int AddressMemPoolMax = 3000; //once this value is reached we start overwriting addr entries in the begining of the peers address mempool
		public static int MaxOutgoingP2PConnections = 8; //The maximum amount of P2P clients we will reach out and connect to
		public static int MaxIncomingP2PConnections = 16; //The maximum amount of P2P clients we will allow to connect to us
		public static int RetrySendTCPOnError = 3; //number of attempts to send message on error;
		public static int RetryRecieveTCPOnError = 3; //number of attempts to recieve message on error;
		public static bool AggressiveReconnect = true; //if we lose conneectuion waiting for packets on recieve or sending packets, if this is on we attempt to reconnect
		public static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static ulong NotCryptoRandomNonce = Convert.ToUInt64(DateTime.UtcNow.Ticks);
		public static String LegoVersionString = "0.0.0";
		public static String LegoCodenameString = "Thashiznets";
		public static String UserAgentString = @"/Lego.NET:"+LegoVersionString+ @"/"+LegoCodenameString+@"/";

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
