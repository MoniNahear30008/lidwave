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
        const int testLen = 30720;
        public uint[,,] icsBuff = new uint[3, 6, 2048];
        public List<uint[,,]> icsTv = new List<uint[,,]>(); // Test, buff, channels
        public uint[,] newBuff = new uint[2048,2];
        public uint[] afterIcsTv = new uint[2048];
        public uint[] afterIcsCalc = new uint[2048];

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
            int nTst = lcnt / testLen;

            //buffNum.Value = 0;
            //buffNum.Maximum = nbuf - 1;

            int line = 0;
            for (int t = 0; t < nTst; t++)
            {
                icsTv.Add(new uint[3, 6, 2048]); // Ch, Buff, Sample
                for (int b = 0; b < 6; b++)
                {
                    for (int i = 0; i < 4096; i++)
                    {
                        if (i < 2048)
                        {
                            List<string> numbers = tvt[line].Split(',').ToList();
                            for (int c = 0; c < 3; c++)
                            {
                                icsTv[t][c,b,i] = (uint.Parse(numbers[2 * c]) * uint.Parse(numbers[2 * c]))
                                            + (uint.Parse(numbers[(2 * c) + 1]) * uint.Parse(numbers[(2 * c) + 1]));
                            }
                        }
                        line++;
                    }
                }
            }

            for (int i = 0; i < 4096; i++)
            {
                if (i < 2048)
                {
                    List<string> numbers = tvt[line].Split(',').ToList();
                    newBuff[i, 0] = uint.Parse(numbers[0]);
                    newBuff[i, 1] = uint.Parse(numbers[1]);
                }
                line++;
            }

            for (int i = 0; i < 2048; i++)
            {
                List<string> numbers = tvt[line++].Split(',').ToList();
                afterIcsTv[i] = uint.Parse(numbers[0]);
            }
        }
        public void run()
        {
            int wrBuff = 0;
            int chNum = 1;

            for (int b = 0; b < 2048; b++)
            {
                //   Bout[b] = buffer[ChNum][WrBuff][b]
                afterIcsCalc[b] = icsTv[0][chNum, wrBuff, b];

                //   buffer[ChNum][WrBuff][b] = power from input complex number
                icsTv[0][chNum, wrBuff, b] = (newBuff[b, 0] * newBuff[b, 0]) + (newBuff[b, 1] * newBuff[b, 1]);

                //   Bout[b] += buffer[ChNum] [WrBuff][b]
                afterIcsCalc[b] += icsTv[0][chNum, wrBuff, b];

                //   Bout[b] += buffer[ChNum] [(WrBuff + 3) mod 6][b]
                afterIcsCalc[b] += icsTv[0][chNum, (wrBuff + 3) % 6, b];

                //   If not top channel, then Bout[b] += buffer[ChNum - 1] [(WrBuff + 3) mod 6][b]

                //   If not bottom channel, then Bout[b] += buffer[ChNum + 1] [(WrBuff + 3) mod 6][b]

                //   If not bottom and not top the Bout[b] /= 5, else Bout[b] /= 4
                afterIcsCalc[b] /= 5;

                //   WrBuff = (WrBuff + 1) mod 6
                wrBuff++;
                if (wrBuff > 5) wrBuff -= 6;
            }
        }
    }
}
