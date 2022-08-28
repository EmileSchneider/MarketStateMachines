namespace MarketStateMachines.Trend;

public class Range : ITrend
{
    public ITrend CandleTransition(Candle candle, Indicators indicators)
    {
        var touchHigh = candle.High > indicators.BollingerBandTop;
        var touchLow = candle.Low < indicators.BollingerBandBottom;
        if (touchHigh && touchLow)
        {
            return new Range();
        }

        if (touchHigh) return new ExpectingUptrend(candle.High, candle.Close);
        if (touchLow) return new ExpectingDowntrend(candle.Low, candle.Close);
        return new Range();
    }

    public ITrend TickTransition(MarketTick tick)
    {
        return this;
    }
}
