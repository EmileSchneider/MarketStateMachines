namespace MarketStateMachines.MeanReversion
{
    public class ShortSetupRTV : IMeanReversion
    {
        public decimal High { get; set; }
        public decimal Low { get; set; }

        public ShortSetupRTV(decimal high, decimal low)
        {
            High = high;
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

