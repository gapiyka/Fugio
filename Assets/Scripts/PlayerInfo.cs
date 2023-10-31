namespace Fugio
{
    [System.Serializable]
    public class PlayerInfo
    {
        public string Name;
        public int Record;

        public PlayerInfo(int record, string name)
        {
            Record = record;
            Name = name;
        }
    }
}