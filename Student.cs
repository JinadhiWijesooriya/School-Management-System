using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace School_Management_System
{
    public partial class Student : Form
    {
        public Student()
        {
            InitializeComponent();
            DisplayStudent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayStudent()
        {
            Con.Open();
            string Query = "Select * from StudentTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            StudentDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtName.Text == "" || txtFees.Text == "" || txtAddress.Text == "" || cmbGen.SelectedIndex == -1 || cmbClass.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into StudentTbl(StName,StGen,StDOB,StClass,StFees,StAdd) values (@Sname,@SGen,@SDob,@SClass,@SFees,@SAdd)", Con);
                    cmd.Parameters.AddWithValue("@Sname", txtName.Text);
                    cmd.Parameters.AddWithValue("@SGen", cmbGen.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDob", DOBPick.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", cmbClass.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", txtFees.Text);
                    cmd.Parameters.AddWithValue("@SAdd", txtAddress.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Added");
                    Con.Close();
                    DisplayStudent();
                    Reset();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void Reset()
        {
            Key = 0;
            txtName.Text = "";
            txtFees.Text = "";
            txtAddress.Text = "";
            cmbGen.SelectedIndex = 0;
            cmbClass.SelectedIndex = 0;
        }

        int Key = 0;
        private void StudentDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = StudentDGV.SelectedRows[0].Cells[1].Value.ToString();
            cmbGen.SelectedItem = StudentDGV.SelectedRows[0].Cells[2].Value.ToString();
            DOBPick.Text = StudentDGV.SelectedRows[0].Cells[3].Value.ToString();
            cmbClass.SelectedItem = StudentDGV.SelectedRows[0].Cells[4].Value.ToString();
            txtFees.Text = StudentDGV.SelectedRows[0].Cells[5].Value.ToString();
            txtAddress.Text = StudentDGV.SelectedRows[0].Cells[6].Value.ToString();
            if (txtName.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(StudentDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtFees.Text == "" || txtAddress.Text == "" || cmbGen.SelectedIndex == -1 || cmbClass.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update StudentTbl set StName=@Sname, StGen=@SGen, StDOB=@SDob,StClass=@SClass,StFees=@SFees,StAdd=@SAdd where StId=@SID",Con);
                    cmd.Parameters.AddWithValue("@Sname", txtName.Text);
                    cmd.Parameters.AddWithValue("@SGen", cmbGen.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDob", DOBPick.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", cmbClass.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", txtFees.Text);
                    cmd.Parameters.AddWithValue("@SAdd", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@SID",Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Updated");
                    Con.Close();
                    DisplayStudent();
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(Key == 0)
            {
                MessageBox.Show("Select Student");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from studentTbl where StId= @StKey", Con);
                    cmd.Parameters.AddWithValue("@StKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Deleted");
                    Con.Close();
                    DisplayStudent();
                    Reset();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }
    }
}
