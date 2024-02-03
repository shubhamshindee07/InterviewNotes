namespace CameraGrabData
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
            this.txtSearchDevice = new System.Windows.Forms.Button();
            this.btnOpenDevice = new System.Windows.Forms.Button();
            this.btnSetTrigger = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnAllInOne = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCloseDevice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSearchDevice
            // 
            this.txtSearchDevice.Location = new System.Drawing.Point(12, 12);
            this.txtSearchDevice.Name = "txtSearchDevice";
            this.txtSearchDevice.Size = new System.Drawing.Size(96, 23);
            this.txtSearchDevice.TabIndex = 0;
            this.txtSearchDevice.Text = "Search Device";
            this.txtSearchDevice.UseVisualStyleBackColor = true;
            this.txtSearchDevice.Click += new System.EventHandler(this.txtSearchDevice_Click);
            // 
            // btnOpenDevice
            // 
            this.btnOpenDevice.Location = new System.Drawing.Point(114, 12);
            this.btnOpenDevice.Name = "btnOpenDevice";
            this.btnOpenDevice.Size = new System.Drawing.Size(96, 23);
            this.btnOpenDevice.TabIndex = 1;
            this.btnOpenDevice.Text = "Open Device";
            this.btnOpenDevice.UseVisualStyleBackColor = true;
            this.btnOpenDevice.Click += new System.EventHandler(this.btnOpenDevice_Click);
            // 
            // btnSetTrigger
            // 
            this.btnSetTrigger.Location = new System.Drawing.Point(216, 12);
            this.btnSetTrigger.Name = "btnSetTrigger";
            this.btnSetTrigger.Size = new System.Drawing.Size(96, 23);
            this.btnSetTrigger.TabIndex = 2;
            this.btnSetTrigger.Text = "Set Trigger";
            this.btnSetTrigger.UseVisualStyleBackColor = true;
            this.btnSetTrigger.Click += new System.EventHandler(this.btnSetTrigger_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(318, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(96, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnAllInOne
            // 
            this.btnAllInOne.Location = new System.Drawing.Point(160, 50);
            this.btnAllInOne.Name = "btnAllInOne";
            this.btnAllInOne.Size = new System.Drawing.Size(96, 23);
            this.btnAllInOne.TabIndex = 4;
            this.btnAllInOne.Text = "All in One";
            this.btnAllInOne.UseVisualStyleBackColor = true;
            this.btnAllInOne.Click += new System.EventHandler(this.btnAllInOne_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(291, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "All in One";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCloseDevice
            // 
            this.btnCloseDevice.Location = new System.Drawing.Point(420, 12);
            this.btnCloseDevice.Name = "btnCloseDevice";
            this.btnCloseDevice.Size = new System.Drawing.Size(96, 23);
            this.btnCloseDevice.TabIndex = 6;
            this.btnCloseDevice.Text = "Close";
            this.btnCloseDevice.UseVisualStyleBackColor = true;
            this.btnCloseDevice.Click += new System.EventHandler(this.btnCloseDevice_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 102);
            this.Controls.Add(this.btnCloseDevice);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAllInOne);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnSetTrigger);
            this.Controls.Add(this.btnOpenDevice);
            this.Controls.Add(this.txtSearchDevice);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button txtSearchDevice;
        private System.Windows.Forms.Button btnOpenDevice;
        private System.Windows.Forms.Button btnSetTrigger;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnAllInOne;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCloseDevice;
    }
}

