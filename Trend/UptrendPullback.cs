using MarketStateMachines.Common;

namespace MarketStateMachines.Trend
{
    public class UptrendPullback : Uptrend 
    {
        private int _counter;
        private decimal _highestHigh;
        private decimal _highestClose;

        private IEmaCrossUptrend _emaCrossUptrend;

        public UptrendPullback(decimal highestHigh, decimal highestClose, decimal previousBollingerBandBottomm, decimal previousEma8) : base(highestHigh, highestClose, previousBollingerBandBottomm, previousEma8)
        {
        }

        public UptrendPullback(int counter, decimal highestHigh, decimal highestClose, IEmaCrossUptrend emaCrossUptrend, decimal previousBollingerBandBottom, decimal previousEma8) : base(counter, highestHigh, highestClose, emaCrossUptrend, previousBollingerBandBottom, previousEma8)
        {
        }

        public ITrend TickTransition(MarketTick tick)
        {
            return this;
        }
    }
}