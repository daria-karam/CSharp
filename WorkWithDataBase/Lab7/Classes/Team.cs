namespace Lab7
{
    public class Team
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public string City { get; set; }
        public int SportsmanCount { get; set; }
        public string Trainer { get; set; }

        public Team(int teamID, string teamName, string city, int sportsmanCount, string trainer)
        {
            this.TeamID = teamID;
            this.TeamName = teamName;
            this.City = city;
            this.SportsmanCount = sportsmanCount;
            this.Trainer = trainer;
        }
    }
}
