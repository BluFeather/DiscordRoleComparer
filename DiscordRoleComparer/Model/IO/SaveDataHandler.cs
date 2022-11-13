using Newtonsoft.Json;
using System;
using System.IO;

namespace DiscordRoleComparer
{
    public static class SaveDataHandler
    {
        private static readonly string saveFileName = Path.Combine(Environment.CurrentDirectory, "AppData.sav");

        public static void WriteSaveDataToDisk(SaveData saveData)
        {
            string jsonString = JsonConvert.SerializeObject(saveData);
            string path = saveFileName;
            File.WriteAllText(path, jsonString);
        }

        public static SaveData LoadSaveDataFromDisk()
        {
            if (!File.Exists(saveFileName)) return null;
            var jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, saveFileName));
            return JsonConvert.DeserializeObject<SaveData>(jsonString);
        }
    }
}
