using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGauss
{
    internal class BlurCS : BlurInterface
    {
        public BlurCS(int _imageWidth, int _imageHeight, int _startIndex, int _endIndex) : base(_imageWidth, _imageHeight, _startIndex, _endIndex)
        {
        }

        public static double[,] GaussianBlurKernelDefault(int length, double weight)
        {
            double[,] kernel = new double[length, length];
            double kernelSum = 0;
            int foff = (length - 1) / 2;
            double distance = 0;
            double constant = 1d / (2 * Math.PI * weight * weight);
            for (int y = -foff; y <= foff; y++)
            {
                for (int x = -foff; x <= foff; x++)
                {
                    distance = ((y * y) + (x * x)) / (2 * weight * weight);
                    kernel[y + foff, x + foff] = constant * Math.Exp(-distance);
                    kernelSum += kernel[y + foff, x + foff];
                }
            }
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    kernel[y, x] = kernel[y, x] * 1d / kernelSum;
                }
            }
            return kernel;
        }
        //later convert to dll
        private void gaussianBlurCS(float[] pixels, float[] output, int _imageWidth, int _imageHeight, int _startIndex, int _endIndex, double[,] _kernel)
        {
            const int magic = 4;
            int stride = _imageWidth * magic; // ilość pixeli w rzędzie
            double[] rgb = new double[3]; // tablica trójki RGB
            int kernelBorder = (_kernel.GetLength(0) - 1) / 2; // liczba pixeli od środka jądra do jego granicy

            if(endIndex == _imageHeight)
            {
                endIndex-=kernelBorder;
            }
            for (int y = _startIndex+1; y < endIndex; y++) //Iterowanie po wysokosci obrazu
            {
                for (int x = 1; x < imageWidth - kernelBorder; x++) //Iterowanie po szerokości obrazu
                {
                    rgb[0] = 0.0; //zerowanie wartości tablicy
                    rgb[1] = 0.0;
                    rgb[2] = 0.0;

                    int pixelPosition = y * stride + x * magic;
                    for (int fy = -kernelBorder; fy < kernelBorder + 1; fy++) //Iterowanie po wysokosci jądra
                    {
                        for (int fx = -kernelBorder; fx < kernelBorder + 1; fx++) //Iterowanie po szerokości jądra
                        {
                            int kpixel = pixelPosition + fy * stride + fx * magic;
                            rgb[0] += pixels[kpixel] * _kernel[fy + kernelBorder, fx + kernelBorder]; //Sunowanie wartości Red pixeli z określioną wagą
                            rgb[1] += pixels[kpixel + 1] * _kernel[fy + kernelBorder, fx + kernelBorder]; //Sunowanie wartości Green pixeli z określioną wagą
                            rgb[2] += pixels[kpixel + 2] * _kernel[fy + kernelBorder, fx + kernelBorder]; //Sunowanie wartości Blue pixeli z określioną wagą
                        }
                    }
                    output[pixelPosition + 0] = (float)rgb[0]; //Przypisanie obliczonych wartości do pixela w tablicy wyjsciowej.
                    output[pixelPosition + 1] = (float)rgb[1];
                    output[pixelPosition + 2] = (float)rgb[2];
                }
            }


        }
        public override void Blur(float[] pixels, float[] output, double[,] kernel)
        {
            
            gaussianBlurCS(pixels, output, imageWidth, imageHeight, startIndex, endIndex, kernel);
        }
    }
}
