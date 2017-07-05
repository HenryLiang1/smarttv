namespace SmartTV
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.handsMsgLabel = new System.Windows.Forms.Label();
            this.faceMsgLabel = new System.Windows.Forms.Label();
            this.menuPanel = new System.Windows.Forms.Panel();
            this.logoutMenuPictureBox = new System.Windows.Forms.PictureBox();
            this.questionMenuPictureBox = new System.Windows.Forms.PictureBox();
            this.backMenuPictureBox = new System.Windows.Forms.PictureBox();
            this.homeMenuPictureBox = new System.Windows.Forms.PictureBox();
            this.mouseOverPictureBox = new System.Windows.Forms.PictureBox();
            this.pictureBoxVoice = new System.Windows.Forms.PictureBox();
            this.pictureBoxRightHand = new System.Windows.Forms.PictureBox();
            this.pictureBoxFace = new System.Windows.Forms.PictureBox();
            this.pictureBoxLeftHand = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoutMenuPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.questionMenuPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backMenuPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.homeMenuPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseOverPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVoice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRightHand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeftHand)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.pictureBoxVoice);
            this.panel1.Controls.Add(this.pictureBoxRightHand);
            this.panel1.Controls.Add(this.pictureBoxFace);
            this.panel1.Controls.Add(this.pictureBoxLeftHand);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1115, 100);
            this.panel1.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.handsMsgLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.faceMsgLabel, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(387, 13);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(16, 50);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // handsMsgLabel
            // 
            this.handsMsgLabel.AutoSize = true;
            this.handsMsgLabel.BackColor = System.Drawing.Color.Transparent;
            this.handsMsgLabel.Font = new System.Drawing.Font("Microsoft JhengHei", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.handsMsgLabel.ForeColor = System.Drawing.Color.White;
            this.handsMsgLabel.Location = new System.Drawing.Point(12, 0);
            this.handsMsgLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.handsMsgLabel.Name = "handsMsgLabel";
            this.handsMsgLabel.Size = new System.Drawing.Size(0, 50);
            this.handsMsgLabel.TabIndex = 11;
            // 
            // faceMsgLabel
            // 
            this.faceMsgLabel.AutoSize = true;
            this.faceMsgLabel.BackColor = System.Drawing.Color.Transparent;
            this.faceMsgLabel.Font = new System.Drawing.Font("Microsoft JhengHei", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.faceMsgLabel.ForeColor = System.Drawing.Color.White;
            this.faceMsgLabel.Location = new System.Drawing.Point(4, 0);
            this.faceMsgLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.faceMsgLabel.Name = "faceMsgLabel";
            this.faceMsgLabel.Size = new System.Drawing.Size(0, 50);
            this.faceMsgLabel.TabIndex = 10;
            // 
            // menuPanel
            // 
            this.menuPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.menuPanel.Controls.Add(this.logoutMenuPictureBox);
            this.menuPanel.Controls.Add(this.questionMenuPictureBox);
            this.menuPanel.Controls.Add(this.backMenuPictureBox);
            this.menuPanel.Controls.Add(this.homeMenuPictureBox);
            this.menuPanel.Controls.Add(this.mouseOverPictureBox);
            this.menuPanel.Enabled = false;
            this.menuPanel.Location = new System.Drawing.Point(16, 119);
            this.menuPanel.Margin = new System.Windows.Forms.Padding(4);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(878, 175);
            this.menuPanel.TabIndex = 8;
            this.menuPanel.Visible = false;
            // 
            // logoutMenuPictureBox
            // 
            this.logoutMenuPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.logoutMenuPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoutMenuPictureBox.Image = global::SmartTV.Properties.Resources.ic_logout;
            this.logoutMenuPictureBox.Location = new System.Drawing.Point(714, 22);
            this.logoutMenuPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.logoutMenuPictureBox.Name = "logoutMenuPictureBox";
            this.logoutMenuPictureBox.Size = new System.Drawing.Size(140, 131);
            this.logoutMenuPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoutMenuPictureBox.TabIndex = 12;
            this.logoutMenuPictureBox.TabStop = false;
            this.logoutMenuPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.logoutMenuPictureBox_MouseClick);
            this.logoutMenuPictureBox.MouseEnter += new System.EventHandler(this.logoutMenuPictureBox_MouseEnter);
            this.logoutMenuPictureBox.MouseLeave += new System.EventHandler(this.logoutMenuPictureBox_MouseLeave);
            // 
            // questionMenuPictureBox
            // 
            this.questionMenuPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.questionMenuPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.questionMenuPictureBox.Image = global::SmartTV.Properties.Resources.ic_question1;
            this.questionMenuPictureBox.Location = new System.Drawing.Point(544, 22);
            this.questionMenuPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.questionMenuPictureBox.Name = "questionMenuPictureBox";
            this.questionMenuPictureBox.Size = new System.Drawing.Size(140, 131);
            this.questionMenuPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.questionMenuPictureBox.TabIndex = 11;
            this.questionMenuPictureBox.TabStop = false;
            this.questionMenuPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.questionMenuPictureBox_MouseClick);
            this.questionMenuPictureBox.MouseEnter += new System.EventHandler(this.questionMenuPictureBox_MouseEnter);
            this.questionMenuPictureBox.MouseLeave += new System.EventHandler(this.questionMenuPictureBox_MouseLeave);
            // 
            // backMenuPictureBox
            // 
            this.backMenuPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.backMenuPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backMenuPictureBox.Image = global::SmartTV.Properties.Resources.ic_back1;
            this.backMenuPictureBox.Location = new System.Drawing.Point(371, 22);
            this.backMenuPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.backMenuPictureBox.Name = "backMenuPictureBox";
            this.backMenuPictureBox.Size = new System.Drawing.Size(140, 131);
            this.backMenuPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backMenuPictureBox.TabIndex = 10;
            this.backMenuPictureBox.TabStop = false;
            this.backMenuPictureBox.Click += new System.EventHandler(this.backMenuPictureBox_Click);
            this.backMenuPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.backMenuPictureBox_MouseDown);
            this.backMenuPictureBox.MouseEnter += new System.EventHandler(this.backMenuPictureBox_MouseEnter);
            this.backMenuPictureBox.MouseLeave += new System.EventHandler(this.backMenuPictureBox_MouseLeave);
            this.backMenuPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.backMenuPictureBox_MouseUp);
            // 
            // homeMenuPictureBox
            // 
            this.homeMenuPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.homeMenuPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.homeMenuPictureBox.Image = global::SmartTV.Properties.Resources.ic_home1;
            this.homeMenuPictureBox.Location = new System.Drawing.Point(199, 22);
            this.homeMenuPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.homeMenuPictureBox.Name = "homeMenuPictureBox";
            this.homeMenuPictureBox.Size = new System.Drawing.Size(140, 131);
            this.homeMenuPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.homeMenuPictureBox.TabIndex = 9;
            this.homeMenuPictureBox.TabStop = false;
            this.homeMenuPictureBox.Click += new System.EventHandler(this.homeMenuPictureBox_Click);
            this.homeMenuPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.homeMenuPictureBox_MouseDown);
            this.homeMenuPictureBox.MouseEnter += new System.EventHandler(this.homeMenuPictureBox_MouseEnter);
            this.homeMenuPictureBox.MouseLeave += new System.EventHandler(this.homeMenuPictureBox_MouseLeave);
            this.homeMenuPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.homeMenuPictureBox_MouseUp);
            // 
            // mouseOverPictureBox
            // 
            this.mouseOverPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.mouseOverPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mouseOverPictureBox.Image = global::SmartTV.Properties.Resources.ic_cursor1;
            this.mouseOverPictureBox.Location = new System.Drawing.Point(25, 22);
            this.mouseOverPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.mouseOverPictureBox.Name = "mouseOverPictureBox";
            this.mouseOverPictureBox.Size = new System.Drawing.Size(140, 131);
            this.mouseOverPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.mouseOverPictureBox.TabIndex = 7;
            this.mouseOverPictureBox.TabStop = false;
            this.mouseOverPictureBox.Click += new System.EventHandler(this.mouseOverPictureBox_Click);
            this.mouseOverPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.mouseOverPictureBox_Paint);
            this.mouseOverPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseOverPictureBox_MouseDown);
            this.mouseOverPictureBox.MouseEnter += new System.EventHandler(this.mouseOverPictureBox_MouseEnter);
            this.mouseOverPictureBox.MouseLeave += new System.EventHandler(this.mouseOverPictureBox_MouseLeave);
            this.mouseOverPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseOverPictureBox_MouseUp);
            // 
            // pictureBoxVoice
            // 
            this.pictureBoxVoice.Image = global::SmartTV.Properties.Resources.voice;
            this.pictureBoxVoice.Location = new System.Drawing.Point(287, 15);
            this.pictureBoxVoice.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxVoice.Name = "pictureBoxVoice";
            this.pictureBoxVoice.Size = new System.Drawing.Size(67, 62);
            this.pictureBoxVoice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxVoice.TabIndex = 7;
            this.pictureBoxVoice.TabStop = false;
            this.pictureBoxVoice.Visible = false;
            // 
            // pictureBoxRightHand
            // 
            this.pictureBoxRightHand.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxRightHand.Image = global::SmartTV.Properties.Resources.r_hand;
            this.pictureBoxRightHand.Location = new System.Drawing.Point(105, 15);
            this.pictureBoxRightHand.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxRightHand.Name = "pictureBoxRightHand";
            this.pictureBoxRightHand.Size = new System.Drawing.Size(67, 62);
            this.pictureBoxRightHand.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRightHand.TabIndex = 2;
            this.pictureBoxRightHand.TabStop = false;
            // 
            // pictureBoxFace
            // 
            this.pictureBoxFace.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxFace.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxFace.Image")));
            this.pictureBoxFace.Location = new System.Drawing.Point(196, 15);
            this.pictureBoxFace.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxFace.Name = "pictureBoxFace";
            this.pictureBoxFace.Size = new System.Drawing.Size(67, 62);
            this.pictureBoxFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFace.TabIndex = 3;
            this.pictureBoxFace.TabStop = false;
            // 
            // pictureBoxLeftHand
            // 
            this.pictureBoxLeftHand.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLeftHand.Image = global::SmartTV.Properties.Resources.l_hand;
            this.pictureBoxLeftHand.Location = new System.Drawing.Point(16, 15);
            this.pictureBoxLeftHand.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxLeftHand.Name = "pictureBoxLeftHand";
            this.pictureBoxLeftHand.Size = new System.Drawing.Size(67, 62);
            this.pictureBoxLeftHand.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLeftHand.TabIndex = 4;
            this.pictureBoxLeftHand.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1115, 701);
            this.Controls.Add(this.menuPanel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoutMenuPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.questionMenuPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backMenuPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.homeMenuPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouseOverPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVoice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRightHand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeftHand)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxRightHand;
        private System.Windows.Forms.PictureBox pictureBoxFace;
        private System.Windows.Forms.PictureBox pictureBoxLeftHand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxVoice;
        private System.Windows.Forms.PictureBox mouseOverPictureBox;
        private System.Windows.Forms.Panel menuPanel;
        private System.Windows.Forms.PictureBox backMenuPictureBox;
        private System.Windows.Forms.PictureBox homeMenuPictureBox;
        private System.Windows.Forms.Label faceMsgLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label handsMsgLabel;
        private System.Windows.Forms.PictureBox questionMenuPictureBox;
        private System.Windows.Forms.PictureBox logoutMenuPictureBox;

    }
}

