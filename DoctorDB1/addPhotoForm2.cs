using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
namespace DoctorDB
{
    public partial class addPhotoForm2 : Form
    {
        public string patient_id;
        public string session_id;
        public addPhotoForm2(string id , string sid)
        {
            patient_id = id;
            session_id = sid;
            InitializeComponent();
        }
        public string s;
        public Image img;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = " Images |*.jpg;*.png;*.bmp;*.gif";
            if (of.ShowDialog() == DialogResult.OK)
            {
                s = of.FileName;
                pictureBox1.Image = Image.FromFile(of.FileName);
                img = pictureBox1.Image;
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
            if (!Directory.Exists("img"))
            {
                Directory.CreateDirectory("img");
            }
            //اخذ قيم رقم الجلسة ورقم المريض من الفورم الرئيسي
            //استعلام عن عدد الصور في الجلسة للمريض كي يتم حساب رقم الصورة
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\database/DocDB1.accdb");
            OleDbDataAdapter da = new OleDbDataAdapter("select count(PH_ID) from Midia_Table where Se_ID = "+session_id+" and Pat_ID = "+patient_id+" ",con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string m = dt.Rows[0][0].ToString();
            int i = Convert.ToInt32(m);
            img.Save("img/" + patient_id + "." + session_id + "." + i + ".jpg");
            //إضافة الصورة إلى الجدول
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "select Max(PH_ID) from Midia_Table ";
            cmd.Connection = con;
            con.Open();
            int newID,id;
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                newID = 1;
            }
            else
            {
                id = Convert.ToInt32(cmd.ExecuteScalar());
                newID = id + 1;
            }
            string str = patient_id+ "." + session_id + "." + Convert.ToString(i)+".jpg";
            cmd.CommandText = "insert into Midia_table (PH_ID,Se_ID,Pat_ID,PH_INDEX) values (" + newID + ",'" + session_id + "','" + patient_id + "','"+ str + "') ";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("the photo is saved successfully ");
        }
        private void addPhotoForm2_Load(object sender, EventArgs e)
        {
        }
    }
}
