using MarketStateMachines.Common;

namespace MarketStateMachines.Trend;

public class ExpectingUptrend : ITrend
{
    private decimal _high;
    private decimal _close;
    private int _counter;

    public ExpectingUptrend(decimal high, decimal close)
    {
        _high = high;
        _close = close;
        _counter = 0;
    }

    public ExpectingUptrend(decimal high, decimal close, int counter)
    {
        _high = high;
        _close = close;
        _counter = counter;
    }

    public ITrend CandleTransition(Candle candle, Indicators indicators)
    {
        var pullback = candle.Low <= indicators.Ema8;
        if (_counter > 20)
            return new Range();

        if (candle.Low < indicators.BollingerBandBottom)
            return new ExpectingDowntrend(candle.Low, candle.Close);

        if (candle.High > _high && candle.Close > _close)
            if (pullback)
                return new UptrendPullback(candle.High, candle.Close, indicators.BollingerBandBottom, indicators.Ema8);
            else 
                return new Uptrend(candle.High, candle.Close, indicators.BollingerBandBottom, indicators.Ema8);

        if (candle.High > indicators.BollingerBandTop)
            return new ExpectingUptrend(candle.High, candle.Close);

        return new ExpectingUptrend(_high, _close, _counter + 1); 
    }

    public ITrend TickTransition(MarketTick tick)
    {
        return this;
    }
}
