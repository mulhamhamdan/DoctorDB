using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace DoctorDB
{  
    public partial class Regesteration : Form
    {
        public Regesteration()
        {
            InitializeComponent();
        }
        Paitent_Info_Input fp = new Paitent_Info_Input();
        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            if (str == "mulhamwalaa")
            {
                FileStream fs = File.Create("E:/test.txt");
                fs.Close();
                File.SetAttributes(
                   "E:/test.txt",
                   FileAttributes.Archive |
                   FileAttributes.Hidden |
                   FileAttributes.ReadOnly
                   );
                Paitent_Info_Input pai = new Paitent_Info_Input();
                pai.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ronge password");
            }
        }
        private void Regesteration_Load(object sender, EventArgs e)
        {
            if (File.Exists("E:/test.txt"))
            {
                Paitent_Info_Input pai = new Paitent_Info_Input();
                pai.ShowDialog();
                this.Close();
            }
        }
    }
}
