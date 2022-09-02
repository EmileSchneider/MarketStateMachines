namespace MarketStateMachines.Trend;

public class Uptrend : ITrend
{
    private int _counter;
    private decimal _highestHigh;
    private decimal _highestClose;
    private decimal _previousBollingerBottom = decimal.MinValue;
    private decimal _previousEma8 = decimal.MinValue;

    private IEmaCrossUptrend _emaCrossUptrend;

    public Uptrend(decimal highestHigh, decimal highestClose, decimal previousBollingerBandBottomm, decimal previousEma8)
    {
        _counter = 0;
        _highestHigh = highestHigh;
        _highestClose = highestClose;
        _emaCrossUptrend = new NoCrossUptrend();
        _previousEma8 = previousEma8;
        _previousBollingerBottom = previousBollingerBandBottomm;
    }

    public Uptrend(int counter, decimal highestHigh, decimal highestClose, IEmaCrossUptrend emaCrossUptrend, decimal previousBollingerBandBottom, decimal previousEma8)
    {
        _counter = counter;
        _highestHigh = highestHigh;
        _highestClose = highestClose;
        _emaCrossUptrend = emaCrossUptrend;
        _previousBollingerBottom = previousBollingerBandBottom;
        _previousEma8= previousEma8;

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
                return new UptrendPullback(candle.High, candle.Close, indicators.BollingerBandBottom, indicators.Ema8);
            else
                return new Uptrend(candle.High, candle.Close, indicators.BollingerBandBottom, indicators.Ema8);

        if (higherHigh)
            if (pullback)
                return new UptrendPullback(candle.High, _highestClose, indicators.BollingerBandBottom, indicators.Ema8);
            else
                return new Uptrend(candle.High, _highestClose, indicators.BollingerBandBottom, indicators.Ema8);

        if (higherClose)
            if (pullback)
                return new UptrendPullback(_highestHigh, candle.Close, indicators.BollingerBandBottom, indicators.Ema8);
            else
                return new Uptrend(_highestHigh, candle.Close, indicators.BollingerBandBottom, indicators.Ema8);

        if (higherTouch)
            if (pullback)
                return new UptrendPullback(_highestHigh, _highestClose, indicators.BollingerBandBottom, indicators.Ema8);
            else
                return new Uptrend(_highestHigh, _highestClose, indicators.BollingerBandBottom, indicators.Ema8);

        if (pullback)
            return new UptrendPullback(_counter + 1, _highestHigh, _highestClose, _emaCrossUptrend, indicators.BollingerBandBottom, indicators.Ema8);


        return new Uptrend(_counter + 1, _highestHigh, _highestClose, _emaCrossUptrend, indicators.BollingerBandBottom, indicators.Ema8);
    }

    public ITrend TickTransition(MarketTick tick)
    {
        if (tick.Ask < _previousBollingerBottom)
            return new Range();
        if (tick.Ask < _previousEma8)
            return new UptrendPullback(_counter, _highestHigh, _highestClose, _emaCrossUptrend, _previousBollingerBottom, _previousEma8) ;
        return this;
    }
}