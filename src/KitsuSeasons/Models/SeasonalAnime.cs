using Kitsu;
using KitsuSeasons.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

namespace KitsuSeasons.Models
{
    public class SeasonalAnime
    {
        public SeasonalAnime(int id, string name, string status, dynamic attributes, string season, bool isInList)
        {
            Id = id;
            Name = name;
            Episodes = GetValueOrDefault((string)attributes.episodeCount);
            TotalLength = GetValueOrDefault((string)attributes.episodeLength);
            Type = UppercaseFirst(GetValueOrDefault((string)attributes.subtype));
            AverageRating = GetValueOrDefault((string)attributes.averageRating);
            StartDate = FormatDate((string)attributes.startDate);
            Synopsis = (string)attributes.synopsis;
            EndDate = FormatDate((string)attributes.endDate);
            AgeRating = UppercaseFirst(GetValueOrDefault((string)attributes.ageRating));
            Nsfw = string.IsNullOrWhiteSpace((string)attributes.nsfw) ? false : (bool)attributes.nsfw;
            StatusInlist = (Status)Enum.Parse(typeof(Status), status);
            IsInList = isInList;
            Season = season;

            if (attributes.titles != null)
            {
                AlternativeTitles = string.Join(", ", ((JObject)attributes.titles).Values());
            }
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public bool IsInList { get; }

        public Status StatusInlist { get; private set; }

        public string Episodes { get; private set; }

        public string Season { get; private set; }

        public string Synopsis { get; private set; }

        public string AlternativeTitles { get; private set; }

        public string Type { get; private set; }

        public string AverageRating { get; private set; }

        public string TotalLength { get; private set; }

        public string StartDate { get; private set; }

        public string EndDate { get; private set; }

        public string AgeRating { get; private set; }

        public bool Nsfw { get; private set; }

        private string UppercaseFirst(string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        private string GetValueOrDefault(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }

        private string FormatDate(string date)
        {
            return string.IsNullOrWhiteSpace(date) ? "-" : DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("dd.MM.yyyy");
        }
    }
}
