﻿using MGL;
using System;
using System.Threading;
using System.Windows.Forms;

namespace CustomMessageBox
{
    class CustomDialog : Form
    {
        private Button yesButton;
        private Label label1;
        private Button button1;
        private Button noButton;

        #region -- Инициализировать компоненты --
        public CustomDialog()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            yesButton.Enabled = false;
        }
        #endregion

        #region -- Делает форму перетаскиваемой --
        private void Form1_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }
        #endregion

        #region -- Custom Box --
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomDialog));
            this.yesButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // yesButton
            // 
            this.yesButton.BackColor = System.Drawing.Color.Transparent;
            this.yesButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.yesButton.ForeColor = System.Drawing.Color.LightGray;
            this.yesButton.Location = new System.Drawing.Point(11, 123);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 0;
            this.yesButton.Text = "Да";
            this.yesButton.UseVisualStyleBackColor = false;
            this.yesButton.Click += new System.EventHandler(this.YesButton_Click);
            // 
            // noButton
            // 
            this.noButton.BackColor = System.Drawing.Color.Transparent;
            this.noButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.noButton.ForeColor = System.Drawing.Color.LightGray;
            this.noButton.Location = new System.Drawing.Point(176, 123);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(75, 23);
            this.noButton.TabIndex = 1;
            this.noButton.Text = "Нет";
            this.noButton.UseVisualStyleBackColor = false;
            this.noButton.Click += new System.EventHandler(this.NoButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(246, 104);
            this.label1.TabIndex = 2;
            this.label1.Text = "У лаунчера нет доступа к севреру обновлений.\nПроверьте подключение к интернету\nИл" +
    "и свяжитесь с Админитрацией в Discord\nКанал: 🎮┇game-launcher\n\n\n\nПродолжить рабо" +
    "ту автономно?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.Cursor = System.Windows.Forms.Cursors.Default;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.ForeColor = System.Drawing.Color.LightGray;
            this.button1.Location = new System.Drawing.Point(92, 68);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Discord";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CustomDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = global::MGL.Properties.Resources.bg_offline;
            this.ClientSize = new System.Drawing.Size(263, 158);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.noButton);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "CustomDialog";
            this.Opacity = 0.97D;
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Внимание!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void YesButton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Нажата кнопка 'Да'");
            Thread offlineMode;
            this.Close();
            offlineMode = new Thread(Open_OfflineMode);
            offlineMode.SetApartmentState(ApartmentState.STA);
            offlineMode.Start();
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Нажата кнопка 'Нет'");
            Application.Exit();
            
        }
        public void Open_OfflineMode(object obj)
        {
            Application.Run(new OfflineMode());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ds.easyplay.su/");
            
            //DialogResult dialogResult = MessageBox.Show("Вы хотите перейти перейти в Discord?", "Подтвердите действие!", MessageBoxButtons.YesNo);
            //if (dialogResult == DialogResult.Yes)
            //{
            //    System.Diagnostics.Process.Start("https://ds.easyplay.su/");
            //}
            //else if (dialogResult == DialogResult.No)
            //{
            //    //
            //}
        }
        #endregion
    }
}