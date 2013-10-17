namespace HandMotionDetection
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.defaultFrame = new Emgu.CV.UI.ImageBox();
            this.blackFrame = new Emgu.CV.UI.ImageBox();
            this.lbRightHand = new System.Windows.Forms.Label();
            this.lbLeftHand = new System.Windows.Forms.Label();
            this.lbmsg = new System.Windows.Forms.Label();
            this.emotionChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lbFileName = new System.Windows.Forms.Label();
            this.txtFileLink = new System.Windows.Forms.TextBox();
            this.lbEmotionConclusion = new System.Windows.Forms.Label();
            this.lbEmotionConclusion2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.defaultFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emotionChart)).BeginInit();
            this.SuspendLayout();
            // 
            // defaultFrame
            // 
            this.defaultFrame.Location = new System.Drawing.Point(15, 366);
            this.defaultFrame.Name = "defaultFrame";
            this.defaultFrame.Size = new System.Drawing.Size(640, 480);
            this.defaultFrame.TabIndex = 3;
            this.defaultFrame.TabStop = false;
            // 
            // blackFrame
            // 
            this.blackFrame.Location = new System.Drawing.Point(661, 366);
            this.blackFrame.Name = "blackFrame";
            this.blackFrame.Size = new System.Drawing.Size(640, 480);
            this.blackFrame.TabIndex = 4;
            this.blackFrame.TabStop = false;
            // 
            // lbRightHand
            // 
            this.lbRightHand.Location = new System.Drawing.Point(804, 9);
            this.lbRightHand.Name = "lbRightHand";
            this.lbRightHand.Size = new System.Drawing.Size(137, 23);
            this.lbRightHand.TabIndex = 5;
            this.lbRightHand.Text = "Right Hand";
            // 
            // lbLeftHand
            // 
            this.lbLeftHand.Location = new System.Drawing.Point(947, 9);
            this.lbLeftHand.Name = "lbLeftHand";
            this.lbLeftHand.Size = new System.Drawing.Size(127, 19);
            this.lbLeftHand.TabIndex = 6;
            this.lbLeftHand.Text = "Left Hand";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 858);
            this.Controls.Add(this.lbEmotionConclusion2);
            this.Controls.Add(this.lbEmotionConclusion);
            this.Controls.Add(this.txtFileLink);
            this.Controls.Add(this.lbFileName);
            this.Controls.Add(this.emotionChart);
            this.Controls.Add(this.lbmsg);
            this.Controls.Add(this.lbLeftHand);
            this.Controls.Add(this.lbRightHand);
            this.Controls.Add(this.blackFrame);
            this.Controls.Add(this.defaultFrame);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.defaultFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emotionChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox defaultFrame;
        private Emgu.CV.UI.ImageBox blackFrame;
        private System.Windows.Forms.Label lbRightHand;
        private System.Windows.Forms.Label lbLeftHand;
        private System.Windows.Forms.Label lbmsg;
        private System.Windows.Forms.DataVisualization.Charting.Chart emotionChart;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.TextBox txtFileLink;
        private System.Windows.Forms.Label lbEmotionConclusion;
        private System.Windows.Forms.Label lbEmotionConclusion2;


    }
}

