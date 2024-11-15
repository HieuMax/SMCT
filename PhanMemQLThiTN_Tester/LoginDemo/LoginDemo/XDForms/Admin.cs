using Project_pharse_setup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginDemo.XDForms
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
        }

        public string temp;

        private void Admin_Load(object sender, EventArgs e)
        {
            lblTenGV.Text = temp;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTaoDeThi_Click(object sender, EventArgs e)
        {
            TaoDeThi_AdminForm taoDeThi = new TaoDeThi_AdminForm();
            taoDeThi.ShowDialog();
        }

        private void btnTaoCauHoi_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            adminForm.ShowDialog();
        }

        private void btnXemKQAdmin_Click(object sender, EventArgs e)
        {
            ChonMaHV_XemKQAdmin beforeKQ = new ChonMaHV_XemKQAdmin();
            beforeKQ.ShowDialog();
        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            DoiMatKhau dmk = new DoiMatKhau();
            dmk.maHV = lblTenGV.Text;
            dmk.ShowDialog();
        }
    }
}
