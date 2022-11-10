using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordRoleComparer
{
    public class DiscordFacade
    {
        public DiscordFacade()
        {
            DiscordSocketConfig config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers
            };

            client = new DiscordSocketClient(config);
        }

        private List<SocketGuild> socketGuilds = new List<SocketGuild>();

        private Dictionary<ulong, string> guildRoles = new Dictionary<ulong, string>();

        public async void Start(string token)
        {
            await MainAsync(token);
        }

        #region Discord.NET
        private DiscordSocketClient client;

        private async Task MainAsync(string token)
        {
            client.GuildAvailable += OnGuildAvailable;
            client.Log += OnLogOutput;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task<List<DiscordMember>> PullGuildMembers(SocketGuild socketGuild)
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
        #endregion

        #region Discord.NET Events
        private Task OnLogOutput(LogMessage logMessage)
        {
            return Task.CompletedTask;
        }

        private Task OnGuildAvailable(SocketGuild socketGuild)
        {
            socketGuilds.Add(socketGuild);
            List<DiscordMember> members = PullGuildMembers(socketGuild)?.Result;
            Dictionary<ulong, string> guildRoles = PullGuildRoles(socketGuild);

            foreach (DiscordMember user in members)
            {
                HashSet<string> friendlyRoleNames = new HashSet<string>();
                foreach(var role in user.roleIDs)
                {
                    friendlyRoleNames.Add(guildRoles[role]);
                }
                Debug.WriteLine($"user ID: {user.userID}, username: {user.username}, role IDs: {string.Join(", ", friendlyRoleNames)}");
            }
            return Task.CompletedTask;
        }
        #endregion
    }
}
