namespace Design.Interfaces
{
    public interface ISeason
    {
        string Image { get; }

        string Name { get; }

        string EpisodeText { get; } //Episodes: xx

        string Type { get; } //TV, OVA...

        string Status { get; } //Airing, Finished...

        string Score { get; } //Score: 80.33%

        string Aired { get; } //Aired: Dec 25, 2017 to Jul 7, 2018

        string Rating { get; } //PG, R...
    }
}
