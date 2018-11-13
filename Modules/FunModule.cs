#region
using Discord;
using Discord.Commands;
using GiphyDotNet.Manager;
using GiphyDotNet.Model.Parameters;
using KSoftDotNet.Manager;
using Newtonsoft.Json.Linq;
using Radon.Core;
using Radon.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
#endregion
namespace Radon.Modules
{
    [CommandCategory(CommandCategory.Fun)]
    [CheckState]
    public class FunModule : CommandBase
    {
        private readonly Configuration _configuration;
        private readonly Giphy _giphy;
        private readonly HttpClient _httpClient;
        private readonly Random _random;
        private readonly KSoft _kSoft;

        public FunModule(HttpClient http, Random random, Configuration configuration, Giphy giphy, KSoft kSoft)
        {
            _httpClient = http;
            _random = random;
            _configuration = configuration;
            _giphy = giphy;
            _kSoft = kSoft;
        }

        [Command("dog")]
        [Alias("woof")]
        [Summary("Gets you a dog")]
        public async Task DogAsync()
        {
            string url;
            do
            {
                url = $"http://random.dog/{await _httpClient.GetStringAsync("https://random.dog/woof")}";
            } while (url.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ||
                     url.EndsWith(".webm", StringComparison.OrdinalIgnoreCase));

            EmbedBuilder embed = new EmbedBuilder()
                .WithImageUrl(url);
            await ReplyEmbedAsync(embed);
        }
        [Command("Cat")]
        [Alias("kitty")]
        [Summary("Gets you a cat")]
        public async Task CatAsync()
        {
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _configuration.CatApiKey);
            JArray data = JArray.Parse(await _httpClient.GetStringAsync($"https://api.thecatapi.com/v1/images/search?format=json&size=full"));
            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle("Kitty")
                .WithImageUrl(data[0]["url"].ToString());
            await ReplyEmbedAsync(embed);
        }
        [Command("fox")]
        [Summary("Gets you a random fox")]
        public async Task FoxAsync()
        {
            string url = $"{JObject.Parse(await _httpClient.GetStringAsync("https://randomfox.ca/floof/"))["image"]}";

            EmbedBuilder embed = new EmbedBuilder()
                .WithImageUrl(url);

            await ReplyEmbedAsync(embed);
        }

        [Command("8ball")]
        [Summary("Ask a question and the 8 ball will answer")]
        public async Task EightballAsync([Remainder] string question)
        {
            JObject data = JObject.Parse(await _httpClient.GetStringAsync("https://nekos.life/api/v2/8ball"));

            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle("8Ball has spoken")
                .WithDescription($"Question ❯ {question}\n\n8Ball's answer ❯ {data["response"]}")
                .WithThumbnailUrl($"{data["url"]}");

            await ReplyEmbedAsync(embed);
        }

        [Command("joke")]
        [Summary("Tells a joke")]
        public async Task JokeAsync()
        {
            string joke = $"{JObject.Parse(await _httpClient.GetStringAsync("http://api.yomomma.info/"))["joke"]}";
            await ReplyAsync(joke);
        }

        [Command("lenny")]
        [Summary("Returns a lenny ( ͡° ͜ʖ ͡°)")]
        public async Task LennyAsync()
        {
            JArray data = JArray.Parse(await _httpClient.GetStringAsync($"https://api.lenny.today/v1/random"));
            EmbedBuilder embed = new EmbedBuilder()
                .WithImageUrl(data[0]["face"].ToString());
            await ReplyEmbedAsync(embed);
        }
        //public async Task LennyAsync()
        //{
        //    string[] lennys = new[]
        //    {
        //        "( ͡° ͜ʖ ͡°)", "(☭ ͜ʖ ☭)", "(ᴗ ͜ʖ ᴗ)", "( ° ͜ʖ °)", "( ͡◉ ͜ʖ ͡◉)", "( ͡☉ ͜ʖ ͡☉)", "( ͡° ͜ʖ ͡°)>⌐■-■",
        //        "<:::::[]=¤ (▀̿̿Ĺ̯̿̿▀̿ ̿)", "( ͡ಥ ͜ʖ ͡ಥ)", "( ͡º ͜ʖ ͡º )", "( ͡ಠ ʖ̯ ͡ಠ)", "ᕦ( ͡°╭͜ʖ╮͡° )ᕤ", "( ♥ ͜ʖ ♥)",
        //        "(つ ♡ ͜ʖ ♡)つ", "✩°｡⋆⸜(▀̿Ĺ̯▀̿ ̿)", "⤜(ʘ_ʘ)⤏", "¯\\_ツ_/¯", "ಠ_ಠ", "ʢ◉ᴥ◉ʡ", "^‿^", "(づ◔ ͜ʖ◔)づ", "⤜(ʘ_ʘ)⤏",
        //        "☞   ͜ʖ  ☞", "ᗒ ͟ʖᗕ", "/͠-. ͝-\\", "(´• ᴥ •`)", "(╯￢ ᗝ￢ ）╯︵ ┻━┻", "ᕦ(・ᨎ・)ᕥ", "◕ ε ◕", "【$ ³$】",
        //        "(╭☞T ε T)╭☞"
        //    };
        //    await ReplyAsync(lennys[_random.Next(lennys.Length)]);
        //}

