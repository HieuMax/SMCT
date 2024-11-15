using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDemo
{
    internal class DeThi
    {
        //Thông tin về đề thi gồm: mã đề thi, ngày thi, các câu hỏi thuộc môn học của đề thi này. 
        private string maDeThi;
        private DateTime ngayThi;
        private string dsMaCauHoi;

        public DeThi()
        {
            this.MaDeThi = "220";
            this.NgayThi = DateTime.Now;
            this.DsMaCauHoi = "202 105 505";
        }

        public DeThi(string maDeThi, DateTime ngayThi, string dsMaCauHoi)
        {
            this.MaDeThi = maDeThi;
            this.NgayThi = ngayThi;
            this.DsMaCauHoi = dsMaCauHoi;
        }

        public string MaDeThi { get => maDeThi; set => maDeThi = value; }
        public DateTime NgayThi { get => ngayThi; set => ngayThi = value; }
        public string DsMaCauHoi { get => dsMaCauHoi; set => dsMaCauHoi = value; }

        public override bool Equals(object obj)
        {
            return obj is DeThi thi &&
                   maDeThi == thi.maDeThi;
        }

        public override int GetHashCode()
        {
            return -329540753 + EqualityComparer<string>.Default.GetHashCode(maDeThi);
        }
    }
}
