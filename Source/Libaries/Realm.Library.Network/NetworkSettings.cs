namespace Realm.Library.Network
{
    public class NetworkSettings
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public bool UseUdp { get; set; }
        public int StaleUserThresholdSeconds { get; set; }
        public bool AllowMxp { get; set; }
        public bool AllowAnsi { get; set; }
        public string PacketMessageFormat { get; set; }
    }
}
