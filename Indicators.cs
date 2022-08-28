namespace MarketStateMachines
{

    public class Indicators
    {
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