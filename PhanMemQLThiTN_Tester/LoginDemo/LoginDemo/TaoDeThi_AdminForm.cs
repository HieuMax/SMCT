using LoginDemo.KBClass;
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
using static System.Net.Mime.MediaTypeNames;

namespace Project_pharse_setup
{
    public partial class TaoDeThi_AdminForm : Form
    {
        string strConnect = @"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QLThiTest04;Integrated Security=True";
        SqlConnection connection = null;
        MonHoc MonHoc = null;
        CauHoi cauHoi = null;
        LopHoc LopHoc = null;
        ChuongCuaMonHoc chuongMH = null;
        public TaoDeThi_AdminForm()
        {
            InitializeComponent();
            lsvDSDeThi.Columns.Add("Mã Đề Thi", 150);
            lsvDSDeThi.Columns.Add("Tên đề thi", 350);
            lsvDSDeThi.Columns.Add("Số lượng câu", 150);
            lsvDSDeThi.Columns.Add("TG làm bài", 150);
            lsvDSDeThi.Columns.Add("Mã lớp", 200);
            lsvDSDeThi.Columns.Add("Môn thi", 200);
            //hiển thị column
            lsvDSDeThi.View = View.Details;
            //Cho phep chon 1 dong du lieu
            lsvDSDeThi.FullRowSelect = true;
        }




        int countSoCauHoi = 0;
        int countSoChuong = 0;
        string maLopSelected = "";
        private void cbMaLopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {

            int index = cbMaLopHoc.SelectedIndex;
            if (index < 0) return;
            maLopSelected = "";
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            lsvDSDeThi.Items.Clear();
            cbMonHoc.Items.Clear();
            cbMonHoc.ResetText();
            Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                btnChuong18 };
            foreach (Button button in buttons)
            {
                // Set the Visible property of each button to true
                button.Visible = false;
            }

            string sql = $"select * from LOPHOC where Malop = '{cbMaLopHoc.Text}'";
            SqlCommand sqlCmd = new SqlCommand(sql);
            sqlCmd.Connection = connection;
            SqlDataReader reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                string maLop = reader.GetString(0).Trim();
                string tenLop = reader.GetString(1).Trim();
                maLopSelected = maLop;
                LopHoc = new LopHoc(maLop, tenLop);
            }
            reader.Close();
            //
            string Count = $"SELECT COUNT (MaMonHoc) FROM LOP_MONHOC where LOP_MONHOC.MALOP = '{LopHoc.MaLop}'";
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
                    $"\n(SELECT MaMonHoc, ROW_NUMBER() OVER (ORDER BY MaMonHoc) AS RowNum FROM LOP_MONHOC WHERE LOP_MONHOC.MALOP = '{LopHoc.MaLop}') AS T" +
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
        string[] tenMonHoc = new string[30];

        private void cbMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {


            int index = cbMonHoc.SelectedIndex;
            if (index < 0) return;
            MonHoc = (MonHoc)cbMonHoc.SelectedItem;
            monThiSelected = MonHoc.MaMonHoc.Trim();
            lsvDSDeThi.Items.Clear();

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
            Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                btnChuong18 };
            foreach (Button button in buttons)
            {
                button.Visible = false;
            }

