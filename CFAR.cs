using ScottPlot;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace lidwave
{
    public partial class Form1
    {
        const int maxLen = 6;
        uint[] buff;
        uint[] TH = new uint[2048];
        uint[,] peaks = new uint[2,2];
        uint[] maxTH = new uint[maxLen] { 10, 10, 10, 10, 10, 10 };
        List<tv> tvs = new List<tv>();
        

        private void initCFAR()
        {
            fftPlot.Configuration.DoubleClickBenchmark = false;
            fftPlot.Configuration.LockVerticalAxis = true;
            peaksView.Rows.Add(0, "na", "na");
            peaksView.Rows.Add(1, "na", "na");

        }
        private void loadTv()
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

            tvName.Text = fname + " contains " + nbuf.ToString() + " FFT buffers";
            buffNum.Value = 0;
            buffNum.Maximum = nbuf - 1;

            int line = 0;
            for (int b = 0; b < nbuf; b++)
            {
                tvs.Add(new tv());
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
                        tvs.Last().fft[fcnt]= uint.Parse(numbers[0]);
                        tvs.Last().ths[fcnt] = uint.Parse(numbers[1]);
                        if (numbers[2] == "1")
                            tvs.Last().peaks.Add(fcnt);
                        fcnt++;
                    }
                }
            }

            procBox.Enabled = true;
            plotFFT();
        }
        private void procFFTBuff()
        {
            pass.Visible = false;
            fail.Visible = false;
            peaksView.Rows[0].Cells[1].Value = "n.a.";
            peaksView.Rows[1].Cells[1].Value = "n.a.";
            peaksView.Rows[0].Cells[2].Value = "n.a.";
            peaksView.Rows[1].Cells[2].Value = "n.a.";
            therr.Items.Clear();

            peaks = new uint[2, 2] { { 0, 0 }, { 0, 0 } };
            int bnum = (int)buffNum.Value;
            buff = tvs[bnum].fft.ToArray();

            // Calculate TH array with one sided on edges with peaks detect
            calcTh();

            plotCFAR();

            bool ispass = compare(true);
        }
        private bool compare(bool withUI)
        {
            int bnum = (int)buffNum.Value;
            tv testv = tvs[bnum];

            // check TH
            List<int> eth = new List<int>();
            for (int i = 0; i < TH.Count(); i++)
            {
                if (TH[i] != testv.ths[i])
                    eth.Add(i);
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
                peaksView.Rows[0].Cells[1].Value = ipeaks[0];
            if (ipeaks.Count > 1)
                peaksView.Rows[1].Cells[1].Value = ipeaks[1];

            List<uint> cpeaks = new List<uint>();
            if (peaks[0,0] != 0)
                    cpeaks.Add(peaks[0,0]);
            if (peaks[1, 0] != 0)
                cpeaks.Add(peaks[1, 0]);
            cpeaks.Sort();

            if (cpeaks.Count > 0)
                peaksView.Rows[0].Cells[2].Value = cpeaks[0];
            if (cpeaks.Count > 1)
                peaksView.Rows[1].Cells[2].Value = cpeaks[1];

            bool[] perr = new bool[3] {false, false,  false};
            if (cpeaks.Count != ipeaks.Count)
                perr[0] = true;
            else
            {
                for (int i = 0; i < cpeaks.Count; i++)
                    perr[i+1] = ipeaks[i] != cpeaks[i];
            }

            if ((eth.Count == 0) && (perr[0] == false) && (perr[1] == false) && (perr[2] == false))
            {
                if (withUI)
                    pass.Visible = true;
                return false;
            }
            else
            {
                if (withUI)
                {
                    fail.Visible = true;
                    for (int i = 0; i < eth.Count; i++)
                    {
                        fftPlot.Plot.AddVerticalLine(eth[i], color: Color.Red, style: LineStyle.DashDot);
                        therr.Items.Add(eth[i].ToString() + ":     " + testv.ths[eth[i]].ToString() + "         " + TH[eth[i]].ToString());
                    }
                }
            }
            return true;
        }
        private void calcTh()
        {
            int T = (int)tBins.Value;
            int G = (int)gBins.Value;   
            int N = (int)nBins.Value;

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
        private void plotFFT()
        {
            int bnum = (int)buffNum.Value;

            int[] iX = Enumerable.Range(0, 2048).ToArray();
            double[] X = Array.ConvertAll<int, double>(iX, x => x);
            double[] dbuff = Array.ConvertAll<uint, double>(tvs[bnum].fft.ToArray(), x => x);
            double[] dth = Array.ConvertAll<uint, double>(tvs[bnum].ths.ToArray(), x => x);

            fftPlot.Plot.Clear();
            fftPlot.Plot.AddScatter(X, dbuff, lineWidth: 1, color: Color.Blue, markerShape: ScottPlot.MarkerShape.filledCircle);
            fftPlot.Plot.AddScatter(X, dth, lineWidth: 0, color: Color.Red);
            foreach (uint p in tvs[bnum].peaks)
                fftPlot.Plot.AddVerticalLine(p, color: Color.Lime);

            fftPlot.Plot.AxisAutoX();
            fftPlot.Plot.AxisAutoY();
            fftPlot.Refresh();

        }
        private void plotCFAR()
        {
            int[] iX = Enumerable.Range(0, 2048).ToArray();
            double[] X = Array.ConvertAll<int, double>(iX, x => x);
            fftPlot.Plot.Clear();

            double[] dbuff = Array.ConvertAll<uint, double>(buff, x => x);
            fftPlot.Plot.AddScatter(X, dbuff, lineWidth: 1, color: Color.Blue, markerShape: ScottPlot.MarkerShape.openCircle);

            double[] dTH = Array.ConvertAll<uint, double>(TH, x => x);
            fftPlot.Plot.AddScatter(X, dTH, lineWidth: 0, color: Color.Red);

            List<double> X1 = new List<double>();
            List<double> Y1 = new List<double>();
            for (int i = 0; i < TH.Count(); i++)
            {
                if (dbuff[i] > TH[i])
                {
                    X1.Add(i);
                    Y1.Add(dbuff[i]);
                }
            }

            fftPlot.Plot.AddScatter(X1.ToArray(), Y1.ToArray(), lineWidth: 0, color: Color.Green, markerShape: ScottPlot.MarkerShape.filledCircle);

            if (peaks[0, 0] > 0)
            {
                uint bin = peaks[0, 0];
                double est = ((double)buff[bin+1] - (double)buff[bin-1]) / (2 * ((2 * (double)buff[bin]) - (double)buff[bin-1] - (double)buff[bin+1]));
                fftPlot.Plot.AddVerticalLine(peaks[0, 0], color: Color.Lime);
                fftPlot.Plot.AddVerticalLine(peaks[0, 0] + est, color: Color.Lime, style: LineStyle.Dash);
                fftPlot.Plot.AddHorizontalLine(peaks[0, 1], color: Color.DarkViolet, style: LineStyle.Dot);
            }
            if (peaks[1, 0] > 0)
            {
                uint bin = peaks[1, 0];
                double est = ((double)buff[bin + 1] - (double)buff[bin - 1]) / (2 * ((2 * (double)buff[bin]) - (double)buff[bin - 1] - (double)buff[bin + 1]));
                fftPlot.Plot.AddVerticalLine(peaks[1, 0], color: Color.Lime);
                fftPlot.Plot.AddVerticalLine(peaks[1, 0] + est, color: Color.Lime, style: LineStyle.Dash);
                fftPlot.Plot.AddHorizontalLine(peaks[1, 1], color: Color.DarkViolet, style: LineStyle.Dot);
            }

            fftPlot.Plot.AxisAutoX();
            fftPlot.Plot.AxisAutoY();
            fftPlot.Refresh();

            //using (StreamWriter writetext = new StreamWriter("c:\\Lidwave\\Tools\\res.txt"))
            //{
            //    writetext.WriteLine("TH");
            //    foreach (uint t in TH)
            //        writetext.WriteLine(t.ToString());
            //    writetext.WriteLine("Peaks");
            //    writetext.WriteLine(peaks[0,0].ToString());
            //    writetext.WriteLine(peaks[1, 0].ToString());
            //}
        }

    }

    public class tv
    {
        public uint T;
        public uint G;
        public List<uint> maxTH;
        public uint[] fft;
        public uint[] ths;
        public List<uint> peaks;
        public tv()
        {
            maxTH = new List<uint>();
            fft = new uint[2048];
            ths = new uint[2048];
            peaks = new List<uint>();
        }
    }
}
