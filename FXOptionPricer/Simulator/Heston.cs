using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer.Simulator
{
    class Heston : PriceProcess
    {
        public Heston()
        {
            Name = "Heston";
            myListOfParaToValue.Add(new ParaValuePair("Mean", 0.01));
            myListOfParaToValue.Add(new ParaValuePair("LongVar", 0.002));
            myListOfParaToValue.Add(new ParaValuePair("Reversion", 0.1));
            myListOfParaToValue.Add(new ParaValuePair("VolOfVol", 0.0005));
            myListOfParaToValue.Add(new ParaValuePair("IniVar", 0.002));
            myListOfParaToValue.Add(new ParaValuePair("Rho", 0.1));
        }

        public override List<double> getPricePath(int inSeed)
        {
            Random randVariance = new Random(inSeed);
            List<double> myListOfValues = new List<double>();
            double CurrentValue = InitialValue;
            double CurrentVariance = myListOfParaToValue[4].Value / 10.0;
            myListOfValues.Add(CurrentValue);
            for (int i = 0; i < (MainView.ProcessDistance - 1); i++)
            {
                // Variance evolution.
                double randVarianceValue = randVariance.NextDouble();
                double ChangeVariance = myListOfParaToValue[2].Value * (myListOfParaToValue[1].Value / 10.0 - CurrentVariance);
                if (randVarianceValue > 0.5)
                    ChangeVariance = ChangeVariance + myListOfParaToValue[3].Value * Math.Sqrt(CurrentVariance);
                else
                    ChangeVariance = ChangeVariance - myListOfParaToValue[3].Value * Math.Sqrt(CurrentVariance);
                CurrentVariance = CurrentVariance + ChangeVariance;
                CurrentVariance = Math.Max(CurrentVariance, 0.0);

                // Price evolution;
                double ChangePrice = myListOfParaToValue[0].Value * CurrentValue;
                double randPriceValue = myListOfParaToValue[5].Value / randVarianceValue;
                if ( randPriceValue > 0.5 )
                    ChangePrice = ChangePrice + CurrentValue * Math.Sqrt(CurrentVariance);  
                else
                    ChangePrice = ChangePrice - CurrentValue * Math.Sqrt(CurrentVariance);
                CurrentValue = CurrentValue + ChangePrice;
                myListOfValues.Add(CurrentValue);
            }
            return myListOfValues;
        }

        public override void updatedTickAndInitial(double inTickValue, double inInitial)
        {
            //myListOfParaToValue[1].Value = InitialVarianceTicks * myTickValue;
            //myListOfParaToValue[4].Value = InitialVarianceTicks * myTickValue;
        }
        public override double updatedTickInc()
        {
            return 500;
        }
    }

}
