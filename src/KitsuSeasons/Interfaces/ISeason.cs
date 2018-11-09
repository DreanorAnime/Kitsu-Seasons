using Design.Enums;

namespace Design.Interfaces
{
    public interface ISeasonEntry
    {
        string ImagePath { get; }

        string Name { get; }

        string EpisodeText { get; }

        SeasonType Type { get; } 

        AiringStatus Status { get; }

        string ScoreText { get; }

        string AiredText { get; } 

        AgeRating Rating { get; }
    }
}
