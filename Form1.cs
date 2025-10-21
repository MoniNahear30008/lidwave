using ScottPlot;

namespace lidwave
{
    public partial class Form1 : Form
    {
        cfar cfar;
        ics ics;
        public Form1()
        {
            InitializeComponent();

            fftPlot.Configuration.DoubleClickBenchmark = false;
            fftPlot.Configuration.LockVerticalAxis = true;
            peaksView.Rows.Add(0, "na", "na");
            peaksView.Rows.Add(1, "na", "na");

            cfar = new cfar(this);
            ics = new ics(this);
        }

        private void loadTV_Click(object sender, EventArgs e)
        {
            cfar.loadCfrTv();
            plotCFRFFT_Input();
        }

        private void procBuff_Click(object sender, EventArgs e)
        {
            if (cfar.procCFR())
                pass.Visible = true;
            else
                fail.Visible = true;

            plotCFAR_Res();
            for (int i = 0; i < cfar.eth.Count; i++)
            {
                fftPlot.Plot.AddVerticalLine(cfar.eth[i][0], color: Color.Red, style: LineStyle.DashDot);
                therr.Items.Add(cfar.eth[i][0].ToString() + ":     " + cfar.eth[i][1].ToString() + "         " + cfar.eth[i][2].ToString());
            }
        }

        private void buffNum_ValueChanged(object sender, EventArgs e)
        {
            plotCFRFFT_Input();
        }

        private void plotCFRFFT_Input()
        {
            pass.Visible = false;
            fail.Visible = false;
            peaksView.Rows[0].Cells[1].Value = "n.a.";
            peaksView.Rows[1].Cells[1].Value = "n.a.";
            peaksView.Rows[0].Cells[2].Value = "n.a.";
            peaksView.Rows[1].Cells[2].Value = "n.a.";
            therr.Items.Clear();

            int bnum = (int)buffNum.Value;

            int[] iX = Enumerable.Range(0, 2048).ToArray();
            double[] X = Array.ConvertAll<int, double>(iX, x => x);
            double[] dbuff = Array.ConvertAll<uint, double>(cfar.tvs[bnum].fft.ToArray(), x => x);
            double[] dth = Array.ConvertAll<uint, double>(cfar.tvs[bnum].ths.ToArray(), x => x);

            fftPlot.Plot.Clear();
            fftPlot.Plot.AddScatter(X, dbuff, lineWidth: 1, color: Color.Blue, markerShape: ScottPlot.MarkerShape.filledCircle);
            fftPlot.Plot.AddScatter(X, dth, lineWidth: 0, color: Color.Red);
            foreach (uint p in cfar.tvs[bnum].peaks)
                fftPlot.Plot.AddVerticalLine(p, color: Color.Lime);

            fftPlot.Plot.AxisAutoX();
            fftPlot.Plot.AxisAutoY();
            fftPlot.Refresh();

        }

        private void plotCFAR_Res()
        {
            int[] iX = Enumerable.Range(0, 2048).ToArray();
            double[] X = Array.ConvertAll<int, double>(iX, x => x);
            fftPlot.Plot.Clear();

            double[] dbuff = Array.ConvertAll<uint, double>(cfar.buff, x => x);
            fftPlot.Plot.AddScatter(X, dbuff, lineWidth: 1, color: Color.Blue, markerShape: ScottPlot.MarkerShape.openCircle);

            double[] dTH = Array.ConvertAll<uint, double>(cfar.TH, x => x);
            fftPlot.Plot.AddScatter(X, dTH, lineWidth: 0, color: Color.Red);

            List<double> X1 = new List<double>();
            List<double> Y1 = new List<double>();
            for (int i = 0; i < cfar.TH.Count(); i++)
            {
                if (dbuff[i] > cfar.TH[i])
                {
                    X1.Add(i);
                    Y1.Add(dbuff[i]);
                }
            }

            fftPlot.Plot.AddScatter(X1.ToArray(), Y1.ToArray(), lineWidth: 0, color: Color.Green, markerShape: ScottPlot.MarkerShape.filledCircle);

            if (cfar.peaks[0, 0] > 0)
            {
                uint bin = cfar.peaks[0, 0];
                double est = ((double)cfar.buff[bin + 1] - (double)cfar.buff[bin - 1]) / (2 * ((2 * (double)cfar.buff[bin]) - (double)cfar.buff[bin - 1] - (double)cfar.buff[bin + 1]));
                fftPlot.Plot.AddVerticalLine(cfar.peaks[0, 0], color: Color.Lime);
                fftPlot.Plot.AddVerticalLine(cfar.peaks[0, 0] + est, color: Color.Lime, style: LineStyle.Dash);
                fftPlot.Plot.AddHorizontalLine(cfar.peaks[0, 1], color: Color.DarkViolet, style: LineStyle.Dot);
            }
            if (cfar.peaks[1, 0] > 0)
            {
                uint bin = cfar.peaks[1, 0];
                double est = ((double)cfar.buff[bin + 1] - (double)cfar.buff[bin - 1]) / (2 * ((2 * (double)cfar.buff[bin]) - (double)cfar.buff[bin - 1] - (double)cfar.buff[bin + 1]));
                fftPlot.Plot.AddVerticalLine(cfar.peaks[1, 0], color: Color.Lime);
                fftPlot.Plot.AddVerticalLine(cfar.peaks[1, 0] + est, color: Color.Lime, style: LineStyle.Dash);
                fftPlot.Plot.AddHorizontalLine(cfar.peaks[1, 1], color: Color.DarkViolet, style: LineStyle.Dot);
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

        private void loadICSTV_Click(object sender, EventArgs e)
        {
            ics.loadIcsTv();
        }
    }
}
