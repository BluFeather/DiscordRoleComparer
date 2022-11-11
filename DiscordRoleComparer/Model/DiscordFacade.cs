using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        #region Save/Load
        public void OpenJsonFile()
        {
            var fileInfo = AskForJsonFile();
            GuildData guildData = LoadGuildJson(fileInfo);
            if (guildData == null) return;

            // Debugging Output Below This Line
            foreach (DiscordMember member in guildData.Members)
            {
                Debug.WriteLine(member.ToString());
            }
            Debug.WriteLine($"Server Name: {guildData.Name}, Number of Members: {guildData.Members?.Count}, Number of Roles: {guildData.Roles.Count}");
        }

        private FileInfo AskForJsonFile()
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return new FileInfo(openFileDialog.FileName);
            }
            return null;
        }

        private GuildData LoadGuildJson(FileInfo jsonFile)
        {
            if (jsonFile == null) return null;
            string jsonString = new StreamReader(jsonFile.FullName).ReadToEnd();
            return JsonConvert.DeserializeObject<GuildData>(jsonString);
        }
        #endregion
    }
}
