using MarketStateMachines.Common;

namespace MarketStateMachines.MeanReversion
{


    public interface IMeanReversion
    {
        public IMeanReversion CandleTransition(Candle candle, Indicators indicators);

        public IMeanReversion TickTransition(MarketTick tick);
    }
}