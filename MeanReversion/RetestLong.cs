using MarketStateMachines.Common;

namespace MarketStateMachines.MeanReversion
{
    public class RetestLong : IMeanReversion
    {
        private decimal _low;
        private int _counter;
        public RetestLong(decimal low, int counter)
        {
            _low = low;
            _counter = counter;
        }
        public IMeanReversion TickTransition(MarketTick tick)
        {
            if (tick.Ask < _low || tick.Bid < _low)
            {
                return new NoSetup();
            }
            return this;
        }
        public IMeanReversion CandleTransition(Candle candle, Indicators indicators)
        {
            if (_counter == 20) return new NoSetup();

            if (candle.Low < _low)
                return new NoSetup().CandleTransition(candle, indicators);

            if (candle.High >= indicators.BollingerBandTop)
                return new NoSetup().CandleTransition(candle, indicators);

            if (candle.Hammer())
                return new LongSetupRTC(_low, candle.High);

            if (candle.Close < candle.Open)
                return new LongSetupRTV(_low, candle.High);
            return new RetestLong(_low, _counter + 1);
        }
    }
}