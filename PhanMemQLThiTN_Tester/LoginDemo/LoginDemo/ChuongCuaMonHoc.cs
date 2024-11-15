using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_pharse_setup
{
    internal class ChuongCuaMonHoc
    {
        private string maChuong;
        private string tenChuong;
        private string maMH;

        public ChuongCuaMonHoc()
        {
            this.MaChuong = "TO01";
            this.TenChuong = "Đại số";
            this.MaMH = "71TO";
        }

        public ChuongCuaMonHoc(string maChuong, string tenChuong, string maMH)
        {
            this.MaChuong = maChuong;
            this.TenChuong = tenChuong;
            this.MaMH = maMH;
        }

        public string MaChuong { get => maChuong; set => maChuong = value; }
        public string TenChuong { get => tenChuong; set => tenChuong = value; }
        public string MaMH { get => maMH; set => maMH = value; }

        public override bool Equals(object obj)
        {
            return obj is ChuongCuaMonHoc hoc &&
                   MaMH == hoc.MaMH;
        }

        public override int GetHashCode()
        {
            return -801632498 + EqualityComparer<string>.Default.GetHashCode(MaMH);
        }
    }
}
