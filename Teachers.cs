using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Teachers : Form
    {
        public Teachers()
        {
            InitializeComponent();
            DisplayTeachers();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayTeachers()
        {
            Con.Open();
            string Query = "Select * from TeacherTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TeacherDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Reset()
        {
            txtName.Text = "";
            cmbSub.SelectedIndex = 0;
            cmbSub.SelectedIndex = 0;
            txtPhone.Text = "";
            txtAddress.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtAddress.Text == "" || cmbGen.SelectedIndex == -1 || cmbSub.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into TeacherTbl(TName,TGen,TPhone,TSub,TAdd,TDOB) values (@Tname,@TGen,@TPhone,@TSub,@TAdd,@TDOB)", Con);
                    cmd.Parameters.AddWithValue("@Tname", txtName.Text);
                    cmd.Parameters.AddWithValue("@TGen", cmbGen.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TPhone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@TSub", cmbSub.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TAdd", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@TDOB", TDOB.Value.Date);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Added");
                    Con.Close();
                    DisplayTeachers();
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPhone.Text == "" || txtAddress.Text == "" || cmbGen.SelectedIndex == -1 || cmbSub.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update TeacherTbl set TName=@Tname,TGen=@TGen,TPhone=@TPhone,TSub=@TSub,TAdd=@TAdd,TDOB=@TDOB where TId=@TeachID", Con);
                    cmd.Parameters.AddWithValue("@Tname", txtName.Text);
                    cmd.Parameters.AddWithValue("@TGen", cmbGen.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TPhone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@TSub", cmbSub.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TAdd", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@TDOB", TDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@TeachID", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Updated");
                    Con.Close();
                    DisplayTeachers();
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
            if (Key == 0)
            {
                MessageBox.Show("Select Student");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from TeacherTbl where TId= @TKey", Con);
                    cmd.Parameters.AddWithValue("@TKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Deleted");
                    Con.Close();
                    DisplayTeachers();
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        int Key = 0;
        private void TeacherDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = TeacherDGV.SelectedRows[0].Cells[1].Value.ToString();
            cmbGen.SelectedItem = TeacherDGV.SelectedRows[0].Cells[2].Value.ToString();
            txtPhone.Text = TeacherDGV.SelectedRows[0].Cells[3].Value.ToString();
            cmbSub.SelectedItem = TeacherDGV.SelectedRows[0].Cells[4].Value.ToString();
            txtAddress.Text =TeacherDGV.SelectedRows[0].Cells[5].Value.ToString();
            TDOB.Text = TeacherDGV.SelectedRows[0].Cells[6].Value.ToString();
            if (txtName.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(TeacherDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }
    }
}
