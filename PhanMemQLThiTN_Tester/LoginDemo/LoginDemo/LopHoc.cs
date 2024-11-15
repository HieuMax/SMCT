using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_pharse_setup
{
    internal class LopHoc
    {
        private string maLop;
        private string tenLop;

        public LopHoc()
        {
            this.MaLop = "71K28";
            this.TenLop = "Kỹ thuật phần mềm";
        }

        public LopHoc(string maLop, string tenLop)
        {
            this.MaLop = maLop;
            this.TenLop = tenLop;
        }

        public string MaLop { get => maLop; set => maLop = value; }
        public string TenLop { get => tenLop; set => tenLop = value; }

        public override bool Equals(object obj)
        {
            return obj is LopHoc hoc &&
                   maLop == hoc.maLop;
        }

        public override string ToString()
        {
            return maLop + " - " + tenLop;
        }
        public override int GetHashCode()
        {
            return 65984906 + EqualityComparer<string>.Default.GetHashCode(maLop);
        }
    }
}
