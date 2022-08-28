namespace MarketStateMachines.MeanReversion
{

    public class BollingerTouchLong : IMeanReversion
    {
        private decimal _low;
        private int _counter;

        public BollingerTouchLong(decimal low)
        {
            _low = low;
        }

        public BollingerTouchLong(decimal low, int counter)
        {
            _low = low;
            _counter = counter;
        }


        public IMeanReversion CandleTransition(Candle candle, Indicators indicators)
        {
            if (_counter == 20) return new NoSetup();

            if (candle.Low < _low)
                return new NoSetup().CandleTransition(candle, indicators);
 
            if (candle.High >= indicators.BollingerBandTop)
                return new NoSetup().CandleTransition(candle, indicators);

            if (_counter <= 5)
                if (candle.Hammer())
                    return new LongSetupRTC(_low, candle.High);

            if (candle.Close > candle.Open)
                return new RetestLong(_low, _counter + 1);

           
            return new BollingerTouchLong(_low, _counter + 1);
        }

        public IMeanReversion TickTransition(MarketTick tick)
        {
            if (tick.Ask < _low || tick.Bid < _low)
            {
                return new NoSetup();
            }
            return this;

        }
    }
}

