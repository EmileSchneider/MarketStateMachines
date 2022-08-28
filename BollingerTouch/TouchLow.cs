namespace MarketStateMachines.BollingerTouch
{

    public class TouchLow : IBollingerTouch
    {
        private readonly int _counter;

        public TouchLow()
        {
            _counter = 1;
        }

        public TouchLow(int counter)
        {
            _counter = counter;
        }

        public IBollingerTouch Transition(Candle candle, Indicators indicators)
        {
            var touchHigh = candle.High > indicators.BollingerBandTop;
            var touchLow = candle.Low < indicators.BollingerBandBottom;

            if (touchHigh && touchLow) return new NoTouch();
            if (touchHigh) return new TouchHigh();
            if (touchLow) return new TouchLow();
            if (_counter == 3) return new NoTouch();

            return new TouchLow(_counter + 1);
        }
    }
}