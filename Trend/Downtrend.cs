namespace MarketStateMachines.Trend;

public class Downtrend : ITrend
{
    private int _counter;
    private decimal _lowestLow;
    private decimal _lowestClose;
    private decimal _previousUpperBollinger = decimal.MaxValue;
    private decimal _previousEma8 = decimal.MaxValue;

    private IEmaCrossDowntrend _emaCrossDowntrend;
    public Downtrend(decimal candleLow, decimal candleClose)
    {
        _lowestLow = candleLow;
        _lowestClose = candleClose;
        _emaCrossDowntrend = new NoCrossDowntrend();
    }

    public Downtrend(int counter, decimal lowestLow, decimal lowestClose, IEmaCrossDowntrend _emaCrossDowntrend)
    {
        _counter = counter;
        _lowestLow = lowestLow;
        _lowestClose = lowestClose;
        this._emaCrossDowntrend = _emaCrossDowntrend;
    }

    public ITrend CandleTransition(Candle candle, Indicators indicators)
    {
        var lowerLow = candle.Low < _lowestLow;
        var lowerClose = candle.Close < _lowestClose;
        var lowerTouch = candle.Low < indicators.BollingerBandBottom; 
        var pullback = candle.High >= indicators.Ema8;

        _emaCrossDowntrend = _emaCrossDowntrend.Transition(candle, indicators);
        _previousUpperBollinger = indicators.BollingerBandTop; 

        if (_emaCrossDowntrend is DowntrendStopped)
            return new Range();

        if (candle.High >= indicators.BollingerBandTop)
            return new Range().CandleTransition(candle, indicators);        

        if (_counter == 20)
            return new Range();

        if (lowerLow && lowerClose)
            if (pullback)
                return new DowntrendPullback(candle.Low, candle.Close);
            else
                return new Downtrend(candle.Low, candle.Close);

        if (lowerLow)
            if (pullback)
                return new DowntrendPullback(candle.Low, _lowestClose);
            else
                return new Downtrend(candle.Low, _lowestClose);

        if (lowerClose)
            if (pullback)
                return new DowntrendPullback(_lowestLow, candle.Close);
            else
                return new Downtrend(_lowestLow, candle.Close);

        if (lowerTouch)
            if (pullback)
                return new DowntrendPullback(_lowestLow, _lowestClose);
            else
                return new Downtrend(_lowestLow, _lowestClose);

        if (pullback)
            return new DowntrendPullback(_counter + 1, _lowestLow, _lowestClose, _emaCrossDowntrend);

        return new Downtrend(_counter + 1, _lowestLow, _lowestClose, _emaCrossDowntrend);
    }

    public ITrend TickTransition(MarketTick tick)
    {
        if (tick.Ask >= _previousUpperBollinger)
            return new Range();
        return this;
    }
}
