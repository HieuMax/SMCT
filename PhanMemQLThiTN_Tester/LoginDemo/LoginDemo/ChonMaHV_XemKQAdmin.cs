using LoginDemo.KBClass;
using Microsoft.VisualBasic;
using Project_pharse_setup;
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

namespace LoginDemo
{
    public partial class ChonMaHV_XemKQAdmin : Form
    {
        string strConnect = @"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QLThiTest04;Integrated Security=True";
        SqlConnection connection = null;
        MonHoc MonHoc = null;

        public ChonMaHV_XemKQAdmin()
        {
            InitializeComponent();
        }

        private void ChonMaHV_XemKQAdmin_Load(object sender, EventArgs e)
        {
            cbMaLopHoc.Items.Clear();
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            string sqlGetMaLop = "select MaLop from LOPHOC";
            SqlCommand sqlGetMaLopCmd = new SqlCommand(sqlGetMaLop);
            sqlGetMaLopCmd.Connection = connection;
            SqlDataReader readerGetMaLop = sqlGetMaLopCmd.ExecuteReader();
            while(readerGetMaLop.Read())
            {
                cbMaLopHoc.Items.Add(readerGetMaLop.GetString(0).Trim());
            }
            readerGetMaLop.Close();
        }

        private void cbMaLopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaLopHoc.Items.Count <0)
            {
                return;
            }
            cbMaHV.Items.Clear();
            lblTenDeThi.Visible = false;
            lblTenDeThi.Text = "";
            cbMaDe.ResetText();
            cbMaDe.Items.Clear();
            cbMaHV.ResetText();
            cbMaHV.Items.Clear();
            txtSdt.Text = string.Empty;
            txtPhai.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            lblTenLop.Visible = true;
            lblTenLop.Text = "";
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            //Get tên lớp
            string sqlGetTenLop = $"select TENLOP from LOPHOC where MALOP = '{cbMaLopHoc.SelectedItem}'";
            SqlCommand sqlGetTenLopCmd = new SqlCommand(sqlGetTenLop);
            sqlGetTenLopCmd.Connection = connection;
            SqlDataReader readerGetTenLop = sqlGetTenLopCmd.ExecuteReader();
            while (readerGetTenLop.Read())
            {
                lblTenLop.Text = "Tên Lớp: " + readerGetTenLop.GetString(0).Trim();
            }
            readerGetTenLop.Close();

