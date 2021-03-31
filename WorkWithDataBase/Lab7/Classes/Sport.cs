namespace Lab7
{
    public class Sport
    {
        public int SportID { get; set; }
        public string SportName { get; set; }

        public Sport(int sportID, string sportName)
        {
            this.SportID = sportID;
            this.SportName = sportName;
        }
    }
}