        [Command("meme")]
        [Summary("Gets a meme")]
        public async Task MemeAsync()
        {
            var data = await _kSoft.Meme();

            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle($"{data.Title}")
                .WithUrl($"{data.Source}")
                .WithImageUrl($"{data.ImageUrl}")
                .WithFooter($"{Context.Message.Author.Username} | KSoft.Si API")
                .WithTimestamp(Formatter.UnixToDateTime((long)data.CreatedAt));
            await ReplyEmbedAsync(embed);

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        [Command("wikihow")]
        [Alias("wh")]
        [Summary("Gets a wikihow article")]
        public async Task WikiHowAsync()
        {
            var wh = _kSoft.Wikihow().Result;
            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle($"{wh.Title}")
                .WithUrl($"{wh.ArticleUrl}")
                .WithImageUrl($"{wh.Url}")
                .WithFooter($"{Context.Message.Author.Username} | KSoft.Si API");
            await ReplyEmbedAsync(embed);
        }

        [Command("out")]
        [Alias("door")]
        [Summary("Shows someone the door")]
        public async Task OutAsync(IUser user)
        {
            await ReplyEmbedAsync(null, $"{user.Mention}  :point_right::skin-tone-1:  :door:");
        }

        [Command("say")]
        [Alias("echo", "e", "s")]
        [Summary("Echoes text")]
        public async Task SayAsync([Remainder] string text)
        {
            await ReplyEmbedAsync(null, text);
        }

        [Command("lmgtfy")]
        [Alias("showgoogle", "sg", "showg", "sgoogle")]
        [Summary("Shows a dumbass how to google")]
        public async Task ShowGoogleAsync([Remainder] string query)
        {
            string url = $"http://lmgtfy.com/?q={HttpUtility.UrlEncode(query)}";
            await ReplyAsync(url);
        }

        [Command("aww")]
        [Alias("awh", "cute")]
        [Summary("Returns random image from r/aww")]
        public async Task AwwAsync()
        {
            var aww = _kSoft.Aww().Result;
            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle($"{aww.Subreddit} | {aww.Title}")
                .WithUrl($"{aww.Source}")
                .WithImageUrl($"{aww.ImageUrl}")
                .WithDescription($"<:upvote:486361126658113536> {aww.Upvotes} | <:downvote:486361126641205268> {aww.Downvotes}")
                .WithFooter($"{Context.Message.Author.Username} | KSoft.Si API")
                .WithTimestamp(Formatter.UnixToDateTime((long)aww.CreatedAt));
            await ReplyEmbedAsync(embed);
        }

        [Command("subreddit")]
        [Alias("reddit", "sub", "r", "r/", "sr", "sreddit")]
        [Summary("Gets a post from a subreddit")]
        public async Task SubredditAsync(string subreddit)
        {
            var post = _kSoft.SubredditImage().Result;
            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle($"{post.Subreddit} | {post.Title}")
                .WithUrl($"{post.Source}")
                .WithImageUrl($"{post.ImageUrl}")
                .WithDescription($"<:upvote:486361126658113536> {post.Upvotes} | <:downvote:486361126641205268> {post.Downvotes}")
                .WithFooter($"{Context.Message.Author.Username} | KSoft.Si API")
                .WithTimestamp(Formatter.UnixToDateTime((long)post.CreatedAt));
            await ReplyEmbedAsync(embed);
        }

        [Command("rip")]
        [Alias("tombstone")]
        [Summary("Gets a tombstone with a custom text")]
        public async Task RipAsync([Remainder] string text)
        {
            string url = "http://tombstonebuilder.com/generate.php" +
                      "?top1=R.I.P." +
                      $"&top2={HttpUtility.UrlEncode(text.Substring(0, Math.Min(text.Length, 25)))}" +
                      $"{(text.Length > 25 ? $"&top3={HttpUtility.UrlEncode(text.Substring(25, Math.Min(25, text.Length - 25)))}" : "")}" +
                      $"{(text.Length > 50 ? $"&top4={HttpUtility.UrlEncode(text.Substring(50))}" : "")}";
            await ReplyEmbedAsync(new EmbedBuilder().WithImageUrl(url));
        }

        [Command("sign")]
        [Alias("roadsign")]
        [Summary("Gets a roadsign with a custom text")]
        public async Task SignAsync([Remainder] string text)
        {
            string url = $"http://www.customroadsign.com/generate.php" +
                      $"?line1={HttpUtility.UrlEncode(text.Substring(0, Math.Min(15, text.Length)))}" +
                      $"{(text.Length > 15 ? $"&line2={HttpUtility.UrlEncode(text.Substring(15, Math.Min(15, text.Length - 15)))}" : "")}" +
                      $"{(text.Length > 30 ? $"&line3={HttpUtility.UrlEncode(text.Substring(30, Math.Min(30, text.Length - 30)))}" : "")}" +
                      $"{(text.Length > 45 ? $"&line4={HttpUtility.UrlEncode(text.Substring(45, Math.Min(45, text.Length - 45)))}" : "")}";
            await ReplyEmbedAsync(new EmbedBuilder().WithImageUrl(url));
        }

        [Command("qr")]
        [Alias("qrcode")]
        [Summary("Creates a qr code")]
        public async Task QrAsync([Remainder] string text)
        {
            await ReplyEmbedAsync(new EmbedBuilder().WithImageUrl(
                $"https://chart.googleapis.com/chart?cht=qr&chl={HttpUtility.UrlEncode(text)}&choe=UTF-8&chld=L&chs=500x500"));
        }

        [Command("ascii")]
        [Summary("Converts text to ascii art")]
        public async Task AsciiAsync([Remainder] string text)
        {
            await ReplyAsync($"{await _httpClient.GetStringAsync($"http://artii.herokuapp.com/make?text={text}")}".BlockCode());
        }

        [Group("gif")]
        [Alias("giphy", "g")]
        [CommandCategory(CommandCategory.Fun)]
        [Summary("Gets a gif")]
        public class GiphyModule : CommandBase
        {
            private readonly Giphy _giphy;
            private readonly Random _random;

            public GiphyModule(Giphy giphy, Random random)
            {
                _giphy = giphy;
                _random = random;
            }

            [Command("")]
            [Summary("Gets a random gif")]
            [Priority(-1)]
            public async Task GifAsync()
            {
                GiphyDotNet.Model.Results.GiphyRandomResult gif = await _giphy.RandomGif(new RandomParameter());
                EmbedBuilder embed = new EmbedBuilder()
                    .WithImageUrl(gif.Data.ImageUrl);

                await ReplyEmbedAsync(embed);
            }

            [Command("")]
            [Summary("Searches for your gif")]
            [Priority(-1)]
            public async Task GifAsync([Remainder] string query)
            {
                GiphyDotNet.Model.Results.GiphySearchResult gif = await _giphy.GifSearch(new SearchParameter { Query = query });
                EmbedBuilder embed = new EmbedBuilder();
                if (!gif.Data.Any())
                {
                    embed.WithTitle("No gif found")
                        .WithDescription($"Couldn't find any gif for {query.InlineCode()}");
                }
                else
                {
                    embed.WithImageUrl(gif.Data[_random.Next(gif.Data.Length)].Images.Original.Url);
                }

                await ReplyEmbedAsync(embed);
            }
        }
    }
}