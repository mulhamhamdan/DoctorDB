using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Diagnostics;
namespace DoctorDB
{
    public partial class Surgeries : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\database/DocDB1.accdb");
        OleDbCommand cmd = new OleDbCommand();
        public Surgeries()
        {
            InitializeComponent();
        }
        private void get_patient_info(string command)
        {
            OleDbDataAdapter da = new OleDbDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void Surgeries_Load(object sender, EventArgs e)
        {
            get_patient_info("select SU_ID,Patient_Name,Patient_Phone,Patient_Age,Action_Name,Action_Date,Action_Place from Surgery_table");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            get_patient_info("select * from Surgery_Table where Patient_Name like'%" + textBox1.Text + "%' ");
        }
        private void button2_Click(object sender, EventArgs e)
        {
             string message = "Are you sure you want to delet this data record ?";
                string title = " Warning message ";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    cmd.CommandText = "Delete from Surgery_Table where SU_ID = " + dataGridView1.CurrentRow.Cells[0].Value + " ";
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    get_patient_info("select * from Surgery_table");
                    MessageBox.Show("the patient information was deleted successfully");
                }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            get_patient_info("select * from Surgery_Table where  Action_Date between #" + dateTimePicker2.Value.ToString("yyyy/MM/dd") + "# and #" + dateTimePicker3.Value.ToString("yyyy/MM/dd") + "# ");
        }
        Bitmap bitmap;
        private void button6_Click(object sender, EventArgs e)
        {
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Patient_ID");
            dt2.Columns.Add("Patient_Name");
            dt2.Columns.Add("Action_Name");
            dt2.Columns.Add("Action_Date");
            dt2.Columns.Add("Action_Place");
            dt2.Columns.Add("Patient_Age");
            dt2.Columns.Add("Patient_Phone");
            for(int i = 0; i< dataGridView1.RowCount; i++)
            {
                dt2.Rows.Add(dataGridView1.Rows[i].Cells[0].Value,
                             dataGridView1.Rows[i].Cells[1].Value,
                             dataGridView1.Rows[i].Cells[2].Value,
                             dataGridView1.Rows[i].Cells[3].Value,
                             dataGridView1.Rows[i].Cells[4].Value,
                             dataGridView1.Rows[i].Cells[5].Value,
                             dataGridView1.Rows[i].Cells[6].Value);
            }
            CrystalReport1 cr = new CrystalReport1();
            cr.SetDataSource(dt2);
            Report_Surgery rsd = new Report_Surgery();
            rsd.Show();
            rsd.crystalReportViewer1.ReportSource = cr;
           
            //string image_path;
            //int height = dataGridView1.Height;
            //dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height  +40 ;
            //bitmap = new Bitmap(dataGridView1.Width,dataGridView1.Height);
            //dataGridView1.DrawToBitmap(bitmap, new Rectangle(0,0,dataGridView1.Width,dataGridView1.Height));
            //Image img = bitmap;
            //saveFileDialog1 = new SaveFileDialog(); 
            //saveFileDialog1.ShowDialog();
            //saveFileDialog1.InitialDirectory = @"C:\";
            //saveFileDialog1.RestoreDirectory = true;
            //image_path = saveFileDialog1.FileName;
            //img.Save(image_path + ".jpg");
            //dataGridView1.Height = height;
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }
        DataTable dt = new DataTable();
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
           //تعليمات حذف وإضافة عادية 
            MessageBox.Show("the patient information was Updated successfully");
        }
        private void button3_Click(object sender, EventArgs e)
        {
        }
    }
}
