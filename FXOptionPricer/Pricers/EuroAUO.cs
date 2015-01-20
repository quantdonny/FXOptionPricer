using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer.Pricers
{
    class EuroAUO : OptionPricer
    {
        public EuroAUO()
        {
            Name = "Euro w/ AUO";
            myListOfParaToValue.Add(new ParaValuePair("UpAndOut", 1.25));
        }

        public override KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>> getOptionPrice(List<List<double>> inListOfPricePath)
        {
            List<KeyValuePair<double, double>> CallPrices = new List<KeyValuePair<double, double>>();
            List<KeyValuePair<double, double>> PutPrices = new List<KeyValuePair<double, double>>();

            double InitialStrike = CurrentUnderlyingPrice - NoOfStrikeIncrementSteps * IncrementInTicks * myTickValue;
            for (int i = 0; i < (2 * NoOfStrikeIncrementSteps + 1); i++)
            {
                double Strike = InitialStrike + i * IncrementInTicks * myTickValue;
                List<double> myListOfCall = new List<double>();
                List<double> myListOfPut = new List<double>();
                double KnockOut = myListOfParaToValue[0].Value;
                foreach (List<double> aPricePath in inListOfPricePath)
                {
                    double EndingPrice = aPricePath[aPricePath.Count - 1];
                    double CallValue = (aPricePath.Where(item => item > KnockOut).Count() > 0) ? 0 : Math.Max(0, EndingPrice - Strike);
                    double PutValue = (aPricePath.Where(item => item > KnockOut).Count() > 0) ? 0 : Math.Max(0, Strike - EndingPrice);
                    myListOfCall.Add(CallValue);
                    myListOfPut.Add(PutValue);
                }
                CallPrices.Add(new KeyValuePair<double, double>(Strike, myListOfCall.Average()));
                PutPrices.Add(new KeyValuePair<double, double>(Strike, myListOfPut.Average()));
            }
            return new KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>>(CallPrices, PutPrices);
        }

        public override void updatedTickAndInitial(double inTickValue, double inInitial)
        {
            myListOfParaToValue[0].Value = inInitial;
        }
    }
}
