using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer.Pricers
{
    class European : OptionPricer
    {
        public European()
        {
            Name = "European";
        }

        public override KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>> getOptionPrice(List<List<double>> inListOfPricePath)
        {
            List<KeyValuePair<double, double>> CallPrices = new List<KeyValuePair<double, double>>();
            List<KeyValuePair<double, double>> PutPrices = new List<KeyValuePair<double, double>>();

            double InitialStrike = CurrentUnderlyingPrice - NoOfStrikeIncrementSteps*IncrementInTicks*myTickValue;
            for (int i = 0; i < (2 * NoOfStrikeIncrementSteps + 1); i++)
            {
                double Strike = InitialStrike + i * IncrementInTicks * myTickValue;
                List<double> myListOfCall = new List<double>();
                List<double> myListOfPut = new List<double>();
                foreach (List<double> aPricePath in inListOfPricePath)
                {
                    double EndingPrice = aPricePath[aPricePath.Count - 1];
                    myListOfCall.Add( Math.Max(0,EndingPrice - Strike));
                    myListOfPut.Add( Math.Max(0,Strike - EndingPrice));
                }
                CallPrices.Add( new KeyValuePair<double,double>( Strike, myListOfCall.Average()) );
                PutPrices.Add(new KeyValuePair<double, double>(Strike, myListOfPut.Average()));
            }
            return new KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>>(CallPrices, PutPrices);
        }

        public override void updatedTickAndInitial(double inTickValue, double inInitial)
        {
        }
    }
}
