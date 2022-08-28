namespace MarketStateMachines.MeanReversion
{

    public class NoSetup : IMeanReversion
    {
        public IMeanReversion CandleTransition(Candle candle, Indicators indicators)
        {
            var touchHigh = candle.High > indicators.BollingerBandTop;
            var touchLow = candle.Low < indicators.BollingerBandBottom;

            if (touchHigh && touchLow) return new NoSetup();
            if (touchHigh)
            {
                if (candle.Open > candle.Close)
                {
                    return new RetestShort(candle.High, 0);
                }
                return new BollingerTouchShort(candle.High);
            }
            if (touchLow)
            {
                if (candle.Open < candle.Close)
                {
                    return new RetestLong(candle.Low, 0);
                }
                return new BollingerTouchLong(candle.Low);
            }
            return new NoSetup();
        }

        public IMeanReversion TickTransition(MarketTick tick)
        {
            return this;
        }
    }
}

