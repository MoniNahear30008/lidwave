using ScottPlot;

namespace lidwave
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initCFAR();
        }

        private void loadTV_Click(object sender, EventArgs e)
        {
            loadTv();
        }

        private void procBuff_Click(object sender, EventArgs e)
        {
            procFFTBuff();
        }

        private void buffNum_ValueChanged(object sender, EventArgs e)
        {
            plotFFT();
        }
    }
}
