namespace MarketStateMachines.BollingerWalk
{
    public class BollingerWalkUp : IBollingerWalk
    {
        public IBollingerWalk Transition(Candle candle, Indicators indicators)
        {
            if (candle.High <= indicators.BollingerBandTop)
                return new NoWalk();
            return new BollingerWalkUp();
        }
    }
}