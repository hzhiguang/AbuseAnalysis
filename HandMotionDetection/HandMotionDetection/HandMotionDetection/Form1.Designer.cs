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
            this.defaultFrame = new Emgu.CV.UI.ImageBox();
            this.blackFrame = new Emgu.CV.UI.ImageBox();
            this.lbRightHand = new System.Windows.Forms.Label();
            this.lbLeftHand = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.defaultFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // defaultFrame
            // 
            this.defaultFrame.Location = new System.Drawing.Point(8, 8);
            this.defaultFrame.Name = "defaultFrame";
            this.defaultFrame.Size = new System.Drawing.Size(677, 419);
            this.defaultFrame.TabIndex = 3;
            this.defaultFrame.TabStop = false;
            // 
            // blackFrame
            // 
            this.blackFrame.Location = new System.Drawing.Point(8, 441);
            this.blackFrame.Name = "blackFrame";
            this.blackFrame.Size = new System.Drawing.Size(677, 419);
            this.blackFrame.TabIndex = 4;
            this.blackFrame.TabStop = false;
            // 
            // lbRightHand
            // 
            this.lbRightHand.Location = new System.Drawing.Point(720, 441);
            this.lbRightHand.Name = "lbRightHand";
            this.lbRightHand.Size = new System.Drawing.Size(137, 23);
            this.lbRightHand.TabIndex = 5;
            // 
            // lbLeftHand
            // 
            this.lbLeftHand.Location = new System.Drawing.Point(720, 9);
            this.lbLeftHand.Name = "lbLeftHand";
            this.lbLeftHand.Size = new System.Drawing.Size(127, 19);
            this.lbLeftHand.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 887);
            this.Controls.Add(this.lbLeftHand);
            this.Controls.Add(this.lbRightHand);
            this.Controls.Add(this.blackFrame);
            this.Controls.Add(this.defaultFrame);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.defaultFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox defaultFrame;
        private Emgu.CV.UI.ImageBox blackFrame;
        private System.Windows.Forms.Label lbRightHand;
        private System.Windows.Forms.Label lbLeftHand;


    }
}