            for (int k = 0; k < countSoChuong; k++)
            {
                buttons[k].Visible = true;
            }

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
                tenMonHoc[i] = tenChuong;
                chuongMH = new ChuongCuaMonHoc(maChuong, tenChuong, monThiSelected);
                i++;
            }
            readerGetMaChuong.Close();
            string tenMH = MonHoc.TenMonHoc.Trim();
            string maLop = maLopSelected.Trim();
            string sqlDSDeThi = $"select * from DETHI_QLADMIN where MONTHI = N'{tenMH}' and MALOP = '{maLop}'";
            SqlCommand sqlCmdDSDeThi = new SqlCommand(sqlDSDeThi);
            sqlCmdDSDeThi.Connection = connection;
            SqlDataReader readerDSDeThi = sqlCmdDSDeThi.ExecuteReader();
            while (readerDSDeThi.Read())
            {
                string maDeThi = readerDSDeThi.GetString(0);
                string tenDeThi = readerDSDeThi.GetString(1);
                string soLuongCH = readerDSDeThi.GetString(2);
                string TGLamBai = readerDSDeThi.GetString(3).Trim();
                string[] obj = new string[] { maDeThi, tenDeThi, soLuongCH, TGLamBai + " phút", maLop, tenMH };
                ListViewItem item = new ListViewItem(obj);
                lsvDSDeThi.Items.Add(item);
            }
            readerDSDeThi.Close();


        }



        private void TaoDeThi_AdminForm_Load(object sender, EventArgs e)
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

        int[] cauHoiChuongSelected = new int[20];
        int[] soCauHoiTrongChuong = new int[20];
        int prevChuong = 0;
        private void btnChuong1_Click(object sender, EventArgs e)
        {

            int j = 0;

            // Cast the sender parameter to a Button type
            Button button = sender as Button;
            string text = "";
            // Check if the cast was successful
            if (button != null)
            {
                // Get the text of the button
                text = button.Text.Substring(button.Text.Length - 1, 1);
            }
            j = int.Parse(text.ToString());
            prevChuong = j;


            //Đếm trong chương có bao nhiêu câu hỏi
            string sqlCountSoCauHoi = $"SELECT COUNT (MaCauHoi) FROM CacCauHoi where MaChuong = '{ChuongMonHoc[j]}'";
            SqlCommand sqlCmdCoutSoCauHoi = new SqlCommand(sqlCountSoCauHoi);
            sqlCmdCoutSoCauHoi.Connection = connection;
            SqlDataReader readerCountSoCauHoi = sqlCmdCoutSoCauHoi.ExecuteReader();
            while (readerCountSoCauHoi.Read())
            {
                countSoCauHoi = readerCountSoCauHoi.GetInt32(0);
            }
            readerCountSoCauHoi.Close();

            MessageBox.Show($"Chương {j}: {tenMonHoc[j]} có {countSoCauHoi} câu hỏi");
            {
                // Show message muốn chọn chương ?
                DialogResult result = MessageBox.Show($"Bạn chọn chương {j}?", "Thông báo", MessageBoxButtons.OKCancel);

                // Check the user's response
                if (result == DialogResult.OK)
                {
                    button.ForeColor = Color.Blue;
                    // The user clicked Yes
                    Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                            btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                            btnChuong18 };
                    // Save the data
                    if (De1Chuong.Checked)
                    {

                        for (int k = 0; k < countSoChuong; k++)
                        {
                            buttons[k].Enabled = false;
                        }
                        buttons[j - 1].Enabled = true;
                        soCauHoiTrongChuong[j - 1] = countSoCauHoi;
                        cauHoiChuongSelected[j - 1] = 1; // 1 toggle on - 0 toggle off
                    }
                    if (rdDeTH.Checked)
                    {
                        soCauHoiTrongChuong[j - 1] = countSoCauHoi;
                        cauHoiChuongSelected[j - 1] = 1;
                    }
                }
                else
                {
                    button.ForeColor = Color.Black;
                    if (De1Chuong.Checked)
                    {
                        Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                            btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                            btnChuong18 };
                        for (int k = 0; k < countSoChuong; k++)
                        {
                            buttons[k].Enabled = true;
                        }
                        cauHoiChuongSelected[j - 1] = 0;
                        soCauHoiTrongChuong[j - 1] = 0;
                    }
                    if (rdDeTH.Checked)
                    {
                        soCauHoiTrongChuong[j - 1] = countSoCauHoi;
                        cauHoiChuongSelected[j - 1] = 0;
                    }

                }
            }
        }

        private void txtThoiGian_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && !(Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
        private void txtSLCauHoi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && !(Char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void btnTaoDeThi_Click(object sender, EventArgs e)
        {
            //Check Điều kiện tạo đề thi
            bool unlock = false;
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
            if (String.IsNullOrEmpty(txtTenBaiKiemTra.Text) || String.IsNullOrWhiteSpace(txtTenBaiKiemTra.Text))
            {
                MessageBox.Show("Tên bài kiểm tra không được để trống", "Thông báo");
                txtTenBaiKiemTra.Focus();
                return;
            }
            if (rdDeTH.Checked == false && De1Chuong.Checked == false)
            {
                MessageBox.Show("Vui lòng chọn hình thức thi", "Thông báo");
                return;
            }
            if (rdDeTH.Checked == true || De1Chuong.Checked == true)
            {
                Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                            btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                            btnChuong18 };
                bool checkFlag = false;
                for (int k = 0; k < countSoChuong; k++)
                {
                    if (buttons[k].ForeColor == Color.Blue)
                    {
                        checkFlag = true;
                    }
                }
                if (checkFlag == false)
                {
                    MessageBox.Show("Chưa chọn chương", "Thông báo");
                    return;
                }
            }
            if (String.IsNullOrEmpty(txtSLCauHoi.Text) || String.IsNullOrWhiteSpace(txtSLCauHoi.Text))
            {
                MessageBox.Show("Nhập số lượng câu hỏi", "Thông báo");
                txtSLCauHoi.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtThoiGian.Text) || String.IsNullOrWhiteSpace(txtThoiGian.Text))
            {
                MessageBox.Show("Nhập thời gian làm bài", "Thông báo");
                txtThoiGian.Focus();
                return;
            }
            if (int.Parse(txtSLCauHoi.Text) <= 0)
            {
                MessageBox.Show("Số lượng câu hỏi không hợp lệ", "Thông báo");
                txtSLCauHoi.Focus();
                return;
            }
            //Check end
            int soLuongCauHoi = int.Parse(txtSLCauHoi.Text);
            if (De1Chuong.Checked == true)
            {
                ////Đếm trong chương có bao nhiêu câu hỏi
                //string sqlCountSoCauHoi = $"SELECT COUNT (MaCauHoi) FROM CacCauHoi where MaChuong = '{ChuongMonHoc[j]}'";
                //SqlCommand sqlCmdCoutSoCauHoi = new SqlCommand(sqlCountSoCauHoi);
                //sqlCmdCoutSoCauHoi.Connection = connection;
                //SqlDataReader readerCountSoCauHoi = sqlCmdCoutSoCauHoi.ExecuteReader();
                //while (readerCountSoCauHoi.Read())
                //{
                //    countSoCauHoi = readerCountSoCauHoi.GetInt32(0);
                //}
                //readerCountSoCauHoi.Close();
                int tongSoCauHoi = 0;
                for (int j = 0; j < countSoChuong; j++)
                {
                    tongSoCauHoi += soCauHoiTrongChuong[j];
                    //MessageBox.Show($"{tongSoCauHoi}");
                }
                if (soLuongCauHoi > tongSoCauHoi)
                {
                    MessageBox.Show("Số lượng câu hỏi cần tạo lớn hơn số lượng câu hỏi trong chương", "Thông báo");
                    return;
                }
                unlock = true;
            }
            if (rdDeTH.Checked == true)
            {
                int tongSoCauHoi = 0;
                for (int j = 0; j < countSoChuong; j++)
                {
                    tongSoCauHoi += soCauHoiTrongChuong[j];
                    //MessageBox.Show($"{tongSoCauHoi}");
                }
                if (soLuongCauHoi > tongSoCauHoi)
                {
                    MessageBox.Show("Số lượng câu hỏi cần tạo lớn hơn số lượng câu hỏi trong chương", "Thông báo");
                    return;
                }
                int tongChuong = 0;
                //Thao tác với những chương được chọn
                for (int j = 0; j < countSoChuong; j++)
                {
                    if (cauHoiChuongSelected[j] == 1)
                    {
                        //MessageBox.Show($"Chuong: {j + 1}");
                        tongChuong++;
                    }
                }
                if (tongChuong > 1)
                {
                    unlock = true;
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn tối thiểu từ 2 chương - Hoặc chuyển sang chế độ Đề 1 chương", "Thông báo");
                    return;
                }
            }
            //bắt đầu tạo đề thi khi điều kiện thỏa mãn
            if (unlock)
            {
                Random random = new Random();
                int tongChuong = 0;
                //Thao tác với những chương được chọn
                for (int j = 0; j < countSoChuong; j++)
                {
                    if (cauHoiChuongSelected[j] == 1)
                    {
                        tongChuong++;
                    }
                }

                // Tạo điều kiện cho đề tổng hợp
                string[] sqlGetDataMaCauHoi = new string[tongChuong];
                if (tongChuong > 1)
                {
                    int[] randomQues = new int[tongChuong];
                    bool keyLock = false;
                    bool Lock = false;

                    //
                    do
                    {
                        int SLCH = soLuongCauHoi;
                        if (Lock == false)
                        {
                            int replaceK = 0;
                            for (int k = 0; k < tongChuong - 1; k++)
                            {

                                while (soCauHoiTrongChuong[replaceK] == 0)
                                {
                                    replaceK += 1;
                                }
                                int num = random.Next(SLCH);
                                while (num == 0 || num == SLCH || num > soCauHoiTrongChuong[replaceK])
                                {

                                    num = random.Next(SLCH);

                                }
                                randomQues[k] = num;
                                SLCH -= num;
                                if (k == tongChuong - 2)
                                {
                                    Lock = true;
                                }

                                replaceK++;
                            }
                        }
                        randomQues[tongChuong - 1] = SLCH; //  lấy ra số câu hỏi chương còn lại trong tổng số chương
                        keyLock = false;
                        int zInside = 0;
                        if (Lock)
                        {
                            for (int j = 0; j < countSoChuong; j++)
                            {
                                if (cauHoiChuongSelected[j] == 1)
                                {
                                    if (randomQues[tongChuong - 1] > soCauHoiTrongChuong[j])
                                    {
                                        Lock = false;
                                        break;
                                    }
                                    zInside++;
                                    if (zInside >= tongChuong)
                                        break;
                                }
                            }
                            if (Lock)
                            {
                                keyLock = true;
                            }
                        }
                    }
                    while (keyLock == false);

                    // Gán lại số câu hỏi cho hợp với số chương đã chọn
                    int[] defineChuong = new int[countSoChuong];
                    int z = 0;
                    for (int j = 0; j < countSoChuong; j++)
                    {
                        if (cauHoiChuongSelected[j] == 1)
                        {
                            defineChuong[j] = randomQues[z];
                            sqlGetDataMaCauHoi[z] = $"SELECT TOP {defineChuong[j]} * FROM CacCauHoi where MaChuong = '{ChuongMonHoc[j + 1]}' ORDER BY RAND(CHECKSUM(NEWID()))";
                            z++;
                            if (z >= tongChuong)
                                break;
                        }
                    }
                    // da xac dinh duoc so cau hoi chuong duoc chon

                }
                if (tongChuong == 1)
                {
                    for (int j = 0; j < countSoChuong; j++)
                    {
                        if (cauHoiChuongSelected[j] == 1)
                        {
                            sqlGetDataMaCauHoi[0] = $"SELECT TOP {soLuongCauHoi} * FROM CacCauHoi where MaChuong = '{ChuongMonHoc[j + 1]}' ORDER BY RAND(CHECKSUM(NEWID()))";
                            break;
                        }
                    }
                }


                string[] CT_MaDeThi = new string[soLuongCauHoi];
                int count_CT_MaDeThi = 0;

                if (connection == null)
                    connection = new SqlConnection(strConnect);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                for (int k = 0; k < tongChuong; k++)
                {
                    string sqlStr = sqlGetDataMaCauHoi[k];
                    SqlCommand sqlCmdStr = new SqlCommand(sqlStr);
                    sqlCmdStr.Connection = connection;
                    SqlDataReader reader = sqlCmdStr.ExecuteReader();
                    while (reader.Read())
                    {
                        string maCauHoi = reader.GetString(0).Trim();
                        CT_MaDeThi[count_CT_MaDeThi] = maCauHoi;
                        count_CT_MaDeThi++;
                    }
                    reader.Close();
                }

                // Tạo mã đề thi
                string maDeThi = "";


                //

                MonHoc monhoc = (MonHoc)cbMonHoc.SelectedItem;
                maDeThi += monhoc.MaMonHoc.Trim() + "_";
                string maCH = "0";
                string sqlTaoMaCH = $"SELECT MAX(MADETHI) AS max_amount FROM DETHI where MaMonHoc = '{monhoc.MaMonHoc}'";
                SqlCommand sqlCmdTaoMaCH = new SqlCommand(sqlTaoMaCH);
                sqlCmdTaoMaCH.Connection = connection;
                SqlDataReader readerTaoMaCH = sqlCmdTaoMaCH.ExecuteReader();
                while (readerTaoMaCH.Read())
                {
                    maCH = readerTaoMaCH.GetString(0).Trim();
                }
                readerTaoMaCH.Close();
                int increase = 1 + int.Parse(maCH.Substring(maCH.Length - 1, 1));

                if (increase >= 100)
                {
                    maDeThi += increase + "";
                }
                else if (increase >= 10)
                {
                    maDeThi += "0" + increase + "";
                }
                else
                {
                    maDeThi += "00" + increase + "";
                }
                string sqlDeThi = $"INSERT INTO DETHI(MADETHI, MaMonHoc) VALUES (@madethi,@monthi)";
                SqlParameter prMaDeThi = new SqlParameter("@madethi", SqlDbType.Char);
                SqlParameter prMonThi = new SqlParameter("@monthi", SqlDbType.Char);
                prMaDeThi.Value = maDeThi.Trim();
                prMonThi.Value = monThiSelected.Trim();
                SqlCommand sqlDeThiCmd = new SqlCommand(sqlDeThi);
                sqlDeThiCmd.Connection = connection;
                sqlDeThiCmd.Parameters.Add(prMaDeThi);
                sqlDeThiCmd.Parameters.Add(prMonThi);

                if (sqlDeThiCmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Thêm đề thi thành công", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Thêm đề thi thất bại. Thử lại.", "Thông báo");
                    return;
                }
                for (int insert = 0; insert < soLuongCauHoi; insert++)
                {
                    string sqlCTDE = $"INSERT INTO CHITIET_DETHI(MADETHI, MACAUHOI) VALUES (@CT_madethi,@CT_maCauHoi)";
                    SqlParameter CT_maDeThi = new SqlParameter("@CT_madethi", SqlDbType.Char, 10);
                    SqlParameter CT_maCauHoi = new SqlParameter("@CT_maCauHoi", SqlDbType.Char, 10);

                    CT_maDeThi.Value = maDeThi.Trim();
                    CT_maCauHoi.Value = CT_MaDeThi[insert].Trim();
                    SqlCommand sqlCTDECmd = new SqlCommand(sqlCTDE);
                    sqlCTDECmd.Connection = connection;
                    sqlCTDECmd.Parameters.Add(CT_maDeThi);
                    sqlCTDECmd.Parameters.Add(CT_maCauHoi);
                    //

                    if (sqlCTDECmd.ExecuteNonQuery() > 0)
                    {
                        //MessageBox.Show("Nạp dữ liệu vào đề thi thành công!", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Nạp dữ liệu vào đề thi thất bại! Thử lại.", "Thông báo");
                    }

                }
                string tenDeThi = txtTenBaiKiemTra.Text;
                string thoiGianLamBai = txtThoiGian.Text;
                string maLop = maLopSelected.Trim();
                string monThi = monhoc.TenMonHoc.Trim();
                string[] obj = new string[] { maDeThi, tenDeThi, soLuongCauHoi + "", thoiGianLamBai + " phút", maLop, monThi };
                ListViewItem item = new ListViewItem(obj);

                string sql = "INSERT INTO DETHI_QLADMIN(MADETHI, TENDETHI, SOLUONGCAUHOI, TGLAMBAI, MALOP, MONTHI) VALUES" +
                    "(@madethi, @tendethi, @soluongcauhoi, @tglambai, @malop, @monthi)";
                SqlParameter prMaDeThi_QL = new SqlParameter("@madethi", SqlDbType.Char, 10);
                SqlParameter prTenDeThi = new SqlParameter("@tendethi", SqlDbType.NVarChar);
                SqlParameter prSLCH = new SqlParameter("@soluongcauhoi", SqlDbType.Char);
                SqlParameter prTGlambai = new SqlParameter("@tglambai", SqlDbType.Char);
                SqlParameter prMaLop = new SqlParameter("@malop", SqlDbType.Char, 12);
                SqlParameter prMonThi_QL = new SqlParameter("@monthi", SqlDbType.NVarChar);
                prMaDeThi_QL.Value = maDeThi.Trim();
                prTenDeThi.Value = tenDeThi.Trim();
                prSLCH.Value = soLuongCauHoi + "";
                prTGlambai.Value = thoiGianLamBai.Trim();
                prMaLop.Value = maLop.Trim();
                prMonThi_QL.Value = monThi.Trim();
                SqlCommand sqlCmd = new SqlCommand(sql);
                sqlCmd.Connection = connection;
                sqlCmd.Parameters.Add(prMaDeThi_QL);
                sqlCmd.Parameters.Add(prTenDeThi);
                sqlCmd.Parameters.Add(prSLCH);
                sqlCmd.Parameters.Add(prTGlambai);
                sqlCmd.Parameters.Add(prMaLop);
                sqlCmd.Parameters.Add(prMonThi_QL);


                if (sqlCmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Thêm vào DS đề thi thành công", "Thông báo");
                    lsvDSDeThi.Items.Add(item);
                    btnTaoDeThi.Enabled = false;
                    btnTaoDeThi.BackColor = Color.CadetBlue;
                    cbMaLopHoc.Enabled = false;
                    cbMonHoc.Enabled = false;
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    txtSLCauHoi.Enabled = false;
                    txtThoiGian.Enabled = false;
                    txtTenBaiKiemTra.Enabled = false;
                    button1.Visible = true;
                }
            }
        }
        private void De1Chuong_CheckedChanged(object sender, EventArgs e)
        {
            if (De1Chuong.Checked)
            {
                groupBox1.Enabled = true;
                Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                            btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                            btnChuong18 };
                for (int k = 0; k < countSoChuong; k++)
                {
                    buttons[k].ForeColor = Color.Black;
                }
                if (prevChuong < 1)
                {
                    cauHoiChuongSelected[prevChuong] = 0;
                    soCauHoiTrongChuong[prevChuong] = 0;
                }
                else
                {
                    cauHoiChuongSelected[prevChuong - 1] = 0;
                    soCauHoiTrongChuong[prevChuong - 1] = 0;
                }
            }
        }
        private void rdDeTH_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDeTH.Checked)
            {

                groupBox1.Enabled = true;
                Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                            btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                            btnChuong18 };
                for (int k = 0; k < countSoChuong; k++)
                {
                    buttons[k].Enabled = true;
                    buttons[k].ForeColor = Color.Black;
                }
                if (prevChuong < 1)
                {
                    cauHoiChuongSelected[prevChuong] = 0;
                    soCauHoiTrongChuong[prevChuong] = 0;
                }
                else
                {
                    cauHoiChuongSelected[prevChuong - 1] = 0;
                    soCauHoiTrongChuong[prevChuong - 1] = 0;
                }
            }
        }

        private void cbMaLopHoc_Click(object sender, EventArgs e)
        {
            Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                            btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                            btnChuong18 };
            for (int k = 0; k < countSoChuong; k++)
            {
                buttons[k].Enabled = true;
                buttons[k].ForeColor = Color.Black;
            }
            if (prevChuong < 1)
            {
                cauHoiChuongSelected[prevChuong] = 0;
                soCauHoiTrongChuong[prevChuong] = 0;
            }
            else
            {
                cauHoiChuongSelected[prevChuong - 1] = 0;
                soCauHoiTrongChuong[prevChuong - 1] = 0;
            }
        }

        private void cbMonHoc_Click(object sender, EventArgs e)
        {
            Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                            btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                            btnChuong18 };
            for (int k = 0; k < countSoChuong; k++)
            {
                buttons[k].Enabled = true;
                buttons[k].ForeColor = Color.Black;
            }
            if (prevChuong < 1)
            {
                cauHoiChuongSelected[prevChuong] = 0;
                soCauHoiTrongChuong[prevChuong] = 0;
            }
            else
            {
                cauHoiChuongSelected[prevChuong - 1] = 0;
                soCauHoiTrongChuong[prevChuong - 1] = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e) // button tạo đề mới
        {
            button1.Visible = false;
            btnTaoDeThi.Enabled = true;
            btnTaoDeThi.BackColor = Color.Teal;
            cbMaLopHoc.Enabled = true;
            cbMonHoc.Enabled = true;
            cbMonHoc.ResetText();
            cbMaLopHoc.ResetText();
            lsvDSDeThi.Items.Clear();

            groupBox1.Enabled = false;
            rdDeTH.Checked = false;
            De1Chuong.Checked = false;
            groupBox2.Enabled = true;
            txtSLCauHoi.Enabled = true;
            txtSLCauHoi.Text = "";
            txtThoiGian.Enabled = true;
            txtThoiGian.Text = "";
            txtTenBaiKiemTra.Enabled = true;
            txtTenBaiKiemTra.Text = "";
            Button[] buttons = new Button[] { btnChuong1, btnChuong2, btnChuong3, btnChuong4, btnChuong5, btnChuong6, btnChuong7,
                            btnChuong8, btnChuong9, btnChuong10, btnChuong11, btnChuong12, btnChuong13, btnChuong14, btnChuong15, btnChuong16, btnChuong17,
                            btnChuong18 };
            for (int k = 0; k < countSoChuong; k++)
            {
                buttons[k].Enabled = true;
                buttons[k].ForeColor = Color.Black;
            }
            if (prevChuong < 1)
            {
                cauHoiChuongSelected[prevChuong] = 0;
                soCauHoiTrongChuong[prevChuong] = 0;
            }
            else
            {
                cauHoiChuongSelected[prevChuong - 1] = 0;
                soCauHoiTrongChuong[prevChuong - 1] = 0;
            }
        }
    }

}
