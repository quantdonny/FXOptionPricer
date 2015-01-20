using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FXOptionPricer
{
    using ZedGraph;

    public class OHLCBar
    {
        public double open;
        public double high;
        public double low;
        public double close;
        public long volume;
        public DateTime dateTime;

        public OHLCBar(double inOpen, double inHigh, double inLow, double inClose, long inVolume, DateTime inDateTime)
        {
            open = inOpen;
            high = inHigh;
            low = inLow;
            close = inClose;
            dateTime = inDateTime;
            volume = inVolume;
        }

        public OHLCBar(double inOpen, double inHigh, double inLow, double inClose, DateTime inDateTime) :
            this(inOpen, inHigh, inLow, inClose, 1, inDateTime)
        {
        }

        public OHLCBar(double open, long volume, DateTime dateTime)
            : this(open, open, open, open, dateTime)
        {
        }

        public override string ToString()
        {
            return string.Format("O:{0} H:{1} L:{2} C:{3} V:{4} T(local):{5}", open, high, low, close, volume, dateTime);
        }

    }

    public class GraphModified : ZedGraph.ZedGraphControl
    {
        List<TextObj> myCallTextObj;
        List<TextObj> myPutTextObj;
        List<TextObj> myPriceTextObj;

        public GraphModified( System.Drawing.Size inSize )
        {
            this.IsSynchronizeXAxes = true;
            this.IsAntiAlias = true;
            this.IsShowPointValues = true;
            this.Location = new System.Drawing.Point(1,1);
            this.Size = inSize;
            this.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));

            // Remap the controls.
            this.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.PanModifierKeys = System.Windows.Forms.Keys.None;
            this.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            this.ZoomModifierKeys = System.Windows.Forms.Keys.Control;

            myCallTextObj = new List<TextObj>();
            myPutTextObj = new List<TextObj>();
            myPriceTextObj = new List<TextObj>();

            this.MasterPane.PaneList.Clear();
        }

        public void ZeroMarginPadding()
        {
            foreach (GraphPane aPane in this.MasterPane.PaneList)
            {
                aPane.Margin.Top = 0;
                aPane.Margin.Right = 0;
                aPane.Margin.Left = 0;
                aPane.Margin.Bottom = 0;
                aPane.Title.IsVisible = false;
            }

        }

        public ZedGraph.GraphPane AddAPane(String TitleOfPane)
        {
            // Add a Pane.
            ZedGraph.GraphPane ASinglePane = new ZedGraph.GraphPane();
            this.MasterPane.Add(ASinglePane);

            // Default settings. Set Margins and Font Spec.
            ASinglePane.Legend.IsVisible = true;
            ASinglePane.Title.IsVisible = false;
            ASinglePane.Margin.Top = 0;
            ASinglePane.Margin.Right = 0;
            ASinglePane.Margin.Left = 0;
            ASinglePane.Margin.Bottom = -2;
            ASinglePane.IsFontsScaled = false;
            ASinglePane.YAxis.MinSpace = 0;
            ASinglePane.YAxis.Scale.FontSpec.Size = 12;
            ASinglePane.YAxis.Scale.IsPreventLabelOverlap = false;
            ASinglePane.YAxis.Scale.IsLabelsInside = true;
            
            ASinglePane.YAxis.Scale.MaxAuto = true;
            ASinglePane.YAxis.Scale.MinAuto = true;
            ASinglePane.XAxis.Scale.MaxAuto = true;
            ASinglePane.XAxis.Scale.MinAuto = true;

            //ASinglePane.XAxis.Type = ZedGraph::AxisType::Date;
            //ASinglePane.XAxis.Type = ZedGraph.AxisType.Ordinal;

            // Edit the Legend.
            ASinglePane.Legend.Position = ZedGraph.LegendPos.Float;
            ASinglePane.Legend.Fill.Type = ZedGraph.FillType.None;

            ASinglePane.Title.Text = TitleOfPane;

            // Edit the Pane.
           // ASinglePane.XAxis.Type = ZedGraph.AxisType.Ordinal;
            ASinglePane.XAxis.Scale.Min = 5;//gcnew ZedGraph.XDate( 2006, 1, 1 );
            ASinglePane.XAxis.MajorGrid.IsVisible = true;
            ASinglePane.XAxis.MajorGrid.DashOff = 0;
            ASinglePane.XAxis.MajorGrid.Color = System.Drawing.Color.Silver;
            ASinglePane.XAxis.MajorTic.Color = System.Drawing.Color.Silver;
            ASinglePane.XAxis.MinorTic.Color = System.Drawing.Color.Silver;
            ASinglePane.XAxis.MajorTic.IsOutside = false;
            ASinglePane.XAxis.MinorTic.IsOutside = false;
            ASinglePane.XAxis.Title.FontSpec.Family = "Microsoft Sans Serif";
            ASinglePane.XAxis.Title.FontSpec.Size = 11;
            ASinglePane.XAxis.Scale.FontSpec.Family = "Microsoft Sans Serif";
            ASinglePane.XAxis.Scale.FontSpec.Size = 11;
            ASinglePane.XAxis.Scale.IsVisible = false;

            // Y Axis.
            ASinglePane.YAxis.MajorGrid.IsVisible = true;
            ASinglePane.YAxis.MajorGrid.DashOff = 0;
            ASinglePane.YAxis.MajorGrid.Color = System.Drawing.Color.Silver;
            ASinglePane.YAxis.MajorTic.Color = System.Drawing.Color.Silver;
            ASinglePane.YAxis.MinorTic.Color = System.Drawing.Color.Silver;
            ASinglePane.YAxis.MajorTic.IsOutside = false;
            ASinglePane.YAxis.MinorTic.IsOutside = false;
            ASinglePane.YAxis.Title.FontSpec.Family = "Microsoft Sans Serif";
            ASinglePane.YAxis.Title.FontSpec.Size = 11;
            ASinglePane.YAxis.Scale.FontSpec.Family = "Microsoft Sans Serif";
            ASinglePane.YAxis.Scale.FontSpec.Size = 11;
            //Console.WriteLine( "Inside DrawCandleStickAtPane 5." );
            // pretty it up a little
            ASinglePane.Chart.Fill = new ZedGraph.Fill(System.Drawing.Color.WhiteSmoke);
            ASinglePane.Fill = new ZedGraph.Fill(System.Drawing.Color.WhiteSmoke);

            ASinglePane.YAxis.Scale.Mag = 0;
            
            return ASinglePane;
        }

        public GraphPane getPaneByTitle( String PaneName )
        {
            List<GraphPane> aListOfGraphPane = this.MasterPane.PaneList;
            foreach (GraphPane aPane in aListOfGraphPane)
            {
                if (String.Compare(aPane.Title.Text, PaneName) == 0)
                    return aPane;
            }
            return null;
        }

        public void setScaleToDefault()
        {
            this.RestoreScale(getPaneByTitle("MainGraph"));
        }

        public LineItem PlotPoints(List< KeyValuePair<double, double> > inListOfValues, String inName, System.Drawing.Color inColor, ZedGraph.SymbolType inSymbolType, String inPane, bool PlotLabels = false)
        {
            GraphPane myPane = getPaneByTitle(inPane);
            PointPairList aPointList = new PointPairList();
            for (int i = 0; i < inListOfValues.Count; i++)
            {
                PointPair aPt = new PointPair(inListOfValues[i].Key, inListOfValues[i].Value);
                aPointList.Add(aPt);

                if (PlotLabels == true)
                {
                    String PriceText = inListOfValues[i].Key.ToString("N2");
                    TextObj aLabel = new TextObj(PriceText, inListOfValues[i].Key, inListOfValues[i].Value);
                    aLabel.FontSpec.Border.IsVisible = false;
                    aLabel.FontSpec.Fill.Color = System.Drawing.Color.FromArgb(100, 250, 250, 250);
                    myPane.GraphObjList.Add(aLabel);
                }
            }
            LineItem myCurve = myPane.AddCurve(inName, aPointList, inColor, inSymbolType);
            myCurve.Line.IsVisible = true;
            myCurve.Label.IsVisible = false;

            this.AxisChange();
            this.Invalidate();

            return myCurve;
        }

        public LineItem PlotPricePath(String inName, List<double> inListOfValues, System.Drawing.Color inColor, String inPane, int displayText = 0)
        {
            GraphPane myPane = getPaneByTitle(inPane);
            PointPairList aPointList = new PointPairList();
            for (int i = 0; i < inListOfValues.Count; i++)
            {
                PointPair aPt = new PointPair(i, inListOfValues[i]);
                aPointList.Add(aPt);

                if (displayText == 1)
                {
                    String PriceText = inListOfValues[i].ToString("N4");
                    TextObj aLabel = new TextObj(PriceText, i, inListOfValues[i]);
                    aLabel.FontSpec.Border.IsVisible = false;
                    aLabel.FontSpec.Fill.Color = System.Drawing.Color.FromArgb(100, 250, 250, 250);
                    myPane.GraphObjList.Add(aLabel);
                }
            }
            LineItem myCurve = myPane.AddCurve(inName, aPointList, inColor, SymbolType.None);
            myCurve.Line.Width = 1;
            myCurve.Line.IsVisible = true;
            myCurve.Label.IsVisible = false;

            if (displayText == 2)
            {
                String PriceText = inListOfValues[inListOfValues.Count - 1].ToString("N4");
                TextObj aLabel = new TextObj(PriceText, (inListOfValues.Count - 1), inListOfValues[inListOfValues.Count - 1]);
                aLabel.FontSpec.Border.IsVisible = false;
                aLabel.FontSpec.Fill.Color = System.Drawing.Color.FromArgb(100, 250, 250, 250);
                myPane.GraphObjList.Add(aLabel);
            }

            this.AxisChange();
            this.Invalidate();

            return myCurve;
        }

        public LineItem PlotPricePath(String inName, List<KeyValuePair<double,double>> inListOfValues, System.Drawing.Color inColor, String inPane, bool LabelIsX = true, int displayText = 0)
        {
            GraphPane myPane = getPaneByTitle(inPane);
            PointPairList aPointList = new PointPairList();
            for (int i = 0; i < inListOfValues.Count; i++)
            {
                PointPair aPt = new PointPair(inListOfValues[i].Key, inListOfValues[i].Value);
                aPointList.Add(aPt);

                if (displayText == 1)
                {
                    String PriceText = (LabelIsX == true) ? inListOfValues[i].Key.ToString("N2") : inListOfValues[i].Value.ToString("N2");
                    TextObj aLabel = new TextObj(PriceText, inListOfValues[i].Key, inListOfValues[i].Value);
                    aLabel.FontSpec.Border.IsVisible = false;
                    aLabel.FontSpec.Fill.Color = System.Drawing.Color.FromArgb(100, 250, 250, 250);
                    myPane.GraphObjList.Add(aLabel);
                }
            }
            LineItem myCurve = myPane.AddCurve(inName, aPointList, inColor, SymbolType.None);
            myCurve.Line.Width = 1;
            myCurve.Line.IsVisible = true;
            myCurve.Label.IsVisible = false;

            if (displayText == 2)
            {
                String PriceText = (LabelIsX == true) ? inListOfValues[inListOfValues.Count - 1].Key.ToString("N2") : inListOfValues[inListOfValues.Count - 1].Value.ToString("N4");
                TextObj aLabel = new TextObj(PriceText, inListOfValues[inListOfValues.Count - 1].Key, inListOfValues[inListOfValues.Count - 1].Value);
                aLabel.FontSpec.Border.IsVisible = false;
                aLabel.FontSpec.Fill.Color = System.Drawing.Color.FromArgb(100, 250, 250, 250);
                myPane.GraphObjList.Add(aLabel);
            }

            this.AxisChange();
            this.Invalidate();

            return myCurve;
        }

        public void PlotPoints(List< List<double> > inListOfValues, String inName, System.Drawing.Color inColor, String inPane, bool PriAxis = true, bool plotLines = false)
        {
            GraphPane myPane = getPaneByTitle(inPane);
            PointPairList aPointList = new PointPairList();
            List<TextObj> myListOfTextObj = null;
            if (inName == "Price") { myListOfTextObj = myPriceTextObj; }
            if (inName == "Call") { myListOfTextObj = myCallTextObj; }
            if (inName == "Put") { myListOfTextObj = myPutTextObj; }
            myListOfTextObj.Clear();
            for (int i = 0; i < inListOfValues.Count; i++)
                for (int j = 0; j < inListOfValues[i].Count; j++)
                {
                    PointPair aPt = new PointPair(i, inListOfValues[i][j]);
                    aPointList.Add(aPt);
                    String PriceText = inListOfValues[i][j].ToString("N4");
                    ZedGraph.CoordType LabelCoordType = (PriAxis == true) ? CoordType.AxisXYScale : CoordType.AxisXY2Scale;
                    TextObj aLabel = new TextObj(PriceText, i, inListOfValues[i][j], LabelCoordType);
                    myListOfTextObj.Add(aLabel);
                    aLabel.FontSpec.Border.IsVisible = false;
                    aLabel.FontSpec.Fill.Color = System.Drawing.Color.FromArgb(100, 250, 250, 250);
                    if (inListOfValues[i][j] > 0) { myPane.GraphObjList.Add(aLabel); }
                }
            LineItem myCurve = myPane.AddCurve(inName, aPointList, inColor, SymbolType.Diamond);
            myCurve.IsY2Axis = (PriAxis == false) ? true : false;
            myCurve.Line.IsVisible = false;

            if (plotLines == true)
            {
                // Plot the lines inbetween.
                for (int i = 0; i < inListOfValues.Count - 1; i++)
                    for (int j = 0; j < inListOfValues[i].Count; j++)
                    {
                        PointPairList aPointListForTopLine = new PointPairList();
                        PointPairList aPointListForBottomLine = new PointPairList();
                        aPointListForTopLine.Add(i, inListOfValues[i][j]);
                        aPointListForTopLine.Add(i + 1, inListOfValues[i + 1][j]);
                        aPointListForBottomLine.Add(i, inListOfValues[i][j]);
                        aPointListForBottomLine.Add(i + 1, inListOfValues[i + 1][j + 1]);
                        LineItem TopCurve = myPane.AddCurve(inName + "Top" + i.ToString() + j.ToString(), aPointListForTopLine, inColor, SymbolType.None);
                        LineItem BottomCurve = myPane.AddCurve(inName + "Bottom" + i.ToString() + j.ToString(), aPointListForBottomLine, inColor, SymbolType.None);
                        TopCurve.IsY2Axis = (PriAxis == false) ? true : false;
                        BottomCurve.IsY2Axis = (PriAxis == false) ? true : false;
                    }
                myPane.Legend.IsVisible = false;
            }

            this.AxisChange();
            this.Invalidate();
        }

        public void HideType(String inText, bool inHideTrue)
        {
            List<TextObj> myListOfTextObj = null;
            if (inText == "Price") { myListOfTextObj = myPriceTextObj; }
            if (inText == "Call") { myListOfTextObj = myCallTextObj; }
            if (inText == "Put") { myListOfTextObj = myPutTextObj; }
            GraphPane MainGraphPane = getPaneByTitle("MainGraph");
            foreach (LineItem anItem in MainGraphPane.CurveList)
                if (anItem.Label.Text.Contains(inText))
                    anItem.IsVisible = (inHideTrue == true) ? false : true;
            foreach (TextObj aTextObj in myListOfTextObj)
                aTextObj.IsVisible = (inHideTrue == true) ? false : true;
            this.AxisChange();
            this.Invalidate();
        }

        public void DrawIndicatorAtPane(List<double> inTimeSeries, int OverlayNo, int PaneNo, String LabelName, System.Drawing.Color inColor, bool inLegendTrue, int Offset = 0)
        {
            // Note. We will always override Ordinal. So need to advance PointPair by 1.
            // Console.WriteLine("LabelName is " + LabelName);
            List<double> myTimeSeries = inTimeSeries;
            PointPairList aPointList = new PointPairList();
            for (int i = 0; i < myTimeSeries.Count; i++)
            {
                PointPair aPt = new PointPair(i + 1 + Offset, myTimeSeries[i]);
                aPt.Tag = myTimeSeries[i].ToString() + " at " + i;
                aPointList.Add(aPt);
            }

            // Do for Overlay.
            if ( OverlayNo == 0 ){ goto PaneProcedure; };
            String OHLCPaneName = "OHLCOne";
            if (OverlayNo == 2) { OHLCPaneName = "OHLCTwo"; };
            if (OverlayNo == 3) { OHLCPaneName = "OHLCThree"; };
            GraphPane OHLCPane = getPaneByTitle(OHLCPaneName);
            LineItem myOverlayCurve = OHLCPane.AddCurve(LabelName, aPointList, inColor, SymbolType.None);
            //myOverlayCurve.Line.Width = 2;
            myOverlayCurve.IsOverrideOrdinal = true;
            myOverlayCurve.Symbol.IsVisible = false;
            myOverlayCurve.Label.IsVisible = inLegendTrue;

            // Do for Pane.
        PaneProcedure:
            if (PaneNo == 0) { goto Finish; };
            // Add Pane if not present.
            String PaneName = "IndOne";
            if (PaneNo == 2) { PaneName = "IndTwo"; };
            if (PaneNo == 3) { PaneName = "IndThree"; };
            if (getPaneByTitle(PaneName) == null)
                AddAPane(PaneName);
            GraphPane IndPane = getPaneByTitle(PaneName);
            LineItem myIndCurve = IndPane.AddCurve(LabelName, aPointList, inColor, SymbolType.None);
            myIndCurve.IsOverrideOrdinal = true;
            myIndCurve.Symbol.IsVisible = false;
            myIndCurve.Label.IsVisible = inLegendTrue;

        Finish:
            ;
        }
        
        public void resetScale()
        {
            this.GraphPane.YAxis.Scale.MinAuto = true;
            this.GraphPane.YAxis.Scale.MaxAuto = true;
            this.GraphPane.YAxis.Scale.MajorStepAuto = true;
            this.GraphPane.YAxis.Scale.MinorStepAuto = true;
            this.GraphPane.YAxis.CrossAuto = true;
            this.GraphPane.YAxis.Scale.MagAuto = true;
            this.GraphPane.YAxis.Scale.FormatAuto = true;

            this.GraphPane.XAxis.Scale.MinAuto = true;
            this.GraphPane.XAxis.Scale.MaxAuto = true;
            this.GraphPane.XAxis.Scale.MajorStepAuto = true;
            this.GraphPane.XAxis.Scale.MinorStepAuto = true;
            this.GraphPane.XAxis.CrossAuto = true;
            this.GraphPane.XAxis.Scale.MagAuto = true;
            this.GraphPane.XAxis.Scale.FormatAuto = true;

            this.AxisChange();
            this.Invalidate();

        }

        public StockPointList getStockPointList(List<OHLCBar> inTimeSeries)
        {

            ZedGraph.StockPointList aStockPtList = new ZedGraph.StockPointList();
            int thisIndex = 0;
            foreach (OHLCBar aBar in inTimeSeries)
            {
                XDate aDate = new XDate();
                DateTime thisDateTime = aBar.dateTime;
                aDate.DateTime = thisDateTime;
                StockPt pt = new StockPt(aDate.XLDate, aBar.high, aBar.low, aBar.open, aBar.close, 1);
                pt.Tag = thisDateTime.ToString() + "\nO: " + aBar.open.ToString() + "\nH: " + aBar.high.ToString() +
                    "\nL: " + aBar.low.ToString() + "\nC: " + aBar.close.ToString() + "\nIndex: " + thisIndex.ToString();
                aStockPtList.Add(pt);
                thisIndex = thisIndex + 1;
            }
            return aStockPtList;

        }

        public void DrawCandleStickAtPane(List<OHLCBar> inTimeSeries, String GraphName, String PaneName)
        {
            String TitleNo = PaneName;
            String CandleNo = GraphName;

            StockPointList aSpl = getStockPointList(inTimeSeries);
            GraphPane myPane = getPaneByTitle(TitleNo);
            JapaneseCandleStickItem myCurve = myPane.AddJapaneseCandleStick(CandleNo, aSpl);
            // Edit the Candlestick here.
            myCurve.Stick.Size = 20;
            myCurve.Stick.IsAutoSize = true;
            myCurve.Stick.FallingColor = System.Drawing.Color.Purple;
            myCurve.Stick.FallingBorder.Color = System.Drawing.Color.Purple;
            myCurve.Stick.FallingFill.Color = System.Drawing.Color.Purple;
            myCurve.Stick.Color = System.Drawing.Color.DarkSlateGray;
            myCurve.Stick.RisingBorder.Color = System.Drawing.Color.DarkSlateGray;
            myCurve.Stick.RisingFill.Color = System.Drawing.Color.DarkSlateGray;
            // Edit the Pane here.
            // Common Properties for all the graphs.
            myPane.Title.FontSpec.Family = "Microsoft Sans Serif";
            myPane.Title.FontSpec.Size = 8;

            this.AxisChange();
            this.Invalidate();
        }

	}

}
