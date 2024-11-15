using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_pharse_setup
{
    internal class MonHoc
    {
        private string maMonHoc;
        private string tenMonHoc;
        public MonHoc()
        {
            this.MaMonHoc = "0";
            this.TenMonHoc = "Unknow";
        }

        public MonHoc(string maMH, string tenMHoc)
        {
            this.MaMonHoc = maMH;
            this.TenMonHoc = tenMHoc;
        }

        public string TenMonHoc { get => tenMonHoc; set => tenMonHoc = value; }
        public string MaMonHoc { get => maMonHoc; set => maMonHoc = value; }

        public override bool Equals(object obj)
        {
            return obj is MonHoc hoc &&
                   maMonHoc == hoc.maMonHoc &&
                   tenMonHoc == hoc.tenMonHoc;
        }

        public override int GetHashCode()
        {
            int hashCode = 1114914783;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(maMonHoc);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(tenMonHoc);
            return hashCode;
        }

        public override string ToString()
        {
            return TenMonHoc;
        }
    }
}
