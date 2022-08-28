namespace MarketStateMachines.Trend
{
    public class DowntrendPullback : Downtrend
    {
        public DowntrendPullback(decimal candleLow, decimal candleClose) : base(candleLow, candleClose)
        {
        }

        public DowntrendPullback(int counter, decimal lowestLow, decimal lowestClose, IEmaCrossDowntrend _emaCrossDowntrend) : base(counter, lowestLow, lowestClose, _emaCrossDowntrend)
        {
        }
       
        public ITrend TickTransition(MarketTick tick)
        {
            return this;
        }
    }
}