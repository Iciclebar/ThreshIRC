namespace willIRC2
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
        	this.txtDisplay = new System.Windows.Forms.TextBox();
        	this.txtList = new System.Windows.Forms.TextBox();
        	this.txtText = new System.Windows.Forms.TextBox();
        	this.btnConnect = new System.Windows.Forms.Button();
        	this.SuspendLayout();
        	// 
        	// txtDisplay
        	// 
        	this.txtDisplay.Location = new System.Drawing.Point(12, 24);
        	this.txtDisplay.Multiline = true;
        	this.txtDisplay.Name = "txtDisplay";
        	this.txtDisplay.Size = new System.Drawing.Size(454, 464);
        	this.txtDisplay.TabIndex = 0;
        	this.txtDisplay.TextChanged += new System.EventHandler(this.txtDisplay_TextChanged);
        	// 
        	// txtList
        	// 
        	this.txtList.Location = new System.Drawing.Point(481, 24);
        	this.txtList.Multiline = true;
        	this.txtList.Name = "txtList";
        	this.txtList.Size = new System.Drawing.Size(177, 461);
        	this.txtList.TabIndex = 1;
        	// 
        	// txtText
        	// 
        	this.txtText.Location = new System.Drawing.Point(12, 492);
        	this.txtText.Multiline = true;
        	this.txtText.Name = "txtText";
        	this.txtText.Size = new System.Drawing.Size(645, 49);
        	this.txtText.TabIndex = 2;
        	this.txtText.TextChanged += new System.EventHandler(this.txtText_TextChanged);
        	// 
        	// btnConnect
        	// 
        	this.btnConnect.Location = new System.Drawing.Point(12, 3);
        	this.btnConnect.Name = "btnConnect";
        	this.btnConnect.Size = new System.Drawing.Size(645, 21);
        	this.btnConnect.TabIndex = 3;
        	this.btnConnect.Text = "Connect";
        	this.btnConnect.UseVisualStyleBackColor = true;
        	this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
        	// 
        	// Form1
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(673, 569);
        	this.Controls.Add(this.btnConnect);
        	this.Controls.Add(this.txtText);
        	this.Controls.Add(this.txtList);
        	this.Controls.Add(this.txtDisplay);
        	this.Name = "Form1";
        	this.Text = "WillIRC";
        	this.Shown += new System.EventHandler(this.Form1_HandleCreated);
        	this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_OnFormClosed);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.TextBox txtList;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Button btnConnect;
    }
}

