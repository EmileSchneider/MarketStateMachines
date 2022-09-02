namespace MarketStateMachines.BollingerTouch
{
    public interface IBollingerTouch
    {
        public IBollingerTouch Transition(Candle candle, Indicators indicators);
        public IBollingerTouch TickTransition(MarketTick tick);
    }
}