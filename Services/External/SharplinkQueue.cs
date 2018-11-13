using SharpLink;

namespace Radon.Services.External
{
    public class SharplinkQueue
    {
        public SharplinkQueue(ulong guildID, LavalinkTrack track)
        {
            GuildID = guildID;
            Track = track;
        }

        public ulong GuildID { get; set; }

        public LavalinkTrack Track { get; set; }
    }
}
