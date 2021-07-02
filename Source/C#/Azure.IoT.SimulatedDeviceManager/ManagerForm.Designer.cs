namespace Azure.IoT.SimulatedDeviceManager
{
    partial class ManagerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_updateDeviceTwin = new System.Windows.Forms.Button();
            this.nud_reportingRate = new System.Windows.Forms.NumericUpDown();
            this.lbl_reportingRate = new System.Windows.Forms.Label();
            this.btn_newVersion = new System.Windows.Forms.Button();
            this.gb_overTheAirUpdate = new System.Windows.Forms.GroupBox();
            this.lbl_versionNumber = new System.Windows.Forms.Label();
            this.gb_deviceTwin = new System.Windows.Forms.GroupBox();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.clb_devices = new System.Windows.Forms.CheckedListBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nud_reportingRate)).BeginInit();
            this.gb_overTheAirUpdate.SuspendLayout();
            this.gb_deviceTwin.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_updateDeviceTwin
            // 
            this.btn_updateDeviceTwin.Location = new System.Drawing.Point(220, 17);
            this.btn_updateDeviceTwin.Name = "btn_updateDeviceTwin";
            this.btn_updateDeviceTwin.Size = new System.Drawing.Size(63, 23);
            this.btn_updateDeviceTwin.TabIndex = 0;
            this.btn_updateDeviceTwin.Text = "Update";
            this.btn_updateDeviceTwin.UseVisualStyleBackColor = true;
            this.btn_updateDeviceTwin.Click += new System.EventHandler(this.OnUpdateDeviceTwinButtonClick);
            // 
            // nud_reportingRate
            // 
            this.nud_reportingRate.Location = new System.Drawing.Point(148, 17);
            this.nud_reportingRate.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nud_reportingRate.Name = "nud_reportingRate";
            this.nud_reportingRate.Size = new System.Drawing.Size(66, 23);
            this.nud_reportingRate.TabIndex = 1;
            this.nud_reportingRate.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lbl_reportingRate
            // 
            this.lbl_reportingRate.AutoSize = true;
            this.lbl_reportingRate.Location = new System.Drawing.Point(6, 19);
            this.lbl_reportingRate.Name = "lbl_reportingRate";
            this.lbl_reportingRate.Size = new System.Drawing.Size(136, 15);
            this.lbl_reportingRate.TabIndex = 2;
            this.lbl_reportingRate.Text = "Reporting rate (seconds)";
            // 
            // btn_newVersion
            // 
            this.btn_newVersion.Location = new System.Drawing.Point(6, 22);
            this.btn_newVersion.Name = "btn_newVersion";
            this.btn_newVersion.Size = new System.Drawing.Size(123, 23);
            this.btn_newVersion.TabIndex = 0;
            this.btn_newVersion.Text = "Publish new version";
            this.btn_newVersion.UseVisualStyleBackColor = true;
            this.btn_newVersion.Click += new System.EventHandler(this.OnOverTheAirUpdateButtonClick);
            // 
            // gb_overTheAirUpdate
            // 
            this.gb_overTheAirUpdate.Controls.Add(this.lbl_versionNumber);
            this.gb_overTheAirUpdate.Controls.Add(this.btn_newVersion);
            this.gb_overTheAirUpdate.Location = new System.Drawing.Point(12, 193);
            this.gb_overTheAirUpdate.Name = "gb_overTheAirUpdate";
            this.gb_overTheAirUpdate.Size = new System.Drawing.Size(313, 59);
            this.gb_overTheAirUpdate.TabIndex = 3;
            this.gb_overTheAirUpdate.TabStop = false;
            this.gb_overTheAirUpdate.Text = "Over-the-air update";
            // 
            // lbl_versionNumber
            // 
            this.lbl_versionNumber.AutoSize = true;
            this.lbl_versionNumber.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbl_versionNumber.Location = new System.Drawing.Point(136, 23);
            this.lbl_versionNumber.Name = "lbl_versionNumber";
            this.lbl_versionNumber.Size = new System.Drawing.Size(0, 11);
            this.lbl_versionNumber.TabIndex = 1;
            // 
            // gb_deviceTwin
            // 
            this.gb_deviceTwin.Controls.Add(this.btn_refresh);
            this.gb_deviceTwin.Controls.Add(this.clb_devices);
            this.gb_deviceTwin.Controls.Add(this.lbl_reportingRate);
            this.gb_deviceTwin.Controls.Add(this.nud_reportingRate);
            this.gb_deviceTwin.Controls.Add(this.btn_updateDeviceTwin);
            this.gb_deviceTwin.Location = new System.Drawing.Point(13, 13);
            this.gb_deviceTwin.Name = "gb_deviceTwin";
            this.gb_deviceTwin.Size = new System.Drawing.Size(313, 174);
            this.gb_deviceTwin.TabIndex = 4;
            this.gb_deviceTwin.TabStop = false;
            this.gb_deviceTwin.Text = "Device Twin editor";
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(284, 17);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(23, 23);
            this.btn_refresh.TabIndex = 6;
            this.btn_refresh.Text = "⭮";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.OnRefreshButtonClick);
            // 
            // clb_devices
            // 
            this.clb_devices.FormattingEnabled = true;
            this.clb_devices.Location = new System.Drawing.Point(6, 46);
            this.clb_devices.Name = "clb_devices";
            this.clb_devices.Size = new System.Drawing.Size(301, 112);
            this.clb_devices.TabIndex = 5;
            // 
            // ManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 264);
            this.Controls.Add(this.gb_deviceTwin);
            this.Controls.Add(this.gb_overTheAirUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManagerForm";
            this.Text = "Simulated weather station manager";
            ((System.ComponentModel.ISupportInitialize)(this.nud_reportingRate)).EndInit();
            this.gb_overTheAirUpdate.ResumeLayout(false);
            this.gb_overTheAirUpdate.PerformLayout();
            this.gb_deviceTwin.ResumeLayout(false);
            this.gb_deviceTwin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_updateDeviceTwin;
        private System.Windows.Forms.NumericUpDown nud_reportingRate;
        private System.Windows.Forms.Label lbl_reportingRate;
        private System.Windows.Forms.Button btn_newVersion;
        private System.Windows.Forms.GroupBox gb_overTheAirUpdate;
        private System.Windows.Forms.GroupBox gb_deviceTwin;
        private System.Windows.Forms.Label lbl_versionNumber;
        private System.Windows.Forms.CheckedListBox clb_devices;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

