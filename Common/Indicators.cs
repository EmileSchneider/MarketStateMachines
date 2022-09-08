namespace MarketStateMachines.Common
{

    public class Indicators
    {
        public Indicators()
        {
        }

        public Indicators(decimal bollingerBandTop, decimal bollingerBandBottom, decimal ema8, decimal ema21, decimal atr14)
        {
            BollingerBandTop = bollingerBandTop;
            BollingerBandBottom = bollingerBandBottom;
            Ema8 = ema8;
            Ema21 = ema21;
            Atr14 = atr14;
        }

        public decimal BollingerBandTop { get; set; }
        public decimal BollingerBandBottom { get; set; }
        public decimal Ema8 { get; set; }
        public decimal Ema21 { get; set; }
        public decimal Atr14 { get; set; }

        public override string ToString()
        {
            return $"{BollingerBandTop};{BollingerBandBottom};{Ema8};{Ema21};{Atr14}";
        }
    }
}