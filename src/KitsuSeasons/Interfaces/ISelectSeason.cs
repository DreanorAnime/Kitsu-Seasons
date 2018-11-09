using Kitsu;

namespace KitsuSeasons.Interfaces
{
    public interface ISelectSeason
    {
        Season SeasonDisplay { get; }

        int Year { get; }
    }
}
