using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace DoctorDB
{
    public partial class FormPhotoSave : Form
    {
        
        public string s;

        public FormPhotoSave()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = " Images |*.jpg;*.png;*.bmp;*.gif";

            if (of.ShowDialog() == DialogResult.OK)
            {
                s = of.FileName;
                pictureBox1.Image = Image.FromFile(of.FileName);
                //if (!Directory.Exists("img"))
                //{
                //    Directory.CreateDirectory("img");
                //    pictureBox1.Image.Save("img/");
                //}

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = Image.FromFile(s);

            Paitent_Info_Input.Count = Paitent_Info_Input.Count + 1;
            Paitent_Info_Input.m1[Paitent_Info_Input.Count] = Image.FromFile(s);

            MessageBox.Show("the image was saved successfuly");

        }
    }
}
