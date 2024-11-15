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

namespace LoginDemo.XDForms
{
    public partial class VaoThi : Form
    {
        string strConnect = @"Data Source=DESKTOP-44BDVUN\SQLEXPRESS;Initial Catalog=QLThiTest04;Integrated Security=True";
        SqlConnection connection = null;
        CauHoi cauHoi = null;

        public string maHV;
        public string hoTen;
        public string maLop;
        public string monHoc;
        public string maDe;

        public VaoThi()
        {
            InitializeComponent();
        }
        int countSoCauHoi = 0;

        //Add time count
        Timer MyTimer;
        DateTime endTime;


        // Update the label with the remaining time in the timer tick event
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainingTime = endTime - DateTime.Now;
            if (remainingTime <= TimeSpan.Zero)
            {
                // Time is up
                lblTimeCount.Text = "Hết giờ!";
                MyTimer.Stop();
                TimeIsUp();
            }
            else
            {
                // Format the remaining time as hh:mm:ss
                lblTimeCount.Text = remainingTime.ToString("hh\\:mm\\:ss");
            }
        }

        void TimeIsUp()
        {

            txtCauHoi.Visible = false;
            rdCauA.Visible = false;
            rdCauB.Visible = false;
            rdCauC.Visible = false;
            rdCauD.Visible = false;
            btnNext.Visible = false;
            btnBack.Visible = false;
            if (rdCauA.Checked)
            {
                SelectedAnswer[j, indexNext+1] = 1;
            }
            if (rdCauB.Checked)
            {
                SelectedAnswer[j, indexNext+1] = 2;
            }
            if (rdCauC.Checked)
            {
                SelectedAnswer[j, indexNext + 1] = 3;
            }
            if (rdCauD.Checked)
            {
                SelectedAnswer[j, indexNext + 1] = 4;
            }
            //btnExit.Visible = true;
            //txtCheck.Visible = true;

            float result = 0;

            for (int checkCauHoi = 1; checkCauHoi <= countSoCauHoi; checkCauHoi++)
            {

                string sqlShowCauHoi = $"select * from cacCauHoi where MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = {checkCauHoi}))";
                SqlCommand sqlCmdShowCauHoi = new SqlCommand(sqlShowCauHoi);
                sqlCmdShowCauHoi.Connection = connection;
                SqlDataReader readerShowCauHoi = sqlCmdShowCauHoi.ExecuteReader();
                while (readerShowCauHoi.Read())
                {

                    cauHoi.Answer = readerShowCauHoi.GetString(6).Trim();

                    if (CorrectAnswer[1, checkCauHoi] < 1)
                    {
                        if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "A")
                        {
                            CorrectAnswer[1, checkCauHoi] = 1;
                        }
                        if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "B")
                        {
                            CorrectAnswer[1, checkCauHoi] = 2;
                        }
                        if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "C")
                        {
                            CorrectAnswer[1, checkCauHoi] = 3;
                        }
                        if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "D")
                        {
                            CorrectAnswer[1, checkCauHoi] = 4;
                        }
                        //MessageBox.Show($"[" + cauHoi.MaCauHoi + "] " + CorrectAnswer[1, 1]);
                        //txtCheck.Text += "[" + cauHoi.MaCauHoi + "] " + $"CorrectAnswer[{1},{1}]: " + CorrectAnswer[1, 1];
                    }
                }
                readerShowCauHoi.Close();
                if (CorrectAnswer[1, checkCauHoi] == SelectedAnswer[1, checkCauHoi])
                {
                    result += 1;
                }
            }
            float tongDiem = (float)(result / countSoCauHoi * 10);

            //Xử lý xuống database kết quả
            string sqlInsertKetQua = "INSERT INTO KETQUA(IDKETQUA, MADETHI, MAHV, TONGSODIEM) VALUES (@idketqua, @madethi, @mahv, @diem)";

            SqlParameter prID = new SqlParameter("@idketqua", SqlDbType.Char);
            SqlParameter prMaDeThi = new SqlParameter("@madethi", SqlDbType.Char);
            SqlParameter prMaHV = new SqlParameter("@mahv", SqlDbType.Char);
            SqlParameter prDiem = new SqlParameter("@diem", SqlDbType.Float);
            prID.Value = txtMaDe.Text.Trim() + "_" + txtMaHV.Text;
            prMaDeThi.Value = txtMaDe.Text.Trim();
            prMaHV.Value = txtMaHV.Text.Trim();
            prDiem.Value = tongDiem;
            SqlCommand sqlCmd = new SqlCommand(sqlInsertKetQua);
            sqlCmd.Connection = connection;
            sqlCmd.Parameters.Add(prID);
            sqlCmd.Parameters.Add(prMaDeThi);
            sqlCmd.Parameters.Add(prMaHV);
            sqlCmd.Parameters.Add(prDiem);
            if (sqlCmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Nộp bài thành công!", "Thông báo");
                //return;
            }
            else
            {
                MessageBox.Show("Nộp bài thất bại!", "Thông báo");
                //return;
            }

            string[] maCauHoi = new string[countSoCauHoi];
            string[] cauTraLoi = new string[countSoCauHoi];
            for (int i = 1; i <= countSoCauHoi; i++)
            {
                string sqlGetCauHoi = $"select MaCauHoi,CauTraLoi from cacCauHoi where MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = {i}))";
                SqlCommand sqlGetCauHoiCmd = new SqlCommand(sqlGetCauHoi);
                sqlGetCauHoiCmd.Connection = connection;
                SqlDataReader readerGetCH = sqlGetCauHoiCmd.ExecuteReader();
                while (readerGetCH.Read())
                {
                    maCauHoi[i - 1] = readerGetCH.GetString(0);
                    cauTraLoi[i - 1] = readerGetCH.GetString(1);
                }
                readerGetCH.Close();
                //MessageBox.Show($"{maCauHoi[i - 1]}");
                //MessageBox.Show($"{cauTraLoi[i - 1]}");
                string str = "insert into CT_KETQUA(IDKETQUA, MACAUHOI, CAUTRALOI, CauTraLoiCuaHV) values(@idketqua_CT, @macauhoi, @cautraloi, @HVTraLoi)";
                SqlParameter prID_CT = new SqlParameter("@idketqua_CT", SqlDbType.Char, 20); // -OK
                SqlParameter prMaCauHoi = new SqlParameter("@macauhoi", SqlDbType.Char, 10);
                SqlParameter prCauTraLoi = new SqlParameter("@cautraloi", SqlDbType.NVarChar, 200);
                SqlParameter prHVTraLoi = new SqlParameter("@HVTraLoi", SqlDbType.Char, 10);

                prID_CT.Value = txtMaDe.Text.Trim() + "_" + txtMaHV.Text; // - OK
                prMaCauHoi.Value = maCauHoi[i - 1].Trim();
                prCauTraLoi.Value = cauTraLoi[i - 1].Trim();
                prHVTraLoi.Value = SelectedAnswer[1, i].ToString().Trim();
                SqlCommand sqlCmdCTKQ = new SqlCommand(str);
                sqlCmdCTKQ.Connection = connection;
                sqlCmdCTKQ.Parameters.Add(prID_CT);
                sqlCmdCTKQ.Parameters.Add(prMaCauHoi);
                sqlCmdCTKQ.Parameters.Add(prCauTraLoi);
                sqlCmdCTKQ.Parameters.Add(prHVTraLoi);

                if (sqlCmdCTKQ.ExecuteNonQuery() > 0)
                {

                }
            }
            //insert into CT_KETQUA(IDKETQUA, MACAUHOI, CAUTRALOI) values()
            this.Close();
        }

        private void VaoThi_Load(object sender, EventArgs e)
        {

            btnBack.BackColor = Color.CadetBlue;


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
            //string;

            //Get time
            int TimeCountDown = 0;
            string sqlGetTime = $"select TGLAMBAI from DETHI_QLADMIN where MADETHI = '{txtMaDe.Text.Trim()}'";
            SqlCommand sqlGetTimeCmd = new SqlCommand(sqlGetTime);
            sqlGetTimeCmd.Connection = connection;
            SqlDataReader reader = sqlGetTimeCmd.ExecuteReader();
            while(reader.Read())
            {
                
                TimeCountDown = int.Parse(reader.GetString(0).Trim());
                txtTime.Text = TimeCountDown.ToString().Trim() + " phút";
            }
            reader.Close();

            //Add time count

            {
                MyTimer = new Timer();
                MyTimer.Interval = 1000; // 1 second
                MyTimer.Tick += new EventHandler(MyTimer_Tick);
                endTime = DateTime.Now.AddMinutes(TimeCountDown); // 3 minutes from now
                MyTimer.Start();
            }

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

                if (CorrectAnswer[1, 1] < 1)
                {
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "A")
                    {
                        CorrectAnswer[1, 1] = 1;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "B")
                    {
                        CorrectAnswer[1, 1] = 2;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "C")
                    {
                        CorrectAnswer[1, 1] = 3;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "D")
                    {
                        CorrectAnswer[1, 1] = 4;
                    }

                }

                txtCauHoi.Text = "[" + cauHoi.MaCauHoi + "] " + cauHoi.NoiDungCauHoi;
                rdCauA.Text = cauHoi.PanA;
                rdCauB.Text = cauHoi.PanB;
                rdCauC.Text = cauHoi.PanC;
                rdCauD.Text = cauHoi.PanD;
            }
            readerShowCauHoi.Close();
            lblSLCauHoi.Text = $"Số lượng câu hỏi: 1 / {countSoCauHoi}";

        }

        int[,] SelectedAnswer = new int[2, 50]; // Fix thanh countSoChuong, countSoCauHoi -> Global
        int i = 1;
        bool keyNext = false;
        int indexNext = 0;
        int j = 1;
        bool toggleFinish = false;
        int countButton = 1;
        int[,] CorrectAnswer = new int[2, 50]; // Tạo biến lưu đáp án đúng của câu hỏi


        private void btnNext_Click(object sender, EventArgs e)
        {
            
            lblSLCauHoi.Text = $"Số lượng câu hỏi: {countButton + 1} / {countSoCauHoi}";
            grBox.Text = $"Câu hỏi số {countButton + 1}";

            if (countButton == countSoCauHoi)
            {
                grBox.Text = $"Câu hỏi số {countSoCauHoi}";
                lblSLCauHoi.Text = $"Số lượng câu hỏi: {countButton} / {countSoCauHoi}";
                
            }
            //if (toggleFinish)
            //{
            //    {
            //        if (indexNext + 1 == countSoCauHoi)
            //        {
            //            if (rdCauA.Checked)
            //            {
            //                SelectedAnswer[j, indexNext + 1] = 1;
            //            }
            //            if (rdCauB.Checked)
            //            {
            //                SelectedAnswer[j, indexNext + 1] = 2;
            //            }
            //            if (rdCauC.Checked)
            //            {
            //                SelectedAnswer[j, indexNext + 1] = 3;
            //            }
            //            if (rdCauD.Checked)
            //            {
            //                SelectedAnswer[j, indexNext + 1] = 4;
            //            }
            //        }
            //    }

            //    txtCauHoi.Visible = false;
            //    rdCauA.Visible = false;
            //    rdCauB.Visible = false;
            //    rdCauC.Visible = false;
            //    rdCauD.Visible = false;
            //    btnNext.Visible = false;
            //    btnBack.Visible = false;


            //    float result = 0;

            //        for (int checkCauHoi = 1; checkCauHoi <= countSoCauHoi; checkCauHoi++)
            //        {
            //            if (CorrectAnswer[1, checkCauHoi] == SelectedAnswer[1, checkCauHoi])
            //                {
            //                    result += 1;
            //                }
            //        }
            //    float tongDiem = (float)(result / countSoCauHoi * 10);

            //    //Xử lý xuống database kết quả
            //    string sqlInsertKetQua = "INSERT INTO KETQUA(IDKETQUA, MADETHI, MAHV, TONGSODIEM) VALUES (@idketqua, @madethi, @mahv, @diem)";

            //    SqlParameter prID = new SqlParameter("@idketqua", SqlDbType.Char);
            //    SqlParameter prMaDeThi = new SqlParameter("@madethi", SqlDbType.Char);
            //    SqlParameter prMaHV = new SqlParameter("@mahv", SqlDbType.Char);
            //    SqlParameter prDiem = new SqlParameter("@diem", SqlDbType.Float);
            //    prID.Value = txtMaDe.Text.Trim() + "_" + txtMaHV.Text;
            //    prMaDeThi.Value = txtMaDe.Text.Trim();
            //    prMaHV.Value = txtMaHV.Text.Trim();
            //    prDiem.Value = tongDiem;
            //    SqlCommand sqlCmd = new SqlCommand(sqlInsertKetQua);
            //    sqlCmd.Connection = connection;
            //    sqlCmd.Parameters.Add(prID);
            //    sqlCmd.Parameters.Add(prMaDeThi);
            //    sqlCmd.Parameters.Add(prMaHV);
            //    sqlCmd.Parameters.Add(prDiem);
            //    if (sqlCmd.ExecuteNonQuery() > 0)
            //    {
            //        MessageBox.Show("Nộp bài thành công!", "Thông báo");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Nộp bài thất bại!", "Thông báo");
            //    }

            //    string[] maCauHoi = new string[countSoCauHoi];
            //    string[] cauTraLoi = new string[countSoCauHoi];
            //    for(int i=1; i<=countSoCauHoi; i++)
            //    {
            //        string sqlGetCauHoi = $"select MaCauHoi,CauTraLoi from cacCauHoi where MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = {i}))";
            //        SqlCommand sqlGetCauHoiCmd = new SqlCommand(sqlGetCauHoi);
            //        sqlGetCauHoiCmd.Connection= connection;
            //        SqlDataReader readerGetCH = sqlGetCauHoiCmd.ExecuteReader();
            //        while (readerGetCH.Read())
            //        {
            //            maCauHoi[i - 1] = readerGetCH.GetString(0);
            //            cauTraLoi[i-1] = readerGetCH.GetString(1);
            //        }
            //        readerGetCH.Close();

            //        string str = "insert into CT_KETQUA(IDKETQUA, MACAUHOI, CAUTRALOI, CauTraLoiCuaHV) values(@idketqua_CT, @macauhoi, @cautraloi, @HVTraLoi)";
            //        SqlParameter prID_CT = new SqlParameter("@idketqua_CT", SqlDbType.Char, 20); // -OK
            //        SqlParameter prMaCauHoi = new SqlParameter("@macauhoi", SqlDbType.Char, 10);
            //        SqlParameter prCauTraLoi = new SqlParameter("@cautraloi", SqlDbType.NVarChar, 200);
            //        SqlParameter prHVTraLoi = new SqlParameter("@HVTraLoi", SqlDbType.Char, 10);

            //        prID_CT.Value = txtMaDe.Text.Trim() + "_" + txtMaHV.Text; // - OK
            //        prMaCauHoi.Value = maCauHoi[i - 1].Trim();
            //        prCauTraLoi.Value = cauTraLoi[i - 1].Trim();
            //        prHVTraLoi.Value = SelectedAnswer[1, i].ToString().Trim();
            //        SqlCommand sqlCmdCTKQ = new SqlCommand(str);
            //        sqlCmdCTKQ.Connection = connection;
            //        sqlCmdCTKQ.Parameters.Add(prID_CT);
            //        sqlCmdCTKQ.Parameters.Add(prMaCauHoi);
            //        sqlCmdCTKQ.Parameters.Add(prCauTraLoi);
            //        sqlCmdCTKQ.Parameters.Add(prHVTraLoi);

            //        if (sqlCmdCTKQ.ExecuteNonQuery() > 0)
            //        {

            //        }
            //    }
            //    this.Close();



            //}
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

            if (countButton > countSoCauHoi+1)
            {
                countButton = 1;
            }
            //

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

                if (CorrectAnswer[i, countButton] < 1)
                {
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "A")
                    {
                        CorrectAnswer[i, countButton] = 1;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "B")
                    {
                        CorrectAnswer[i, countButton] = 2;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "C")
                    {
                        CorrectAnswer[i, countButton] = 3;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "D")
                    {
                        CorrectAnswer[i, countButton] = 4;
                    }
                }
                txtCauHoi.Text = "[" + cauHoi.MaCauHoi + "] " + cauHoi.NoiDungCauHoi;
                rdCauA.Text = cauHoi.PanA;
                rdCauB.Text = cauHoi.PanB;
                rdCauC.Text = cauHoi.PanC;
                rdCauD.Text = cauHoi.PanD;
            }
            readerShowCauHoi.Close();
            if(countButton == countSoCauHoi)
            {
                btnNext.Text = "Nộp bài";

            }
            if ( countButton == countSoCauHoi+1)
            {
                countButton = countSoCauHoi;
                DialogResult resulta = MessageBox.Show($"Bạn có chắn chắc muốn nộp bài?", "Thông báo", MessageBoxButtons.OKCancel);
                if (resulta == DialogResult.OK)

                {
                    {
                        if (indexNext + 1 == countSoCauHoi)
                        {
                            if (rdCauA.Checked)
                            {
                                SelectedAnswer[j, indexNext + 1] = 1;
                            }
                            if (rdCauB.Checked)
                            {
                                SelectedAnswer[j, indexNext + 1] = 2;
                            }
                            if (rdCauC.Checked)
                            {
                                SelectedAnswer[j, indexNext + 1] = 3;
                            }
                            if (rdCauD.Checked)
                            {
                                SelectedAnswer[j, indexNext + 1] = 4;
                            }
                        }
                    }
                    {
                        if (indexNext + 1 > countSoCauHoi)
                        {
                            {
                                if (rdCauA.Checked)
                                {
                                    SelectedAnswer[j, indexNext] = 1;
                                }
                                if (rdCauB.Checked)
                                {
                                    SelectedAnswer[j, indexNext] = 2;
                                }
                                if (rdCauC.Checked)
                                {
                                    SelectedAnswer[j, indexNext] = 3;
                                }
                                if (rdCauD.Checked)
                                {
                                    SelectedAnswer[j, indexNext] = 4;
                                }

                            }
                        }
                        else if (indexNext < countSoCauHoi)
                        {
                            if (rdCauA.Checked)
                            {
                                SelectedAnswer[j, indexNext] = 1;
                            }
                            if (rdCauB.Checked)
                            {
                                SelectedAnswer[j, indexNext] = 2;
                            }
                            if (rdCauC.Checked)
                            {
                                SelectedAnswer[j, indexNext] = 3;
                            }
                            if (rdCauD.Checked)
                            {
                                SelectedAnswer[j, indexNext] = 4;
                            }
                            if (SelectedAnswer[j, indexNext + 1] < 1)
                            {
                                rdCauA.Checked = false;
                                rdCauB.Checked = false;
                                rdCauC.Checked = false;
                                rdCauD.Checked = false;
                            }
                        }
                    }
                    if (indexNext < countSoCauHoi)
                    {
                        if (SelectedAnswer[j, indexNext + 1] == 1)
                        {
                            rdCauA.Checked = true;
                        }
                        if (SelectedAnswer[j, indexNext + 1] == 2)
                        {
                            rdCauB.Checked = true;
                        }
                        if (SelectedAnswer[j, indexNext + 1] == 3)
                        {
                            rdCauC.Checked = true;
                        }
                        if (SelectedAnswer[j, indexNext + 1] == 4)
                        {
                            rdCauD.Checked = true;
                        }

                    }

                    txtCauHoi.Visible = false;
                    rdCauA.Visible = false;
                    rdCauB.Visible = false;
                    rdCauC.Visible = false;
                    rdCauD.Visible = false;
                    btnNext.Visible = false;
                    btnBack.Visible = false;


                    float result = 0;

                    for (int checkCauHoi = 1; checkCauHoi <= countSoCauHoi; checkCauHoi++)
                    {
                        if (CorrectAnswer[1, checkCauHoi] == SelectedAnswer[1, checkCauHoi])
                        {
                            result += 1;
                        }
                    }
                    float tongDiem = (float)(result / countSoCauHoi * 10);

                    //Xử lý xuống database kết quả
                    string sqlInsertKetQua = "INSERT INTO KETQUA(IDKETQUA, MADETHI, MAHV, TONGSODIEM) VALUES (@idketqua, @madethi, @mahv, @diem)";

                    SqlParameter prID = new SqlParameter("@idketqua", SqlDbType.Char);
                    SqlParameter prMaDeThi = new SqlParameter("@madethi", SqlDbType.Char);
                    SqlParameter prMaHV = new SqlParameter("@mahv", SqlDbType.Char);
                    SqlParameter prDiem = new SqlParameter("@diem", SqlDbType.Float);
                    prID.Value = txtMaDe.Text.Trim() + "_" + txtMaHV.Text;
                    prMaDeThi.Value = txtMaDe.Text.Trim();
                    prMaHV.Value = txtMaHV.Text.Trim();
                    prDiem.Value = tongDiem;
                    SqlCommand sqlCmd = new SqlCommand(sqlInsertKetQua);
                    sqlCmd.Connection = connection;
                    sqlCmd.Parameters.Add(prID);
                    sqlCmd.Parameters.Add(prMaDeThi);
                    sqlCmd.Parameters.Add(prMaHV);
                    sqlCmd.Parameters.Add(prDiem);
                    if (sqlCmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Nộp bài thành công!", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Nộp bài thất bại!", "Thông báo");
                    }

                    string[] maCauHoi = new string[countSoCauHoi];
                    string[] cauTraLoi = new string[countSoCauHoi];
                    for (int i = 1; i <= countSoCauHoi; i++)
                    {
                        string sqlGetCauHoi = $"select MaCauHoi,CauTraLoi from cacCauHoi where MaCauHoi = ((SELECT MaCauHoi FROM (SELECT MaCauHoi, ROW_NUMBER() OVER (ORDER BY MaCauHoi) AS RowNum FROM CHITIET_DETHI WHERE CHITIET_DETHI.MADETHI = '{txtMaDe.Text.Trim()}') AS T WHERE RowNum = {i}))";
                        SqlCommand sqlGetCauHoiCmd = new SqlCommand(sqlGetCauHoi);
                        sqlGetCauHoiCmd.Connection = connection;
                        SqlDataReader readerGetCH = sqlGetCauHoiCmd.ExecuteReader();
                        while (readerGetCH.Read())
                        {
                            maCauHoi[i - 1] = readerGetCH.GetString(0);
                            cauTraLoi[i - 1] = readerGetCH.GetString(1);
                        }
                        readerGetCH.Close();

                        string str = "insert into CT_KETQUA(IDKETQUA, MACAUHOI, CAUTRALOI, CauTraLoiCuaHV) values(@idketqua_CT, @macauhoi, @cautraloi, @HVTraLoi)";
                        SqlParameter prID_CT = new SqlParameter("@idketqua_CT", SqlDbType.Char, 20); // -OK
                        SqlParameter prMaCauHoi = new SqlParameter("@macauhoi", SqlDbType.Char, 10);
                        SqlParameter prCauTraLoi = new SqlParameter("@cautraloi", SqlDbType.NVarChar, 200);
                        SqlParameter prHVTraLoi = new SqlParameter("@HVTraLoi", SqlDbType.Char, 10);

                        prID_CT.Value = txtMaDe.Text.Trim() + "_" + txtMaHV.Text; // - OK
                        prMaCauHoi.Value = maCauHoi[i - 1].Trim();
                        prCauTraLoi.Value = cauTraLoi[i - 1].Trim();
                        prHVTraLoi.Value = SelectedAnswer[1, i].ToString().Trim();
                        SqlCommand sqlCmdCTKQ = new SqlCommand(str);
                        sqlCmdCTKQ.Connection = connection;
                        sqlCmdCTKQ.Parameters.Add(prID_CT);
                        sqlCmdCTKQ.Parameters.Add(prMaCauHoi);
                        sqlCmdCTKQ.Parameters.Add(prCauTraLoi);
                        sqlCmdCTKQ.Parameters.Add(prHVTraLoi);

                        if (sqlCmdCTKQ.ExecuteNonQuery() > 0)
                        {

                        }
                    }
                    this.Close();
                    
                } 
                else 
                {

                }
            }
            {
                if (indexNext + 1 > countSoCauHoi)
                {
                    {
                        if (rdCauA.Checked)
                        {
                            SelectedAnswer[j, indexNext] = 1;
                        }
                        if (rdCauB.Checked)
                        {
                            SelectedAnswer[j, indexNext] = 2;
                        }
                        if (rdCauC.Checked)
                        {
                            SelectedAnswer[j, indexNext] = 3;
                        }
                        if (rdCauD.Checked)
                        {
                            SelectedAnswer[j, indexNext] = 4;
                        }

                    }
                }
                else if (indexNext < countSoCauHoi)
                {
                    if (rdCauA.Checked)
                    {
                        SelectedAnswer[j, indexNext] = 1;
                    }
                    if (rdCauB.Checked)
                    {
                        SelectedAnswer[j, indexNext] = 2;
                    }
                    if (rdCauC.Checked)
                    {
                        SelectedAnswer[j, indexNext] = 3;
                    }
                    if (rdCauD.Checked)
                    {
                        SelectedAnswer[j, indexNext] = 4;
                    }
                    if (SelectedAnswer[j, indexNext + 1] < 1)
                    {
                        rdCauA.Checked = false;
                        rdCauB.Checked = false;
                        rdCauC.Checked = false;
                        rdCauD.Checked = false;
                    }
                }
            }
            if (indexNext < countSoCauHoi)
            {
                if (SelectedAnswer[j, indexNext + 1] == 1)
                {
                    rdCauA.Checked = true;
                }
                if (SelectedAnswer[j, indexNext + 1] == 2)
                {
                    rdCauB.Checked = true;
                }
                if (SelectedAnswer[j, indexNext + 1] == 3)
                {
                    rdCauC.Checked = true;
                }
                if (SelectedAnswer[j, indexNext + 1] == 4)
                {
                    rdCauD.Checked = true;
                }

            }

            btnBack.Enabled = true;
            btnBack.BackColor = Color.Teal;

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            
            btnNext.Text = "Tiếp";

            lblSLCauHoi.Text = $"Số lượng câu hỏi: {countButton - 1} / {countSoCauHoi}";
            grBox.Text = $"Câu hỏi số {countButton-1}";

            if (toggleFinish)
            {
                btnNext.Text = "Tiếp";
                toggleFinish = false;
            }

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
            if (countButton < 1)
            {
                countButton = countSoCauHoi;
            }
            if (indexNext < 1)
            {
                indexNext = countButton;
            }
            //
            //if (checkFlag)
            //{
            //    i--;
            //    j = i;

            //}
            //if (i < 1)
            //{
            //    i = 1;
            //    countButton = 1;
            //    j = i;
            //}
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
                //MessageBox.Show(cauHoi.MaChuong);
                //MessageBox.Show(cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1));
                if (CorrectAnswer[i, countButton] < 1)
                {
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "A")
                    {
                        CorrectAnswer[i, countButton] = 1;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "B")
                    {
                        CorrectAnswer[i, countButton] = 2;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "C")
                    {
                        CorrectAnswer[i, countButton] = 3;
                    }
                    if (cauHoi.Answer.Substring(cauHoi.Answer.Length - 1, 1) == "D")
                    {
                        CorrectAnswer[i, countButton] = 4;
                    }
                    //txtCheck.Text += "[" + cauHoi.MaCauHoi + "] " + CorrectAnswer[i, countButton];
                }
                txtCauHoi.Text = "[" + cauHoi.MaCauHoi + "] " + cauHoi.NoiDungCauHoi;
                rdCauA.Text = cauHoi.PanA;
                rdCauB.Text = cauHoi.PanB;
                rdCauC.Text = cauHoi.PanC;
                rdCauD.Text = cauHoi.PanD;
            }
            readerShowCauHoi.Close();

            //
            {
                //if (indexNext + 1 > countSoCauHoi)
                //{
                //    {
                //        if (rdCauA.Checked)
                //        {
                //            SelectedAnswer[j + 1, 1] = 1;
                //        }
                //        if (rdCauB.Checked)
                //        {
                //            SelectedAnswer[j + 1, 1] = 2;
                //        }
                //        if (rdCauC.Checked)
                //        {
                //            SelectedAnswer[j + 1, 1] = 3;
                //        }
                //        if (rdCauD.Checked)
                //        {
                //            SelectedAnswer[j + 1, 1] = 4;
                //        }
                //        //txtCheck.Text += $"[j:{j+1 }, {1}]: " + SelectedAnswer[j+1 , 1].ToString();

                //    }
                if (indexNext < countSoCauHoi)
                {
                    //MessageBox.Show("3");
                    if (rdCauA.Checked)
                    {
                        SelectedAnswer[j, indexNext + 1] = 1;
                    }
                    if (rdCauB.Checked)
                    {
                        SelectedAnswer[j, indexNext + 1] = 2;
                    }
                    if (rdCauC.Checked)
                    {
                        SelectedAnswer[j, indexNext + 1] = 3;
                    }
                    if (rdCauD.Checked)
                    {
                        SelectedAnswer[j, indexNext + 1] = 4;
                    }
                    //txtCheck.Text += $"[j:{j}, {indexNext + 1}]: " + SelectedAnswer[j, indexNext + 1].ToString();

                }
            }
            if (SelectedAnswer[j, indexNext] < 1)
            {
                rdCauA.Checked = false;
                rdCauB.Checked = false;
                rdCauC.Checked = false;
                rdCauD.Checked = false;
            }
            //
            if (indexNext < countSoCauHoi)
            {
                if (SelectedAnswer[j, indexNext] == 1)
                {
                    rdCauA.Checked = true;
                }
                if (SelectedAnswer[j, indexNext] == 2)
                {
                    rdCauB.Checked = true;
                }
                if (SelectedAnswer[j, indexNext] == 3)
                {
                    rdCauC.Checked = true;
                }
                if (SelectedAnswer[j, indexNext] == 4)
                {
                    rdCauD.Checked = true;
                }

            }
            else if (indexNext == countSoCauHoi)
            {

                if (SelectedAnswer[j, countSoCauHoi] == 1)
                {
                    rdCauA.Checked = true;
                }
                if (SelectedAnswer[j, countSoCauHoi] == 2)
                {
                    rdCauB.Checked = true;
                }
                if (SelectedAnswer[j, countSoCauHoi] == 3)
                {
                    rdCauC.Checked = true;
                }
                if (SelectedAnswer[j, countSoCauHoi] == 4)
                {
                    rdCauD.Checked = true;
                }
            }
            if (j == 1 && countButton == 1)
            {
                btnBack.Enabled = false;
                btnBack.BackColor = Color.CadetBlue;
                if (SelectedAnswer[j, indexNext] < 1)
                {
                    rdCauA.Checked = false;
                    rdCauB.Checked = false;
                    rdCauC.Checked = false;
                    rdCauD.Checked = false;
                }
            }
        }
        private void btnTat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn thoát khỏi bài thi?", "Thông báo", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.Close();
            }
            else
            {

            }
        }

    }
}
