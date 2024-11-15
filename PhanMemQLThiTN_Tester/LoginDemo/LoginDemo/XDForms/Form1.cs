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
using LoginDemo.XDForms;

namespace LoginDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
           
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Hãy nhập tài khoản/mã HV của bạn");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Hãy nhập mật khẩu của bạn");
                txtPassword.Focus();
                return;
            }

            if (rdHocVien.Checked)
            {
                SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QUANLYTK;Integrated Security=True");

                conn.Open();
                string maHV = txtUsername.Text.Trim();
                string mk = txtPassword.Text.Trim();
                string sql = "Select * from NguoiDung where MaHV = '" + maHV + "' and MatKhau = '" + mk + "' ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader data = cmd.ExecuteReader();
                if (data.Read() == true)
                {
                    MessageBox.Show("đăng nhập thành công");

                    HocVienForm hvf = new HocVienForm();
                    hvf.temp = txtUsername.Text;
                    hvf.ShowDialog();
                    txtPassword.Clear();

                }
                else
                {
                    MessageBox.Show("đăng nhập thất bại");
                    txtUsername.Focus();
                    txtUsername.Clear();
                    txtPassword.Clear();
                }
            }
            else
            {
                SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QUANLYTK;Integrated Security=True");

                conn.Open();
                string tk = txtUsername.Text.Trim();
                string mk = txtPassword.Text.Trim();
                string sql = "Select * from GiaoVien where TaiKhoan = '" + tk + "' and MatKhau = '" + mk + "' ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader data = cmd.ExecuteReader();
                if (data.Read() == true)
                {
                    MessageBox.Show("đăng nhập thành công");
                    
                    Admin admin = new Admin();
                    admin.temp = txtUsername.Text.Trim();
                    admin.ShowDialog();
                    txtPassword.Clear();
                }
                else
                {
                    MessageBox.Show("đăng nhập thất bại");
                    txtUsername.Focus();
                    txtUsername.Clear();
                    txtPassword.Clear();

                }
            }

        }

        private void rdHocVien_CheckedChanged(object sender, EventArgs e)
        {
            if (rdHocVien.Checked)
            {
                lblUsername.Text = "Mã Học Viên:";
                txtUsername.PlaceholderText = "Nhập mã học viên của bạn";
            }
        }

        private void rdAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (rdAdmin.Checked)
            {
                lblUsername.Text = "Tài Khoản:";
                txtUsername.PlaceholderText = "Nhập tên tài khoản của bạn";

            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Hãy nhập tài khoản/mã HV của bạn");
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Hãy nhập mật khẩu của bạn");
                    txtPassword.Focus();
                    return;
                }

                if (rdHocVien.Checked)
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QUANLYTK;Integrated Security=True");

                    conn.Open();
                    string maHV = txtUsername.Text.Trim();
                    string mk = txtPassword.Text.Trim();
                    string sql = "Select * from NguoiDung where MaHV = '" + maHV + "' and MatKhau = '" + mk + "' ";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.Read() == true)
                    {
                        MessageBox.Show("đăng nhập thành công");

                        HocVienForm hvf = new HocVienForm();
                        hvf.temp = txtUsername.Text;
                        hvf.ShowDialog();
                        txtPassword.Clear();

                    }
                    else
                    {
                        MessageBox.Show("đăng nhập thất bại");
                        txtUsername.Focus();
                        txtUsername.Clear();
                        txtPassword.Clear();
                    }
                }
                else
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QUANLYTK;Integrated Security=True");

                    conn.Open();
                    string tk = txtUsername.Text.Trim();
                    string mk = txtPassword.Text.Trim();
                    string sql = "Select * from GiaoVien where TaiKhoan = '" + tk + "' and MatKhau = '" + mk + "' ";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.Read() == true)
                    {
                        MessageBox.Show("đăng nhập thành công");

                        Admin admin = new Admin();
                        admin.temp = txtUsername.Text.Trim();
                        admin.ShowDialog();
                        txtPassword.Clear();
                    }
                    else
                    {
                        MessageBox.Show("đăng nhập thất bại");
                        txtUsername.Focus();
                        txtUsername.Clear();
                        txtPassword.Clear();

                    }
                }
            }    
        }
    }
}
