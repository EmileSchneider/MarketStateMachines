namespace MarketStateMachines.Trend;

public class Uptrend : ITrend
{
    private int _counter;
    private decimal _highestHigh;
    private decimal _highestClose;
    private decimal _previousBollingerBottom = decimal.MinValue;
    private decimal _previousEma8 = decimal.MinValue;

    private IEmaCrossUptrend _emaCrossUptrend;

    public Uptrend(decimal highestHigh, decimal highestClose)
    {
        _counter = 0;
        _highestHigh = highestHigh;
        _highestClose = highestClose;
        _emaCrossUptrend = new NoCrossUptrend();
    }

    public Uptrend(int counter, decimal highestHigh, decimal highestClose, IEmaCrossUptrend emaCrossUptrend)
    {
        _counter = counter;
        _highestHigh = highestHigh;
        _highestClose = highestClose;
        _emaCrossUptrend = emaCrossUptrend;
    }

    public ITrend CandleTransition(Candle candle, Indicators indicators)
    {
        var higherHigh = candle.High > _highestHigh;
        var higherClose = candle.Close > _highestClose;
        var pullback = candle.Low <= indicators.Ema8;

        var higherTouch = candle.High >= indicators.BollingerBandTop;

        _emaCrossUptrend = _emaCrossUptrend.Transition(candle, indicators);
        if (_emaCrossUptrend is UptrendStopped)
            return new Range();

        if (_counter == 20)
            return new Range();

        if (candle.Low < indicators.BollingerBandBottom)
            return new Range().CandleTransition(candle, indicators);

        if (higherHigh && higherClose)
            if (pullback)
                return new UptrendPullback(candle.High, candle.Close);
            else
                return new Uptrend(candle.High, candle.Close);

        if (higherHigh)
            if (pullback)
                return new UptrendPullback(candle.High, _highestClose);
            else
                return new Uptrend(candle.High, _highestClose);

        if (higherClose)
            if (pullback)
                return new UptrendPullback(_highestHigh, candle.Close);
            else
                return new Uptrend(_highestHigh, candle.Close);

        if (higherTouch)
            if (pullback)
                return new UptrendPullback(_highestHigh, _highestClose);
            else
                return new Uptrend(_highestHigh, _highestClose);

        if (pullback)
            return new UptrendPullback(_counter + 1, _highestHigh, _highestClose, _emaCrossUptrend);


        return new Uptrend(_counter + 1, _highestHigh, _highestClose, _emaCrossUptrend);
    }

    public ITrend TickTransition(MarketTick tick)
    {
        if (tick.Ask < _previousBollingerBottom)
            return new Range();
        if (tick.Ask < _previousEma8)
            return new UptrendPullback(_counter, _highestHigh, _highestClose, _emaCrossUptrend);
        return this;
    }
}