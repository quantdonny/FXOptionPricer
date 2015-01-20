namespace FXOptionPricer
{
    partial class MainView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.MonteCarloTab = new System.Windows.Forms.TabPage();
            this.FormulaTab = new System.Windows.Forms.TabPage();
            this.BacktestTab = new System.Windows.Forms.TabPage();
            this.LiveTab = new System.Windows.Forms.TabPage();
            this.SmileTab = new System.Windows.Forms.TabPage();
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.OptionParaLabel = new System.Windows.Forms.Label();
            this.OptionParaPanel = new System.Windows.Forms.Panel();
            this.StrikeIncrementLabel = new System.Windows.Forms.Label();
            this.StrikeIncrementBox = new System.Windows.Forms.TextBox();
            this.ParaUpdateBut = new System.Windows.Forms.Button();
            this.ParameterLabel = new System.Windows.Forms.Label();
            this.ParameterPanel = new System.Windows.Forms.Panel();
            this.SelectCcyLabel = new System.Windows.Forms.Label();
            this.PriceLabel = new System.Windows.Forms.Label();
            this.WaitLabel = new System.Windows.Forms.Label();
            this.OptionTypeBox = new System.Windows.Forms.ComboBox();
            this.ModelBox = new System.Windows.Forms.ComboBox();
            this.CurrencySelectBox = new System.Windows.Forms.ComboBox();
            this.MainTabControl.SuspendLayout();
            this.LeftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTabControl.Controls.Add(this.MonteCarloTab);
            this.MainTabControl.Controls.Add(this.FormulaTab);
            this.MainTabControl.Controls.Add(this.BacktestTab);
            this.MainTabControl.Controls.Add(this.LiveTab);
            this.MainTabControl.Controls.Add(this.SmileTab);
            this.MainTabControl.Location = new System.Drawing.Point(165, 1);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(648, 613);
            this.MainTabControl.TabIndex = 0;
            // 
            // MonteCarloTab
            // 
            this.MonteCarloTab.Location = new System.Drawing.Point(4, 22);
            this.MonteCarloTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MonteCarloTab.Name = "MonteCarloTab";
            this.MonteCarloTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MonteCarloTab.Size = new System.Drawing.Size(640, 587);
            this.MonteCarloTab.TabIndex = 0;
            this.MonteCarloTab.Text = "Monte Carlo";
            this.MonteCarloTab.UseVisualStyleBackColor = true;
            // 
            // FormulaTab
            // 
            this.FormulaTab.Location = new System.Drawing.Point(4, 22);
            this.FormulaTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FormulaTab.Name = "FormulaTab";
            this.FormulaTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FormulaTab.Size = new System.Drawing.Size(640, 587);
            this.FormulaTab.TabIndex = 1;
            this.FormulaTab.Text = "Graph";
            this.FormulaTab.UseVisualStyleBackColor = true;
            // 
            // BacktestTab
            // 
            this.BacktestTab.Location = new System.Drawing.Point(4, 22);
            this.BacktestTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BacktestTab.Name = "BacktestTab";
            this.BacktestTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BacktestTab.Size = new System.Drawing.Size(640, 587);
            this.BacktestTab.TabIndex = 2;
            this.BacktestTab.Text = "Historical";
            this.BacktestTab.UseVisualStyleBackColor = true;
            // 
            // LiveTab
            // 
            this.LiveTab.Location = new System.Drawing.Point(4, 22);
            this.LiveTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LiveTab.Name = "LiveTab";
            this.LiveTab.Size = new System.Drawing.Size(640, 587);
            this.LiveTab.TabIndex = 3;
            this.LiveTab.Text = "Live";
            this.LiveTab.UseVisualStyleBackColor = true;
            // 
            // SmileTab
            // 
            this.SmileTab.Location = new System.Drawing.Point(4, 22);
            this.SmileTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SmileTab.Name = "SmileTab";
            this.SmileTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SmileTab.Size = new System.Drawing.Size(640, 587);
            this.SmileTab.TabIndex = 4;
            this.SmileTab.Text = "Smile";
            this.SmileTab.UseVisualStyleBackColor = true;
            // 
            // LeftPanel
            // 
            this.LeftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LeftPanel.BackColor = System.Drawing.SystemColors.Menu;
            this.LeftPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LeftPanel.Controls.Add(this.OptionParaLabel);
            this.LeftPanel.Controls.Add(this.OptionParaPanel);
            this.LeftPanel.Controls.Add(this.StrikeIncrementLabel);
            this.LeftPanel.Controls.Add(this.StrikeIncrementBox);
            this.LeftPanel.Controls.Add(this.ParaUpdateBut);
            this.LeftPanel.Controls.Add(this.ParameterLabel);
            this.LeftPanel.Controls.Add(this.ParameterPanel);
            this.LeftPanel.Controls.Add(this.SelectCcyLabel);
            this.LeftPanel.Controls.Add(this.PriceLabel);
            this.LeftPanel.Controls.Add(this.WaitLabel);
            this.LeftPanel.Controls.Add(this.OptionTypeBox);
            this.LeftPanel.Controls.Add(this.ModelBox);
            this.LeftPanel.Controls.Add(this.CurrencySelectBox);
            this.LeftPanel.Location = new System.Drawing.Point(2, 1);
            this.LeftPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(163, 613);
            this.LeftPanel.TabIndex = 1;
            // 
            // OptionParaLabel
            // 
            this.OptionParaLabel.AutoSize = true;
            this.OptionParaLabel.Location = new System.Drawing.Point(8, 356);
            this.OptionParaLabel.Name = "OptionParaLabel";
            this.OptionParaLabel.Size = new System.Drawing.Size(97, 13);
            this.OptionParaLabel.TabIndex = 19;
            this.OptionParaLabel.Text = "Option Parameters:";
            // 
            // OptionParaPanel
            // 
            this.OptionParaPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.OptionParaPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.OptionParaPanel.Location = new System.Drawing.Point(7, 374);
            this.OptionParaPanel.Margin = new System.Windows.Forms.Padding(0);
            this.OptionParaPanel.Name = "OptionParaPanel";
            this.OptionParaPanel.Size = new System.Drawing.Size(144, 105);
            this.OptionParaPanel.TabIndex = 18;
            // 
            // StrikeIncrementLabel
            // 
            this.StrikeIncrementLabel.AutoSize = true;
            this.StrikeIncrementLabel.Location = new System.Drawing.Point(9, 482);
            this.StrikeIncrementLabel.Name = "StrikeIncrementLabel";
            this.StrikeIncrementLabel.Size = new System.Drawing.Size(118, 13);
            this.StrikeIncrementLabel.TabIndex = 17;
            this.StrikeIncrementLabel.Text = "Strike Increment (ticks):";
            // 
            // StrikeIncrementBox
            // 
            this.StrikeIncrementBox.Location = new System.Drawing.Point(11, 498);
            this.StrikeIncrementBox.Name = "StrikeIncrementBox";
            this.StrikeIncrementBox.Size = new System.Drawing.Size(100, 20);
            this.StrikeIncrementBox.TabIndex = 16;
            this.StrikeIncrementBox.Text = "100";
            this.StrikeIncrementBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.StrikeIncrementBox_KeyDown);
            this.StrikeIncrementBox.Leave += new System.EventHandler(this.StrikeIncrementBox_Leave);
            // 
            // ParaUpdateBut
            // 
            this.ParaUpdateBut.Location = new System.Drawing.Point(8, 293);
            this.ParaUpdateBut.Name = "ParaUpdateBut";
            this.ParaUpdateBut.Size = new System.Drawing.Size(143, 23);
            this.ParaUpdateBut.TabIndex = 14;
            this.ParaUpdateBut.Text = "Parameters Updated";
            this.ParaUpdateBut.UseVisualStyleBackColor = true;
            this.ParaUpdateBut.Click += new System.EventHandler(this.ParaUpdateBut_Click);
            // 
            // ParameterLabel
            // 
            this.ParameterLabel.AutoSize = true;
            this.ParameterLabel.Location = new System.Drawing.Point(7, 122);
            this.ParameterLabel.Name = "ParameterLabel";
            this.ParameterLabel.Size = new System.Drawing.Size(95, 13);
            this.ParameterLabel.TabIndex = 13;
            this.ParameterLabel.Text = "Model Parameters:";
            // 
            // ParameterPanel
            // 
            this.ParameterPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ParameterPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ParameterPanel.Location = new System.Drawing.Point(7, 138);
            this.ParameterPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ParameterPanel.Name = "ParameterPanel";
            this.ParameterPanel.Size = new System.Drawing.Size(144, 152);
            this.ParameterPanel.TabIndex = 12;
            // 
            // SelectCcyLabel
            // 
            this.SelectCcyLabel.AutoSize = true;
            this.SelectCcyLabel.Location = new System.Drawing.Point(8, 27);
            this.SelectCcyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SelectCcyLabel.Name = "SelectCcyLabel";
            this.SelectCcyLabel.Size = new System.Drawing.Size(61, 13);
            this.SelectCcyLabel.TabIndex = 11;
            this.SelectCcyLabel.Text = "Select Ccy:";
            // 
            // PriceLabel
            // 
            this.PriceLabel.AutoSize = true;
            this.PriceLabel.Location = new System.Drawing.Point(7, 70);
            this.PriceLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PriceLabel.Name = "PriceLabel";
            this.PriceLabel.Size = new System.Drawing.Size(34, 13);
            this.PriceLabel.TabIndex = 10;
            this.PriceLabel.Text = "Price:";
            // 
            // WaitLabel
            // 
            this.WaitLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WaitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WaitLabel.ForeColor = System.Drawing.Color.Crimson;
            this.WaitLabel.Location = new System.Drawing.Point(10, 530);
            this.WaitLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.WaitLabel.Name = "WaitLabel";
            this.WaitLabel.Size = new System.Drawing.Size(141, 33);
            this.WaitLabel.TabIndex = 8;
            this.WaitLabel.Text = "Please Wait";
            this.WaitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.WaitLabel.Visible = false;
            // 
            // OptionTypeBox
            // 
            this.OptionTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OptionTypeBox.FormattingEnabled = true;
            this.OptionTypeBox.Items.AddRange(new object[] {
            "European",
            "American",
            "Euro w/ AUI",
            "Euro w/ ADI",
            "Euro w/ AUO",
            "Euro w/ ADO"});
            this.OptionTypeBox.Location = new System.Drawing.Point(7, 324);
            this.OptionTypeBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OptionTypeBox.Name = "OptionTypeBox";
            this.OptionTypeBox.Size = new System.Drawing.Size(144, 21);
            this.OptionTypeBox.TabIndex = 7;
            this.OptionTypeBox.SelectedIndexChanged += new System.EventHandler(this.OptionTypeBox_SelectedIndexChanged);
            // 
            // ModelBox
            // 
            this.ModelBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModelBox.FormattingEnabled = true;
            this.ModelBox.Items.AddRange(new object[] {
            "Geometric",
            "Brownian",
            "CIR",
            "Heston",
            "Sabr"});
            this.ModelBox.Location = new System.Drawing.Point(8, 93);
            this.ModelBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ModelBox.Name = "ModelBox";
            this.ModelBox.Size = new System.Drawing.Size(143, 21);
            this.ModelBox.TabIndex = 6;
            this.ModelBox.SelectedIndexChanged += new System.EventHandler(this.ModelBox_SelectedIndexChanged);
            // 
            // CurrencySelectBox
            // 
            this.CurrencySelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CurrencySelectBox.FormattingEnabled = true;
            this.CurrencySelectBox.Items.AddRange(new object[] {
            "EURUSD",
            "GBPUSD",
            "USDJPY",
            "USDCHF",
            "AUDUSD"});
            this.CurrencySelectBox.Location = new System.Drawing.Point(7, 45);
            this.CurrencySelectBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CurrencySelectBox.Name = "CurrencySelectBox";
            this.CurrencySelectBox.Size = new System.Drawing.Size(144, 21);
            this.CurrencySelectBox.TabIndex = 0;
            this.CurrencySelectBox.SelectedIndexChanged += new System.EventHandler(this.CurrencySelectBox_SelectedIndexChanged);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 614);
            this.Controls.Add(this.LeftPanel);
            this.Controls.Add(this.MainTabControl);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainView";
            this.Text = "FX Option Pricer";
            this.MainTabControl.ResumeLayout(false);
            this.LeftPanel.ResumeLayout(false);
            this.LeftPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage MonteCarloTab;
        private System.Windows.Forms.TabPage FormulaTab;
        private System.Windows.Forms.TabPage BacktestTab;
        private System.Windows.Forms.TabPage LiveTab;
        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.ComboBox CurrencySelectBox;
        private System.Windows.Forms.ComboBox ModelBox;
        private System.Windows.Forms.ComboBox OptionTypeBox;
        private System.Windows.Forms.Label WaitLabel;
        private System.Windows.Forms.Label PriceLabel;
        private System.Windows.Forms.Label SelectCcyLabel;
        private System.Windows.Forms.Label ParameterLabel;
        private System.Windows.Forms.Panel ParameterPanel;
        private System.Windows.Forms.Button ParaUpdateBut;
        private System.Windows.Forms.Label StrikeIncrementLabel;
        private System.Windows.Forms.TextBox StrikeIncrementBox;
        private System.Windows.Forms.Label OptionParaLabel;
        private System.Windows.Forms.Panel OptionParaPanel;
        private System.Windows.Forms.TabPage SmileTab;
    }
}

