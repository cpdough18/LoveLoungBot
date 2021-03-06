﻿using Discord;
using Radon.Core;
using SharpLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radon.Services.External
{
    public class SharplinkService
    {
        private readonly LavalinkManager _lavalinkManager;
        public SharplinkService(Configuration configuration, LavalinkManager lavalinkManager) => _lavalinkManager = lavalinkManager;

        public Queue<SharplinkQueue> queue;

        public async Task<string> SearchAsync(string query)
        {
            LoadTracksResponse response = await _lavalinkManager.GetTracksAsync($"ytsearch:{query}");
            return string.Join("\n \n", response.Tracks.Select((x, i) => $"**{i + 1}** `{x.Title}`"));
        }
        public async Task<LavalinkTrack> GetTrackAsync(string query)
        {
            LoadTracksResponse response = await _lavalinkManager.GetTracksAsync($"ytsearch:{query}");
            LavalinkTrack track = response.Tracks.First();
            return track;
        }
        public async Task PlayAsync(ulong guildID, IVoiceChannel voiceChannel, LavalinkTrack track)
        {
            LavalinkPlayer player = _lavalinkManager.GetPlayer(guildID) ?? await _lavalinkManager.JoinAsync(voiceChannel);
            await player.PlayAsync(track);
        }
        public async Task PlayUrlAsync(ulong guildID, IVoiceChannel voiceChannel, string url)
        {
            LavalinkPlayer player = _lavalinkManager.GetPlayer(guildID) ?? await _lavalinkManager.JoinAsync(voiceChannel);
            LoadTracksResponse response = await _lavalinkManager.GetTracksAsync(url);
            LavalinkTrack track = response.Tracks.First();

            if (GetTrackInfo(guildID) == null)
            {
                await player.PlayAsync(track);
            }
            else
            {
                AddToQueue(guildID, track);
            }
        }

        public void AddToQueue(ulong guildID, LavalinkTrack track)
        {
            SharplinkQueue queueObject = new SharplinkQueue(guildID, track);
            queue.Enqueue(queueObject);
            Console.WriteLine(string.Join("\n", queue.ToList()));
        }
        public async Task StopAsync(ulong guildID) => await _lavalinkManager.LeaveAsync(guildID);
        public async Task JoinAsync(IVoiceChannel voiceChannel) => await _lavalinkManager.JoinAsync(voiceChannel);
        public async Task LeaveAsync(ulong guildID) => await _lavalinkManager.LeaveAsync(guildID);
        public async Task PauseAsync(ulong guildID) => await _lavalinkManager.GetPlayer(guildID).PauseAsync();
        public async Task ResumeAsync(ulong guildID) => await _lavalinkManager.GetPlayer(guildID).ResumeAsync();
        public async Task SeekAsync(ulong guildID, int position) => await _lavalinkManager.GetPlayer(guildID).SeekAsync(position);
        public async Task SetVolumeAsync(ulong guildID, uint volume) => await _lavalinkManager.GetPlayer(guildID).SetVolumeAsync(volume);

        public LavalinkTrack GetTrackInfo(ulong guildID) => _lavalinkManager.GetPlayer(guildID).CurrentTrack;
    }
}
