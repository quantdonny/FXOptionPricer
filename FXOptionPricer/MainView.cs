using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace FXOptionPricer
{
    public partial class MainView : Form
    {
        Dictionary<String, PriceProcess> myDictOfPriceProcess;
        Dictionary<String, OptionPricer> myDictOfOptionPricer;
        GraphModified myMonteCarloGraph;
        GraphModified myOptionPriceGraph;
        GraphModified myOHLCGraph;
        GraphModified mySmileGraph;

        public static double MinBSSmileSD = 0.001;
        public static double MaxBSSmileSD = 0.1;
        public static double IncBSSmileSD = 0.0001;

        public static int NumberOfInstances = 100;
        public static int ProcessDistance = 100;
        public static int NumberOfInstancesForHistorical = 20;

        PriceProcess selectedPriceProcess;
        OptionPricer selectedOptionPricer;

        GenericFactory<OptionPricer> myOptionPricerFactory = new GenericFactory<OptionPricer>("FXOptionPricer.Pricers", "OptionPricer");
        GenericFactory<PriceProcess> myPriceProcessFactory = new GenericFactory<PriceProcess>("FXOptionPricer.Simulator", "PriceProcess");

        public MainView()
        {
            // Initialize Model Componenets.
            InitializeComponent();

            myDictOfOptionPricer = myOptionPricerFactory.getDictionaryOfNameToProducts();
            myDictOfPriceProcess = myPriceProcessFactory.getDictionaryOfNameToProducts();

            selectedPriceProcess = myDictOfPriceProcess["Geometric"];
            selectedOptionPricer = myDictOfOptionPricer["European"];

            this.ModelBox.Text = "Geometric";
            this.OptionTypeBox.Text = "European";
            this.CurrencySelectBox.Text = "EURUSD";
        }

        private void CurrencySelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MonteCarloTab.Controls.Clear();
            double Underlying = Database.getInitialValueForCcy(this.CurrencySelectBox.Text);
            double TickSize = Database.getTickValueForCcy(this.CurrencySelectBox.Text);          
            foreach (KeyValuePair<String, PriceProcess> aPriceProcess in myDictOfPriceProcess)
            {
                aPriceProcess.Value.updatedTickAndInitial(TickSize, Underlying);
                aPriceProcess.Value.InitialValue = Underlying;
                aPriceProcess.Value.myTickValue = TickSize;
            }
            foreach (KeyValuePair<String, OptionPricer> anOptionPricer in myDictOfOptionPricer)
            {
                anOptionPricer.Value.updatedTickAndInitial(TickSize, Underlying);
                anOptionPricer.Value.myTickValue = TickSize;
                anOptionPricer.Value.CurrentUnderlyingPrice = Underlying;
            }
            this.PriceLabel.Text = "Price: " + Underlying.ToString();
            DisplayModelParameters(this.ParameterPanel, myDictOfPriceProcess[this.ModelBox.Text].myListOfParaToValue, "Price");
            DisplayModelParameters(this.OptionParaPanel, myDictOfOptionPricer[this.OptionTypeBox.Text].myListOfParaToValue, "Option");
            this.WaitLabel.Visible = true;
            this.WaitLabel.Update();
            CalculateAndPlotSimulators();
            this.WaitLabel.Visible = false;
        }

        public void CalculateAndPlotSimulators()
        {
            List<List<double>> myListOfSimulators = new List<List<double>>();
            for (int i = 0; i < NumberOfInstances; i++)
                myListOfSimulators.Add(selectedPriceProcess.getPricePath(i));

            KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>> myListOfOptionValues = selectedOptionPricer.getOptionPrice(myListOfSimulators);
            List<KeyValuePair<double, double>> CallPrices = myListOfOptionValues.Key;
            List<KeyValuePair<double, double>> PutPrices = myListOfOptionValues.Value;
            List<KeyValuePair<double, double>> myStrikes = new List<KeyValuePair<double, double>>();
            foreach (KeyValuePair<double, double> aCallPair in CallPrices)
                myStrikes.Add(new KeyValuePair<double, double>(aCallPair.Key, 0));

            // Graphing.
            myMonteCarloGraph = new GraphModified(this.MonteCarloTab.Size);
            myMonteCarloGraph.MasterPane.PaneList.Clear();
            myMonteCarloGraph.AddAPane("MainGraph");
            foreach (List<double> aTrace in myListOfSimulators)            
                myMonteCarloGraph.PlotPricePath("PricePath", aTrace, System.Drawing.Color.DarkBlue, "MainGraph");
            
            // Graph the Strikes.
            foreach (List<double> aStrike in selectedOptionPricer.getStrikeLines())            
                myMonteCarloGraph.PlotPricePath("Strike", aStrike, System.Drawing.Color.LightGray, "MainGraph", 2);
            AddGraphToTable(myMonteCarloGraph, this.MonteCarloTab);
            
            // Graphing.
            myOptionPriceGraph = new GraphModified(this.FormulaTab.Size);
            myOptionPriceGraph.AddAPane("MainGraph");
            ZedGraph.LineItem CallLine = myOptionPriceGraph.PlotPricePath("Call", CallPrices, Color.Blue, "MainGraph");
            ZedGraph.LineItem PutLine = myOptionPriceGraph.PlotPricePath("Put", PutPrices, Color.DarkViolet, "MainGraph");
            CallLine.Label.IsVisible = true; CallLine.Line.Width = 2;
            PutLine.Label.IsVisible = true; PutLine.Line.Width = 2;
            myOptionPriceGraph.PlotPricePath("Strikes", myStrikes, Color.DarkGray, "MainGraph", true, 1);
            AddGraphToTable(myOptionPriceGraph, this.FormulaTab);

            // OHLC Plotting.
            String selectedCcy = this.CurrencySelectBox.Text;
            if (selectedCcy.Length < 3) { selectedCcy = "EURUSD"; }
            List<OHLCBar> thisCcyOHLCBar = Database.getOHLCForCcy(selectedCcy);
            myOHLCGraph = new GraphModified(this.BacktestTab.Size);
            myOHLCGraph.MasterPane.PaneList.Clear();
            myOHLCGraph.AddAPane("OHLCPane");
            myOHLCGraph.getPaneByTitle("OHLCPane").XAxis.Type = ZedGraph.AxisType.Ordinal;
            myOHLCGraph.DrawCandleStickAtPane(thisCcyOHLCBar, selectedCcy, "OHLCPane");
           
            // Get historical Call and Puts.
            List<List<double>> HistoricalStrikeToCallPrice = new List<List<double>>();
            List<List<double>> HistoricalStrikeToPutPrice = new List<List<double>>();
            int NumberOfStrikes = CallPrices.Count;
            List<List<double>> myListOfSimulatorsHistorical = new List<List<double>>();
            for (int i = 0; i < NumberOfStrikes; i++)
            {
                HistoricalStrikeToCallPrice.Add(new List<double>());
                HistoricalStrikeToPutPrice.Add(new List<double>());
            }
            double OriginalUnderying = selectedPriceProcess.InitialValue;
            for (int i = 0; i < thisCcyOHLCBar.Count; i++)
            {
                double thisClose = thisCcyOHLCBar[i].close;
                selectedPriceProcess.InitialValue = thisClose;
                selectedOptionPricer.CurrentUnderlyingPrice = thisClose;
                myListOfSimulatorsHistorical.Clear();
                for (int j = 0; j < NumberOfInstancesForHistorical; j++)
                    myListOfSimulatorsHistorical.Add(selectedPriceProcess.getPricePath(j));
                KeyValuePair<List<KeyValuePair<double, double>>, List<KeyValuePair<double, double>>> thisListOfOptionValues = selectedOptionPricer.getOptionPrice(myListOfSimulatorsHistorical);
                List<KeyValuePair<double, double>> thisCallPrices = thisListOfOptionValues.Key;
                List<KeyValuePair<double, double>> thisPutPrices = thisListOfOptionValues.Value;
                for (int j = 0; j < thisCallPrices.Count; j++)
                {
                    HistoricalStrikeToCallPrice[j].Add( thisCallPrices[j].Value );
                    HistoricalStrikeToPutPrice[j].Add(thisPutPrices[j].Value);
                }
            }
            // Reset Underlying.
            selectedPriceProcess.InitialValue = OriginalUnderying;
            selectedOptionPricer.CurrentUnderlyingPrice = OriginalUnderying;

            // Plot historical option prices.
            myOHLCGraph.AddAPane("CallPriceGraph");
            myOHLCGraph.getPaneByTitle("CallPriceGraph").XAxis.Type = ZedGraph.AxisType.Ordinal;
            for (int i = 0; i < HistoricalStrikeToCallPrice.Count; i++ )
            {
                List<double> aCallSeries = HistoricalStrikeToCallPrice[i];
                ZedGraph.LineItem thisCall = myOHLCGraph.PlotPricePath("Call Prices", aCallSeries, Color.Blue, "CallPriceGraph");
                if (i == 0)
                    thisCall.Label.IsVisible = true;
            }
            myOHLCGraph.AddAPane("PutPriceGraph");
            myOHLCGraph.getPaneByTitle("PutPriceGraph").XAxis.Type = ZedGraph.AxisType.Ordinal;
            for (int i = 0; i < HistoricalStrikeToPutPrice.Count; i++)
            {
                List<double> aPutSeries = HistoricalStrikeToPutPrice[i];
                ZedGraph.LineItem thisPut = myOHLCGraph.PlotPricePath("Put Prices", aPutSeries, Color.DarkMagenta, "PutPriceGraph");
                if (i == 0)
                    thisPut.Label.IsVisible = true;
            }

            //myOHLCGraph.realignPane();
            myOHLCGraph.ZeroMarginPadding();
            myOHLCGraph.MasterPane.SetLayout(this.BacktestTab.CreateGraphics(), true, new int[]{1,1,1}, new float[]{3,1,1});
            //myOHLCGraph.MasterPane.SetLayout(this.BacktestTab.CreateGraphics(), ZedGraph.PaneLayout.SingleColumn);
            AddGraphToTable(myOHLCGraph, this.BacktestTab);   

            // Volatility Smile Plotting.            
            List<KeyValuePair<double, double>> getVolSmile = PlotSmile(CallPrices);
            mySmileGraph = new GraphModified(this.SmileTab.Size);
            mySmileGraph.AddAPane("MainGraph");
            ZedGraph.LineItem SmileLine = mySmileGraph.PlotPricePath("Volatility Smile", getVolSmile, Color.Blue, "MainGraph");
            SmileLine.Label.IsVisible = true; SmileLine.Line.Width = 2;
            mySmileGraph.PlotPricePath("Strikes", myStrikes, Color.DarkGray, "MainGraph", true, 1);
            AddGraphToTable(mySmileGraph, this.SmileTab);
          
        }

        public void AddGraphToTable(GraphModified inGraph, TabPage inTab)
        {
            inGraph.Dock = DockStyle.Fill;
            inTab.Controls.Clear();
            inTab.Controls.Add(inGraph);
            inGraph.setScaleToDefault();
        }

        public List<KeyValuePair<double, double>> PlotSmile( List<KeyValuePair<double, double>> inListOfCallPrices )
        {
            // Need to get the Price List from GBM with 0 mean.
            Dictionary<double, List<KeyValuePair<double, double>>> VolToPriceList = new Dictionary<double, List<KeyValuePair<double, double>>>();
            double OriginalVol = myDictOfPriceProcess["Geometric"].myListOfParaToValue[1].Value;
            PriceProcess GBMPriceProcess = myDictOfPriceProcess["Geometric"];
            double CurrentSD = MinBSSmileSD;
            while( CurrentSD <= MaxBSSmileSD )
            {
                GBMPriceProcess.myListOfParaToValue[1].Value = CurrentSD;
                List<List<double>> SimulatorsForThisSD = new List<List<double>>();
                for (int i = 0; i < NumberOfInstances; i++)
                    SimulatorsForThisSD.Add(GBMPriceProcess.getPricePath(i));
                List<KeyValuePair<double, double>> PriceListForThisSD = selectedOptionPricer.getOptionPrice(SimulatorsForThisSD).Key;
                VolToPriceList.Add(CurrentSD, PriceListForThisSD);
                CurrentSD = CurrentSD + IncBSSmileSD;
            }
            List<KeyValuePair<double, double>> StrikeToVol = SmileCalc.getSmile(inListOfCallPrices, VolToPriceList);           
            // Change back to Original Value.
            myDictOfPriceProcess["Geometric"].myListOfParaToValue[1].Value = OriginalVol;
            return StrikeToVol;
        }

        private void ModelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MonteCarloTab.Controls.Clear();
            selectedPriceProcess = myDictOfPriceProcess[this.ModelBox.Text];
            double NewTickIncrement = selectedPriceProcess.updatedTickInc();
            if (NewTickIncrement != 0)
            {
                foreach (KeyValuePair<String, OptionPricer> NameToPricer in myDictOfOptionPricer)
                    NameToPricer.Value.IncrementInTicks = NewTickIncrement;
                this.StrikeIncrementBox.Text = NewTickIncrement.ToString();
            }
            this.WaitLabel.Visible = true;
            this.WaitLabel.Update();
            CalculateAndPlotSimulators();
            this.WaitLabel.Visible = false;
            DisplayModelParameters(this.ParameterPanel, myDictOfPriceProcess[this.ModelBox.Text].myListOfParaToValue, "Price");
        }

        public void DisplayModelParameters(Panel thisPanel, List<ParaValuePair> myListOfParaToValue, String inName)
        {
            thisPanel.Controls.Clear();
            int HeightOffset = 3;
            int HeightDiff = 20;
            int WidthOffsetOne = 3;
            int WidthOffsetTwo = 75;
            int WidthTB = 60;
            for (int i = 0; i < myListOfParaToValue.Count; i++)
            {
                Label aLabel = new Label();
                aLabel.Left = WidthOffsetOne;
                aLabel.Top = HeightOffset + 2 + i*HeightDiff;
                aLabel.AutoSize = true;
                aLabel.Text = myListOfParaToValue[i].ParaName;
                TextBox aTextBox = new TextBox();
                aTextBox.Name = myListOfParaToValue[i].ParaName;
                aTextBox.Width = WidthTB;
                aTextBox.Left = WidthOffsetTwo;
                aTextBox.Top = HeightOffset + i * HeightDiff;
                aTextBox.AutoSize = true;
                aTextBox.Text = myListOfParaToValue[i].Value.ToString();
                aTextBox.Leave += new System.EventHandler(LeftaParaBox);
                aTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyDownaParaBox);
                aLabel.TabIndex = 999;
                aTextBox.TabIndex = (inName == "Price") ? 999 : 1000;
                thisPanel.Controls.Add(aLabel);
                thisPanel.Controls.Add(aTextBox);
            }
        }

        private void KeyDownaParaBox(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LeftaParaBox(sender, new EventArgs());            
        }

        private void LeftaParaBox(object sender, EventArgs e)
        {
            // Check the value.            
            TextBox SenderTB = (TextBox)sender;
            ParaValuePair thisValuePair = null;
            if (SenderTB.TabIndex == 999)
                thisValuePair = selectedPriceProcess.myListOfParaToValue.Where(item => item.ParaName == SenderTB.Name).First();
            else
                thisValuePair = selectedOptionPricer.myListOfParaToValue.Where(item => item.ParaName == SenderTB.Name).First();
            double OriginalValue = thisValuePair.Value;
            String thisValueString = SenderTB.Text;
            double thisValue;
            if (Double.TryParse(thisValueString, out thisValue) == false)
            {
                MessageBox.Show("You did not enter a number.");
                SenderTB.Text = OriginalValue.ToString();
                return;
            }
            thisValuePair.Value = thisValue;
            SenderTB.Text = thisValue.ToString();

            // Change the Label.
            this.ParaUpdateBut.Text = "Awaiting replot";
        }



        private void ParaUpdateBut_Click(object sender, EventArgs e)
        {
            this.MonteCarloTab.Controls.Clear();
            selectedPriceProcess = myDictOfPriceProcess[this.ModelBox.Text];
            this.WaitLabel.Visible = true;
            this.WaitLabel.Update();
            CalculateAndPlotSimulators();
            this.WaitLabel.Visible = false;
            this.ParaUpdateBut.Text = "Parameters updated";
        }


        private void StrikeIncrementBox_Leave(object sender, EventArgs e)
        {
            // Check the value.            
            TextBox SenderTB = (TextBox)sender;
            String thisValueString = SenderTB.Text;
            double thisValue;
            if (Double.TryParse(thisValueString, out thisValue) == false)
            {
                MessageBox.Show("You did not enter a number.");
                SenderTB.Text = thisValue.ToString();
                return;
            }
            SenderTB.Text = thisValue.ToString();
            foreach (KeyValuePair<String, OptionPricer> NameToPricer in myDictOfOptionPricer)
                NameToPricer.Value.IncrementInTicks = thisValue;
            this.WaitLabel.Visible = true;
            this.WaitLabel.Update();
            CalculateAndPlotSimulators();
            this.WaitLabel.Visible = false;
        }

        private void StrikeIncrementBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                StrikeIncrementBox_Leave(sender, new EventArgs());         
        }

        private void OptionTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String SelectedText = ((ComboBox)sender).Text;
            selectedOptionPricer = myDictOfOptionPricer[SelectedText];
            DisplayModelParameters(this.OptionParaPanel, selectedOptionPricer.myListOfParaToValue, "Option");
        }
               
    }
}
