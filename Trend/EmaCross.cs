using MarketStateMachines.Common;

namespace MarketStateMachines.Trend;

public interface IEmaCrossUptrend
{
    public IEmaCrossUptrend Transition(Candle candle, Indicators indicators);
}

public interface IEmaCrossDowntrend
{
    public IEmaCrossDowntrend Transition(Candle candle, Indicators indicators);
}

public class NoCrossDowntrend : IEmaCrossDowntrend
{
    public IEmaCrossDowntrend Transition(Candle candle, Indicators indicators)
    {
        if (candle.Close > indicators.Ema8 && candle.Close > indicators.Ema21)
        {
            return new CrossAboveDowntrend();
        }

        return new NoCrossDowntrend();
    }
}

public class CrossAboveDowntrend : IEmaCrossDowntrend
{
    public IEmaCrossDowntrend Transition(Candle candle, Indicators indicators)
    {
        if (candle.Close < indicators.Ema8 && candle.Close < indicators.Ema21)
        {
            return new CrossBelowDowntrend();
        }

        return new CrossAboveDowntrend();
    }
}

public class CrossBelowDowntrend : IEmaCrossDowntrend
{
    public IEmaCrossDowntrend Transition(Candle candle, Indicators indicators)
    {
        if (candle.Close > indicators.Ema8 && candle.Close > indicators.Ema21)
        {
            return new DowntrendStopped();
        }

        return new CrossBelowDowntrend();
    }
}

public class DowntrendStopped : IEmaCrossDowntrend
{
    public IEmaCrossDowntrend Transition(Candle candle, Indicators indicators)
    {
        return new NoCrossDowntrend();
    }
}

public class NoCrossUptrend : IEmaCrossUptrend
{
    public IEmaCrossUptrend Transition(Candle candle, Indicators indicators)
    {
        if (candle.Close < indicators.Ema8 && candle.Close < indicators.Ema21)
        {
            return new CrossBelowUptrend();
        }
        return new NoCrossUptrend();
    }
}

public class CrossBelowUptrend : IEmaCrossUptrend
{
    public IEmaCrossUptrend Transition(Candle candle, Indicators indicators)
    {
        if (candle.Close > indicators.Ema8 && candle.Close > indicators.Ema21)
        {
            return new CrossAboveUptrend();
        }

        return new CrossBelowUptrend();
    }
}

public class CrossAboveUptrend : IEmaCrossUptrend
{
    public IEmaCrossUptrend Transition(Candle candle, Indicators indicators)
    {
        if (candle.Close < indicators.Ema8 && candle.Close < indicators.Ema21)
        {
            return new UptrendStopped();
        }

        return new CrossAboveUptrend();
    }
}

public class UptrendStopped : IEmaCrossUptrend
{
    public IEmaCrossUptrend Transition(Candle candle, Indicators indicators)
    {
        return new NoCrossUptrend();
    }
}