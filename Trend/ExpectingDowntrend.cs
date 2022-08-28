﻿namespace MarketStateMachines.Trend;

public class ExpectingDowntrend : ITrend
{
    private int _counter;
    private decimal _low;
    private decimal _close;

    public ExpectingDowntrend(decimal low, decimal close)
    {
        _low = low;
        _close = close;
        _counter = 0;
    }

    public ExpectingDowntrend(int counter, decimal low, decimal close)
    {
        _counter = counter;
        _low = low;
        _close = close;
    }

    public ITrend CandleTransition(Candle candle, Indicators indicators)
    {
        var touchHigh = candle.High > indicators.BollingerBandTop;
        var touchLow = candle.Low < indicators.BollingerBandBottom;
        if (touchHigh)
        {
            return new ExpectingUptrend(candle.High, candle.Close);
        }

        if (_counter == 20)
            return new Range();

        if (candle.Low < _low && candle.Close < _close)
        {
            return new Downtrend(candle.Low, candle.Close);
        }

        return new ExpectingDowntrend(_counter + 1, _low, _close);
    }

    public ITrend TickTransition(MarketTick tick)
    {
        return this;
    }
}