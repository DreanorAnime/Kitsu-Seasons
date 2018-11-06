using Design.Enums;
using Design.Interfaces;

namespace Design.Logic
{
    public class Season : ISeason
    {
        public Season(string name, int episodes, string imagePath, SeasonType type, AiringStatus status, double score, string airTime, AgeRating rating)
        {
            Name = name;
            ImagePath = imagePath;
            EpisodeText = $"Episodes: {episodes}";
            Type = type;
            Status = status;
            ScoreText = $"Score: {score}%";
            AiredText = $"Aired: Dec 25, 2017 to Jul 7, 2018"; //airTime
            Rating = rating;
        }

        public string ImagePath { get; }

        public string Name { get; }

        public string EpisodeText { get; }

        public SeasonType Type { get; }

        public AiringStatus Status { get; }

        public string ScoreText { get; }

        public string AiredText { get; }

        public AgeRating Rating { get; }
    }
}
