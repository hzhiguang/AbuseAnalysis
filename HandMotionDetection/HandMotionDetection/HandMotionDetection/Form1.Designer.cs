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
            this.btnStart = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.defaultFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // defaultFrame
            // 
            this.defaultFrame.Location = new System.Drawing.Point(8, 8);
            this.defaultFrame.Name = "defaultFrame";
            this.defaultFrame.Size = new System.Drawing.Size(640, 480);
            this.defaultFrame.TabIndex = 3;
            this.defaultFrame.TabStop = false;
            // 
            // blackFrame
            // 
            this.blackFrame.Location = new System.Drawing.Point(659, 8);
            this.blackFrame.Name = "blackFrame";
            this.blackFrame.Size = new System.Drawing.Size(640, 480);
            this.blackFrame.TabIndex = 4;
            this.blackFrame.TabStop = false;
            // 
            // lbRightHand
            // 
            this.lbRightHand.Location = new System.Drawing.Point(916, 506);
            this.lbRightHand.Name = "lbRightHand";
            this.lbRightHand.Size = new System.Drawing.Size(137, 23);
            this.lbRightHand.TabIndex = 5;
            this.lbRightHand.Text = "Right Hand";
            // 
            // lbLeftHand
            // 
            this.lbLeftHand.Location = new System.Drawing.Point(752, 506);
            this.lbLeftHand.Name = "lbLeftHand";
            this.lbLeftHand.Size = new System.Drawing.Size(127, 19);
            this.lbLeftHand.TabIndex = 6;
            this.lbLeftHand.Text = "Left Hand";
            // 
            // lbmsg
            // 
            this.lbmsg.Location = new System.Drawing.Point(598, 506);
            this.lbmsg.Name = "lbmsg";
            this.lbmsg.Size = new System.Drawing.Size(127, 19);
            this.lbmsg.TabIndex = 8;
            this.lbmsg.Text = "Label";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(8, 802);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(8, 494);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(562, 286);
            this.chart1.TabIndex = 10;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 839);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lbmsg);
            this.Controls.Add(this.lbLeftHand);
            this.Controls.Add(this.lbRightHand);
            this.Controls.Add(this.blackFrame);
            this.Controls.Add(this.defaultFrame);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.defaultFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox defaultFrame;
        private Emgu.CV.UI.ImageBox blackFrame;
        private System.Windows.Forms.Label lbRightHand;
        private System.Windows.Forms.Label lbLeftHand;
        private System.Windows.Forms.Label lbmsg;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;


    }
}

