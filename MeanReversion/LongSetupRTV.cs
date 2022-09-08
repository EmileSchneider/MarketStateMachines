using MarketStateMachines.Common;

namespace MarketStateMachines.MeanReversion
{
    public class LongSetupRTV : IMeanReversion
    {
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public LongSetupRTV(decimal low, decimal candleHigh)
        {
            High = candleHigh;
            Low = low;
        }

        public IMeanReversion CandleTransition(Candle candle, Indicators indicators)
        {
            return new NoSetup().CandleTransition(candle, indicators);
        }

        public IMeanReversion TickTransition(MarketTick tick)
        {
            return this;
        }
    }
}