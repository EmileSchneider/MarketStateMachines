using MarketStateMachines.Common;

namespace MarketStateMachines.Trend;

public interface ITrend
{
    public ITrend CandleTransition(Candle candle, Indicators indicators);
    public ITrend TickTransition(MarketTick tick);
}