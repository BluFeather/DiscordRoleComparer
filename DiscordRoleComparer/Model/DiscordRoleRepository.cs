using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private EventHandler<Dictionary<string, List<SubscriberRole>>> SubscriberRolesPulled;

        public Func<LogMessage, Task> Log { get; set; }

        public void PullDiscordPatreonRoles(string token, EventHandler<Dictionary<string, List<SubscriberRole>>> SubscriberRolesPulledCallback)
        {
            SubscriberRolesPulled = SubscriberRolesPulledCallback;
            StartBot(token);
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
            SubscriberRolesPulled?.Invoke(null, DiscordSubscriberRoles);
            StopBot();
        }

        private async Task<Dictionary<string, List<SubscriberRole>>> AsyncPullDiscordUserRoles()
        {
            var discordUserRoles = new Dictionary<string, List<SubscriberRole>>();

            IEnumerable<IGuildUser> users = await socketGuild.GetUsersAsync().FlattenAsync();
            var guildUsers = users as IGuildUser[] ?? users.ToArray();

            foreach (var guildUser in guildUsers)
            {
                string username = $"{guildUser.Username}#{guildUser.Discriminator}";
                var subscriberRoles = new List<SubscriberRole>();
                foreach (ulong roleID in guildUser.RoleIds)
                {
                    string roleName = socketGuild.GetRole(roleID).ToString();
                    SubscriberRole? subscriberRole = SubscriberRoleHelperFunctions.ParseSubscriberRole(roleName);
                    if (subscriberRole != null)
                    {
                        subscriberRoles.Add(subscriberRole.GetValueOrDefault());
                    }
                }
                if (subscriberRoles.Any())
                {
                    discordUserRoles.Add(username, subscriberRoles);
                }
            }

            return discordUserRoles;
        }
    }
}
