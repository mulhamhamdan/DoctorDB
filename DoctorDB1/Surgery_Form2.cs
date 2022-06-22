using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace DoctorDB
{
    public partial class Surgery_Form2 : Form
    {
        public Surgery_Form2()
        {
            InitializeComponent();
        }
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\database/DocDB1.accdb");
        OleDbCommand cmd = new OleDbCommand();
        string pat_id, se_id, name, age, phone;
        public Surgery_Form2(string pat_id , string se_id , string name , string age , string phone  )
        {
            this.pat_id = pat_id;
            this.se_id = se_id;
            this.name = name;
            this.age = age;
            this. phone = phone;
            InitializeComponent();
        }
        private void Surgery_Form2_Load(object sender, EventArgs e)
        {
            Patient_IDtextBox1.Text = pat_id;
            name_textBox1.Text = name;
            age_textBox2.Text = age;
            phone_textBox3.Text = phone;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            string totalstring = comboBox3.Text + ":" + comboBox2.Text + " " + comboBox1.Text;
            int id2, newID2;
            cmd.CommandText = "select Max(SU_ID) from Surgery_Table ";
            cmd.Connection = con;
            //increse the id value for the new record inserting.
            con.Open();
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                newID2 = 1;
            }
            else
            {
                id2 = Convert.ToInt32(cmd.ExecuteScalar());
                newID2 = id2 + 1;
            }
            cmd.CommandText = "insert into Surgery_Table (SU_ID,Patient_ID,Patient_Name,Patient_Phone,Patient_Age,Action_Name,Action_Date,Action_Time,Action_Place) values (" + newID2 + "," + pat_id + ",'" + name + "','" + phone + "','" + age + "','" + action_name.Text + "','" + dateTimePicker1.Text + "','" + totalstring + "','" + action_place.Text + "')";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show(" surgery is added ");
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }
    }
}
