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
    public partial class Fees : Form
    {
        public Fees()
        {
            InitializeComponent();
            DisplayFees();
            FillStuId();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Administrator\Documents\SchoolDb.mdf;Integrated Security=True;Connect Timeout=30");
        private void DisplayFees()
        {
            Con.Open();
            string Query = "Select * from FeesTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            FeesDGV.DataSource = ds.Tables[0];
            Con.Close();
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
            SqlCommand Cmd = new SqlCommand("Select * from StudentTbl where StId=@SID", Con);
            Cmd.Parameters.AddWithValue("@SID", cmbid.SelectedValue.ToString());
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(Cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                txtName.Text = dr["StName"].ToString();
            }
            Con.Close();
        }
        private void Reset()
        {
            txtAmount.Text = "";
            txtName.Text = "";
            cmbid.SelectedIndex = -1;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtAmount.Text=="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                String paymentperiod;
                paymentperiod = PeriodDate.Value.Month.ToString() +"/"+ PeriodDate.Value.Year.ToString();
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("Select COUNT(*) from FeesTbl where StId = '" + cmbid.SelectedValue.ToString() + "' and Month = '" + paymentperiod.ToString() + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("There is No Due for this month");
                }
                else
                {
                    //Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into FeesTbl(StId,StName,Month,Amt) values (@SId,@SName,@SMonth,@SAmt)", Con);
                    cmd.Parameters.AddWithValue("@SId", cmbid.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@SName", txtName.Text);
                    cmd.Parameters.AddWithValue("@SMonth", paymentperiod);
                    cmd.Parameters.AddWithValue("@SAmt", txtAmount.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Fees Successfully Paid");
                }
                    Con.Close();
                DisplayFees();
                Reset();
            }
        }

        private void cmbid_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetStudName();
        }

        private void cmbid_SelectionChangeCommitted_1(object sender, EventArgs e)
        {
            GetStudName();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }
    }
}
