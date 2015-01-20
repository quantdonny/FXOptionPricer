using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer
{
    abstract class OptionPricer
    {
        public String Name;
        public int NoOfStrikeIncrementSteps = 10;
        public double IncrementInTicks = 100;        
        public double myTickValue = 0.0001;
        public double CurrentUnderlyingPrice = 0;
        public List<ParaValuePair> myListOfParaToValue;
        public OptionPricer()
        {
            myListOfParaToValue = new List<ParaValuePair>();
        }
        public abstract KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>> getOptionPrice(List<List<double>> inListOfPricePath);
        public abstract void updatedTickAndInitial(double inTickValue, double inInitialValue);

        public List<List<double>> getStrikeLines()
        {
            List<List<double>> myListOfStrikesLines = new List<List<double>>();
            double InitialStrike = CurrentUnderlyingPrice - NoOfStrikeIncrementSteps * IncrementInTicks * myTickValue;
            for (int i = 0; i < (2 * NoOfStrikeIncrementSteps + 1); i++)
            {
                List<double> thisStrikeLineList = new List<double>();
                double thisStrike = InitialStrike + i * IncrementInTicks * myTickValue;
                for (int j = 0; j < MainView.ProcessDistance; j++)
                    thisStrikeLineList.Add(thisStrike);
                myListOfStrikesLines.Add(thisStrikeLineList);
            }
            return myListOfStrikesLines;
        }

    }
}
