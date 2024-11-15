using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginDemo.XDForms
{
    public partial class DoiMatKhau : Form
    {
        string strConnect = @"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QuanLyTK;Integrated Security=True";
        SqlConnection connection = null;

        public string maHV;
        public DoiMatKhau()
        {
            InitializeComponent();
        }
        string matKhauHT;
        private void button1_Click(object sender, EventArgs e)
        {
            

            if (String.IsNullOrEmpty(txtMatKhauCu.Text) || String.IsNullOrWhiteSpace(txtMatKhauCu.Text))
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu cũ", "Thông báo");
                txtMatKhauCu.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtMatKhauMoi.Text) || String.IsNullOrWhiteSpace(txtMatKhauMoi.Text))
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu mới", "Thông báo");
                txtMatKhauMoi.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtXacNhanMK.Text) || String.IsNullOrWhiteSpace(txtXacNhanMK.Text))
            {
                MessageBox.Show("Hãy xác nhận mật khẩu mới", "Thông báo");
                txtXacNhanMK.Focus();
                return;
            }
            if(txtMatKhauMoi.Text != txtXacNhanMK.Text)
            {
                MessageBox.Show("Nhâp lại mật khẩu mới lần 2 sai!!");
                txtMatKhauMoi.Focus();
                return;
            }

            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            if (Application.OpenForms.OfType<HocVienForm>().Any())
            {
                string sqls = "SELECT * " + $"FROM NGUOIDUNG WHERE MAHV = '{txtMaHV.Text}'";

                SqlCommand sqlCmds = new SqlCommand(sqls);
                sqlCmds.Connection = connection;
                SqlDataReader reader = sqlCmds.ExecuteReader();
                while (reader.Read())
                {
                    string matKhau = reader.GetString(1);
                    matKhauHT = matKhau;
                }
                reader.Close();

                if (txtMatKhauCu.Text != matKhauHT)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng");
                    return;
                }
                if (txtMatKhauCu.Text == txtMatKhauMoi.Text)
                {
                    MessageBox.Show("Mật khẩu bạn muốn đổi giống với mật khẩu hiện tại");
                    return;
                }

                string sql = "UPDATE NGUOIDUNG SET MATKHAU = @matkhau where MAHV = @mahv";
                //khai bao cac tham so
                SqlParameter prMaHV = new SqlParameter("@mahv", SqlDbType.NVarChar);
                SqlParameter prMatKhau = new SqlParameter("@matkhau", SqlDbType.NVarChar);
                //gan gia tri cho tham so
                prMaHV.Value = maHV;
                prMatKhau.Value = txtMatKhauMoi.Text;
                //thuc thi query
                SqlCommand sqlCmd = new SqlCommand(sql);
                sqlCmd.Parameters.Add(prMaHV);
                sqlCmd.Parameters.Add(prMatKhau);

                sqlCmd.Connection = connection;

                int n = sqlCmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Chưa đổi được mật khẩu");
                }
            }


            if (Application.OpenForms.OfType<Admin>().Any())
            {
                string sqls = "SELECT * " + $"FROM GIAOVIEN WHERE TAIKHOAN = '{txtMaHV.Text}'";

                SqlCommand sqlCmds = new SqlCommand(sqls);
                sqlCmds.Connection = connection;
                SqlDataReader reader = sqlCmds.ExecuteReader();
                while (reader.Read())
                {
                    string matKhau = reader.GetString(1);
                    matKhauHT = matKhau;
                }
                reader.Close();

                if (txtMatKhauCu.Text != matKhauHT)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng");
                    return;
                }
                if (txtMatKhauCu.Text == txtMatKhauMoi.Text)
                {
                    MessageBox.Show("Mật khẩu bạn muốn đổi giống với mật khẩu hiện tại");
                    return;
                }

                string sql = "UPDATE GIAOVIEN SET MATKHAU = @matkhau where TAIKHOAN = @taikhoan";
                //khai bao cac tham so
                SqlParameter prTaiKhoan = new SqlParameter("@taikhoan", SqlDbType.NVarChar);
                SqlParameter prMatKhau = new SqlParameter("@matkhau", SqlDbType.NVarChar);
                //gan gia tri cho tham so
                prTaiKhoan.Value = maHV;
                prMatKhau.Value = txtMatKhauMoi.Text;
                //thuc thi query
                SqlCommand sqlCmd = new SqlCommand(sql);
                sqlCmd.Parameters.Add(prTaiKhoan);
                sqlCmd.Parameters.Add(prMatKhau);

                sqlCmd.Connection = connection;

                int n = sqlCmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Chưa đổi được mật khẩu");
                }
            }
            
        }

        private void DoiMatKhau_Load(object sender, EventArgs e)
        {
            txtMaHV.Text = maHV;
          
            if (Application.OpenForms.OfType<HocVienForm>().Any())
            {
                lblTaiKhoan.Text = "Mã học viên: ";
            }
            if (Application.OpenForms.OfType<Admin>().Any())
            {
                lblTaiKhoan.Text = "Tài khoản: ";
            }
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked)
            {
                txtMatKhauCu.PasswordChar = (char)0;
                txtMatKhauMoi.PasswordChar = (char)0;
                txtXacNhanMK.PasswordChar = (char)0;
            }
            else
            {
                txtMatKhauCu.PasswordChar = '*';
                txtMatKhauMoi.PasswordChar = '*';
                txtXacNhanMK.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
