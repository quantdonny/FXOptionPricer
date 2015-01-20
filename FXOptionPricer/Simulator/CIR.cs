using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer.Simulator
{
    class CIR : PriceProcess
    {
        public CIR()
        {
            Name = "CIR";
            myListOfParaToValue.Add(new ParaValuePair("Mean", 0.01));
            myListOfParaToValue.Add(new ParaValuePair("Speed", InitialValue));
            myListOfParaToValue.Add(new ParaValuePair("Vol", 0.05));
        }

        public override List<double> getPricePath(int inSeed)
        {
            Random rand = new Random(inSeed);
            List<double> myListOfValues = new List<double>();
            double CurrentValue = InitialValue;
            myListOfValues.Add(CurrentValue);
            for (int i = 0; i < (MainView.ProcessDistance - 1); i++)
            {
                double Change = myListOfParaToValue[0].Value * (myListOfParaToValue[1].Value - CurrentValue);
                if (rand.Next(0, 2) == 0)
                    Change = Change + myListOfParaToValue[2].Value * Math.Sqrt(CurrentValue);
                else
                    Change = Change - myListOfParaToValue[2].Value * Math.Sqrt(CurrentValue);
                CurrentValue = CurrentValue + Change;
                myListOfValues.Add(CurrentValue);
            }
            return myListOfValues;
        }

        public override void updatedTickAndInitial(double inTickValue, double inInitial)
        {
            myListOfParaToValue[1].Value = inInitial;
        }

        public override double updatedTickInc()
        {
            return 100;
        }
    }

}
