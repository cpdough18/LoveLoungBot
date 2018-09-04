﻿#region

using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

#endregion

namespace Discord.Addons.Interactive
{
    internal class EnsureReactionFromSourceUserCriterion : ICriterion<SocketReaction>
    {
        public Task<bool> JudgeAsync(SocketCommandContext sourceContext, SocketReaction parameter)
        {
            var ok = parameter.UserId == sourceContext.User.Id;
            return Task.FromResult(ok);
        }
    }
}