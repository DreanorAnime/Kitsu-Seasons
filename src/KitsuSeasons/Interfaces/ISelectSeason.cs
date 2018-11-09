using Kitsu;

namespace Design.Interfaces
{
    public interface ISelectSeason
    {
        Season SeasonDisplay { get; }

        int Year { get; }
    }
}
