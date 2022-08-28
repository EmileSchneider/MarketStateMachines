namespace MarketStateMachines
{


    public class Candle
    {
        public DateTime OpenTime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }

        public bool Hammer()
        {

            var body = Math.Abs(Open - Close);
            var wick = Math.Min(Open, Close) - Low;
            if (body * 2 <= wick)
                if (Close > ((High + Low) / 2))
                    return true;
            return false;
        }

        public bool Shootingstar()
        {
            var body = Math.Abs(Open - Close);
            var wick = High - Math.Max(Open, Close);

            if (body * 2 <= wick)
                if (Close < ((High + Low) / 2))
                    return true;
            return false;
        }

        public override string ToString()
        {
            return $"{OpenTime};{Open};{High};{Low};{Close};Hammer: {Hammer()};Shootingstar: {Shootingstar()}";
        }
    }
}