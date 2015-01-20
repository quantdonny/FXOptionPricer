using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer.Pricers
{
    class American : OptionPricer
    {
        public American()
        {
            Name = "American";
        }

        public override KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>> getOptionPrice(List<List<double>> inListOfPricePath)
        {
            List<KeyValuePair<double, double>> CallPrices = new List<KeyValuePair<double, double>>();
            List<KeyValuePair<double, double>> PutPrices = new List<KeyValuePair<double, double>>();

            double InitialStrike = CurrentUnderlyingPrice - NoOfStrikeIncrementSteps * IncrementInTicks * myTickValue;
            
            for (int i = 0; i < (2 * NoOfStrikeIncrementSteps + 1); i++)
            {
                double Strike = InitialStrike + i * IncrementInTicks * myTickValue;
                int LengthOfPricePath = inListOfPricePath[0].Count;
                // Matrix Of Stops.
                List<List<double>> myMatrixOfStopCall = new List<List<double>>();
                List<List<double>> myMatrixOfStopPut = new List<List<double>>();
                foreach (List<double> aPricePath in inListOfPricePath) {
                    List<double> aListOfMatrixScoreCall = new List<double>();
                    List<double> aListOfMatrixScorePut = new List<double>();
                    for (int l = 0; l < aPricePath.Count; l++ ) {
                        aListOfMatrixScoreCall.Add(0);
                        aListOfMatrixScorePut.Add(0);
                    }
                    myMatrixOfStopCall.Add(aListOfMatrixScoreCall);
                    myMatrixOfStopPut.Add(aListOfMatrixScorePut);
                }

                // Initialize.
                for (int k = 0; k < myMatrixOfStopCall.Count; k++)
                {
                    myMatrixOfStopCall[k][LengthOfPricePath - 1] = Math.Max(0, inListOfPricePath[k][LengthOfPricePath - 1] - Strike);
                    myMatrixOfStopPut[k][LengthOfPricePath - 1] = Math.Max(0, Strike - inListOfPricePath[k][LengthOfPricePath - 1]);
                }
                // Recursive.
                for (int j = (LengthOfPricePath - 2); j >= 0; j--)
                {
                    List<double> aListOfPayOffCall = new List<double>();
                    List<double> aListOfPayOffPut = new List<double>();
                    List<double> aListOfPriceCall = new List<double>();
                    List<double> aListOfPricePut = new List<double>();
                    for (int k = 0; k < myMatrixOfStopCall.Count; k++)
                    {
                        if (inListOfPricePath[k][j] > Strike)       // Condition to consider exercising.
                        {
                            aListOfPayOffCall.Add(myMatrixOfStopCall[k][j + 1]);
                            aListOfPriceCall.Add(inListOfPricePath[k][j]);
                        }
                        if (inListOfPricePath[k][j] < Strike)
                        {
                            aListOfPayOffPut.Add(myMatrixOfStopPut[k][j + 1]);
                            aListOfPricePut.Add(inListOfPricePath[k][j]);
                        }
                    }

                    if (aListOfPayOffCall.Count > 0)
                    {
                        // Do the regression.
                        double[] outputsCall = aListOfPayOffCall.ToArray();
                        double[][] inputsCall = new double[aListOfPriceCall.Count][];
                        for (int m = 0; m < aListOfPriceCall.Count; m++)
                            inputsCall[m] = new double[] { aListOfPriceCall[m], aListOfPriceCall[m] * aListOfPriceCall[m] };
                        Accord.Statistics.Models.Regression.Linear.MultipleLinearRegression targetCall = new Accord.Statistics.Models.Regression.Linear.MultipleLinearRegression(2, true);
                        double ErrorCall = targetCall.Regress(inputsCall, outputsCall);

                        // Now populate myMatrixOfStop at j.
                        for (int k = 0; k < myMatrixOfStopCall.Count; k++)
                        {
                            if (inListOfPricePath[k][j] > Strike)       // Condition to consider exercising.
                            {
                                double ExpPayoff = targetCall.Compute(new double[] { inListOfPricePath[k][j], inListOfPricePath[k][j] * inListOfPricePath[k][j] });
                                if (ExpPayoff > myMatrixOfStopCall[k][j + 1])
                                {
                                    myMatrixOfStopCall[k][j] = ExpPayoff;
                                    myMatrixOfStopCall[k][j + 1] = 0;
                                }

                            }
                        }
                    }

                    if (aListOfPayOffPut.Count > 0)
                    {
                        double[] outputsPut = aListOfPayOffPut.ToArray();
                        double[][] inputsPut = new double[aListOfPricePut.Count][];
                        for (int m = 0; m < aListOfPricePut.Count; m++)
                            inputsPut[m] = new double[] { aListOfPricePut[m], aListOfPricePut[m] * aListOfPricePut[m] };
                        Accord.Statistics.Models.Regression.Linear.MultipleLinearRegression targetPut = new Accord.Statistics.Models.Regression.Linear.MultipleLinearRegression(2, true);
                        double ErrorPut = targetPut.Regress(inputsPut, outputsPut);

                        for (int k = 0; k < myMatrixOfStopPut.Count; k++)
                        {
                            if (inListOfPricePath[k][j] < Strike)       // Condition to consider exercising.
                            {
                                double ExpPayoffPut = targetPut.Compute(new double[] { inListOfPricePath[k][j], inListOfPricePath[k][j] * inListOfPricePath[k][j] });
                                if (ExpPayoffPut > myMatrixOfStopPut[k][j + 1])
                                {
                                    myMatrixOfStopPut[k][j] = ExpPayoffPut;
                                    myMatrixOfStopPut[k][j + 1] = 0;
                                }
                            }
                        }
                    }
                }
                
                double CallPrice = myMatrixOfStopCall.Sum(item => item.Max()) / myMatrixOfStopCall.Count;
                double PutPrice = myMatrixOfStopPut.Sum(item => item.Max()) / myMatrixOfStopPut.Count;
                CallPrices.Add(new KeyValuePair<double, double>(Strike, CallPrice));
                PutPrices.Add(new KeyValuePair<double, double>(Strike, PutPrice));

            }
            return new KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>>(CallPrices, PutPrices);
        }
        
        public override void updatedTickAndInitial(double inTickValue, double inInitial)
        {
        }
    }
}
