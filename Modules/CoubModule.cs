//using CoubSharp;
//using CoubSharp.Managers;
//using Discord.Commands;
//using Radon.Core;
//using System;
//using System.Threading.Tasks;

//namespace Radon.Modules
//{
//    public class CoubModule : CommandBase
//    {
//        private readonly CoubService _coubService;

//        public CoubModule(CoubService coubService)
//        {
//            _coubService = coubService;
//        }

//        [Command("coub")]
//        public async Task Coub()
//        {
//            var timeLineCannel = await _coubService.Timelines.GetChannelTimelineAsync("royal.coubs", 1, 20, TimelineManager.ChannelTimelineOrderBy.LikesCount);


//            foreach (var item in timeLineCannel.Coubs)
//            {
//                Console.WriteLine(item.ToString());
//            }
//        }
//    }
//}
