using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace FXOptionPricer
{
    class PricerGBM
    {
        double UnderlyingPrice;
        double StdDev;
        double StrikeIncrement;
        int MaxStrikeSteps;
        int MaxExpiry;


        List<double> myListOfStrikes;
        List<List<double>> myCallForEachTime;
        List<List<double>> myPutForEachTime;

        public PricerGBM()
        {
            myCallForEachTime = new List<List<double>>();
            myPutForEachTime = new List<List<double>>();
            myListOfStrikes = new List<double>();
        }

        public void PopulatePrices()
        {
            myListOfStrikes.Clear();
            double StartingStrike = UnderlyingPrice - MaxStrikeSteps * StrikeIncrement;
            for (int i = 0; i < ((2 * MaxStrikeSteps) + 1); i++)
                myListOfStrikes.Add(StartingStrike + i * StrikeIncrement);

            myCallForEachTime.Clear();
            myPutForEachTime.Clear();
            for (int i = 1; i <= MaxExpiry; i++)
            {
                List<double> myCallPrices = new List<double>();
                List<double> myPutPrices = new List<double>();
                for (int j = 0; j < myListOfStrikes.Count; j++)
                {
                    KeyValuePair<double, double> CallPutPair = getCallPutForTimeAndStrike(i, UnderlyingPrice, myListOfStrikes[j], StdDev);
                    myCallPrices.Add(CallPutPair.Key);
                    myPutPrices.Add(CallPutPair.Value);
                }
                myCallForEachTime.Add(myCallPrices);
                myPutForEachTime.Add(myPutPrices);
            }
        }

        private KeyValuePair<double,double> getCallPutForTimeAndStrike(int inTime, double inUnderlying, double inStrike, double inStdDev)
        {
            Chart aChart = new Chart();
            double Num = Math.Log(inUnderlying / inStrike) + (inStdDev * inStdDev / 2.0) * inTime;
            double Den = inStdDev * Math.Sqrt((double)inTime);
            double DOne = Num / Den;
            double DTwo = DOne - StdDev * Math.Sqrt((double)inTime);
            double NormDistOne = aChart.DataManipulator.Statistics.NormalDistribution(DOne);
            double NormDistTwo = aChart.DataManipulator.Statistics.NormalDistribution(DTwo);

            double CallPrice = inUnderlying * NormDistOne - inStrike * NormDistTwo;
            double PutPrice = inStrike * (1.0 - NormDistTwo) - inUnderlying * (1.0 - NormDistOne);

            return new KeyValuePair<double, double>(CallPrice, PutPrice);

        }

        public void printCallPutPrices()
        {
            for (int i = 0; i < myCallForEachTime.Count; i++)
            {
                for (int j = 0; j < myCallForEachTime[i].Count; j++)
                {
                    Console.WriteLine("Time " + (i+1) + " Strike " + myListOfStrikes[j]);
                    Console.WriteLine("Time " + (i + 1) + " Call " + myCallForEachTime[i][j]);
                    Console.WriteLine("Time " + (i + 1) + " Put " + myPutForEachTime[i][j]);
                }
            }
        }

        public GraphModified outputGraph(System.Drawing.Size inSize)
        {
            GraphModified myGraph = new GraphModified(inSize);
            myGraph.MasterPane.PaneList.Clear();
            myGraph.AddAPane("MainGraph");

            for (int i = 0; i < myCallForEachTime.Count; i++)
            {
                List<KeyValuePair<double, double>> myCallPlot = new List<KeyValuePair<double, double>>();
                List<KeyValuePair<double, double>> myPutPlot = new List<KeyValuePair<double, double>>();
                for (int j = 0; j < myListOfStrikes.Count; j++)
                {
                    myCallPlot.Add( new KeyValuePair<double,double>( myListOfStrikes[j], myCallForEachTime[i][j] ) );
                    myPutPlot.Add( new KeyValuePair<double,double>( myListOfStrikes[j], myPutForEachTime[i][j] ) );
                }
                ZedGraph.LineItem thisCall = myGraph.PlotPoints(myCallPlot, "Call", System.Drawing.Color.Blue, ZedGraph.SymbolType.None, "MainGraph");
                ZedGraph.LineItem thisPut = myGraph.PlotPoints(myPutPlot, "Put", System.Drawing.Color.DarkViolet, ZedGraph.SymbolType.None, "MainGraph");
                if (i == 0) { thisCall.Label.IsVisible = true; }
                if (i == 0) { thisPut.Label.IsVisible = true; }
            }

            List<KeyValuePair<double, double>> myPricePlot = new List<KeyValuePair<double, double>>();
            for (int i = 0; i < myListOfStrikes.Count; i++)
                myPricePlot.Add(new KeyValuePair<double, double>(myListOfStrikes[i], 0));
            myGraph.PlotPoints(myPricePlot, "Price", System.Drawing.Color.DarkViolet, ZedGraph.SymbolType.Diamond, "MainGraph", true);
            return myGraph;
        }

        public void setUnderlying(double inUnderlying) { UnderlyingPrice = inUnderlying; }
        public void setStdDev(double inStdDev) { StdDev = inStdDev; }
        public void setStrikeIncrement(double inStrikeIncrement) { StrikeIncrement = inStrikeIncrement; }
        public void setMaxExpiry(int inMaxExpiry) { MaxExpiry = inMaxExpiry; }
        public void setMaxStrikeSteps(int inMaxStrikeSteps) { MaxStrikeSteps = inMaxStrikeSteps; }

    }
}
