using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sessao2
{
    public partial class Form1 : Form
    {
        private SessionRecorder rec;

        public Form1()
        {
            InitializeComponent();
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {
            rec = new SessionRecorder(this); 
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Start Recording....");
            rec.StartRecorder();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Stop Recording....");
            rec.StopRecorder();
        }

        private void isel_Click(object sender, EventArgs e)
        {
            textbox.Text = ((Button)sender).Text;
        }

        private void prompt_Click(object sender, EventArgs e)
        {
            textbox.Text = ((Button)sender).Text;
        }

       
    }
}
