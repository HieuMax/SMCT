using LoginDemo.KBClass;
using Project_pharse_setup;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LoginDemo.XDForms
{
    public partial class HocVienForm : Form
    {

        string strConnect = @"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QLThiTest04;Integrated Security=True";
        SqlConnection connection = null;
        LopHoc lopHoc = null;
        MonHoc MonHoc = null;

        public string temp;
        public HocVienForm()
        {
            InitializeComponent();
        }

        private void HocVien_Load(object sender, EventArgs e)
        {
            cbMonHoc.Items.Clear();
            cbMonHoc.ResetText();
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            string sql = "SELECT MALOP, TENLOP FROM LopHoc";

            SqlCommand sqlCmd = new SqlCommand(sql);
            sqlCmd.Connection = connection;
            SqlDataReader read = sqlCmd.ExecuteReader();
            while (read.Read())
            {

                LopHoc s = new LopHoc();
                s.MaLop = read.GetString(0).Trim();
                s.TenLop = read.GetString(1).Trim();

            }
            read.Close();
            txtMaHV.Text = temp;

            string sqls = "SELECT MAHV, HOLOT, TEN, PHAI, SODT, MALOP " +
                "FROM HOCVIEN WHERE MAHV = @maHV";
            SqlParameter prMaHV = new SqlParameter("@maHV", SqlDbType.Char);
            prMaHV.Value = txtMaHV.Text;

            SqlCommand sqlCmds = new SqlCommand(sqls);
            sqlCmds.Parameters.Add(prMaHV);
            sqlCmds.Connection = connection;
            SqlDataReader readers = sqlCmds.ExecuteReader();
            while (readers.Read())
            {
                string maHV = readers.GetString(0);
                string hoLot = readers.GetString(1);
                string ten = readers.GetString(2);
                string phai = readers.GetString(3);
                string soDT = readers.GetString(4);
                string maLop = readers.GetString(5);

                txtMaHV.Text = maHV.Trim();
                txtHoLot.Text = hoLot.Trim();
                txtTen.Text = ten;
                txtGioiTinh.Text = phai.Trim();
                txtSĐT.Text = soDT.Trim();
                txtMaLop.Text = maLop.Trim();
            }
            readers.Close();

            string sqlHV = $"select * from LOPHOC where Malop = '{txtMaLop.Text}'";
            SqlCommand sqlHVCmd = new SqlCommand(sql);
            sqlHVCmd.Connection = connection;
            SqlDataReader reader = sqlHVCmd.ExecuteReader();
            while (reader.Read())
            {
                string maLop = reader.GetString(0).Trim();
                string tenLop = reader.GetString(1).Trim();
                lopHoc = new LopHoc(maLop, tenLop);
            }
            reader.Close();
            //
            string Count = $"SELECT COUNT (MaMonHoc) FROM LOP_MONHOC where LOP_MONHOC.MALOP = '{txtMaLop.Text}'";
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
                    $"\n(SELECT MaMonHoc, ROW_NUMBER() OVER (ORDER BY MaMonHoc) AS RowNum FROM LOP_MONHOC WHERE LOP_MONHOC.MALOP = '{txtMaLop.Text}') AS T" +
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

        private void btnThi_Click(object sender, EventArgs e)
        {

            if (cbMonHoc.Items == null || cbMonHoc.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn môn thi", "Thông báo");
                return;
            }
            if (cbMaDe.Items == null || cbMaDe.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn mã đề", "Thông báo");
                return;
            }
            //Còn tạo điều kiện cho phép thi 
            bool checkFlag = false; 
            string sqlCheck = $"select IDKETQUA from KETQUA where MAHV = '{txtMaHV.Text.Trim()}' and MADETHI = '{cbMaDe.Text.Trim()}'";
            SqlCommand sqlCheckCmd = new SqlCommand(sqlCheck);
            sqlCheckCmd.Connection = connection;
            SqlDataReader readerCheck = sqlCheckCmd.ExecuteReader();
            while (readerCheck.Read())
            {
                string check = readerCheck.GetString(0).Trim();
                if (!(String.IsNullOrEmpty(check)) && !(String.IsNullOrWhiteSpace(check)) )
                {
                    checkFlag = true;
                }
            }
            readerCheck.Close();
            if (checkFlag)
            {
                MessageBox.Show("Bạn đã làm đề thi này trước đó", "Thông báo");
                btnXemKetQua.Visible = true;
                return;
            }

            MessageBox.Show("Bắt đầu thi");
            VaoThi vt = new VaoThi();
            vt.maHV = txtMaHV.Text.Trim();
            vt.hoTen = txtHoLot.Text.Trim() + " " + txtTen.Text.Trim();
            vt.maLop = txtMaLop.Text.Trim();
            MonHoc = (MonHoc)cbMonHoc.SelectedItem;
            vt.monHoc = MonHoc.TenMonHoc.Trim();
            vt.maDe = cbMaDe.Text.Trim();
            vt.ShowDialog();
        }

        private void cbMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnXemKetQua.Visible = false;
            //Xử lý dữ liệu load đề thi
            cbMaDe.ResetText();
            cbMaDe.Items.Clear();
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
            while(readers.Read())
            {
                cbMaDe.Items.Add(readers.GetString(0));
            }
            readers.Close();
            //

        }

        private void cbMaDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnXemKetQua.Visible = false;
        }

        private void btnXemKetQua_Click(object sender, EventArgs e)
        {
            XemKetQuaHV xemKetQua = new XemKetQuaHV();
            xemKetQua.maHV = txtMaHV.Text.Trim();
            xemKetQua.hoTen = txtHoLot.Text.Trim() + " " + txtTen.Text.Trim();
            xemKetQua.maLop = txtMaLop.Text.Trim();
            MonHoc = (MonHoc)cbMonHoc.SelectedItem;
            xemKetQua.monHoc = MonHoc.TenMonHoc.Trim();
            xemKetQua.maDe = cbMaDe.Text.Trim();
            xemKetQua.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            DoiMatKhau dmk = new DoiMatKhau();
            dmk.maHV = txtMaHV.Text;
            dmk.ShowDialog();
        }
    }
}
