namespace MarketStateMachines.BollingerWalk
{
    public class BollingerWalkDown : IBollingerWalk
    {
        public IBollingerWalk Transition(Candle candle, Indicators indicators)
        {
            if (candle.Low >= indicators.BollingerBandBottom)
                return new NoWalk();
            return new BollingerWalkDown();
        }
    }
}