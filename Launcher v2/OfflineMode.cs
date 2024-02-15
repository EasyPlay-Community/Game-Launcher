using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGL
{
    
    public partial class OfflineMode : Form
    {
        #region -- Инициализировать компоненты --
        public OfflineMode()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.MouseDown += new MouseEventHandler(OffMode_MouseDown);
        }
        #endregion

        #region -- Делает форму перетаскиваемой --
        private void OffMode_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }
        #endregion
    }
}
