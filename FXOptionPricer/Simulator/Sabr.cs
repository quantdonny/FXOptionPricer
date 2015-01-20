using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer.Simulator
{
    class Sabr : PriceProcess
    {
        public Sabr()
        {
            Name = "Sabr";
            myListOfParaToValue.Add(new ParaValuePair("Alpha", 0.001));
            myListOfParaToValue.Add(new ParaValuePair("Beta", 0.5));
            myListOfParaToValue.Add(new ParaValuePair("Rho", 0.2));
            myListOfParaToValue.Add(new ParaValuePair("IniVol", 0.001));
        }

        public override List<double> getPricePath(int inSeed)
        {
            Random randVol = new Random(inSeed);
            List<double> myListOfValues = new List<double>();
            double CurrentValue = InitialValue;
            double CurrentVol = myListOfParaToValue[3].Value;
            myListOfValues.Add(CurrentValue);
            for (int i = 0; i < (MainView.ProcessDistance - 1); i++)
            {
                double ranVolValue = randVol.NextDouble();
                double ChangeVol = myListOfParaToValue[0].Value * CurrentVol;
                ChangeVol = (ranVolValue > 0.5) ? ChangeVol : -ChangeVol;
                CurrentVol = CurrentVol + ChangeVol;

                double ranPriceValue = myListOfParaToValue[2].Value / ranVolValue;

                double ChangePrice = CurrentVol * Math.Pow(CurrentValue, myListOfParaToValue[1].Value);
                ChangePrice = (ranPriceValue > 0.5) ? ChangePrice : -ChangePrice;
                CurrentValue = CurrentValue + ChangePrice;

                myListOfValues.Add(CurrentValue);
            }
            return myListOfValues;
        }

        public override void updatedTickAndInitial(double inTickValue, double inInitial)
        {

        }

        public override double updatedTickInc()
        {
            return 50;
        }
    }

}
