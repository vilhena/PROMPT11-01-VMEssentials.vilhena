using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sessao2Forms
{
    public partial class Form1 : Form
    {
        SessionRecorder rec;
            

        public Form1()
        {
            InitializeComponent();

            rec = new SessionRecorder(this);

            rec.StartRecorder();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rec.StopRecorder();   
        }
    }
}
