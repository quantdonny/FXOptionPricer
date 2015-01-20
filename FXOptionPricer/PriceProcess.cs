using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer
{
    public class ParaValuePair
    {
        public String ParaName;
        public double Value;
        public ParaValuePair(String inParaName, double inValue)
        {
            ParaName = inParaName;
            Value = inValue;
        }
    }

    abstract class PriceProcess
    {
        public String Name;
        public double InitialValue = 0;
        public double myTickValue = 0.0001;
        public List<ParaValuePair> myListOfParaToValue;
        public abstract List<double> getPricePath(int inSeed);
        public PriceProcess()
        {
            myListOfParaToValue = new List<ParaValuePair>();
        }
        public abstract void updatedTickAndInitial(double inTickValue, double inInitialValue);
        public abstract double updatedTickInc();
    }
}
