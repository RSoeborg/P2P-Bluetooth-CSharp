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
            this.btnHide = new System.Windows.Forms.Button();
            this.txtPeersCount = new System.Windows.Forms.TextBox();
            this.lblPeers = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbMaster = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblScanResults
            // 
            this.lblScanResults.FormattingEnabled = true;
            this.lblScanResults.Location = new System.Drawing.Point(11, 24);
            this.lblScanResults.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblScanResults.Name = "lblScanResults";
            this.lblScanResults.Size = new System.Drawing.Size(254, 147);
            this.lblScanResults.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fundet Enheder";
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(11, 175);
            this.btnScan.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(254, 19);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "Skan";
            this.btnScan.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(11, 337);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(254, 19);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Opret forbindelse til enhed";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // lblConnected
            // 
            this.lblConnected.FormattingEnabled = true;
            this.lblConnected.Location = new System.Drawing.Point(296, 24);
            this.lblConnected.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(242, 147);
            this.lblConnected.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Forbundet Enheder";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 193);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Enheder jeg lytter til";
            // 
            // lblListening
            // 
            this.lblListening.FormattingEnabled = true;
            this.lblListening.Location = new System.Drawing.Point(296, 210);
            this.lblListening.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblListening.Name = "lblListening";
            this.lblListening.Size = new System.Drawing.Size(242, 147);
            this.lblListening.TabIndex = 7;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(9, 245);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 13);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Status: Idle";
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(11, 314);
            this.btnHide.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(254, 19);
            this.btnHide.TabIndex = 10;
            this.btnHide.Text = "Klar";
            this.btnHide.UseVisualStyleBackColor = true;
            // 
            // txtPeersCount
            // 
            this.txtPeersCount.Location = new System.Drawing.Point(192, 282);
            this.txtPeersCount.Name = "txtPeersCount";
            this.txtPeersCount.Size = new System.Drawing.Size(73, 20);
            this.txtPeersCount.TabIndex = 11;
            this.txtPeersCount.Text = "2";
            // 
            // lblPeers
            // 
            this.lblPeers.AutoSize = true;
            this.lblPeers.Location = new System.Drawing.Point(153, 285);
            this.lblPeers.Name = "lblPeers";
            this.lblPeers.Size = new System.Drawing.Size(34, 13);
            this.lblPeers.TabIndex = 12;
            this.lblPeers.Text = "Peers";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Jeg er master:";
            // 
            // cbMaster
            // 
            this.cbMaster.AutoSize = true;
            this.cbMaster.Location = new System.Drawing.Point(91, 210);
            this.cbMaster.Name = "cbMaster";
            this.cbMaster.Size = new System.Drawing.Size(58, 17);
            this.cbMaster.TabIndex = 14;
            this.cbMaster.Text = "Master";
            this.cbMaster.UseVisualStyleBackColor = true;
            // 
            // FrmConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 366);
            this.Controls.Add(this.cbMaster);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblPeers);
            this.Controls.Add(this.txtPeersCount);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblListening);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblConnected);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblScanResults);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.TextBox txtPeersCount;
        private System.Windows.Forms.Label lblPeers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbMaster;
    }
}