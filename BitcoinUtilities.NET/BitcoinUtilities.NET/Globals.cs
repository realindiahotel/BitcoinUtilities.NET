using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		public const int LocalP2PListeningPort = 20966; //8333; //if we want to accept messages from only private nodes we can change this port and listen outside the normal network
		public const uint ClientVersion = 70002;
		public const uint MinimumAcceptedClientVersion = 70001; //we don't connect to anything below this, AIDs.
		public enum Services :ulong {SPV_NODE_NETWORK=0, NODE_NETWORK=1};
		public enum Relay {RELAY_ON_DEMAND=0, RELAY_ALWAYS=1 };
		public static String[] DNSSeedHosts = { "seed.bitcoin.sipa.be", "dnsseed.bitcoin.dashjr.org", "bitseed.xf2.org", "dnsseed.bluematt.me" }; //in future lego will also have it's own dns responder
		public static int HeartbeatTimeout = 1800000; //30 minute interval, heartbeat
		public static int AddrFartInterval = 86400000; //24 hour interval to transmit my addr
		public static bool HeartbeatKeepAlive = true; //if true then send heartbeat to keep connection open every 30 minutes-ish
		public static bool AllowP2PConnectToSelf = true; //Allows this client to connect to itself if it gets served its own address
		public static bool AdvertiseExternalIP = true; //If set to false we return local host instead of actual external IP
		public static int AddressMemPoolMax = 5000;	//once this value is reached we start overwriting addr entries in the begining of the peers address mempool
		public static int SeedNodeCount = 200; //the maximum number of seed nodes to attempt to connect too.
		public static int MaxOutgoingP2PConnections = 8; //The maximum amount of P2P clients we will reach out and connect to
		public static int MaxIncomingP2PConnections = 128; //The maximum amount of P2P clients we will allow to connect to us
		public static int RetrySendTCPOnError = 3; //number of attempts to send message on error;
		public static int RetryRecieveTCPOnError = 3; //number of attempts to recieve message , after 3 attempts we kill the connection
		public static bool AggressiveReconnect = true; //if we lose connection waiting for packets on recieve or sending packets, if this is on we attempt to reconnect, but only for outbond peers the we connect to, it's up to incoming to reconnect to us
		public static bool UPNPMapPort = true; //Attempts to send UPNP message to set up port forwarding in NAT of connected router, won't always work, especially if on VPN or behind multiple routers, but should work for most homes
		public static bool EnableListenForPeers = true; //if this is true we listen for peers on startup
		public static DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		public static ulong NotCryptoRandomNonce = Convert.ToUInt64(DateTime.UtcNow.Ticks);
		public static String LegoVersionString = "0.0.0.0";
		public static String LegoCodenameString = "Thashiznets-Testing";
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
