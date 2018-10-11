#region

using Discord;
using Discord.Commands;
using E621DotNet;
using KSoftDotNet.Manager;
using KSoftDotNet.Model.Results;
using Newtonsoft.Json.Linq;
using Radon.Core;
using Radon.Services;
using Radon.Services.Nsfw;
using System;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace Radon.Modules
{
    [CheckNsfw]
    [CommandCategory(CommandCategory.Nsfw)]
    [CheckState]
    public class NsfwModule : CommandBase
    {
        private readonly HttpClient _httpClient;
        private readonly NSFWService _nsfwService;
        private readonly Random _random;
        private readonly KSoft _kSoft;
        private readonly E621 _e621;

        public NsfwModule(HttpClient httpClient, Random random, NSFWService nsfwService, KSoft kSoft, E621 e621)
        {
            _httpClient = httpClient;
            _random = random;
            _nsfwService = nsfwService;
            _kSoft = kSoft;
            _e621 = e621;
        }

        [Command("ass")]
        [Summary("Gets an ass")]
        public async Task AssAsync()
        {
            var random = _random.Next(6012);
            var data = JArray.Parse(await _httpClient.GetStringAsync($"http://api.obutts.ru/butts/{random}")).First;
            var embed = new EmbedBuilder()
                .WithImageUrl($"http://media.obutts.ru/{data["preview"]}");

            await ReplyEmbedAsync(embed, ColorType.Normal, true);
        }

        [Command("boobs")]
        [Summary("Gets some boobs")]
        public async Task BoobsAsync()
        {
            var random = _random.Next(12965);
            var data = JArray.Parse(await _httpClient.GetStringAsync($"http://api.oboobs.ru/boobs/{random}")).First;
            var embed = new EmbedBuilder()
                .WithImageUrl($"http://media.oboobs.ru/{data["preview"]}");

            await ReplyEmbedAsync(embed, ColorType.Normal, true);
        }

        [Command("hentai")]
        [Summary("Gets some hentai")]
        public async Task HentaiAsync()
        {
            await _nsfwService.SendImageFromCategory(Context, "hentai", Server);
        }

        [Command("nude")]
        [Summary("Gets some nudes")]
        public async Task NudeAsync()
        {
            await _nsfwService.SendImageFromCategory(Context, "4k", Server);
        }

        [Command("nudegif")]
        [Summary("Gets some more nudes, a gif this time")]
        public async Task NudeGifAsync()
        {
            await _nsfwService.SendImageFromCategory(Context, "pgif", Server);
        }

        [Command("anal")]
        [Summary("Gets some anal")]
        public async Task AnalAsync()
        {
            await _nsfwService.SendImageFromCategory(Context, "anal", Server);
        }

        [Command("pussy")]
        [Summary("Gets some pussy")]
        public async Task PussyAsync()
        {
            await _nsfwService.SendImageFromCategory(Context, "pussy", Server);
        }

        [Command("RandNsfw")]
        [Alias("randomnsfw", "rnsfw")]
        [Summary("Gets a random nsfw image")]
        public async Task NsfwAsync(bool gifs = true)
        {
            KSoftRedditPost post = await _kSoft.GetRandomNsfw(gifs);
            EmbedBuilder embed = new EmbedBuilder()
               .WithTitle($"{post.Author} | {post.Title}")
               .WithUrl($"{post.Source}")
               .WithImageUrl(post.ImageUrl)
               .WithDescription($"<:upvote:486361126658113536> {post.Upvotes.ToString()} | <:downvote:486361126641205268> {post.Downvotes.ToString()}")
               .WithFooter($"{Context.Message.Author.Username} | KSoft.Si API")
               .WithTimestamp(Formatter.UnixToDateTime((long)post.CreatedAt));
            await ReplyEmbedAsync(embed: embed);
        }
        [Group("E621")]
        [Alias("e6", "621")]
        [Summary("Gets a post from E621")]
        public class GetE621 : CommandBase
        {
            private readonly E621 _e621;

            public GetE621(E621 e621)
            {
                _e621 = e621;
            }

            [Command("")]
            public async Task GetRandomE621Async()
            {
                string[] tags = null;
                int limit = 1;
                var posts = await _e621.GetLatest(limit: limit, tags: tags);
                EmbedBuilder embed = new EmbedBuilder()
                    .WithTitle($"{posts.Author} | {posts.Tags.WithMaxLength(20)}")
                    .WithDescription($"<:upvote:486361126658113536> {posts.Score} :heart: {posts.FavCount}")
                    .WithImageUrl(posts.FileUrl)
                    .WithFooter($"{posts.Height}x{posts.Width} | E621.Net");
                await ReplyEmbedAsync(embed);
            }

            [Command("")]
            public async Task GetRandomE621Async([Remainder] string tags)
            {
                string[] tagslist = null;
                string.Join(" ", tagslist);
                int limit = 1;
                var posts = await _e621.GetLatest(limit: limit, tags: tagslist);
                EmbedBuilder embed = new EmbedBuilder()
                    .WithTitle($"{posts.Author} | {posts.Tags.WithMaxLength(20)}")
                    .WithDescription($"<:upvote:486361126658113536> {posts.Score} :heart: {posts.FavCount}")
                    .WithImageUrl(posts.FileUrl)
                    .WithFooter($"{posts.Height}x{posts.Width} | E621.Net");
                await ReplyEmbedAsync(embed);
            }
            [Command("")]
            public async Task GetRandomE621Async(int limit, [Remainder] string tags)
            {
                string[] tagslist = null;
                string.Join(" ", tagslist);
                var posts = await _e621.GetLatest(limit: limit, tags: tagslist);
                EmbedBuilder embed = new EmbedBuilder()
                    .WithTitle($"{posts.Author} | {posts.Tags.WithMaxLength(20)}")
                    .WithDescription($"<:upvote:486361126658113536> {posts.Score} :heart: {posts.FavCount}")
                    .WithImageUrl(posts.FileUrl)
                    .WithFooter($"{posts.Height}x{posts.Width} | E621.Net");
                await ReplyEmbedAsync(embed);
            }
        }
    }
}