using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShirtColorChange
{
    public partial class Form1 : Form
    {
        private Bitmap originalImageCopy; 

        public Form1()
        {
            InitializeComponent();

            
            button1.Click += button1_Click;
            button2.Click += button2_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                   
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                    originalImageCopy = new Bitmap(openFileDialog.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null && originalImageCopy != null)
            {
               
                string colorCode = textBoxColorCode.Text;

               
                if (!string.IsNullOrWhiteSpace(colorCode))
                {
                    
                    if (ColorTranslator.FromHtml(colorCode) is Color targetColor)
                    {
                      
                        Bitmap newImage = new Bitmap(originalImageCopy);

                       
                        for (int x = 0; x < originalImageCopy.Width; x++)
                        {
                            for (int y = 0; y < originalImageCopy.Height; y++)
                            {
                             
                                Color pixelColor = originalImageCopy.GetPixel(x, y);

                               
                                float hue = targetColor.GetHue();
                                float saturation = pixelColor.GetSaturation();
                                float brightness = pixelColor.GetBrightness();

                                Color newPixelColor = ColorHelper.ChangeHue(pixelColor, hue);

                                newImage.SetPixel(x, y, newPixelColor);
                            }
                        }
                        pictureBox1.Image = newImage;
                    }
                    else
                    {
                        MessageBox.Show("Invalid color code. Please enter a valid HTML color code.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a color code.");
                }
            }
            else
            {
                MessageBox.Show("Please select an image first.");
            }
        }

    }

    public static class ColorHelper
    {
        public static Color ChangeHue(Color color, float targetHue)
        {
            float saturation = color.GetSaturation();
            float brightness = color.GetBrightness();

            return FromAhsb(color.A, targetHue, saturation, brightness);
        }

        private static Color FromAhsb(int alpha, float hue, float saturation, float brightness)
        {
            int r, g, b;

            if (saturation == 0)
            {
                r = g = b = (int)(brightness * 255.0 + 0.5);
            }
            else
            {
                hue /= 60; // Convert hue to the range [0, 6)
                int i = (int)Math.Floor(hue);
                float f = hue - i;

                float p = brightness * (1.0f - saturation);
                float q = brightness * (1.0f - saturation * f);
                float t = brightness * (1.0f - saturation * (1.0f - f));

                switch (i)
                {
                    case 0:
                        r = (int)(brightness * 255.0 + 0.5);
                        g = (int)(t * 255.0 + 0.5);
                        b = (int)(p * 255.0 + 0.5);
                        break;

                    case 1:
                        r = (int)(q * 255.0 + 0.5);
                        g = (int)(brightness * 255.0 + 0.5);
                        b = (int)(p * 255.0 + 0.5);
                        break;

                    case 2:
                        r = (int)(p * 255.0 + 0.5);
                        g = (int)(brightness * 255.0 + 0.5);
                        b = (int)(t * 255.0 + 0.5);
                        break;

                    case 3:
                        r = (int)(p * 255.0 + 0.5);
                        g = (int)(q * 255.0 + 0.5);
                        b = (int)(brightness * 255.0 + 0.5);
                        break;

                    case 4:
                        r = (int)(t * 255.0 + 0.5);
                        g = (int)(p * 255.0 + 0.5);
                        b = (int)(brightness * 255.0 + 0.5);
                        break;

                    case 5:
                        r = (int)(brightness * 255.0 + 0.5);
                        g = (int)(p * 255.0 + 0.5);
                        b = (int)(q * 255.0 + 0.5);
                        break;

                    default:
                        r = g = b = (int)(brightness * 255.0 + 0.5);
                        break;
                }
            }

            return Color.FromArgb(alpha, r, g, b);
        }
    }
}
