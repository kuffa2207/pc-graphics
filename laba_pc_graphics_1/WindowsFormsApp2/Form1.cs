using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Bitmap image;
        OpenFileDialog dial;

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 3; ++i)
            {
                dataGridView1.Rows.Add();
            }
            image = null;
            dial = new OpenFileDialog();
            MorphOps.mask = new float[3, 3];
            DataGridViewColumn col = new DataGridViewColumn(dataGridView1[0,0]);
            DataGridViewColumn col1 = new DataGridViewColumn(dataGridView1[0,0]);
            DataGridViewColumn col2 = new DataGridViewColumn(dataGridView1[0,0]);
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add(col);
            dataGridView1.Columns.Add(col1);
            dataGridView1.Columns.Add(col2);
            for (int i = 0; i < 3; ++i)
            {
                dataGridView1.Rows.Add();
            }
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                dataGridView1.Rows[i].Height = dataGridView1.Height / dataGridView1.Rows.Count;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; ++j)
                {
                    var cell = (DataGridViewButtonCell)dataGridView1[i, j];
                    cell.FlatStyle = FlatStyle.Flat;
                    dataGridView1[i, j].Style.BackColor = Color.White;
                    dataGridView1[i, j].Style.ForeColor = Color.White;
                }
            }
        }

        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png;*.jpg;*.bmp|All files(*.*)|*.*";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
                dial.FileName = dialog.FileName;
            }
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertFilter filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (image != null)
            {
                Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
                if (backgroundWorker1.CancellationPending != true)
                {
                    image = newImage;
                }
            }
            else
            {
                MessageBox.Show("Выберите файл для редактирования");
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
        
        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        this.image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        this.image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        this.image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 4:
                        this.image.Save(fs,
                            System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }

                fs.Close();
            }
        }

        private void gaussianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GrayScaleFilter filter = new GrayScaleFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SepiaFilter filter = new SepiaFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void maxBrightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaxBrightnessFilter filter = new MaxBrightnessFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

       private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
       {
            Filters filter = new SobelFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void maxSherpnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new maxSharpness();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void embossingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new embossingFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void grayWorldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayWorldFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        

        private void linearExtensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new linearExtension();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void morphOpsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Erosion();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Dilation();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void openingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Opening();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void closingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Closing();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void gradToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Grad();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MedianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            image = new Bitmap(dial.FileName);
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DataGrid dataGrid = new DataGrid();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.Style.BackColor != Color.DarkBlue)
            {
                dataGridView1.CurrentCell.Style.BackColor = Color.DarkBlue;
                dataGridView1.CurrentCell.Style.ForeColor = Color.DarkBlue;
                MorphOps.setMask(dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex, 1);
            }
            else
            {
                dataGridView1.CurrentCell.Style.BackColor = Color.White;
                dataGridView1.CurrentCell.Style.ForeColor = Color.White;
                MorphOps.setMask(dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex, 0);
            }
        }

        

        private void glassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GlassFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void wavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new WavesFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                dataGridView1.Rows[i].Height = dataGridView1.Height / dataGridView1.Rows.Count;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            {
                try
                {
                    ((DataGridView)sender).SelectedCells[0].Selected = false;
                }
                catch { }
            }
        }

        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    };

}
