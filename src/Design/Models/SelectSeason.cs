using Design.Interfaces;
using Kitsu;
using System.Globalization;

namespace Design.Models
{
    public class SelectSeason : ISelectSeason
    {
        public SelectSeason(Season seasonDisplay, int year)
        {
            SeasonDisplay = seasonDisplay;
            Year = year;
        }

        public Season SeasonDisplay { get; }

        public int Year { get; }

        public override string ToString()
        {
            return $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SeasonDisplay.ToString().ToLower())} - {Year}";
        }
    }
}
