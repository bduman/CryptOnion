namespace CryptOnion
{
    public class Ticker
    {
        public Ticker(Pair pair)
        {
            this.Pair = pair;
        }

        public Pair Pair { get; }
        public decimal LowestAsk { get; set; }
        public decimal HighestBid { get; set; }
        public decimal Last { get; set; }
        public decimal Volume { get; set; }
        public decimal Change { get; set; }
    }
}
