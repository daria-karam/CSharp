namespace Lab7
{
    public class Sportsman
    {
        public int SportsmanNumber { get; set; }
        public int TeamID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }

        public Sportsman(int sportsmanNumber, int teamID, string surname, string name, string patronymic)
        {
            this.SportsmanNumber = sportsmanNumber;
            this.TeamID = teamID;
            this.Surname = surname;
            this.Name = name;
            this.Patronymic = patronymic;
        }
    }
}
