using Newtonsoft.Json;
using System.IO;

namespace DiscordRoleComparer.Model
{
    public class AppSaveLoad
    {
        public void OpenJsonFile()
        {
            var fileInfo = AskForJsonFile();
            GuildData guildData = LoadGuildDataFromJson(fileInfo);
            if (guildData == null) return;
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

        private GuildData LoadGuildDataFromJson(FileInfo jsonFile)
        {
            if (jsonFile == null) return null;
            string jsonString = new StreamReader(jsonFile.FullName).ReadToEnd();
            return JsonConvert.DeserializeObject<GuildData>(jsonString);
        }

        private void SaveGuildDataToJson(GuildData guildData)
        {
            var saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string jsonString = JsonConvert.SerializeObject(guildData, Formatting.Indented);
                File.WriteAllText(saveFileDialog.FileName, jsonString);
            }
        }
    }
}
