using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStateMachines.DV_Mode
{
    public class PivotTwoHighRecognizer
    {

        Queue<decimal> queue;
        decimal pivotTwoHigh;
        public PivotTwoHighRecognizer()
        {
            queue = new Queue<decimal>();
            pivotTwoHigh = decimal.MaxValue;
        } 
        private void CheckNewHigh()
        {
            var l = queue.ToList();
            if (l.Count != 5)
                return;
            var mid = l[2];
            if (mid > l[0] && mid > l[1] && mid > l[3] && mid > l[4])
                pivotTwoHigh = mid;
        }
        private void Enqueue(decimal atr)
        {
            if (queue.Count == 5)
                queue.Dequeue();
            queue.Enqueue(atr);
            CheckNewHigh();
        }
        public void NewAtr(decimal atr)
        {
            Enqueue(atr);
        }
        public decimal CurrentPivotTwoHigh()
        {
            return pivotTwoHigh;
        }
    }
}
