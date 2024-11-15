using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginDemo.KBClass
{
    class HocVien
    {
        private string maHV;
        private string hoLot;
        private string ten;
        private string phai;
        private string soDt;
        private string maLop;
        public HocVien()
        {
            this.MaHV = "227480100001";
            this.HoLot = "XiaoMy";
            this.ten = "Tommy";
            this.phai = "Nam";
            this.soDt = "0123456789";
            this.maLop = "71K28";
        }
        public HocVien(string maHV, string hoLot, string ten, string phai, string soDt, string maLop)
        {
            this.MaHV = maHV;
            this.HoLot = hoLot;
            this.Ten = ten;
            this.Phai = phai;
            this.SoDt = soDt;
            this.maLop = maLop;
        }

        public string MaHV { get => maHV; set => maHV = value; }
        public string HoLot { get => hoLot; set => hoLot = value; }
        public string Ten { get => ten; set => ten = value; }
        public string Phai { get => phai; set => phai = value; }
        public string SoDt { get => soDt; set => soDt = value; }
        public string MaLop { get => maLop; set => maLop = value; }

        public override bool Equals(object obj)
        {
            return obj is HocVien vien &&
                   MaHV == vien.MaHV;
        }

        public override int GetHashCode()
        {
            return -39797835 + EqualityComparer<string>.Default.GetHashCode(MaHV);
        }
    }
}
