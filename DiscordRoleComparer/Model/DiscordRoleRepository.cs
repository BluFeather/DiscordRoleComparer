using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private EventHandler<List<DiscordMember>> DiscordRolesPulled;

        public Func<LogMessage, Task> Log { get; set; }

        public void PullDiscordPatreonRoles(string token, EventHandler<List<DiscordMember>> DiscordRolesPulledCallback)
        {
#if DEBUG
            AskForAndParseDiscordUserRolesJson(token, DiscordRolesPulledCallback);
#else
            DiscordRolesPulled = DiscordRolesPulledCallback;
            StartBot(token);
#endif
        }

        public void AskForAndParseDiscordUserRolesJson(string token, EventHandler<List<DiscordMember>> DiscordRolesPulledCallback)
        {
            var result = new List<DiscordMember>();
            DiscordRolesPulled = DiscordRolesPulledCallback;
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string jsonString = File.ReadAllText(fileDialog.FileName);
                Dictionary<string, List<string>> jsonResult = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonString);
                foreach(var item in jsonResult)
                {
                    result.Add(new DiscordMember(item.Key, item.Value));
                }
            }
            DiscordRolesPulled?.Invoke(null, result);
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
            DiscordRolesPulled?.Invoke(null, DiscordSubscriberRoles);
            StopBot();
        }

        private async Task<List<DiscordMember>> AsyncPullDiscordUserRoles()
        {
            var discordUserRoles = new List<DiscordMember>();

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
                    discordUserRoles.Add(new DiscordMember(username, subscriberRoles));
                }
            }

            return discordUserRoles;
        }

        /*
        private async Task<Dictionary<string, List<string>>> AsyncPullDiscordUserRoles()
        {
            var discordUserRoles = new Dictionary<string, List<string>>();

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
                    discordUserRoles.Add(username, subscriberRoles);
                }
            }

            return discordUserRoles;
        }
        */
    }
}
