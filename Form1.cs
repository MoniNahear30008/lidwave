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

            cftPlot.Configuration.DoubleClickBenchmark = false;
            cftPlot.Configuration.LockVerticalAxis = true;
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
                cftPlot.Plot.AddVerticalLine(cfar.eth[i][0], color: Color.Red, style: LineStyle.DashDot);
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

            cftPlot.Plot.Clear();
            cftPlot.Plot.AddScatter(X, dbuff, lineWidth: 1, color: Color.Blue, markerShape: ScottPlot.MarkerShape.filledCircle);
            cftPlot.Plot.AddScatter(X, dth, lineWidth: 0, color: Color.Red);
            foreach (uint p in cfar.tvs[bnum].peaks)
                cftPlot.Plot.AddVerticalLine(p, color: Color.Lime);

            cftPlot.Plot.AxisAutoX();
            cftPlot.Plot.AxisAutoY();
            cftPlot.Refresh();

        }

        private void plotCFAR_Res()
        {
            int[] iX = Enumerable.Range(0, 2048).ToArray();
            double[] X = Array.ConvertAll<int, double>(iX, x => x);
            cftPlot.Plot.Clear();

            double[] dbuff = Array.ConvertAll<uint, double>(cfar.buff, x => x);
            cftPlot.Plot.AddScatter(X, dbuff, lineWidth: 1, color: Color.Blue, markerShape: ScottPlot.MarkerShape.openCircle);

            double[] dTH = Array.ConvertAll<uint, double>(cfar.TH, x => x);
            cftPlot.Plot.AddScatter(X, dTH, lineWidth: 0, color: Color.Red);

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

            cftPlot.Plot.AddScatter(X1.ToArray(), Y1.ToArray(), lineWidth: 0, color: Color.Green, markerShape: ScottPlot.MarkerShape.filledCircle);

            if (cfar.peaks[0, 0] > 0)
            {
                uint bin = cfar.peaks[0, 0];
                double est = ((double)cfar.buff[bin + 1] - (double)cfar.buff[bin - 1]) / (2 * ((2 * (double)cfar.buff[bin]) - (double)cfar.buff[bin - 1] - (double)cfar.buff[bin + 1]));
                cftPlot.Plot.AddVerticalLine(cfar.peaks[0, 0], color: Color.Lime);
                cftPlot.Plot.AddVerticalLine(cfar.peaks[0, 0] + est, color: Color.Lime, style: LineStyle.Dash);
                cftPlot.Plot.AddHorizontalLine(cfar.peaks[0, 1], color: Color.DarkViolet, style: LineStyle.Dot);
            }
            if (cfar.peaks[1, 0] > 0)
            {
                uint bin = cfar.peaks[1, 0];
                double est = ((double)cfar.buff[bin + 1] - (double)cfar.buff[bin - 1]) / (2 * ((2 * (double)cfar.buff[bin]) - (double)cfar.buff[bin - 1] - (double)cfar.buff[bin + 1]));
                cftPlot.Plot.AddVerticalLine(cfar.peaks[1, 0], color: Color.Lime);
                cftPlot.Plot.AddVerticalLine(cfar.peaks[1, 0] + est, color: Color.Lime, style: LineStyle.Dash);
                cftPlot.Plot.AddHorizontalLine(cfar.peaks[1, 1], color: Color.DarkViolet, style: LineStyle.Dot);
            }

            cftPlot.Plot.AxisAutoX();
            cftPlot.Plot.AxisAutoY();
            cftPlot.Refresh();

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
            plotICS(false);
        }
        private void plotICS(bool res)
        {

            int[] iX = Enumerable.Range(0, 2048).ToArray();
            double[] X = Array.ConvertAll<int, double>(iX, x => x);
            double[] Y = Array.ConvertAll<uint, double>(ics.afterIcsTv.ToArray(), x => x);

            icsPlot.Plot.Clear();
            icsPlot.Plot.AddScatter(X, Y, lineWidth: 1, color: Color.Blue, markerShape: ScottPlot.MarkerShape.filledCircle);
            if (res)
            {
                double[] Y1 = Array.ConvertAll<uint, double>(ics.afterIcsCalc.ToArray(), x => x);
                icsPlot.Plot.AddScatter(X, Y1, lineWidth: 1, color: Color.Red, markerShape: ScottPlot.MarkerShape.filledCircle);
            }

            icsPlot.Plot.AxisAutoX();
            icsPlot.Plot.AxisAutoY();
            icsPlot.Refresh();

        }

        private void doICS_Click(object sender, EventArgs e)
        {
            ics.run();
            plotICS(true);
        }
    }
}
