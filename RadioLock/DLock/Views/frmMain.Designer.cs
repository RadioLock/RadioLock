namespace RadioLock
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnCreate = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtBuilding = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFloor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtSystemCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHotelCode = new System.Windows.Forms.TextBox();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.txtCardNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button4 = new System.Windows.Forms.Button();
            this.IDD102_1001 = new System.Windows.Forms.RadioButton();
            this.IDD102_1000 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(13, 13);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create Card";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Visible = false;
            //this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Read Card";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            //this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBuilding
            // 
            this.txtBuilding.Location = new System.Drawing.Point(201, 127);
            this.txtBuilding.Name = "txtBuilding";
            this.txtBuilding.Size = new System.Drawing.Size(65, 20);
            this.txtBuilding.TabIndex = 2;
            this.txtBuilding.Text = "01";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(118, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Building";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(301, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Floor";
            // 
            // txtFloor
            // 
            this.txtFloor.Location = new System.Drawing.Point(364, 127);
            this.txtFloor.Name = "txtFloor";
            this.txtFloor.Size = new System.Drawing.Size(65, 20);
            this.txtFloor.TabIndex = 4;
            this.txtFloor.Text = "01";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Room No";
            // 
            // txtRoom
            // 
            this.txtRoom.Location = new System.Drawing.Point(201, 147);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(65, 20);
            this.txtRoom.TabIndex = 6;
            this.txtRoom.Text = "101";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(201, 126);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(181, 21);
            this.comboBox1.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(117, 57);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(419, 20);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Xin mời bạn thực hiện Kết nối đến hệ thống khóa từ RLock";
            // 
            // txtSystemCode
            // 
            this.txtSystemCode.Location = new System.Drawing.Point(99, 420);
            this.txtSystemCode.Name = "txtSystemCode";
            this.txtSystemCode.Size = new System.Drawing.Size(181, 20);
            this.txtSystemCode.TabIndex = 10;
            this.txtSystemCode.Text = "7c02";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 423);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "System Code";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 465);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Hotel Code";
            // 
            // txtHotelCode
            // 
            this.txtHotelCode.Location = new System.Drawing.Point(99, 462);
            this.txtHotelCode.Name = "txtHotelCode";
            this.txtHotelCode.Size = new System.Drawing.Size(181, 20);
            this.txtHotelCode.TabIndex = 12;
            this.txtHotelCode.Text = "FEBBA5B4";
            // 
            // fromDate
            // 
            this.fromDate.Location = new System.Drawing.Point(201, 193);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(200, 20);
            this.fromDate.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(126, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "From";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(136, 243);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "To";
            // 
            // toDate
            // 
            this.toDate.Location = new System.Drawing.Point(201, 237);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(200, 20);
            this.toDate.TabIndex = 16;
            // 
            // txtCardNo
            // 
            this.txtCardNo.Location = new System.Drawing.Point(201, 280);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Size = new System.Drawing.Size(181, 20);
            this.txtCardNo.TabIndex = 18;
            this.txtCardNo.Text = "2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(112, 280);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Card No";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(107, 319);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Date Info";
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(201, 319);
            this.txtDate.Name = "txtDate";
            this.txtDate.ReadOnly = true;
            this.txtDate.Size = new System.Drawing.Size(181, 20);
            this.txtDate.TabIndex = 20;
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(115, 16);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(262, 20);
            this.txtLocation.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(379, 427);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Chọn loại khóa";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(383, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(162, 23);
            this.button2.TabIndex = 24;
            this.button2.Text = "Lựa chọn thư mục cài đặt";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(201, 369);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(157, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "Kết nối";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            //this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RadioLock.Properties.Resources.icon1;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 101);
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(329, 147);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(157, 23);
            this.button4.TabIndex = 27;
            this.button4.Text = "Cập nhật danh sách phòng";
            this.button4.UseVisualStyleBackColor = true;
            //this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // IDD102_1001
            // 
            this.IDD102_1001.AutoSize = true;
            this.IDD102_1001.Checked = true;
            this.IDD102_1001.Font = new System.Drawing.Font("SimSun", 12F);
            this.IDD102_1001.Location = new System.Drawing.Point(412, 280);
            this.IDD102_1001.Name = "IDD102_1001";
            this.IDD102_1001.Size = new System.Drawing.Size(74, 20);
            this.IDD102_1001.TabIndex = 30;
            this.IDD102_1001.TabStop = true;
            this.IDD102_1001.Text = "5-RF50";
            this.IDD102_1001.UseVisualStyleBackColor = true;
            // 
            // IDD102_1000
            // 
            this.IDD102_1000.AutoSize = true;
            this.IDD102_1000.Font = new System.Drawing.Font("SimSun", 12F);
            this.IDD102_1000.Location = new System.Drawing.Point(383, 359);
            this.IDD102_1000.Name = "IDD102_1000";
            this.IDD102_1000.Size = new System.Drawing.Size(74, 20);
            this.IDD102_1000.TabIndex = 29;
            this.IDD102_1000.Text = "4-RF57";
            this.IDD102_1000.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 106);
            this.Controls.Add(this.IDD102_1001);
            this.Controls.Add(this.IDD102_1000);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtCardNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.toDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.fromDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtHotelCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSystemCode);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRoom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFloor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBuilding);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCreate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(573, 535);
            this.Name = "frmMain";
            this.Text = "ezCloud Hotel - GLock, DLock Connector";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtBuilding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFloor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtSystemCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHotelCode;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.TextBox txtCardNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton IDD102_1001;
        private System.Windows.Forms.RadioButton IDD102_1000;
    }
}

