using System;

namespace Kitsu
{
    public class SeasonalAnime
    {
        public SeasonalAnime(int id, string name, string status, bool isInList)
        {
            Id = id;
            Name = name;
            StatusInlist = (Status)Enum.Parse(typeof(Status), status);
            IsInList = isInList;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public bool IsInList { get; }

        public Status StatusInlist { get; private set; }
    }
}
