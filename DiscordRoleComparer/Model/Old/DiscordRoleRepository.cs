using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordRoleComparer
{
    public class DiscordRoleRepository
    {
        public DiscordRoleRepository()
        {
            DiscordSocketConfig config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers
            };

            client = new DiscordSocketClient(config);
        }

        private DiscordSocketClient client;

        private SocketGuild socketGuild;

        private EventHandler<List<OLDDiscordMember>> DiscordUsersPulledCallback;

        EventHandler<HashSet<string>> DiscordRolesPulledCallback;

        public Func<LogMessage, Task> Log { get; set; }

        public void PullDiscordPatreonRoles(string token, EventHandler<List<OLDDiscordMember>> DiscordUsersPulledCallback, EventHandler<HashSet<string>> DiscordRolesPulledCallback)
        {
#if DEBUG
            AskForAndParseDiscordUserRolesJson(token, DiscordUsersPulledCallback, DiscordRolesPulledCallback);
#else
            this.DiscordUsersPulledCallback = DiscordUsersPulledCallback;
            this.DiscordRolesPulledCallback = DiscordRolesPulledCallback;
            StartBot(token);
#endif
        }

        public void AskForAndParseDiscordUserRolesJson(string token, EventHandler<List<OLDDiscordMember>> DiscordUsersPulledCallback, EventHandler<HashSet<string>> DiscordRolesPulledCallback)
        {
            var result = new List<OLDDiscordMember>();
            var serverRoles = new HashSet<string>();
            this.DiscordUsersPulledCallback = DiscordUsersPulledCallback;
            this.DiscordRolesPulledCallback = DiscordRolesPulledCallback;

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string jsonString = File.ReadAllText(fileDialog.FileName);
                Dictionary<string, List<string>> jsonResult = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonString);
                foreach (var item in jsonResult)
                {
                    result.Add(new OLDDiscordMember(item.Key, item.Value));
                    foreach (string role in item.Value)
                    {
                        serverRoles.Add(role);
                    }
                }
            }
            this.DiscordUsersPulledCallback?.Invoke(null, result);
            this.DiscordRolesPulledCallback?.Invoke(null, serverRoles);
        }

        private async void StartBot(string token)
        {
            await MainAsync(token);
        }

        public async void StopBot()
        {
            await client.StopAsync();
            await client.LogoutAsync();
        }

        private async Task MainAsync(string token)
        {
            client.Log += Log;
            client.GuildAvailable += GuildAvailable;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task GuildAvailable(SocketGuild arg)
        {
            socketGuild = arg;
            var DiscordSubscriberRoles = await AsyncPullDiscordUserRoles();
            var UniqueDiscordRoles = AsyncPullUniqueDiscordRoles();
            DiscordUsersPulledCallback?.Invoke(null, DiscordSubscriberRoles.Item1);
            DiscordRolesPulledCallback?.Invoke(null, DiscordSubscriberRoles.Item2);
            StopBot();
        }

        private HashSet<string> AsyncPullUniqueDiscordRoles()
        {
            HashSet<string> result = new HashSet<string>();

            foreach (SocketRole guildRole in socketGuild.Roles)
            {
                result.Add(guildRole.Name);
            }

            return result;
        }

        private async Task<(List<OLDDiscordMember>, HashSet<string>)> AsyncPullDiscordUserRoles()
        {
            var discordUserRoles = new List<OLDDiscordMember>();

            IEnumerable<IGuildUser> users = await socketGuild.GetUsersAsync().FlattenAsync();
            var guildUsers = users as IGuildUser[] ?? users.ToArray();

            foreach (var guildUser in guildUsers)
            {
                string username = $"{guildUser.Username}#{guildUser.Discriminator}";
                var subscriberRoles = new List<string>();
                foreach (ulong roleID in guildUser.RoleIds)
                {
                    string roleName = socketGuild.GetRole(roleID).ToString();
                    if (!string.IsNullOrWhiteSpace(roleName))
                    {
                        subscriberRoles.Add(roleName);
                    }
                }

                if (subscriberRoles.Any())
                {
                    discordUserRoles.Add(new OLDDiscordMember(username, subscriberRoles));
                }
            }

            var serverRoles = AsyncPullUniqueDiscordRoles();
            return (discordUserRoles, serverRoles);
        }
    }
}
