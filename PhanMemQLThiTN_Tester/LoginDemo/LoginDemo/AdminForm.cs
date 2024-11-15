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

namespace Project_pharse_setup
{
    public partial class AdminForm : Form
    {
        string strConnect = @"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QLThiTest04;Integrated Security=True";
        SqlConnection connection = null;
        MonHoc MonHoc = null;
        CauHoi cauHoi = null;
        LopHoc lopHoc = null;
        ChuongCuaMonHoc chuongMH = null;
        public AdminForm()
        {
            InitializeComponent();
            lsvDSCauTracNghiem.Columns.Add("Mã Câu Hỏi", 120);
            lsvDSCauTracNghiem.Columns.Add("Nội dung câu hỏi", 500);
            lsvDSCauTracNghiem.Columns.Add("Đáp án A", 150);
            lsvDSCauTracNghiem.Columns.Add("Đáp án B", 150);
            lsvDSCauTracNghiem.Columns.Add("Đáp án C", 150);
            lsvDSCauTracNghiem.Columns.Add("Đáp án D", 150);
            lsvDSCauTracNghiem.Columns.Add("Đáp án đúng", 150);
            //hiển thị column
            lsvDSCauTracNghiem.View = View.Details;
            //Cho phep chon 1 dong du lieu
            lsvDSCauTracNghiem.FullRowSelect = true;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {

        }

        private void cbChuongMonhoc_SelectedIndexChanged(object sender, EventArgs e)
        {


            string sqlShowCauHoi = $"select * from CacCauHoi where MaChuong = '{ChuongMonHoc[cbChuongMonhoc.SelectedIndex + 1]}'";
            SqlCommand sqlCmdShowCauHoi = new SqlCommand(sqlShowCauHoi);
            sqlCmdShowCauHoi.Connection = connection;
            SqlDataReader readerShowCauHoi = sqlCmdShowCauHoi.ExecuteReader();
            lsvDSCauTracNghiem.Items.Clear();
            while (readerShowCauHoi.Read())
            {
                string MaCauHoi = readerShowCauHoi.GetString(0).Trim();
                string NoiDungCauHoi = readerShowCauHoi.GetString(1);
                string PanA = readerShowCauHoi.GetString(2);
                string PanB = readerShowCauHoi.GetString(3);
                string PanC = readerShowCauHoi.GetString(4);
                string PanD = readerShowCauHoi.GetString(5);
                string Answer = readerShowCauHoi.GetString(6).Trim();
                string MaChuong = readerShowCauHoi.GetString(7).Trim();
                CauHoi cauHoi = new CauHoi(MaCauHoi, NoiDungCauHoi, PanA, PanB, PanC, PanD, Answer, MaChuong);
                string[] obj = new string[] {cauHoi.MaCauHoi , cauHoi.NoiDungCauHoi , cauHoi.PanA ,
                    cauHoi.PanB , cauHoi.PanC , cauHoi.PanD , cauHoi.Answer.Substring(cauHoi.Answer.Length-1,1)};
                ListViewItem item = new ListViewItem(obj);
                lsvDSCauTracNghiem.Items.Add(item);
            }
            readerShowCauHoi.Close();
        }

        private void cbMaLopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbMaLopHoc.SelectedIndex;
            if (index < 0) return;
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            lsvDSCauTracNghiem.Items.Clear();
            cbMonHoc.Items.Clear();
            cbMonHoc.ResetText();
            cbChuongMonhoc.Items.Clear();
            cbChuongMonhoc.ResetText();
            string sql = $"select * from LOPHOC where Malop = '{cbMaLopHoc.Text}'";

            SqlCommand sqlCmd = new SqlCommand(sql);
            sqlCmd.Connection = connection;
            SqlDataReader reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {

                string maLop = reader.GetString(0).Trim();
                string tenLop = reader.GetString(1).Trim();
                lopHoc = new LopHoc(maLop, tenLop);


            }
            reader.Close();
            //
            string Count = $"SELECT COUNT (MaMonHoc) FROM LOP_MONHOC where LOP_MONHOC.MALOP = '{lopHoc.MaLop}'";
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
                    $"\n(SELECT MaMonHoc, ROW_NUMBER() OVER (ORDER BY MaMonHoc) AS RowNum FROM LOP_MONHOC WHERE LOP_MONHOC.MALOP = '{lopHoc.MaLop}') AS T" +
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


        string monThiSelected = "";
        string[] ChuongMonHoc = new string[30];
        int countSoCauHoi = 0;
        int countSoChuong = 0;
        private void cbMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cbMonHoc.SelectedIndex;
            if (index < 0) return;
            MonHoc = (MonHoc)cbMonHoc.SelectedItem;
            monThiSelected = MonHoc.MaMonHoc.Trim();
            cbChuongMonhoc.Items.Clear();
            cbChuongMonhoc.ResetText();

            // Show cau hoi len form
            cauHoi = new CauHoi();
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            //Đếm số chương của môn học
            string sqlCountSoChuongMH = $"SELECT COUNT (MaChuong) FROM chuongcacmonhoc_ where MaMonHoc = '{monThiSelected}'";
            SqlCommand sqlCmdCountSoChuongMH = new SqlCommand(sqlCountSoChuongMH);
            sqlCmdCountSoChuongMH.Connection = connection;
            SqlDataReader readerCountSoChuongMH = sqlCmdCountSoChuongMH.ExecuteReader();
            while (readerCountSoChuongMH.Read())
            {
                countSoChuong = readerCountSoChuongMH.GetInt32(0);
            }
            readerCountSoChuongMH.Close();

            //Get ma chuong mon 
            string sqlGetMaChuong = $"select * from chuongcacmonhoc_ where MaMonHoc = '{monThiSelected}'";
            SqlCommand sqlCmdGetMaChuong = new SqlCommand(sqlGetMaChuong);
            sqlCmdGetMaChuong.Connection = connection;
            SqlDataReader readerGetMaChuong = sqlCmdGetMaChuong.ExecuteReader();

            int i = 1;
            while (readerGetMaChuong.Read())
            {
                string maChuong = readerGetMaChuong.GetString(0).Trim();
                string tenChuong = readerGetMaChuong.GetString(1).Trim();
                ChuongMonHoc[i] = maChuong;
                chuongMH = new ChuongCuaMonHoc(maChuong, tenChuong, monThiSelected);
                cbChuongMonhoc.Items.Add(chuongMH.MaChuong + " - " + chuongMH.TenChuong);
                i++;
            }
            readerGetMaChuong.Close();

            // Đếm trong chương có bao nhiêu câu hỏi
            string sqlCountSoCauHoi = $"SELECT COUNT (MaCauHoi) FROM CacCauHoi where MaChuong = '{ChuongMonHoc[cbChuongMonhoc.SelectedIndex + 1]}'";
            SqlCommand sqlCmdCoutSoCauHoi = new SqlCommand(sqlCountSoCauHoi);
            sqlCmdCoutSoCauHoi.Connection = connection;
            SqlDataReader readerCountSoCauHoi = sqlCmdCoutSoCauHoi.ExecuteReader();
            while (readerCountSoCauHoi.Read())
            {
                countSoCauHoi = readerCountSoCauHoi.GetInt32(0);
            }
            readerCountSoCauHoi.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
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
            if (cbChuongMonhoc.Items == null || cbChuongMonhoc.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn chương", "Thông báo");
                cbMonHoc.Focus();
                return;
            }
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            //Tạo mã câu hỏi
            string maCH = "0";
            string sqlTaoMaCH = "SELECT MAX(MaCauHoi) AS max_amount FROM CacCauHoi";
            SqlCommand sqlCmdTaoMaCH = new SqlCommand(sqlTaoMaCH);
            sqlCmdTaoMaCH.Connection = connection;
            SqlDataReader readerTaoMaCH = sqlCmdTaoMaCH.ExecuteReader();
            while (readerTaoMaCH.Read())
            {
                maCH = readerTaoMaCH.GetString(0).Trim();
            }
            readerTaoMaCH.Close();

            if ((String.IsNullOrEmpty(txtNDCauHoi.Text)) && (String.IsNullOrWhiteSpace(txtNDCauHoi.Text)))
            {
                MessageBox.Show("Nội dung câu hỏi không được bỏ trống", "Thông báo");
                txtNDCauHoi.Focus();
                return;
            }
            if ((String.IsNullOrEmpty(txtPAnB.Text)) && (String.IsNullOrWhiteSpace(txtPAnB.Text)))
            {
                MessageBox.Show("Nội dung câu trả lời không được bỏ trống", "Thông báo");
                txtPAnB.Focus();
                return;
            }
            if ((String.IsNullOrEmpty(txtPAnC.Text)) && (String.IsNullOrWhiteSpace(txtPAnC.Text)))
            {
                MessageBox.Show("Nội dung câu trả lời không được bỏ trống", "Thông báo");
                txtPAnC.Focus();
                return;
            }
            if ((String.IsNullOrEmpty(txtPAnA.Text)) && (String.IsNullOrWhiteSpace(txtPAnA.Text)))
            {
                MessageBox.Show("Nội dung câu trả lời không được bỏ trống", "Thông báo");
                txtPAnA.Focus();
                return;
            }
            if ((String.IsNullOrEmpty(txtPAnD.Text)) && (String.IsNullOrWhiteSpace(txtPAnD.Text)))
            {
                MessageBox.Show("Nội dung câu trả lời không được bỏ trống", "Thông báo");
                txtPAnD.Focus();
                return;
            }
            if (rdPAnA.Checked == false && rdPAnB.Checked == false && rdPAnC.Checked == false && rdPAnD.Checked == false)
            {
                MessageBox.Show("Vui lòng chọn câu trả lời đúng", "Thông báo");
                return;
            }
            string sql = "insert into CacCauHoi(MaCauHoi,NoiDungCauHoi,DapAnA,DapAnB,DapAnC,DapAnD,CauTraLoi,MaChuong) values" +
                "(@macauhoi, @NDCH, @PAnA, @PAnB, @PAnC, @PAnD, @cautraloi, @machuong)";
            SqlParameter prMaCauHoi = new SqlParameter("@macauhoi", SqlDbType.Char);
            SqlParameter prNoiDungCauHoi = new SqlParameter("@NDCH", SqlDbType.NVarChar);
            SqlParameter prPAnA = new SqlParameter("@PAnA", SqlDbType.NVarChar);
            SqlParameter prPAnB = new SqlParameter("@PAnB", SqlDbType.NVarChar);
            SqlParameter prPAnC = new SqlParameter("@PAnC", SqlDbType.NVarChar);
            SqlParameter prPAnD = new SqlParameter("@PAnD", SqlDbType.NVarChar);
            SqlParameter prCauTraLoi = new SqlParameter("@cautraloi", SqlDbType.NVarChar);
            SqlParameter prMaChuong = new SqlParameter("@machuong", SqlDbType.Char);

            //Thêm giá trị cho tham số
            int test = int.Parse(maCH.ToString());
            prMaCauHoi.Value = (test + 1).ToString();
            prNoiDungCauHoi.Value = txtNDCauHoi.Text;
            prPAnA.Value = txtPAnA.Text;
            prPAnB.Value = txtPAnB.Text;
            prPAnC.Value = txtPAnC.Text;
            prPAnD.Value = txtPAnD.Text;
            if (rdPAnA.Checked)
            {
                prCauTraLoi.Value = "A";
            }
            if (rdPAnB.Checked)
            {
                prCauTraLoi.Value = "B";
            }
            if (rdPAnC.Checked)
            {
                prCauTraLoi.Value = "C";
            }
            if (rdPAnD.Checked)
            {
                prCauTraLoi.Value = "D";
            }
            prMaChuong.Value = ChuongMonHoc[cbChuongMonhoc.SelectedIndex + 1];
            SqlCommand sqlCmd = new SqlCommand(sql);
            sqlCmd.Connection = connection;
            sqlCmd.Parameters.Add(prMaCauHoi);
            sqlCmd.Parameters.Add(prNoiDungCauHoi);
            sqlCmd.Parameters.Add(prPAnA);
            sqlCmd.Parameters.Add(prPAnB);
            sqlCmd.Parameters.Add(prPAnC);
            sqlCmd.Parameters.Add(prPAnD);
            sqlCmd.Parameters.Add(prCauTraLoi);
            sqlCmd.Parameters.Add(prMaChuong);


            int n = sqlCmd.ExecuteNonQuery();
            string MaCauHoi = (test + 1).ToString();
            string NoiDungCauHoi = txtNDCauHoi.Text;
            string PanA = txtPAnA.Text;
            string PanB = txtPAnB.Text;
            string PanC = txtPAnC.Text;
            string PanD = txtPAnD.Text;

            string Answer = prCauTraLoi.Value.ToString();
            string MaChuong = ChuongMonHoc[cbChuongMonhoc.SelectedIndex + 1];
            CauHoi cauHoi = new CauHoi(MaCauHoi, NoiDungCauHoi, PanA, PanB, PanC, PanD, Answer, MaChuong);
            string[] obj = new string[] {cauHoi.MaCauHoi , cauHoi.NoiDungCauHoi , cauHoi.PanA ,
                    cauHoi.PanB , cauHoi.PanC , cauHoi.PanD , cauHoi.Answer.Substring(cauHoi.Answer.Length-1,1)};
            ListViewItem item = new ListViewItem(obj);
            if (n > 0)
            {
                MessageBox.Show("Thêm thành công", "Thông báo");
                txtNDCauHoi.Text = "";
                txtPAnA.Text = "";
                txtPAnB.Text = "";
                txtPAnC.Text = "";
                txtPAnD.Text = "";
                rdPAnA.Checked = false; rdPAnB.Checked = false;
                rdPAnC.Checked = false; rdPAnD.Checked = false;
                lsvDSCauTracNghiem.Items.Add(item);
            }
            else
            {
                MessageBox.Show("Chưa thêm được dữ liệu");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
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
            if (cbChuongMonhoc.Items == null || cbChuongMonhoc.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn chương", "Thông báo");
                cbMonHoc.Focus();
                return;
            }
            if (lsvDSCauTracNghiem.SelectedItems == null || lsvDSCauTracNghiem.TabIndex < 0)
                return;

            try
            {
                if (connection == null)
                    connection = new SqlConnection(strConnect);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                string sql = " DELETE FROM CacCauHoi WHERE MaCauHoi = @MACAUHOI";

                //khai bao cac tham so
                SqlParameter prMaCauHoi = new SqlParameter("@MACAUHOI", SqlDbType.Int);

                //gan gia tri cho tham so
                ListViewItem item = lsvDSCauTracNghiem.SelectedItems[0];
                CauHoi cauHoi = new CauHoi();
                cauHoi.MaCauHoi = item.SubItems[0].Text;
                int index = lsvDSCauTracNghiem.SelectedItems[0].Index;
                prMaCauHoi.Value = cauHoi.MaCauHoi;

                //thuc thi query
                SqlCommand sqlCmd = new SqlCommand(sql);
                sqlCmd.Parameters.Add(prMaCauHoi);

                sqlCmd.Connection = connection;

                int n = sqlCmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show("Xóa thành công", "Thông báo");
                    lsvDSCauTracNghiem.Items.RemoveAt(index);

                }
                else
                {
                    MessageBox.Show("Chưa thêm được dữ liệu");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void AdminForm_Load_1(object sender, EventArgs e)
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
            while (readerGetMaLop.Read())
            {
                cbMaLopHoc.Items.Add(readerGetMaLop.GetString(0).Trim());
            }
            readerGetMaLop.Close();
        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
