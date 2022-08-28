namespace MarketStateMachines.BollingerTouch
{
    public class NoTouch : IBollingerTouch
    {
        public IBollingerTouch Transition(Candle candle, Indicators indicators)
        {
            var touchHigh = candle.High > indicators.BollingerBandTop;
            var touchLow = candle.Low < indicators.BollingerBandBottom;

            if (touchHigh && touchLow) return new NoTouch();
            if (touchHigh) return new TouchHigh();
            if (touchLow) return new TouchLow();

            return new NoTouch();
        }
    }
}

