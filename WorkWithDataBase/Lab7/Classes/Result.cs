namespace Lab7
{
    public class Result
    {
        public int AttemptNumber { get; set; }
        public int ContestID { get; set; }
        public int TeamID { get; set; }
        public string Date { get; set; }
        public int AttemptResult { get; set; }

        public Result(int attemptNumber, int contestID, int teamID, string date, int attemptResult)
        {
            this.AttemptNumber = attemptNumber;
            this.ContestID = contestID;
            this.TeamID = teamID;
            this.Date = date;
            this.AttemptResult = attemptResult;
        }
    }
}