            //
            cbMonHoc.Items.Clear();
            cbMonHoc.ResetText();
            string Count = $"SELECT COUNT (MaMonHoc) FROM LOP_MONHOC where LOP_MONHOC.MALOP = '{cbMaLopHoc.SelectedItem}'";
            int countNum = 0;
            SqlCommand sqlCount = new SqlCommand(Count);
            sqlCount.Connection = connection;
            SqlDataReader readerCount = sqlCount.ExecuteReader();
            while (readerCount.Read())
            {
                countNum = readerCount.GetInt32(0);
            }
            readerCount.Close();
            for (int i = 1; i <= countNum; i++)
            {
                string getMonHoc =
                    $"SELECT MaMonHoc FROM" +
                    $"\n(SELECT MaMonHoc, ROW_NUMBER() OVER (ORDER BY MaMonHoc) AS RowNum FROM LOP_MONHOC WHERE LOP_MONHOC.MALOP = '{cbMaLopHoc.SelectedItem}') AS T" +
                    $"\nWHERE RowNum = {i}";
                SqlCommand GETsqlCmdMonHoc = new SqlCommand(getMonHoc);
                GETsqlCmdMonHoc.Connection = connection;
                SqlDataReader GETreaderMonHoc = GETsqlCmdMonHoc.ExecuteReader();
                string GetTenMonHoc = "";
                while (GETreaderMonHoc.Read())
                {
                    GetTenMonHoc = GETreaderMonHoc.GetString(0);
                }
                GETreaderMonHoc.Close();
                string sqlTenMonHoc = $"select * from monhoc where MaMonHoc = '{GetTenMonHoc}'";
                SqlCommand sqlCmdTenMonHoc = new SqlCommand(sqlTenMonHoc);
                sqlCmdTenMonHoc.Connection = connection;
                SqlDataReader readerTenMonHoc = sqlCmdTenMonHoc.ExecuteReader();
                while (readerTenMonHoc.Read())
                {
                    string maMonHoc = readerTenMonHoc.GetString(0);
                    string tenMonHoc = readerTenMonHoc.GetString(1);

                    MonHoc = new MonHoc(maMonHoc, tenMonHoc);
                    cbMonHoc.Items.Add(MonHoc);
                }
                readerTenMonHoc.Close();
            }
        }

        private void cbMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Xử lý dữ liệu load đề thi
            lblTenDeThi.Visible = false;
            lblTenDeThi.Text = "";
            cbMaHV.ResetText();
            cbMaHV.Items.Clear();
            cbMaDe.ResetText();
            cbMaDe.Items.Clear();
            txtSdt.Text = string.Empty;
            txtPhai.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            MonHoc = (MonHoc)cbMonHoc.SelectedItem;

            string sql = $"select MaDeThi from DETHI where MaMonHoc = @mamonhoc";
            SqlParameter prMaMonHoc = new SqlParameter("@mamonhoc", SqlDbType.Char);
            prMaMonHoc.Value = MonHoc.MaMonHoc.Trim();
            SqlCommand sqlCmd = new SqlCommand(sql);
            sqlCmd.Connection = connection;
            sqlCmd.Parameters.Add(prMaMonHoc);
            SqlDataReader readers = sqlCmd.ExecuteReader();
            while (readers.Read())
            {
                cbMaDe.Items.Add(readers.GetString(0));
            }
            readers.Close();
            //

        }

        private void cbMaDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaDe.Items.Count <0 || cbMaDe.SelectedIndex < 0)
            {
                return;
            }
            cbMaHV.Items.Clear();
            lblTenDeThi.Visible = true;
            lblTenDeThi.Text = "";

            string sqlGetTenDeThi = $"select TENDETHI from DETHI_QLADMIN where MADETHI = '{cbMaDe.SelectedItem.ToString().Trim()}'";
            SqlCommand sqlGetTenDeThiCmd = new SqlCommand(sqlGetTenDeThi);
            sqlGetTenDeThiCmd.Connection= connection;
            SqlDataReader readerGetTenDeThi = sqlGetTenDeThiCmd.ExecuteReader();
            while(readerGetTenDeThi.Read())
            {
                lblTenDeThi.Text = "Tên Đề Thi: " + (readerGetTenDeThi.GetString(0).Trim());
            }
            readerGetTenDeThi.Close();

            //Xử lý dữ liệu database có học viên đã thi chưa

            //Check Đề thi đã có HV thi chưa 
            int count = 0;
            string sqlCheck = $"SELECT COUNT (madethi) FROM KETQUA where MADETHI = '{cbMaDe.SelectedItem.ToString().Trim()}'";
            SqlCommand sqlCheckCmd = new SqlCommand(sqlCheck);
            sqlCheckCmd.Connection= connection;
            SqlDataReader readerCheck = sqlCheckCmd.ExecuteReader();
            while(readerCheck.Read())
            {
                count = readerCheck.GetInt32(0);
            }
            readerCheck.Close();
            if (count <=0 )
            {
                MessageBox.Show("Đề thi này chưa có học viên thi", "Thông báo");
                cbMaDe.ResetText();
                cbMaHV.ResetText();
                txtSdt.Text = string.Empty;
                txtPhai.Text = string.Empty;
                txtHoTen.Text = string.Empty;
                return;
            }
            //Check end
            cbMaHV.ResetText();
            txtSdt.Text = string.Empty;
            txtPhai.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            //Load Mã HV đã thi lên form
            string sqlLoadMaHV = $"select MAHV from KETQUA where MADETHI = '{cbMaDe.SelectedItem.ToString().Trim()}'";
            SqlCommand sqlCommandLoadHV = new SqlCommand(sqlLoadMaHV);
            sqlCommandLoadHV.Connection= connection;
            SqlDataReader reader = sqlCommandLoadHV.ExecuteReader();
            while(reader.Read())
            {
                cbMaHV.Items.Add(reader.GetString(0).Trim());

            }
            reader.Close();


        }

        private void btnXemKetQua_Click(object sender, EventArgs e)
        {
            
            //Check Điều kiện nhấn nút
            if (cbMaLopHoc.Items == null || cbMaLopHoc.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn mã lớp", "Thông báo");
                cbMaLopHoc.Focus();
                return;
            }
            if (cbMonHoc.Items == null || cbMonHoc.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn môn thi", "Thông báo");
                cbMonHoc.Focus();
                return;
            }
            if (cbMaHV.Items == null || cbMaHV.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn mã học viên", "Thông báo");
                cbMaHV.Focus();
                return;
            }

            //Load Data HV được chọn
            string holot_ = "";
            string ten_ = "";
            string sql = $"select HOLOT, TEN from HOCVIEN where MAHV = '{cbMaHV.Text.Trim()}'";
            SqlCommand sqlCommand = new SqlCommand(sql);
            sqlCommand.Connection= connection;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while(reader.Read())
            {
                holot_ = reader.GetString(0).Trim();
                ten_ = reader.GetString(1).Trim();
            }
            reader.Close();

            XemKetQuaHV xemKetQua = new XemKetQuaHV();
            xemKetQua.maHV = cbMaHV.Text.Trim();
            xemKetQua.hoTen = holot_ + " " + ten_;
            xemKetQua.maLop = cbMaLopHoc.SelectedItem.ToString().Trim();
            MonHoc = (MonHoc)cbMonHoc.SelectedItem;
            xemKetQua.monHoc = MonHoc.TenMonHoc.Trim();
            xemKetQua.maDe = cbMaDe.Text.Trim();
            xemKetQua.ShowDialog();
        }

        private void cbMaHV_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if (cbMaHV.Items.Count < 0 || cbMaHV.SelectedIndex < 0)
            {
                
                return;
            }

            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            string sql = "SELECT * " +
                $"FROM HOCVIEN WHERE MAHV = '{cbMaHV.SelectedItem.ToString().Trim()}'";
           
            SqlCommand sqlCmd = new SqlCommand(sql);
            sqlCmd.Connection = connection;
            SqlDataReader reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {


                string hoLot = reader.GetString(1);
                string ten = reader.GetString(2);
                string phai = reader.GetString(3);
                string soDT = reader.GetString(4);

                txtHoTen.Text = hoLot + " " + ten;
                txtPhai.Text = phai;
                txtSdt.Text = soDT;
            }
            reader.Close();
        }

    }
}
