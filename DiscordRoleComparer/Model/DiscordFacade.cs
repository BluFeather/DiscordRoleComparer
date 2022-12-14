using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordRoleComparer
{
    public class DiscordFacade
    {
        public DiscordFacade() { }

        private List<GuildData> guildDatas = new List<GuildData>();

        public async Task<List<GuildData>> AsyncPullGuildData(string token)
        {
            if (!await ConnectClientAsync(token)) return new List<GuildData>();

            List<GuildData> pulledGuildDatas = new List<GuildData>();
            foreach (SocketGuild socketGuild in client.Guilds)
            {
                string guildName = socketGuild.Name;
                ulong guildId = socketGuild.Id;
                List<DiscordMember> guildMembers = await AsyncPullGuildMembers(socketGuild);
                Dictionary<ulong, string> guildRoles = PullGuildRoles(socketGuild);

                pulledGuildDatas.Add(new GuildData(guildName, guildId, guildMembers, guildRoles));
            }
            return pulledGuildDatas;
        }



        #region Discord.NET
        private DiscordSocketClient client;

        private SocketGuild socketGuild;

        private async Task<bool> ConnectClientAsync(string token)
        {
            if (client?.ConnectionState == ConnectionState.Connected) return true;

            client = new DiscordSocketClient(new DiscordSocketConfig() { GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers });
            client.GuildAvailable += OnGuildAvailable;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            int timeoutMillisecondsDelay = 20000;
            int millisecondsDelay = 500;
            int elapsedTime = 0;
            do
            {
                elapsedTime += millisecondsDelay;
                Debug.WriteLine($"Connection State: {client.ConnectionState} | Elapsed Time: {elapsedTime}");
                await Task.Delay(millisecondsDelay);
            }
            while (elapsedTime <= timeoutMillisecondsDelay && client.ConnectionState != ConnectionState.Connected);

            Debug.WriteLine($"Connection State: {client.ConnectionState}");
            return elapsedTime <= timeoutMillisecondsDelay;
        }

        private Task OnGuildAvailable(SocketGuild arg)
        {
            socketGuild = arg;
            return Task.CompletedTask;
        }

        private async Task<List<DiscordMember>> AsyncPullGuildMembers(SocketGuild socketGuild)
        {
            IEnumerable<IGuildUser> users = await socketGuild.GetUsersAsync().FlattenAsync();
            var guildUsers = users as IGuildUser[] ?? users.ToArray();

            List<DiscordMember> guildMembers = new List<DiscordMember>();
            foreach (IGuildUser user in guildUsers)
            {
                ulong userID = user.Id;
                string username = user.Username + "#" + user.Discriminator;
                HashSet<ulong> roleIDs = user.RoleIds.ToHashSet();
                guildMembers.Add(new DiscordMember(userID, username, roleIDs));
            }
            return guildMembers;
        }

        private Dictionary<ulong, string> PullGuildRoles(SocketGuild socketGuild)
        {
            Dictionary<ulong, string> roles = new Dictionary<ulong, string>();
            var something = socketGuild.Roles;
            foreach (SocketRole socketRole in socketGuild.Roles)
            {
                roles.Add(socketRole.Id, socketRole.Name);
            }
            return roles;
        }

        public async void AsyncRemoveRole(ulong discordMemberID, ulong roleID)
        {
            await socketGuild.GetUser(discordMemberID)?.RemoveRoleAsync(roleID);
        }

        public async void AsyncAddRole(ulong discordMemberID, ulong roleID)
        {
            await socketGuild.GetUser(discordMemberID)?.AddRoleAsync(roleID);
        }
        #endregion
    }
}
