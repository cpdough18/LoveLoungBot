﻿#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

#endregion

namespace Discord.Addons.Interactive
{
    public class InteractiveBase<T> : ModuleBase<T>
        where T : SocketCommandContext
    {
        public InteractiveService Interactive { get; set; }

        public Task<SocketMessage> NextMessageAsync(ICriterion<SocketMessage> criterion, TimeSpan? timeout = null)
        {
            return Interactive.NextMessageAsync(Context, criterion, timeout);
        }

        public Task<SocketMessage> NextMessageAsync(bool fromSourceUser = true, bool inSourceChannel = true,
            TimeSpan? timeout = null)
        {
            return Interactive.NextMessageAsync(Context, fromSourceUser, inSourceChannel, timeout);
        }

        public Task<IUserMessage> ReplyAndDeleteAsync(string content = null, bool isTTS = false, Embed embed = null,
            TimeSpan? timeout = null, RequestOptions options = null)
        {
            return Interactive.ReplyAndDeleteAsync(Context, content, isTTS, embed, timeout, options);
        }

        public Task<IUserMessage> PagedReplyAsync(IEnumerable<EmbedBuilder> pages, bool fromSourceUser = true)
        {
            PaginatedMessage pager = new PaginatedMessage
            {
                Pages = pages
            };
            return PagedReplyAsync(pager, fromSourceUser);
        }

        public Task<IUserMessage> PagedReplyAsync(PaginatedMessage pager, bool fromSourceUser = true)
        {
            Criteria<SocketReaction> criterion = new Criteria<SocketReaction>();
            if (fromSourceUser)
                criterion.AddCriterion(new EnsureReactionFromSourceUserCriterion());
            return PagedReplyAsync(pager, criterion);
        }

        public Task<IUserMessage> PagedReplyAsync(PaginatedMessage pager, ICriterion<SocketReaction> criterion)
        {
            return Interactive.SendPaginatedMessageAsync(Context, pager, criterion);
        }

        public RuntimeResult Ok(string reason = null)
        {
            return new OkResult(reason);
        }
    }
}