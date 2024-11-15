using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_pharse_setup
{
    internal class CauHoi
    {
        private string maCauHoi;
        private string noiDungCauHoi;
        private string panA;
        private string panB;
        private string panC;
        private string panD;
        private string answer;
        private ArrayList selectedAnswer = new ArrayList();
        private string maChuong;

        public CauHoi()
        {
            this.MaCauHoi = "Toan12";
            this.NoiDungCauHoi = "ABC";
            this.PanA = "A";
            this.PanB = "B";
            this.PanC = "C";
            this.PanD = "D";
            this.Answer = "answer";
            this.MaChuong = "CT";
        }

        public CauHoi(string maCH, string ND, string A, string B, string C, string D, string answer, string maChuong)
        {
            this.MaCauHoi = maCH;
            this.NoiDungCauHoi = ND;
            this.PanA = A;
            this.PanB = B;
            this.PanC = C;
            this.PanD = D;
            this.Answer = answer;
            this.MaChuong = maChuong;
        }

        public string MaCauHoi { get => maCauHoi; set => maCauHoi = value; }
        public string PanA { get => panA; set => panA = value; }
        public string PanB { get => panB; set => panB = value; }
        public string PanC { get => panC; set => panC = value; }
        public string PanD { get => panD; set => panD = value; }
        public string Answer { get => answer; set => answer = value; }
        public string MaChuong { get => maChuong; set => maChuong = value; }
        public string NoiDungCauHoi { get => noiDungCauHoi; set => noiDungCauHoi = value; }
        public ArrayList SelectedAnswer { get => selectedAnswer; set => selectedAnswer = value; }

        public override bool Equals(object obj)
        {
            return obj is CauHoi hoi &&
                   maCauHoi == hoi.maCauHoi;
        }

        public override int GetHashCode()
        {
            return 2080286214 + EqualityComparer<string>.Default.GetHashCode(maCauHoi);
        }
    }
}
