namespace MarketStateMachines.Trend
{
    public class UptrendPullback : Uptrend 
    {
        private int _counter;
        private decimal _highestHigh;
        private decimal _highestClose;

        private IEmaCrossUptrend _emaCrossUptrend;

        public UptrendPullback(decimal highestHigh, decimal highestClose) : base(highestHigh, highestClose)
        {
        }

        public UptrendPullback(int counter, decimal highestHigh, decimal highestClose, IEmaCrossUptrend emaCrossUptrend) : base(counter, highestHigh, highestClose, emaCrossUptrend)
        {
        }

             public ITrend TickTransition(MarketTick tick)
        {
            return this;
        }
    }
}