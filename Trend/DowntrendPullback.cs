namespace MarketStateMachines.Trend
{
    public class DowntrendPullback : Downtrend
    {
        public DowntrendPullback(decimal candleLow, decimal candleClose, decimal previousUpperBollinger, decimal previousEma8) : base(candleLow, candleClose, previousUpperBollinger, previousEma8)
        {
        }

        public DowntrendPullback(int counter, decimal lowestLow, decimal lowestClose, IEmaCrossDowntrend _emaCrossDowntrend, decimal previousUpperBollinger, decimal previousEma8) : base(counter, lowestLow, lowestClose, _emaCrossDowntrend, previousUpperBollinger, previousEma8)
        {
        }

        public ITrend TickTransition(MarketTick tick)
        {
            return this;
        }
    }
}