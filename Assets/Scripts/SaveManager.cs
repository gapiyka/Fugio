using UnityEngine;
namespace Fugio
{
    public static class SaveManager
    {
        private const string currentRecord = "PlayerCurrentRecord";
        private const string currentName = "PlayerCurrentName";
        private const int startRecord = 0;
        private const string startName = "";
        public static PlayerInfo Info;

        // WARN for WebGL!!!
        // Unity stores up to 1MB of PlayerPrefs using the browser's IndexedDB API

        public static void Save(PlayerInfo player)
        {
            Info.Name = player.Name;
            Info.Record = player.Record;
            PlayerPrefs.SetString(currentName, player.Name);
            PlayerPrefs.SetInt(currentRecord, player.Record);
        }

        public static void Save(string name)
        {
            Info.Name = name;
            PlayerPrefs.SetString(currentName, Info.Name);
        }

        public static void Save(int record)
        {
            Info.Record = record;
            PlayerPrefs.SetInt(currentRecord, Info.Record);
        }

        public static PlayerInfo Load() =>
            Info = new PlayerInfo(
                PlayerPrefs.GetInt(currentRecord, startRecord),
                PlayerPrefs.GetString(currentName, startName));

        public static PlayerInfo Reset()
        {
            Info = new PlayerInfo(startRecord, startName);
            Save(Info);
            return Info;
        }
    }
}