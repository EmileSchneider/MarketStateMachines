namespace MarketStateMachines.MeanReversion
{
    public class RetestShort : IMeanReversion
    {
        private decimal _high;
        private decimal _counter;

        public RetestShort(decimal high, decimal counter)
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

            if (candle.Shootingstar())
                return new ShortSetupRTC(_high, candle.Low);

            if (candle.Close > candle.Open)
                return new ShortSetupRTV(_high, candle.Low);

            return new RetestShort(_high, _counter + 1);
        }
    }
}

