namespace uDrawTablet
{
    partial class ButtonPreferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonPreferences));
            this.optLeft = new System.Windows.Forms.RadioButton();
            this.optRight = new System.Windows.Forms.RadioButton();
            this.optOpenApp = new System.Windows.Forms.RadioButton();
            this.optDefault = new System.Windows.Forms.RadioButton();
            this.btnSpecify = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.AppLocation = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // optLeft
            // 
            this.optLeft.AutoSize = true;
            this.optLeft.Location = new System.Drawing.Point(13, 13);
            this.optLeft.Name = "optLeft";
            this.optLeft.Size = new System.Drawing.Size(104, 17);
            this.optLeft.TabIndex = 0;
            this.optLeft.Text = "Left Mouse Click";
            this.optLeft.UseVisualStyleBackColor = true;
            this.optLeft.CheckedChanged += new System.EventHandler(this.optLeft_CheckedChanged);
            // 
            // optRight
            // 
            this.optRight.AutoSize = true;
            this.optRight.Location = new System.Drawing.Point(126, 12);
            this.optRight.Name = "optRight";
            this.optRight.Size = new System.Drawing.Size(111, 17);
            this.optRight.TabIndex = 1;
            this.optRight.Text = "Right Mouse Click";
            this.optRight.UseVisualStyleBackColor = true;
            this.optRight.CheckedChanged += new System.EventHandler(this.optRight_CheckedChanged);
            // 
            // optOpenApp
            // 
            this.optOpenApp.AutoSize = true;
            this.optOpenApp.Location = new System.Drawing.Point(13, 36);
            this.optOpenApp.Name = "optOpenApp";
            this.optOpenApp.Size = new System.Drawing.Size(106, 17);
            this.optOpenApp.TabIndex = 2;
            this.optOpenApp.Text = "Open Application";
            this.optOpenApp.UseVisualStyleBackColor = true;
            this.optOpenApp.CheckedChanged += new System.EventHandler(this.optOpenApp_CheckedChanged);
            // 
            // optDefault
            // 
            this.optDefault.AutoSize = true;
            this.optDefault.Checked = true;
            this.optDefault.Location = new System.Drawing.Point(126, 36);
            this.optDefault.Name = "optDefault";
            this.optDefault.Size = new System.Drawing.Size(79, 17);
            this.optDefault.TabIndex = 3;
            this.optDefault.TabStop = true;
            this.optDefault.Text = "Do Nothing";
            this.optDefault.UseVisualStyleBackColor = true;
            this.optDefault.CheckedChanged += new System.EventHandler(this.optDefault_CheckedChanged);
            // 
            // btnSpecify
            // 
            this.btnSpecify.Enabled = false;
            this.btnSpecify.Location = new System.Drawing.Point(13, 59);
            this.btnSpecify.Name = "btnSpecify";
            this.btnSpecify.Size = new System.Drawing.Size(112, 23);
            this.btnSpecify.TabIndex = 4;
            this.btnSpecify.Text = "Specify Application";
            this.btnSpecify.UseVisualStyleBackColor = true;
            this.btnSpecify.Click += new System.EventHandler(this.btnSpecify_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(13, 93);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 5;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(94, 93);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(175, 93);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AppLocation
            // 
            this.AppLocation.Enabled = false;
            this.AppLocation.Location = new System.Drawing.Point(132, 60);
            this.AppLocation.Name = "AppLocation";
            this.AppLocation.Size = new System.Drawing.Size(100, 20);
            this.AppLocation.TabIndex = 8;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ButtonPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 128);
            this.Controls.Add(this.AppLocation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnSpecify);
            this.Controls.Add(this.optDefault);
            this.Controls.Add(this.optOpenApp);
            this.Controls.Add(this.optRight);
            this.Controls.Add(this.optLeft);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ButtonPreferences";
            this.Text = "Button Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton optLeft;
        private System.Windows.Forms.RadioButton optRight;
        private System.Windows.Forms.RadioButton optOpenApp;
        private System.Windows.Forms.RadioButton optDefault;
        private System.Windows.Forms.Button btnSpecify;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox AppLocation;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}