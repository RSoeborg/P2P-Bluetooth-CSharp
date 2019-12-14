namespace ProjectGreenEnvironment
{
    partial class FrmConnect
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
            this.lblScanResults = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblConnected = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblListening = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblScanResults
            // 
            this.lblScanResults.FormattingEnabled = true;
            this.lblScanResults.ItemHeight = 16;
            this.lblScanResults.Location = new System.Drawing.Point(15, 29);
            this.lblScanResults.Name = "lblScanResults";
            this.lblScanResults.Size = new System.Drawing.Size(338, 180);
            this.lblScanResults.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fundet Enheder";
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(15, 215);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(338, 23);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "Skan";
            this.btnScan.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(15, 415);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(338, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Opret forbindelse til enhed";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // lblConnected
            // 
            this.lblConnected.FormattingEnabled = true;
            this.lblConnected.ItemHeight = 16;
            this.lblConnected.Location = new System.Drawing.Point(395, 29);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(322, 180);
            this.lblConnected.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(392, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Forbundet Enheder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(392, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Enheder jeg lytter til";
            // 
            // lblListening
            // 
            this.lblListening.FormattingEnabled = true;
            this.lblListening.ItemHeight = 16;
            this.lblListening.Location = new System.Drawing.Point(395, 258);
            this.lblListening.Name = "lblListening";
            this.lblListening.Size = new System.Drawing.Size(322, 180);
            this.lblListening.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 301);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(78, 17);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Status: Idle";
            // 
            // FrmConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 450);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblListening);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblConnected);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblScanResults);
            this.Name = "FrmConnect";
            this.Text = "Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lblScanResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox lblConnected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lblListening;
        private System.Windows.Forms.Label lblStatus;
    }
}