namespace ChildAbuseAnalysis
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbmsg = new System.Windows.Forms.Label();
            this.emotionChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lbFileName = new System.Windows.Forms.Label();
            this.txtFileLink = new System.Windows.Forms.TextBox();
            this.lbEmotionConclusion = new System.Windows.Forms.Label();
            this.lbEmotionConclusion2 = new System.Windows.Forms.Label();
            this.motionChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.zedGraphControl2 = new ZedGraph.ZedGraphControl();
            ((System.ComponentModel.ISupportInitialize)(this.emotionChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.motionChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbmsg
            // 
            this.lbmsg.Location = new System.Drawing.Point(639, 9);
            this.lbmsg.Name = "lbmsg";
            this.lbmsg.Size = new System.Drawing.Size(127, 19);
            this.lbmsg.TabIndex = 8;
            this.lbmsg.Text = "Label";
            // 
            // emotionChart
            // 
            chartArea1.Name = "ChartArea1";
            this.emotionChart.ChartAreas.Add(chartArea1);
            this.emotionChart.Location = new System.Drawing.Point(12, 32);
            this.emotionChart.Name = "emotionChart";
            this.emotionChart.Size = new System.Drawing.Size(562, 286);
            this.emotionChart.TabIndex = 10;
            this.emotionChart.Text = "chart1";
            // 
            // lbFileName
            // 
            this.lbFileName.AutoSize = true;
            this.lbFileName.Location = new System.Drawing.Point(12, 9);
            this.lbFileName.Name = "lbFileName";
            this.lbFileName.Size = new System.Drawing.Size(54, 13);
            this.lbFileName.TabIndex = 11;
            this.lbFileName.Text = "File Path :";
            // 
            // txtFileLink
            // 
            this.txtFileLink.Location = new System.Drawing.Point(72, 6);
            this.txtFileLink.Name = "txtFileLink";
            this.txtFileLink.Size = new System.Drawing.Size(547, 20);
            this.txtFileLink.TabIndex = 12;
            this.txtFileLink.TextChanged += new System.EventHandler(this.txtFileLink_TextChanged);
            // 
            // lbEmotionConclusion
            // 
            this.lbEmotionConclusion.AutoSize = true;
            this.lbEmotionConclusion.Location = new System.Drawing.Point(12, 331);
            this.lbEmotionConclusion.Name = "lbEmotionConclusion";
            this.lbEmotionConclusion.Size = new System.Drawing.Size(87, 13);
            this.lbEmotionConclusion.TabIndex = 13;
            this.lbEmotionConclusion.Text = "The Conclusion :";
            // 
            // lbEmotionConclusion2
            // 
            this.lbEmotionConclusion2.Location = new System.Drawing.Point(105, 331);
            this.lbEmotionConclusion2.Name = "lbEmotionConclusion2";
            this.lbEmotionConclusion2.Size = new System.Drawing.Size(400, 15);
            this.lbEmotionConclusion2.TabIndex = 14;
            // 
            // motionChart
            // 
            chartArea2.Name = "ChartArea1";
            this.motionChart.ChartAreas.Add(chartArea2);
            this.motionChart.Location = new System.Drawing.Point(661, 35);
            this.motionChart.Name = "motionChart";
            this.motionChart.Size = new System.Drawing.Size(562, 286);
            this.motionChart.TabIndex = 15;
            this.motionChart.Text = "chart1";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(632, 799);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 16;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(12, 674);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(581, 148);
            this.axWindowsMediaPlayer1.TabIndex = 17;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(713, 690);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(286, 69);
            this.listBox1.TabIndex = 18;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(649, 690);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Media File:";
            // 
            // zedGraphControl2
            // 
            this.zedGraphControl2.Location = new System.Drawing.Point(15, 364);
            this.zedGraphControl2.Name = "zedGraphControl2";
            this.zedGraphControl2.ScrollGrace = 0D;
            this.zedGraphControl2.ScrollMaxX = 0D;
            this.zedGraphControl2.ScrollMaxY = 0D;
            this.zedGraphControl2.ScrollMaxY2 = 0D;
            this.zedGraphControl2.ScrollMinX = 0D;
            this.zedGraphControl2.ScrollMinY = 0D;
            this.zedGraphControl2.ScrollMinY2 = 0D;
            this.zedGraphControl2.Size = new System.Drawing.Size(984, 295);
            this.zedGraphControl2.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 831);
            this.Controls.Add(this.zedGraphControl2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.motionChart);
            this.Controls.Add(this.lbEmotionConclusion2);
            this.Controls.Add(this.lbEmotionConclusion);
            this.Controls.Add(this.txtFileLink);
            this.Controls.Add(this.lbFileName);
            this.Controls.Add(this.emotionChart);
            this.Controls.Add(this.lbmsg);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.emotionChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.motionChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbmsg;
        private System.Windows.Forms.DataVisualization.Charting.Chart emotionChart;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.TextBox txtFileLink;
        private System.Windows.Forms.Label lbEmotionConclusion;
        private System.Windows.Forms.Label lbEmotionConclusion2;
        private System.Windows.Forms.DataVisualization.Charting.Chart motionChart;
        private System.Windows.Forms.Button btnBrowse;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private ZedGraph.ZedGraphControl zedGraphControl2;


    }
}

