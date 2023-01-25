using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Imaging;
using Image = System.Drawing.Image;

namespace AssemblyGauss
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap pictureOriginal;
        public MainWindow()
        {
            InitializeComponent();
            slider.Value = System.Environment.ProcessorCount;
            //imageData = new GaussBlurifyer();
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            threadLabel.Content = "Choose number of threads: " + slider.Value.ToString();
        }

        //private bool isValid(string center, string side, string corner)
        //{
        //    double i,j,k;
        //    if(double.TryParse(center, out i)&& double.TryParse(side, out j)&& double.TryParse(corner, out k)&&i>=j&&j>=k)
        //    {
        //        return true;
        //    }
        //    else if(center.Equals("") && side.Equals("") && corner.Equals(""))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        private void buttonLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileWindow = new OpenFileDialog();
            openFileWindow.Title = "Wybierz obraz";
            openFileWindow.Filter = "Wspierane formaty|*.bmp|" +
              "BMP (*.bmp)|*.bmp";
            if (openFileWindow.ShowDialog() == true)
            {
                String directory = openFileWindow.FileName;
                try
                {
                    //imageBefore.Source = imageData.loadImageFromFile(directory);
                    //imageAfter.Source = null;
                    Image image = Image.FromFile(directory);
                    pictureOriginal = (Bitmap)image;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(directory);
                    bitmap.EndInit();
                    imageBefore.Source = bitmap;

                    buttonRun.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    buttonRun.IsEnabled = false;
                }
            }
            else
            {
                buttonRun.IsEnabled = false;
            }
        }

        private void buttonRun_Click(object sender, RoutedEventArgs e)
        {
            //check if parameters are ok and send them to class
            //get num of threads and do something about it
            //execute filter
            
            int threads = (int)slider.Value;
            
            
            BitmapSource source = BmpConverter.BitmapToBitmapSource(pictureOriginal);
            //int kernelSize = 3
            int iter;
            if (int.TryParse(iterText.Text, out iter) == false||iter<0||iter>5)
                { iter = 1; }
                
            TimeSpan time = TimeSpan.FromSeconds(0);
            BlurManager manager;
            BitmapSource output = source;
            for (int i = 0; i < iter; i++)
            {
                if (radioButtonASM.IsChecked == true)
                {
                    manager = new BlurManager(source, BlurType.Assembly, threads);
                }
                else
                {
                    manager = new BlurManager(source, BlurType.Cs, threads);
                }
                TimeSpan iterTime;
                output = manager.Run(out iterTime);
                source = output;
                time += iterTime;
            }
            
            
            //imageData.SaveFile();
            
            
            //convert bmap to bmapimage
            Bitmap bmap = BmpConverter.BitmapFromSource(output);
            imageAfter.Source = BmpConverter.ToBitmapImage(bmap);
            labelTime.Content = "Time elapsed: " + time.ToString();
        }
    }
}
