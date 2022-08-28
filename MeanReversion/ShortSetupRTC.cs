namespace MarketStateMachines.MeanReversion
{
    public class ShortSetupRTC : IMeanReversion
    {
        public ShortSetupRTC(decimal high, decimal low)
        {
            High = high;
            Low = low;
        }

        public decimal High { get; }
        public decimal Low { get; }

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