using MarketStateMachines.Common;

namespace MarketStateMachines.MeanReversion
{

    public class BollingerTouchShort : IMeanReversion
    {
        private decimal _high;
        private int _counter;

        public BollingerTouchShort(decimal high)
        {
            _high = high;
            _counter = 0;
        }

        public BollingerTouchShort(decimal high, int counter)
        {
            _high = high;
            _counter = counter;
        }

        public IMeanReversion TickTransition(MarketTick tick)
        {
            if (tick.Ask > _high || tick.Bid > _high)
            {
                return new NoSetup();
            }
            return this;
        }

        public IMeanReversion CandleTransition(Candle candle, Indicators indicators)
        {
            if (_counter == 20) return new NoSetup();

            if (candle.High > _high)
                return new NoSetup().CandleTransition(candle, indicators);

            if (candle.Low <= indicators.BollingerBandBottom)
                return new NoSetup().CandleTransition(candle, indicators);

            if (_counter <= 5)
                if (candle.Shootingstar())
                    return new ShortSetupRTC(_high, candle.Low);

            if (candle.Close < candle.Open)
                return new RetestShort(_high, _counter + 1);


            return new BollingerTouchShort(_high, _counter + 1);
        }
    }
}