using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer
{
    class Deprecated
    {

        /*
        public void CalculateAndPlotTree()
        {
            myTree.populateTrees();
            myTree.populateNodeValues();
            //myTree.printTreeOfNodes();
            myMonteCarloGraph = myTree.outputGraph(this.MonteCarloTab.Size);
            myMonteCarloGraph.Dock = DockStyle.Fill;
            this.MonteCarloTab.Controls.Clear();
            this.MonteCarloTab.Controls.Add(this.ShowCallBino);
            this.MonteCarloTab.Controls.Add(this.ShowPutBino);
            this.MonteCarloTab.Controls.Add(this.ShowPriceBino);
            this.MonteCarloTab.Controls.Add(myMonteCarloGraph);
            myMonteCarloGraph.setScaleToDefault();
        }

        public void CalculateAndPlotGraph()
        {
            myPricerGBM.PopulatePrices();

            myFormulaGraph = myPricerGBM.outputGraph(this.FormulaTab.Size);
            myFormulaGraph.Dock = DockStyle.Fill;
            this.FormulaTab.Controls.Clear();
            this.FormulaTab.Controls.Add(myFormulaGraph);
            myFormulaGraph.setScaleToDefault();

        }

        private void ShowHideBino_Click(object sender, EventArgs e)
        {
            Button ThisButton = (Button)sender;
            String HideShow = ThisButton.Text.Substring(0, ThisButton.Text.IndexOf(" "));
            String Product = ThisButton.Text.Substring(ThisButton.Text.IndexOf(" ") + 1);
            Color ForeColor = ThisButton.ForeColor;
            Color BackColor = ThisButton.BackColor;
            if (HideShow == "Hide")
            {
                ThisButton.Text = "Show " + Product;
                myMonteCarloGraph.HideType(Product, true);
            }
            else
            {
                ThisButton.Text = "Hide " + Product;
                myMonteCarloGraph.HideType(Product, false);
            }
            ThisButton.ForeColor = BackColor;
            ThisButton.BackColor = ForeColor;
        }
        */
    }
}
