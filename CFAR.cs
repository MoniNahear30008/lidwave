using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lidwave
{
    public class cfar
    {
        Form1 mainfrm;
        const int maxLen = 6;
        public uint[] buff;
        public uint[] TH = new uint[2048];
        public uint[,] peaks = new uint[2, 2];
        public uint[] maxTH = new uint[maxLen] { 10, 10, 10, 10, 10, 10 };
        public List<cfartv> tvs = new List<cfartv>();
        public List<List<uint>> eth = new List<List<uint>>();

        public cfar(Form1 parent) 
        { 
            mainfrm = parent;
        }

        public void loadCfrTv()
        {
            OpenFileDialog lf = new OpenFileDialog();
            lf.Filter = "tv files (*.csv)|*.csv";

            lf.Multiselect = false;
            if (lf.ShowDialog() != DialogResult.OK)
                return;

            string fname = lf.FileName;
            List<string> tvt = File.ReadAllLines(lf.FileName).ToList();
            int lcnt = tvt.Count;
            int nbuf = lcnt / 4098;

            mainfrm.tvName.Text = fname + " contains " + nbuf.ToString() + " FFT buffers";
            mainfrm.buffNum.Value = 0;
            mainfrm.buffNum.Maximum = nbuf - 1;

            int line = 0;
            for (int b = 0; b < nbuf; b++)
            {
                tvs.Add(new cfartv());
                uint fcnt = 0;
                for (int i = 0; i < 4098; i++)
                {
                    List<string> numbers = tvt[line++].Split(',').ToList();
                    if (i == 0)
                    {
                        tvs.Last().T = uint.Parse(numbers[0]);
                        tvs.Last().G = uint.Parse(numbers[1]);
                    }
                    else if (i == 1)
                    {
                        foreach (string number in numbers)
                        {
                            if (number == "")
                                break;
                            tvs.Last().maxTH.Add(uint.Parse(number));
                        }
                    }
                    else if (fcnt < 2048)
                    {
                        tvs.Last().fft[fcnt] = uint.Parse(numbers[0]);
                        tvs.Last().ths[fcnt] = uint.Parse(numbers[1]);
                        if (numbers[2] == "1")
                            tvs.Last().peaks.Add(fcnt);
                        fcnt++;
                    }
                }
            }

            mainfrm.procBox.Enabled = true;
        }

        public bool procCFR()
        {
            peaks = new uint[2, 2] { { 0, 0 }, { 0, 0 } };
            int bnum = (int)mainfrm.buffNum.Value;
            buff = tvs[bnum].fft.ToArray();

            // Calculate TH array with one sided on edges with peaks detect
            calcTh();

            return(compare());
        }
        private bool compare()
        {
            int bnum = (int)mainfrm.buffNum.Value;
            cfartv testv = tvs[bnum];

            // check TH
            eth.Clear();
            for (uint i = 0; i < TH.Count(); i++)
            {
                if (TH[i] != testv.ths[i])
                    eth.Add(new List<uint>() { i, testv.ths[i], TH[i] });
            }

            // check peaks
            List<uint> ipeaks = new List<uint>();
            foreach (uint p in testv.peaks)
            {
                if (p > 0)
                    ipeaks.Add(p);
            }
            ipeaks.Sort();

            if (ipeaks.Count > 0)
                mainfrm.peaksView.Rows[0].Cells[1].Value = ipeaks[0];
            if (ipeaks.Count > 1)
                mainfrm.peaksView.Rows[1].Cells[1].Value = ipeaks[1];

            List<uint> cpeaks = new List<uint>();
            if (peaks[0, 0] != 0)
                cpeaks.Add(peaks[0, 0]);
            if (peaks[1, 0] != 0)
                cpeaks.Add(peaks[1, 0]);
            cpeaks.Sort();

            if (cpeaks.Count > 0)
                mainfrm.peaksView.Rows[0].Cells[2].Value = cpeaks[0];
            if (cpeaks.Count > 1)
                mainfrm.peaksView.Rows[1].Cells[2].Value = cpeaks[1];

            bool[] perr = new bool[3] { false, false, false };
            if (cpeaks.Count != ipeaks.Count)
                perr[0] = true;
            else
            {
                for (int i = 0; i < cpeaks.Count; i++)
                    perr[i + 1] = ipeaks[i] != cpeaks[i];
            }

            if ((eth.Count == 0) && (perr[0] == false) && (perr[1] == false) && (perr[2] == false))
                return true;
            else
                return false;
        }
        private void calcTh()
        {
            int T = (int)mainfrm.tBins.Value;
            int G = (int)mainfrm.gBins.Value;
            int N = (int)mainfrm.nBins.Value;

            uint leftsum = 0;
            uint rightsum = 0;

            // calculate inital left and right sums
            for (int i = 0; i < T; i++)
            {
                rightsum += buff[i + G];
                leftsum += buff[i + N];
            }

            TH[0] = Math.Max((uint)(rightsum / T), maxTH[0]);

            // Calculate left side THs
            int bnum = 1;
            for (; bnum < T + G + N; bnum++)
            {
                rightsum = rightsum - buff[bnum + G] + buff[bnum + T + G];
                TH[bnum] = (uint)(rightsum / T);
                if (bnum < maxLen)
                    TH[bnum] = Math.Max(TH[bnum], maxTH[bnum]);

                if (buff[bnum] < TH[bnum]) continue;
                if ((buff[bnum] > buff[bnum + 1]) && (buff[bnum] >= buff[bnum - 1]))
                {
                    if ((peaks[0, 1] < peaks[1, 1]) && (buff[bnum] > peaks[0, 1]))
                    {
                        peaks[0, 1] = (uint)buff[bnum];
                        peaks[0, 0] = (uint)bnum;
                    }
                    else if (buff[bnum] > peaks[1, 1])
                    {
                        peaks[1, 1] = (uint)buff[bnum];
                        peaks[1, 0] = (uint)bnum;
                    }
                }
            }



            // Calculate middle THs
            for (; bnum < 2048 - T - G; bnum++)
            {
                rightsum = rightsum - buff[bnum + G] + buff[bnum + T + G];
                leftsum = leftsum - buff[bnum - T - G] + buff[bnum - G];
                TH[bnum] = (uint)((leftsum + rightsum) / (2 * T));

                if (buff[bnum] < TH[bnum]) continue;
                if ((buff[bnum] > buff[bnum + 1]) && (buff[bnum] >= buff[bnum - 1]))
                {
                    if ((peaks[0, 1] < peaks[1, 1]) && (buff[bnum] > peaks[0, 1]))
                    {
                        peaks[0, 1] = (uint)buff[bnum];
                        peaks[0, 0] = (uint)bnum;
                    }
                    else if (buff[bnum] > peaks[1, 1])
                    {
                        peaks[1, 1] = (uint)buff[bnum];
                        peaks[1, 0] = (uint)bnum;
                    }
                }
            }


            // Calculate right side THs
            for (; bnum < 2047; bnum++)
            {
                leftsum = leftsum - buff[bnum - T - G] + buff[bnum - G];
                TH[bnum] = (uint)(leftsum / T);

                if (buff[bnum] < TH[bnum]) continue;
                if ((buff[bnum] > buff[bnum + 1]) && (buff[bnum] >= buff[bnum - 1]))
                {
                    if ((peaks[0, 1] < peaks[1, 1]) && (buff[bnum] > peaks[0, 1]))
                    {
                        peaks[0, 1] = (uint)buff[bnum];
                        peaks[0, 0] = (uint)bnum;
                    }
                    else if (buff[bnum] > peaks[1, 1])
                    {
                        peaks[1, 1] = (uint)buff[bnum];
                        peaks[1, 0] = (uint)bnum;
                    }
                }
            }

        }
        private void peakSearch()
        {
            for (uint i = 1; i < 2046; i++)
            {
                if (buff[i] < TH[i]) continue;

                if ((buff[i] > buff[i + 1]) && (buff[i] >= buff[i - 1]))
                {
                    if ((peaks[0, 1] < peaks[1, 1]) && (buff[i] > peaks[0, 1]))
                    {
                        peaks[0, 1] = buff[i];
                        peaks[0, 0] = i;
                    }
                    else if (buff[i] > peaks[1, 1])
                    {
                        peaks[1, 1] = buff[i];
                        peaks[1, 0] = i;
                    }
                }
            }
        }
    }
    public class cfartv
    {
        public uint T;
        public uint G;
        public List<uint> maxTH;
        public uint[] fft;
        public uint[] ths;
        public List<uint> peaks;
        public cfartv()
        {
            maxTH = new List<uint>();
            fft = new uint[2048];
            ths = new uint[2048];
            peaks = new List<uint>();
        }
    }


}
