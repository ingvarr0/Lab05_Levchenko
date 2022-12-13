using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Lab05_Levchenko
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            labelSizeIN.Text = "";
            labelSizeOUT.Text = "";
            labeltime.Text = "";
        }

        private byte[] myEncoding(byte[] inArr)
        {
            byte[] result = { 0 };

            if (rB_CRC32.Checked == true)
            {
                uint a = Crc.Crc32(inArr);
                return BitConverter.GetBytes(a);
            }
            if (rB_HAVAL.Checked == true)
            {
                HashAlgorithm sha = KeyedHashAlgorithm.Create();
                return sha.ComputeHash(inArr);
            }
            if (rB_RIPEMD160.Checked == true)
            {
                HashAlgorithm sha = RIPEMD160.Create();
                return sha.ComputeHash(inArr);
            }
            if (rB_MD5.Checked == true)
            {
                HashAlgorithm sha = MD5.Create();
                return sha.ComputeHash(inArr);
            }
            if (rB_SHA1.Checked == true)
            {
                HashAlgorithm sha = SHA1.Create();
                return sha.ComputeHash(inArr);
            }
            if (rB_SHA256.Checked == true)
            {
                HashAlgorithm sha = SHA256.Create();
                return sha.ComputeHash(inArr);
            }
            if (rB_SHA384.Checked == true)
            {
                HashAlgorithm sha = SHA384.Create();
                return sha.ComputeHash(inArr);
            }
            if (rB_SHA512.Checked == true)
            {
                HashAlgorithm sha = SHA512.Create();
                return sha.ComputeHash(inArr);
            }

            return result;
        }

        private void bClean_Click(object sender, EventArgs e)
        {
            tInFilePath.Text = "";
            tOutFilePath.Text = "";
            labelSizeIN.Text = "";
            labelSizeOUT.Text = "";
            labeltime.Text = "";
        }

        static String BytesToString(long byteCount)
        {
            string[] suf = { "Byt", "KB", "MB", "GB", "TB", "PB", "EB" }; //
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        private void bInFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileInPath = openFileDialog.FileName;
                    tInFilePath.Text = fileInPath;
                }
            }
        }

        private void bOutFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileOutPath = saveFileDialog.FileName;
                tOutFilePath.Text = fileOutPath;
            }
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            byte[] inArr = File.ReadAllBytes(tInFilePath.Text);


            byte[] outArr = myEncoding(inArr);


            stopwatch.Stop();
            labeltime.Text = stopwatch.Elapsed.ToString(@"mm\:ss\.fff");

            File.WriteAllBytes(tOutFilePath.Text, outArr);


            string result = BitConverter.ToString(outArr).Replace("-", "");
            MessageBox.Show("У файлі записане число (контрольна сума вхідного файлу):\n" + result, "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);

            string fileInPath = tInFilePath.Text;
            long byteCountIn = new System.IO.FileInfo(fileInPath).Length;
            string bytesizeIn = BytesToString(byteCountIn);
            labelSizeIN.Text = bytesizeIn;

            string fileOUTPath = tOutFilePath.Text;
            long byteCountOut = new System.IO.FileInfo(fileOUTPath).Length;
            string bytesizeOUT = BytesToString(byteCountOut);
            labelSizeOUT.Text = bytesizeOUT;
        }
    }
}
