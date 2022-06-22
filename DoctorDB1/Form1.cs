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
    public partial class Paitent_Info_Input : Form
    {
        string deepth = "1";
        public static Image[] m1 = new Image[10];
        public static int Count ;
        //public int count = -1;
        OleDbCommand cmd = new OleDbCommand();
        OleDbCommand cmd1 = new OleDbCommand();
        //OleDbCommand cmd2 = new OleDbCommand();
        int new_id;
        int new_Sid;
       Boolean New_Session_is_on;
       Boolean new_button_cliked;
        public Paitent_Info_Input()
        {
            InitializeComponent();
        }
        // connection string define
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\database/DocDB1.accdb");
        //load_form event 
        private void Paitent_Info_Input_Load(object sender, EventArgs e)
        {
            deepth = "1";
            Count = -1;
            new_button_cliked = true;
            get_patient_info("Select * from Main_Information");
            //--------------------------------------
            //for New_Session data selection if it from database or from datagridview--------
            New_Session_is_on = false;
            //----------------------------------
            //for select new data row id correctly------------هون فينا نسوي تابع نسميه اسم  
            set_new_id();
            SessionTextbox.Text = Convert.ToString(new_Sid);
            Patient_IDtextBox1.Text = Convert.ToString(new_id);
        }

        private void set_new_id()//this function for set new number for session id and patient id in the textbox correctly 
        {
            int id, newID;
            cmd1.CommandText = "select Max(Patient_ID) from Main_Information ";
            cmd1.Connection = con;
            cmd.CommandText = " select Max(Se_ID) from New_Session ";
            cmd.Connection = con;
            con.Open();
            //command.ExecuteScalar() function ??
            if (cmd1.ExecuteScalar() == DBNull.Value)
            {
                newID = 1;
            }
            else
            {
                id = Convert.ToInt32(cmd1.ExecuteScalar());
                newID = id + 1;
            }
           // Patient_IDtextBox1.Text = Convert.ToString(newID);
            //for new button to add correct id in text box  Patient_IDtextBox1-------
            new_id = newID;
            //-----------------------
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                newID = 1;
            }
            else
            {
                id = Convert.ToInt32(cmd.ExecuteScalar());
                newID = id + 1;
            }
            //SessionTextbox.Text = Convert.ToString(newID);
            new_Sid = newID;
            con.Close();
        }

        // New_patient save button----------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            if (new_button_cliked && name_textBox1.Text != "" && age_textBox2.Text != "" && phone_textBox3.Text!="")
            {
                string date = Convert.ToString(DateTime.Now);
                set_new_id();
                SessionTextbox.Text = Convert.ToString(new_Sid);
                Patient_IDtextBox1.Text = Convert.ToString(new_id);
                cmd.CommandText = "insert into Main_Information(Patient_ID,Name,Age,Phone,Data_Date) values(" + Patient_IDtextBox1.Text + ",'" + name_textBox1.Text + "','" + age_textBox2.Text + "','" + phone_textBox3.Text + "','" + date + "')";
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                New_Session_is_on = false;
                //------------------------------------------
                //حفظ الجلسة
                //save new session code ------------------------------------------------------------
                cmd.Connection = con;
                cmd.CommandText = "insert into New_Session (Se_ID,Main_Complaint,Details,Medical_History,On_Examination,Invastigation,Diagnosis,Treatment,SData_Date,Patient_ID) values(" + new_Sid + ",'" + main_complaint_textBox4.Text + "','" + details_richTextBox1.Text + "','" + Medical_History_TextBox1.Text + "','" + on_exam_richTextBox2.Text + "','" + invast_richTextBox3.Text + "','" + diagno_richTextBox4.Text + "','" + treat_richTextBox5.Text + "','" + date + "'," + Patient_IDtextBox1.Text + ")";
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                get_patient_info("Select * from Main_Information");
                deepth = "1";
                New_Session_is_on = false;
                MessageBox.Show("Data Saved Successfully");
                save_photo();
                clear_data();
            }
            else
            {
                MessageBox.Show("data you entered not valid.please click new button and set the data correctly");
            }
            Photo_is_On = false;
        }
        private void save_photo()
        {
            int id, newID;
            con.Open();
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            if (Count != -1)
            {
                if (!Directory.Exists("img"))
                {
                    Directory.CreateDirectory("img");//يجب وضع حلقة لحفظ جميع صور الجلسة هنا 
                }
                for (int i = 0; i <= Count; i++)
                {
                    m1[i].Save("img/" + Patient_IDtextBox1.Text + "." + Convert.ToString(new_Sid) + "." + Convert.ToString(i) + ".jpg");//you must make index name of image file*****هنا طريقة الحفظ معقدة قليلا يجب ان يحوي اسم الصورة على ايدي المريض ايدي الجلسة  + رقم الصورة 
                    //استخراج الID الجديد 
                    cmd1.CommandText = "select Max(PH_ID) from Midia_Table ";
                    if (cmd1.ExecuteScalar() == DBNull.Value)
                    {
                        newID = 1;
                    }
                    else
                    {
                        id = Convert.ToInt32(cmd1.ExecuteScalar());
                        newID = id + 1;
                    }
                    string index = Patient_IDtextBox1.Text + "." + Convert.ToString(new_Sid) + "." + Convert.ToString(i) + ".jpg";
                    cmd.CommandText = "insert into Midia_Table (PH_ID,Se_ID,Pat_ID,PH_INDEX) values (" + newID + "," + Convert.ToString(new_Sid) + "," + Patient_IDtextBox1.Text + " , '" + index + "') ";
                    cmd.ExecuteNonQuery();
                }
                Count = -1;
            }
            con.Close();
        }
        //Session save button.
        private void button5_Click(object sender, EventArgs e)
        {
            cmd.CommandText = "select Max(Patient_ID) from Main_Information where Patient_ID = " + Convert.ToInt32(Patient_IDtextBox1.Text) + " ";
            cmd.Connection = con;
            con.Open();
            if (cmd.ExecuteScalar() != DBNull.Value)
            {
               con.Close();
               set_new_id();
               save_photo();
               string date = Convert.ToString(DateTime.Now);

                // Save record command .
                cmd.CommandText = "insert into New_Session (Se_ID,Main_Complaint,Details,Medical_History,On_Examination,Invastigation,Diagnosis,Treatment,SData_Date,Patient_ID) values(" + new_Sid + ",'" + main_complaint_textBox4.Text + "','" + details_richTextBox1.Text + "','" + Medical_History_TextBox1.Text + "','" + on_exam_richTextBox2.Text + "','" + invast_richTextBox3.Text + "','" + diagno_richTextBox4.Text + "','" + treat_richTextBox5.Text + "','" + date + "'," + Patient_IDtextBox1.Text + ")";
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                get_patient_info("Select * from New_Session where Patient_ID = " + Patient_IDtextBox1.Text+ "");
                deepth = "2";
                 New_Session_is_on = true;
                 MessageBox.Show("Data Saved Successfully");
                 con.Close();
                    
                 //clear_data();
                 main_complaint_textBox4.Text = "";
                 details_richTextBox1.Text = "";
                 Medical_History_TextBox1.Text = "";
                 on_exam_richTextBox2.Text = "";
                 invast_richTextBox3.Text = "";
                 diagno_richTextBox4.Text = "";
                 treat_richTextBox5.Text = "";
                 //get_patient_info("select * from New_Session where Patient_ID=" + Patient_IDtextBox1 + " ");
                 deepth = "2";
                }
                else
                {
                    MessageBox.Show("you have to select a record befor save the session");
                    con.Close();
                }
            
        }
        private void button7_Click(object sender, EventArgs e)
        {  //منشان ما ننسى ممكن هون نخلي البيانات يلي نكتبت بالحقول لعرضها تختفي بكون احسن
           //button10.Visible = false;
           get_patient_info("Select * from Main_Information");
           New_Session_is_on = false;
           Photo_is_On = false;
           deepth = "1";
           label14.Text = "المرضى";
           appear();
        }
        Boolean record_selected;
        //select record from data grid view and present it on the form.
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
        }
        private void Back_button_Click(object sender, EventArgs e)
        {
            if(deepth == "3")
            {
                get_patient_info("select * from New_Session where Patient_ID =" + Patient_IDtextBox1.Text+ " ");
                deepth = "2";
                label14.Text = "الجلسات";
                hidden();
            }
            else if (deepth == "2")
            {
                get_patient_info("select * from Main_Information ");
                deepth = "1";
                label14.Text = "المرضى";
                appear();
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (deepth == "2")
            {
                main_complaint_textBox4.Text = "";
                details_richTextBox1.Text = "";
                Medical_History_TextBox1.Text = "";
                on_exam_richTextBox2.Text = "";
                invast_richTextBox3.Text = "";
                diagno_richTextBox4.Text = "";
                treat_richTextBox5.Text = "";
                //get_patient_info("select * from New_Session where Patient_ID=" + Patient_IDtextBox1 + " ");
                deepth = "2";
                label14.Text = "الجلسات";
                hidden();
            }
            else
            {
                clear_data();
                get_patient_info("select * from Main_Information");
                deepth = "1";
                label14.Text = "المرضى";
            }
        }
       private void get_patient_info( string command )
        {
            OleDbDataAdapter da = new OleDbDataAdapter(command,con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
       private void button6_Click(object sender, EventArgs e)
       {
           // كود حذف الجلسة فقط 
           if (deepth == "1")
           {
               string message = "Are you sure you want to delet this data record ?";
               string title = " Warning message ";
               MessageBoxButtons buttons = MessageBoxButtons.YesNo;
               DialogResult result = MessageBox.Show(message, title, buttons);
               if (result == DialogResult.Yes)
               {
                   cmd.CommandText = "Delete from Main_Information where Patient_ID = " + dataGridView1.CurrentRow.Cells[0].Value + " ";
                   cmd.Connection = con;
                   cmd1.CommandText = "Delete from New_Session where Patient_ID = " + dataGridView1.CurrentRow.Cells[0].Value + "";
                   cmd1.Connection = con;
                   con.Open();
                   cmd.ExecuteNonQuery();
                   cmd1.ExecuteNonQuery();
                   MessageBox.Show("the patient information was deleted successfully");
                   //+++++++++++++++++++++++++++++++++++++++++++++++++++++
                   OleDbDataAdapter da = new OleDbDataAdapter("Select PH_INDEX from Midia_Table Where Pat_ID = " + dataGridView1.CurrentRow.Cells[0].Value + "", con);
                   DataTable dt = new DataTable();
                   //حذف السجلات من جدول الصور وملفات الصور من المجلد 
                   //da.SelectCommand.CommandText = "select PH_INDEX from Midia_Table Where Pat_ID = " + dataGridView1.CurrentRow.Cells[0].Value + "";
                   da.Fill(dt);
                   for (int i = 0; i <= dt.Rows.Count - 1; i++)
                   {
                       // تعليمة حذف الصورة من المجلد 
                       string filePath = "img/" + dt.Rows[i][0].ToString(); ;
                       if (File.Exists(filePath))
                       {
                           File.Delete(filePath);
                       }
                   }

                   cmd.CommandText = "select PH_INDEX from Midia_Table Where Pat_ID = " + dataGridView1.CurrentRow.Cells[0].Value + " ";
                   if (cmd.ExecuteScalar() != DBNull.Value)
                   {
                       cmd1.CommandText = "Delete from Midia_Table where Pat_ID = " + dataGridView1.CurrentRow.Cells[0].Value + "";
                       cmd1.ExecuteNonQuery();
                   }
                   //--------------------------------------------------------------------------
                   get_patient_info("select * from Main_Information");
                   New_Session_is_on = false;
                   con.Close();
                   //+++++++++++++++++++++++++++++++++++++++++++++++++++++
               }
               clear_data();
           }
           else if (deepth == "2")
           {
               string message = "Are you sure you want to delet this data record ?";
               string title = " Warning message ";
               MessageBoxButtons buttons = MessageBoxButtons.YesNo;
               DialogResult result = MessageBox.Show(message, title, buttons);
               if (result == DialogResult.Yes)
               {
                   cmd1.CommandText = "Delete from New_Session where Se_ID  = " + dataGridView1.CurrentRow.Cells[1].Value + "";
                   cmd1.Connection = con;
                   con.Open();
                   cmd1.ExecuteNonQuery();
                   //+++++++++++++++++++++++++++++++++++++++++++++++++++++
                   OleDbDataAdapter da = new OleDbDataAdapter("Select PH_INDEX from Midia_Table Where Se_ID = " + dataGridView1.CurrentRow.Cells[1].Value + "", con);
                   DataTable dt = new DataTable();
                   //حذف السجلات من جدول الصور وملفات الصور من المجلد 
                   //da.SelectCommand.CommandText = "select PH_INDEX from Midia_Table Where Pat_ID = " + dataGridView1.CurrentRow.Cells[0].Value + "";
                   da.Fill(dt);
                   for (int i = 0; i <= dt.Rows.Count - 1; i++)
                   {
                       // تعليمة حذف الصورة من المجلد 
                       string filePath = "img/" + dt.Rows[i][0].ToString(); ;
                       if (File.Exists(filePath))
                       {
                           File.Delete(filePath);
                       }
                   }
                   //cmd.CommandText = "select PH_INDEX from Midia_Table Where Se_ID = " + dataGridView1.CurrentRow.Cells[1].Value + " ";
                   if (cmd.ExecuteScalar() != DBNull.Value)
                   {
                       cmd1.CommandText = "Delete from Midia_Table where Se_ID = " + dataGridView1.CurrentRow.Cells[1].Value + "";
                       cmd1.ExecuteNonQuery();
                   }
                   //--------------------------------------------------------------------------
                   get_patient_info("select * from New_Session where Patient_ID = " + Patient_IDtextBox1.Text + " ");
                   New_Session_is_on = false;
                   con.Close();
                   //+++++++++++++++++++++++++++++++++++++++++++++++++++++
               }
               MessageBox.Show("the patient information was deleted successfully");
               clear_data();
           }
           else if (deepth == "3")
           {
               string message = "Are you sure you want to delet this photo ?";
               string title = " Warning message ";
               MessageBoxButtons buttons = MessageBoxButtons.YesNo;
               DialogResult result = MessageBox.Show(message, title, buttons);
               if (result == DialogResult.Yes)
               {
                   OleDbDataAdapter da = new OleDbDataAdapter("Select PH_INDEX from Midia_Table Where PH_ID = " + dataGridView1.CurrentRow.Cells[0].Value + "", con);
                   DataTable dt = new DataTable();
                   //حذف السجلات من جدول الصور وملفات الصور من المجلد 
                   //da.SelectCommand.CommandText = "select PH_INDEX from Midia_Table Where Pat_ID = " + dataGridView1.CurrentRow.Cells[0].Value + "";
                   da.Fill(dt);
                   for (int i = 0; i <= dt.Rows.Count - 1; i++)
                   {
                       // تعليمة حذف الصورة من المجلد 
                       string filePath = "img/" + dt.Rows[i][0].ToString(); ;
                       if (File.Exists(filePath))
                       {
                           File.Delete(filePath);
                       }
                   }
                   //cmd.CommandText = "select PH_INDEX from Midia_Table Where PH_ID = " + dataGridView1.CurrentRow.Cells[0].Value + " ";
                   //cmd.Connection = con;
                   con.Open();
                   if (cmd.ExecuteScalar() != DBNull.Value)
                   {
                       cmd1.CommandText = "Delete from Midia_Table where PH_ID = " + dataGridView1.CurrentRow.Cells[0].Value + "";
                       cmd1.ExecuteNonQuery();
                   }
                   con.Close();
                   //هنا يجد كتابة كود إعادة إظهار صور الجلسة في جدول الإضهار
                   MessageBox.Show("photo have been deleted enter the photo record again ");
               }
           }
       }
        private void clear_data()
        {
            main_complaint_textBox4.Text = "";
            details_richTextBox1.Text = "";
            Medical_History_TextBox1.Text = "";
            on_exam_richTextBox2.Text = "";
            invast_richTextBox3.Text = "";
            diagno_richTextBox4.Text = "";
            treat_richTextBox5.Text = "";
            //-----------------------------------------
            name_textBox1.Text = "";
            age_textBox2.Text = "";
            phone_textBox3.Text = "";
            //-----------------------------------------
            int id2, newID2, newID, id;
            cmd1.CommandText = "select Max(Patient_ID) from Main_Information ";
            cmd1.Connection = con;
            //increse the id value for the new record inserting.
            con.Open();
            if (cmd1.ExecuteScalar() == DBNull.Value)
            {
                newID2 = 1;
            }
            else
            {
                id2 = Convert.ToInt32(cmd1.ExecuteScalar());
                newID2 = id2 + 1;
            }

            Patient_IDtextBox1.Text = Convert.ToString(newID2);
            cmd.CommandText = "select Max(Se_ID) from New_Session ";
            cmd.Connection = con;
            if (cmd.ExecuteScalar() == DBNull.Value)
            {
                newID = 1;
            }
            else
            {
                id = Convert.ToInt32(cmd.ExecuteScalar());
                newID = id + 1;
            }
            SessionTextbox.Text = Convert.ToString(newID);
            new_Sid = newID;
            con.Close();
            Photo_is_On = false;
           // button10.Visible = false;
            new_button_cliked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            get_patient_info("select * from Main_Information where Name like'%" + search_textBox.Text + "%' ");
            New_Session_is_on = false;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
         if(deepth == "3")
         {
             addPhotoForm2 f1 = new addPhotoForm2(Patient_IDtextBox1.Text, SessionTextbox.Text);
             f1.Show();
             //if (Application.OpenForms.Count == 0)
             //{
             //    get_patient_info("select * from Midia_table where Se_ID=" + SessionTextbox.Text + " and Pat_ID=" + Patient_IDtextBox1 + "");
             //}
         }
         else
         {
             FormPhotoSave f1 = new FormPhotoSave();
                f1.Show();
         }
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        Boolean Photo_is_On = false;
        private void button10_Click(object sender, EventArgs e)
        {
          ////  get_patient_info("select * from Midia_Table where Se_ID = " + Convert.ToInt32(SessionTextbox.Text) + " and Pat_ID = " + Convert.ToInt32(Patient_IDtextBox1.Text) + "");
          //  Photo_is_On = true;
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[index];
            if (deepth == "1")
            {
                //هنا كود كتابة البيانات في في حقول الشاشة 
                Patient_IDtextBox1.Text = selectedRow.Cells[0].Value.ToString();
                int id = Convert.ToInt32(Patient_IDtextBox1.Text);
                OleDbDataAdapter da = new OleDbDataAdapter("select * from Main_Information where Patient_ID=" + id + "", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                name_textBox1.Text = dt.Rows[0][1].ToString();
                age_textBox2.Text = dt.Rows[0][2].ToString();
                phone_textBox3.Text = dt.Rows[0][3].ToString();
                OleDbDataAdapter da1 = new OleDbDataAdapter("select * from New_Session where Patient_ID=" + id + "", con);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                int n = dt1.Rows.Count - 1;
                SessionTextbox.Text = dt1.Rows[n][1].ToString();
                main_complaint_textBox4.Text = dt1.Rows[n][2].ToString();
                details_richTextBox1.Text = dt1.Rows[n][3].ToString();
                Medical_History_TextBox1.Text = dt1.Rows[n][4].ToString();
                on_exam_richTextBox2.Text = dt1.Rows[n][5].ToString();
                invast_richTextBox3.Text = dt1.Rows[n][6].ToString();
                diagno_richTextBox4.Text = dt1.Rows[n][7].ToString();
                treat_richTextBox5.Text = dt1.Rows[n][8].ToString();
                //هنا كود الاستعلام عن جلسات المريض المختار وعرضها في الجدول 
                get_patient_info("select * from New_Session where Patient_ID = " + Convert.ToInt32(Patient_IDtextBox1.Text) + "");
                //هنا كود جعل متحول العمق مساوي إلى 2
                deepth = "2";
                label14.Text = "الجلسات";
                hidden();
            }
            else if (deepth == "2")
            {   //عرض بيانات الجلسة في حقول الواجهة 
                //التعامل مع حالة مريض ليس لديه جلسات 
                OleDbDataAdapter da12 = new OleDbDataAdapter("select * from New_Session where Se_ID=" + dataGridView1.CurrentRow.Cells[1].Value + "", con);
                DataTable dt12 = new DataTable();
                da12.Fill(dt12);
                SessionTextbox.Text = dt12.Rows[0][1].ToString();
                main_complaint_textBox4.Text = dt12.Rows[0][2].ToString();
                details_richTextBox1.Text = dt12.Rows[0][3].ToString();
                Medical_History_TextBox1.Text = dt12.Rows[0][4].ToString();
                on_exam_richTextBox2.Text = dt12.Rows[0][5].ToString();
                invast_richTextBox3.Text = dt12.Rows[0][6].ToString();
                diagno_richTextBox4.Text = dt12.Rows[0][7].ToString();
                treat_richTextBox5.Text = dt12.Rows[0][8].ToString();
                //هنا كود عرض صور الجلسة في الجدول إن وجدت
                get_patient_info("select * from Midia_Table where Se_ID = " + Convert.ToInt32(SessionTextbox.Text) + " and Pat_ID = " + Convert.ToInt32(Patient_IDtextBox1.Text) + "");
                //هنا كود جعل متحول العمق مساوي إلى 3
                deepth = "3";
                label14.Text = "الصور";
                hidden();
            }
            else if (deepth == "3")
            {
                string str = selectedRow.Cells[3].Value.ToString();
                string directory = Path.Combine(Environment.CurrentDirectory, "img");
                Process.Start(directory + "/" + str);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            if (name_textBox1.Text == "" || age_textBox2.Text == "" || phone_textBox3.Text=="")
            {
                MessageBox.Show("some data of your patient is not complet,please enter the main iformation like name phone and age");
            }
            else{
            Surgery_Form2 f1 = new Surgery_Form2(Patient_IDtextBox1.Text, SessionTextbox.Text, name_textBox1.Text, age_textBox2.Text, phone_textBox3.Text);
            f1.Show();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Surgeries f1 = new Surgeries();
            f1.Show();
        }
        private void hidden()
        {
            name_textBox1.Enabled = false;
            age_textBox2.Enabled = false;
            phone_textBox3.Enabled = false;
        }
        private void appear()
        {
            name_textBox1.Enabled = true;
            age_textBox2.Enabled = true;
            phone_textBox3.Enabled = true;
        }

        private void Paitent_Info_Input_MouseHover(object sender, EventArgs e)
        {
            if (deepth == "3")
            {
                get_patient_info("select * from Midia_Table where Se_ID = " + Convert.ToInt32(SessionTextbox.Text) + " and Pat_ID = " + Convert.ToInt32(Patient_IDtextBox1.Text) + "");
            }
        }
    }
}
