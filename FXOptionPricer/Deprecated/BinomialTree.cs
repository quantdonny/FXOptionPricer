using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer
{
    class TreeNode
    {
        public List<TreeNode> myListOfChild;
        public double CallValue;
        public double PutValue;
        public double CalculatedUnderlying;
        public TreeNode()
        {
            myListOfChild = new List<TreeNode>();
        }
    }

    class BinomialTree
    {
        double UnderlyingPrice;
        public double StdDev;
        double Strike;
        int MaxExpiry;
        List<List<TreeNode>> myTimeToTreeNode;

        public BinomialTree()
        {
            myTimeToTreeNode = new List<List<TreeNode>>();
        }

        public void setUnderlying(double inUnderlying) { UnderlyingPrice = inUnderlying; }
        public void setStdDev(double inStdDev) { StdDev = inStdDev; }
        public void setStrike(double inStrike) { Strike = inStrike; }
        public void setMaxExpiry(int inMaxExpiry) { MaxExpiry = inMaxExpiry; }

        public void populateTrees()
        {
            myTimeToTreeNode.Clear();
            for (int i = 0; i <= MaxExpiry; i++)
            {
                List<TreeNode> ListAtThisTime = new List<TreeNode>();
                for (int j = 0; j < (i + 1); j++)
                {
                    ListAtThisTime.Add(new TreeNode());
                }
                myTimeToTreeNode.Add( ListAtThisTime );
            }
            for ( int i = 0; i < (myTimeToTreeNode.Count - 1); i++ )
                for( int j = 0; j < myTimeToTreeNode[i].Count; j++ ){
                    myTimeToTreeNode[i][j].myListOfChild.Add(myTimeToTreeNode[i+1][j]);
                    myTimeToTreeNode[i][j].myListOfChild.Add(myTimeToTreeNode[i+1][j+1]);
                }
        }

        public void populateNodeValues()
        {
            // Populate Calculated Underlying.
            for (int i = 0; i < myTimeToTreeNode.Count; i++)
            {
                double uValue = Math.Exp(StdDev * Math.Sqrt(i));
                double dValue = 1.0 / uValue;
                for (int j = 0; j < myTimeToTreeNode[i].Count; j++)
                {
                    double CalculatedUnderlying = Math.Pow(uValue, i - j) * Math.Pow(dValue, j) * UnderlyingPrice;
                    myTimeToTreeNode[i][j].CalculatedUnderlying = CalculatedUnderlying;
                }
            }

            // Calculate the End nodes.
            for (int i = 0; i < myTimeToTreeNode[myTimeToTreeNode.Count - 1].Count; i++)
            {
                double CalculatedUnderlying = myTimeToTreeNode[myTimeToTreeNode.Count - 1][i].CalculatedUnderlying;
                myTimeToTreeNode[myTimeToTreeNode.Count - 1][i].CallValue = Math.Max(CalculatedUnderlying - Strike, 0);
                myTimeToTreeNode[myTimeToTreeNode.Count - 1][i].PutValue = Math.Max(Strike - CalculatedUnderlying, 0);
            }

            // Back populate. Start from the second last node.
            for ( int i = myTimeToTreeNode.Count - 2; i >= 0; i-- ) {
                double uValue = Math.Exp(StdDev * Math.Sqrt(i));
                double dValue = 1.0 / uValue;
                double ProbUp = (1 - dValue) / (uValue - dValue);
                for (int j = 0; j < myTimeToTreeNode[i].Count; j++)
                {
                    myTimeToTreeNode[i][j].CallValue =
                        ProbUp * myTimeToTreeNode[i][j].myListOfChild[0].CallValue + (1 - ProbUp) * myTimeToTreeNode[i][j].myListOfChild[1].CallValue;
                    myTimeToTreeNode[i][j].PutValue =
                        ProbUp * myTimeToTreeNode[i][j].myListOfChild[0].PutValue + (1 - ProbUp) * myTimeToTreeNode[i][j].myListOfChild[1].PutValue;
                }
            }
        }

        public GraphModified outputGraph( System.Drawing.Size inSize, bool ShowPrice = true, bool ShowCall = true, bool ShowPut = true )
        {
            GraphModified myGraph = new GraphModified(inSize);
            myGraph.MasterPane.PaneList.Clear();
            myGraph.AddAPane("MainGraph");

            List<List<double>> myListOfPlotValues = new List<List<double>>();

            for (int i = 0; i < myTimeToTreeNode.Count; i++)
            {
                List<double> myListOfPointsForThisTime = new List<double>();
                for (int j = 0; j < myTimeToTreeNode[i].Count; j++)
                    myListOfPointsForThisTime.Add( myTimeToTreeNode[i][j].CalculatedUnderlying);
                myListOfPlotValues.Add(myListOfPointsForThisTime);
            }
            myGraph.PlotPoints(myListOfPlotValues, "Price", System.Drawing.Color.DarkGray, "MainGraph", true, true);

            List<List<double>> myListOfCallValues = new List<List<double>>();
            for (int i = 0; i < myTimeToTreeNode.Count; i++)
            {
                List<double> myListOfPointsForThisTime = new List<double>();
                for (int j = 0; j < myTimeToTreeNode[i].Count; j++)
                    myListOfPointsForThisTime.Add(myTimeToTreeNode[i][j].CallValue);
                myListOfCallValues.Add(myListOfPointsForThisTime);
            }
            myGraph.PlotPoints(myListOfCallValues, "Call", System.Drawing.Color.Blue, "MainGraph", false, true);

            List<List<double>> myListOfPutValues = new List<List<double>>();
            for (int i = 0; i < myTimeToTreeNode.Count; i++)
            {
                List<double> myListOfPointsForThisTime = new List<double>();
                for (int j = 0; j < myTimeToTreeNode[i].Count; j++)
                    myListOfPointsForThisTime.Add(myTimeToTreeNode[i][j].PutValue);
                myListOfPutValues.Add(myListOfPointsForThisTime);
            }
            myGraph.PlotPoints(myListOfPutValues, "Put", System.Drawing.Color.DarkViolet, "MainGraph", false, true);

            List<double> myStrikeLine = new List<double>();
            for (int i = 0; i <= MaxExpiry; i++ )
                myStrikeLine.Add( Strike );
            myGraph.PlotPricePath("Strike", myStrikeLine, System.Drawing.Color.Purple, "MainGraph");

            return myGraph;
        }

        public void printTreeOfNodes()
        {
            for( int i = 0; i < myTimeToTreeNode.Count; i++ )
                for( int j = 0; j < myTimeToTreeNode[i].Count; j++ )
                {
                    double CalculatedUnderyling = myTimeToTreeNode[i][j].CalculatedUnderlying;
                    double CallValue = myTimeToTreeNode[i][j].CallValue;
                    double PutValue = myTimeToTreeNode[i][j].PutValue;
                    Console.WriteLine("Time " + i + " Node " + j + " CalUnder " + CalculatedUnderyling
                        + " Call " + CallValue + " Put " + PutValue);
                }
        }

    }


}
