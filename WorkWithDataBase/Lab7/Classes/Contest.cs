namespace Lab7
{
    public class Contest
    {
        public int ContestID { get; set; }
        public int SportID { get; set; }
        public int StadiumID { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }

        public Contest(int contestID, int sportID, int stadiumID, string beginDate, string endDate)
        {
            this.ContestID = contestID;
            this.SportID = sportID;
            this.StadiumID = stadiumID;
            this.BeginDate = beginDate;
            this.EndDate = endDate;
        }
    }
}
