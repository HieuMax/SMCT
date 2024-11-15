using Dapper;
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
    public partial class XemKetQuaHV : Form
    {
        string strConnect = @"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QLThiTest04;Integrated Security=True";
        SqlConnection connection = null;
        CauHoi cauHoi = null;

        public string maHV;
        public string hoTen;
        public string maLop;
        public string monHoc;
        public string maDe;
        public XemKetQuaHV()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            lblNote.Text = "Ghi chú:";
            btnNext.Enabled = true;
            btnNext.BackColor = Color.Teal;


            rdCauA.ForeColor = SystemColors.ControlText;
            rdCauB.ForeColor = SystemColors.ControlText;
            rdCauC.ForeColor = SystemColors.ControlText;
            rdCauD.ForeColor = SystemColors.ControlText;
            grBox.Text = $"Câu hỏi số {countButton - 1}";
            lblSLCauHoi.Text = $"Câu hỏi số {countButton - 1} / {countSoCauHoi}";

            if (keyNext == false)
            {
                countButton--;
                indexNext = countButton;
            }
            else if (keyNext)
            {
                countButton--;
                indexNext = countButton;
            }
            string sqlShowCauHoi = $"select * from cacCauHoi where MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = {countButton}))";

            SqlCommand sqlCmdShowCauHoi = new SqlCommand(sqlShowCauHoi);
            sqlCmdShowCauHoi.Connection = connection;
            SqlDataReader readerShowCauHoi = sqlCmdShowCauHoi.ExecuteReader();
            while (readerShowCauHoi.Read())
            {

                cauHoi.MaCauHoi = readerShowCauHoi.GetString(0).Trim();
                cauHoi.NoiDungCauHoi = readerShowCauHoi.GetString(1);
                cauHoi.PanA = readerShowCauHoi.GetString(2);
                cauHoi.PanB = readerShowCauHoi.GetString(3);
                cauHoi.PanC = readerShowCauHoi.GetString(4);
                cauHoi.PanD = readerShowCauHoi.GetString(5);
                cauHoi.Answer = readerShowCauHoi.GetString(6).Trim();
                cauHoi.MaChuong = readerShowCauHoi.GetString(7).Trim();
                if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "A")
                {
                    rdCauA.ForeColor = Color.Green;
                    cauTraLoiDung = "1";
                }
                if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "B")
                {
                    rdCauB.ForeColor = Color.Green;
                    cauTraLoiDung = "2";
                }
                if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "C")
                {
                    rdCauC.ForeColor = Color.Green;
                    cauTraLoiDung = "3";
                }
                if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "D")
                {
                    rdCauD.ForeColor = Color.Green;
                    cauTraLoiDung = "4";
                }
                txtCauHoi.Text = "[" + cauHoi.MaCauHoi + "] " + cauHoi.NoiDungCauHoi;
                rdCauA.Text = cauHoi.PanA;
                rdCauB.Text = cauHoi.PanB;
                rdCauC.Text = cauHoi.PanC;
                rdCauD.Text = cauHoi.PanD;
            }
            readerShowCauHoi.Close();

            //so sánh đối chiếu kết quả
            string sqlCompareResult =
                $"select CauTraLoiCuaHV from CT_KETQUA where IDKETQUA = '{txtMaDe.Text.Trim() + "_" + txtMaHV.Text.Trim()}' and MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum =  {countButton} ))";
            SqlCommand sqlCompareResultCmd = new SqlCommand(sqlCompareResult);
            sqlCompareResultCmd.Connection = connection;
            SqlDataReader readerCompareResult = sqlCompareResultCmd.ExecuteReader();
            while (readerCompareResult.Read())
            {
                cauTraLoiCuaHV = readerCompareResult.GetString(0).Trim();
            }
            readerCompareResult.Close();
            //MessageBox.Show($"{cauTraLoiCuaHV}");
            if (cauTraLoiCuaHV != cauTraLoiDung)
            {
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 0)
                {
                    lblNote.Text = "Ghi chú: Học viên không chọn đáp án cho câu này";
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 1)
                {
                    rdCauA.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 2)
                {
                    rdCauB.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 3)
                {
                    rdCauC.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 4)
                {
                    rdCauD.ForeColor = Color.Red;
                }
            }
            //
            if (countButton == 1)
                {
                    btnBack.Enabled = false;
                    btnBack.BackColor = Color.CadetBlue;

                }
        }

        bool keyNext = false;
        int indexNext = 0;
        int countButton = 1;
        private void btnNext_Click(object sender, EventArgs e)
        {
            lblNote.Text = "Ghi chú:";
            rdCauA.ForeColor = SystemColors.ControlText;
            rdCauB.ForeColor = SystemColors.ControlText;
            rdCauC.ForeColor = SystemColors.ControlText;
            rdCauD.ForeColor = SystemColors.ControlText;
            lblSLCauHoi.Text = $"Câu hỏi số {countButton + 1} / {countSoCauHoi}";
            grBox.Text = $"Câu hỏi số {countButton + 1}";
            if (keyNext == false)
            {
                indexNext = countButton;
                countButton++;

            }
            else if (keyNext)
            {
                countButton++;
                indexNext = countButton;
            }

            string sqlShowCauHoi = $"select * from cacCauHoi where MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = {countButton}))";

            SqlCommand sqlCmdShowCauHoi = new SqlCommand(sqlShowCauHoi);
            sqlCmdShowCauHoi.Connection = connection;
            SqlDataReader readerShowCauHoi = sqlCmdShowCauHoi.ExecuteReader();
            while (readerShowCauHoi.Read())
            {

                cauHoi.MaCauHoi = readerShowCauHoi.GetString(0).Trim();
                cauHoi.NoiDungCauHoi = readerShowCauHoi.GetString(1);
                cauHoi.PanA = readerShowCauHoi.GetString(2);
                cauHoi.PanB = readerShowCauHoi.GetString(3);
                cauHoi.PanC = readerShowCauHoi.GetString(4);
                cauHoi.PanD = readerShowCauHoi.GetString(5);
                cauHoi.Answer = readerShowCauHoi.GetString(6).Trim();

                cauHoi.MaChuong = readerShowCauHoi.GetString(7).Trim();

                if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "A")
                {
                    rdCauA.ForeColor = Color.Green;
                    cauTraLoiDung = "1";
                }
                if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "B")
                {
                    rdCauB.ForeColor = Color.Green;
                    cauTraLoiDung = "2";
                }
                if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "C")
                {
                    rdCauC.ForeColor = Color.Green;
                    cauTraLoiDung = "3";
                }
                if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "D")
                {
                    rdCauD.ForeColor = Color.Green;
                    cauTraLoiDung = "4";
                }
                txtCauHoi.Text = "[" + cauHoi.MaCauHoi + "] " + cauHoi.NoiDungCauHoi;
                rdCauA.Text = cauHoi.PanA;
                rdCauB.Text = cauHoi.PanB;
                rdCauC.Text = cauHoi.PanC;
                rdCauD.Text = cauHoi.PanD;
            }
            readerShowCauHoi.Close();
            if (countButton == countSoCauHoi)
            {
                  btnNext.Enabled = false;
                  btnNext.BackColor = Color.CadetBlue;

            }

            //so sánh đối chiếu kết quả
            string sqlCompareResult =
                $"select CauTraLoiCuaHV from CT_KETQUA where IDKETQUA = '{txtMaDe.Text.Trim() + "_" + txtMaHV.Text.Trim()}' and MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = {countButton}))";
            SqlCommand sqlCompareResultCmd = new SqlCommand(sqlCompareResult);
            sqlCompareResultCmd.Connection = connection;
            SqlDataReader readerCompareResult = sqlCompareResultCmd.ExecuteReader();
            while (readerCompareResult.Read())
            {
                cauTraLoiCuaHV = readerCompareResult.GetString(0).Trim();
            }
            readerCompareResult.Close();

            if (cauTraLoiCuaHV != cauTraLoiDung)
            {
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 0)
                {
                    lblNote.Text = "Ghi chú: Học viên không chọn đáp án cho câu này";
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 1)
                {
                    rdCauA.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 2)
                {
                    rdCauB.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 3)
                {
                    rdCauC.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 4)
                {
                    rdCauD.ForeColor = Color.Red;
                }
            }
            
            btnBack.Enabled = true;
            btnBack.BackColor = Color.Teal;

        }

        int countSoCauHoi = 0;
        string cauTraLoiCuaHV = "";
        string cauTraLoiDung = "";

        private void XemKetQuaHV_Load(object sender, EventArgs e)
        {

            txtHoTen.Text = hoTen;
            txtMaHV.Text = maHV;
            txtMaLop.Text = maLop;
            txtMaDe.Text = maDe;
            txtMonHoc.Text = monHoc;
            lblKiemTra.Text += monHoc;
            cauHoi = new CauHoi();
            if (connection == null)
                connection = new SqlConnection(strConnect);
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            //Get time
            string sqlGetTime = $"select TGLAMBAI from DETHI_QLADMIN where MADETHI = '{txtMaDe.Text.Trim()}'";
            SqlCommand sqlGetTimeCmd = new SqlCommand(sqlGetTime);
            sqlGetTimeCmd.Connection = connection;
            SqlDataReader reader = sqlGetTimeCmd.ExecuteReader();
            while (reader.Read())
            {

                txtTime.Text = reader.GetString(0).Trim() + " phút";
            }
            reader.Close();

            // Đếm trong đề thi có bao nhiêu câu hỏi
            string sqlCountSoCauHoi = $"select count (macauhoi) from CHITIET_DETHI where MADETHI = '{txtMaDe.Text.Trim()}'";
            SqlCommand sqlCmdCoutSoCauHoi = new SqlCommand(sqlCountSoCauHoi);
            sqlCmdCoutSoCauHoi.Connection = connection;
            SqlDataReader readerCountSoCauHoi = sqlCmdCoutSoCauHoi.ExecuteReader();
            while (readerCountSoCauHoi.Read())
            {
                countSoCauHoi = readerCountSoCauHoi.GetInt32(0);
            }
            readerCountSoCauHoi.Close();


            //Get data tổng điểm
            double Mark = 0;
            string sqlGetMark = $"select TONGSODIEM from KETQUA where MAHV = '{txtMaHV.Text.Trim()}' and MADETHI = '{txtMaDe.Text.Trim()}'";
            SqlCommand sqlGetMarkCmd = new SqlCommand(sqlGetMark);
            sqlGetMarkCmd.Connection = connection;
            SqlDataReader readerGetMark= sqlGetMarkCmd.ExecuteReader();
            while (readerGetMark.Read())
            {
                Mark = readerGetMark.GetDouble(0);
                lblMark.Text += Mark.ToString().Trim();
            }
            readerGetMark.Close();
            float soCauTraLoiDung = (float)((Mark / 10) * countSoCauHoi);
            lblCauTraLoiDung.Text += soCauTraLoiDung.ToString() + "/" + countSoCauHoi.ToString();

            //show cau hoi len form
            string sqlShowCauHoi = $"select * from cacCauHoi where MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = 1))";
            SqlCommand sqlCmdShowCauHoi = new SqlCommand(sqlShowCauHoi);
            sqlCmdShowCauHoi.Connection = connection;
            SqlDataReader readerShowCauHoi = sqlCmdShowCauHoi.ExecuteReader();
            while (readerShowCauHoi.Read())
            {
                cauHoi.MaCauHoi = readerShowCauHoi.GetString(0).Trim();
                cauHoi.NoiDungCauHoi = readerShowCauHoi.GetString(1);
                cauHoi.PanA = readerShowCauHoi.GetString(2);
                cauHoi.PanB = readerShowCauHoi.GetString(3);
                cauHoi.PanC = readerShowCauHoi.GetString(4);
                cauHoi.PanD = readerShowCauHoi.GetString(5);
                cauHoi.Answer = readerShowCauHoi.GetString(6).Trim();
                cauHoi.MaChuong = readerShowCauHoi.GetString(7).Trim();

                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "A")
                    {
                        rdCauA.ForeColor = Color.Green;
                        cauTraLoiDung = "1";
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "B")
                    {
                        rdCauB.ForeColor = Color.Green;
                        cauTraLoiDung = "2";
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "C")
                    {
                        rdCauC.ForeColor = Color.Green;
                        cauTraLoiDung = "3";
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "D")
                    {
                        rdCauD.ForeColor = Color.Green;
                        cauTraLoiDung = "4";
                    }


                txtCauHoi.Text = "[" + cauHoi.MaCauHoi + "] " + cauHoi.NoiDungCauHoi;
                rdCauA.Text = cauHoi.PanA;
                rdCauB.Text = cauHoi.PanB;
                rdCauC.Text = cauHoi.PanC;
                rdCauD.Text = cauHoi.PanD;
            }
            readerShowCauHoi.Close();

            //so sánh đối chiếu kết quả
            string sqlCompareResult = 
                $"select CauTraLoiCuaHV from CT_KETQUA where IDKETQUA = '{txtMaDe.Text.Trim() + "_" + txtMaHV.Text.Trim()}' and MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = 1))";
            SqlCommand sqlCompareResultCmd = new SqlCommand(sqlCompareResult);
            sqlCompareResultCmd.Connection = connection;
            SqlDataReader readerCompareResult = sqlCompareResultCmd.ExecuteReader();
            while(readerCompareResult.Read())
            {
                cauTraLoiCuaHV = readerCompareResult.GetString(0).Trim();
            }
            readerCompareResult.Close();

            if (cauTraLoiCuaHV != cauTraLoiDung)
            {
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 0)
                {
                    lblNote.Text = "Ghi chú: Học viên không chọn đáp án cho câu này";
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 1)
                {
                    rdCauA.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 2)
                {
                    rdCauB.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 3)
                {
                    rdCauC.ForeColor = Color.Red;
                }
                if (int.Parse(cauTraLoiCuaHV.ToString()) == 4)
                {
                    rdCauD.ForeColor = Color.Red;
                }
            }
            lblSLCauHoi.Text = $"Câu hỏi số: 1 / {countSoCauHoi}";

        }

        private void rdCauA_Click(object sender, EventArgs e)
        {
            rdCauA.Checked = false;
            rdCauB.Checked = false;
            rdCauC.Checked = false;
            rdCauD.Checked = false;
        }

        private void rdCauA_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
