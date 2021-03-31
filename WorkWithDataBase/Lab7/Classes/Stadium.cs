namespace Lab7
{
    public class Stadium
    {
        public int StadiumID { get; set; }
        public string StadiumName { get; set; }
        public string StadiumAddress { get; set; }
        public int StadiumCapacity { get; set; }

        public Stadium(int stadiumID, string stadiumName, string stadiumAddress, int stadiumCapacity)
        {
            this.StadiumID = stadiumID;
            this.StadiumName = stadiumName;
            this.StadiumAddress = stadiumAddress;
            this.StadiumCapacity = stadiumCapacity;
        }
    }
}
