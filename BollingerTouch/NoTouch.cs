namespace MarketStateMachines.BollingerTouch
{
    public class NoTouch : IBollingerTouch
    {
        Indicators indicators;

        public NoTouch()
        {
        }

        public NoTouch(Indicators indicators)
        {
            this.indicators = indicators;
        }

        public IBollingerTouch TickTransition(MarketTick tick)
        {
            if (indicators == null)
                return this;

            if (tick.Ask > indicators.BollingerBandTop)
                return new TouchHigh();

            if (tick.Ask < indicators.BollingerBandBottom)
                return new TouchLow();

            return this;
        }

        public IBollingerTouch Transition(Candle candle, Indicators indicators)
        {
            this.indicators = indicators;
            var touchHigh = candle.High > indicators.BollingerBandTop;
            var touchLow = candle.Low < indicators.BollingerBandBottom;

            if (touchHigh && touchLow) return new NoTouch(indicators);
            if (touchHigh) return new TouchHigh();
            if (touchLow) return new TouchLow();

            return new NoTouch(indicators);
        }
    }
}

