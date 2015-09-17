using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Color_Organizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap newBitmap;
        private void Upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Browse Image Files";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = "jpg";
            openFileDialog1.Filter = "JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String name = openFileDialog1.FileName;
                Bitmap myBitmap = new Bitmap(name);
                List<Color> colors = new List<Color>();
                for(int x = 0; x < myBitmap.Width; x++)
                {
                    for (int y = 0; y < myBitmap.Height; y++)
                    {
                        colors.Add(myBitmap.GetPixel(x,y));
                    }
                }
                colors.OrderBy(p => p.Name);
                Dictionary<Color, int> counts = colors.GroupBy(b => b).ToDictionary(g => g.Key, g => g.Count());

                newBitmap = new Bitmap(myBitmap.Width, myBitmap.Height);
                int X = 0;
                int currentPixel = 0;
                int yLocation = 0;
                foreach (var c in counts)
                {
                    int num = c.Value;
                    Color _col = c.Key;

                    if (yLocation == myBitmap.Height - 1) 
                    {
                        yLocation = 0;
                        X++;
                    }
                    for (int count = 0; count < c.Value; count++) 
                    {
                        if (currentPixel % (myBitmap.Height - 1) == 0 && currentPixel != 0)
                        {
                            X++;
                            yLocation = 0;
                        }
                        if (X < myBitmap.Width && yLocation < myBitmap.Height)
                        {
                            newBitmap.SetPixel(X, yLocation, _col);
                        }
                        yLocation++;
                        currentPixel++;
                    }
                }
                pictureBox1.Image = newBitmap;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (newBitmap != null)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save Image File";
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "jpg";
                dialog.Filter = "JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    newBitmap.Save(dialog.FileName + ".jpg", ImageFormat.Jpeg);
                }
            }
        }
    }
}
