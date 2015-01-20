using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer.Simulator
{
    class GBM : PriceProcess
    {
        public GBM()
        {
            Name = "Geometric";
            myListOfParaToValue.Add(new ParaValuePair("Mean", myTickValue));
            myListOfParaToValue.Add(new ParaValuePair("StdDev", 0.002));
        }

        public override List<double>getPricePath( int inSeed )
        {
            Random rand = new Random(inSeed);
            List<double> myListOfValues = new List<double>();
            double CurrentValue = InitialValue;
            myListOfValues.Add(CurrentValue);
            for (int i = 0; i < (MainView.ProcessDistance - 1); i++)
            {
                double Change = myListOfParaToValue[0].Value;
                if (rand.Next(0, 2) == 0)
                    Change = Change + myListOfParaToValue[1].Value * CurrentValue;                
                else
                    Change = Change - myListOfParaToValue[1].Value * CurrentValue;
                CurrentValue = CurrentValue + Change;
                myListOfValues.Add(CurrentValue);
            }
            return myListOfValues;
        }

        public override void updatedTickAndInitial(double inTickValue, double inInitial)
        {
            myListOfParaToValue[0].Value = inTickValue;
        }

        public override double updatedTickInc()
        {
            return 100;
        }

    }

}
