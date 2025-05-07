using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //txtUname.Text = "";
            //txtPass.Text = "";
    
            if(txtUname.Text=="" || txtPass.Text == "")
            {
                //MessageBox.Show("Enter Username and Password");
            }else if(txtUname.Text== "Admin" && txtPass.Text == "Admin@123")
            {
             MainMenu Obj = new MainMenu();
             Obj.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong Username or Password");
                txtUname.Text = "";
                txtPass.Text = "";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
