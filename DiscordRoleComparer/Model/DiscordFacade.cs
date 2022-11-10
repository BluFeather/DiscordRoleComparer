using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordRoleComparer
{
    public class DiscordFacade
    {
        public DiscordFacade()
        {
            
        }

        private List<GuildData> guildDatas = new List<GuildData>();

        public async void Start(string token) => await MainAsync(token);

        #region Discord.NET
        private DiscordSocketClient client;

        private async Task MainAsync(string token)
        {
            client = new DiscordSocketClient(new DiscordSocketConfig() { GatewayIntents =  GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers });
            client.GuildAvailable += OnGuildAvailable;
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
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

        private Task OnGuildAvailable(SocketGuild socketGuild)
        {
            var guildData = new GuildData(
                socketGuild.Name,
                AsyncPullGuildMembers(socketGuild)?.Result,
                PullGuildRoles(socketGuild));

            guildDatas.Add(guildData);
            return Task.CompletedTask;
        }
        #endregion
    }
}
