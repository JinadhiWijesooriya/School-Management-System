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
    public partial class Attendence : Form
    {
        public Attendence()
        {
            InitializeComponent();
            DisplayAttendace();
            FillStuId();
        }
        private void FillStuId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select StId from StudentTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("StId", typeof(int));
            dt.Load(rdr);
            cmbid.ValueMember = "StId";
            cmbid.DataSource = dt;
            Con.Close();
        }
        private void GetStudName()
        {
            Con.Open();
            SqlCommand Cmd  = new SqlCommand ("Select * from StudentTbl where StId=@SID",Con);
            Cmd.Parameters.AddWithValue("@SID", cmbid.SelectedValue.ToString());
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(Cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                txtName.Text = dr["StName"].ToString();
            }
            Con.Close();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayAttendace()
        {
            Con.Open();
            string Query = "Select * from AttendanceTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AttendanceDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Reset()
        {
            cmbStatus.SelectedIndex = -1;
            txtName.Text = "";
            cmbid.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into AttendanceTbl(AttStId,AttStName,AttDate,AttStatus) values (@StId,@StName,@AttDate,@Status)", Con);
                    cmd.Parameters.AddWithValue("@StId",cmbid.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName", txtName.Text);
                    cmd.Parameters.AddWithValue("@AttDate", AttDatePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance Taken");
                    Con.Close();
                    DisplayAttendace();
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cmbid_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetStudName();
        }

        int Key = 0;
        private void AttendanceDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cmbid.SelectedValue = AttendanceDGV.SelectedRows[0].Cells[1].Value.ToString();
            txtName.Text = AttendanceDGV.SelectedRows[0].Cells[2].Value.ToString();
            AttDatePicker.Text = AttendanceDGV.SelectedRows[0].Cells[3].Value.ToString();
            cmbStatus.SelectedItem = AttendanceDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (txtName.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(AttendanceDGV.SelectedRows[0].Cells[0].Value.ToString());
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
                    MessageBox.Show("Attenda Deleted");
                    Con.Close();
                    DisplayAttendace();
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
            if (txtName.Text == "" || cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update AttendanceTbl set AttStId=@StId,AttStName=@StName,AttDate=@ADate,AttStatus=@AStatus where AttNum=@ANum", Con);
                    cmd.Parameters.AddWithValue("@StId", cmbid.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName",txtName.Text);
                    cmd.Parameters.AddWithValue("@ADate",AttDatePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@AStatus", cmbStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@ANum",Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance Updated");
                    Con.Close();
                    DisplayAttendace();
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }
    }
}
