using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lidwave
{
    public class ics
    {
        Form1 mainfrm;
        const int testLen = 28672;
        public uint[,,] icsBuff = new uint[3, 6, 2048];
        public uint[] newBuff = new uint[2048];

        public ics(Form1 mainfrm)
        {
            this.mainfrm = mainfrm;
        }

        public void loadIcsTv()
        {
            OpenFileDialog lf = new OpenFileDialog();
            lf.Filter = "tv files (*.csv)|*.csv";

            lf.Multiselect = false;
            if (lf.ShowDialog() != DialogResult.OK)
                return;

            string fname = lf.FileName;
            mainfrm.IscTv.Text = fname;

            List<string> tvt = File.ReadAllLines(lf.FileName).ToList();
            int lcnt = tvt.Count;
            int nbuf = lcnt / testLen;

            //buffNum.Value = 0;
            //buffNum.Maximum = nbuf - 1;

            //int line = 0;
            //for (int b = 0; b < nbuf; b++)
            //{
            //    tvs.Add(new tv());
            //    uint fcnt = 0;
            //    for (int i = 0; i < 4098; i++)
            //    {
            //        List<string> numbers = tvt[line++].Split(',').ToList();
            //        if (i == 0)
            //        {
            //            tvs.Last().T = uint.Parse(numbers[0]);
            //            tvs.Last().G = uint.Parse(numbers[1]);
            //        }
            //        else if (i == 1)
            //        {
            //            foreach (string number in numbers)
            //            {
            //                if (number == "")
            //                    break;
            //                tvs.Last().maxTH.Add(uint.Parse(number));
            //            }
            //        }
            //        else if (fcnt < 2048)
            //        {
            //            tvs.Last().fft[fcnt] = uint.Parse(numbers[0]);
            //            tvs.Last().ths[fcnt] = uint.Parse(numbers[1]);
            //            if (numbers[2] == "1")
            //                tvs.Last().peaks.Add(fcnt);
            //            fcnt++;
            //        }
            //    }
            //}

            //procBox.Enabled = true;
            //plotCRFFFT();
        }

    }
}
