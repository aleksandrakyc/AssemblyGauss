using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGauss
{
    internal class BlurAssembly : BlurInterface
    {
        public BlurAssembly(int _imageWidth, int _imageHeight, int _startIndex, int _endIndex) : base(_imageWidth, _imageHeight, _startIndex, _endIndex)
        {
        }
        [DllImport(@"D:\Coding\AssemblyGauss\x64\Debug\GaussDll.dll")]
        private static extern void gaussianBlur(byte[] input, int size, int width, int start, int end);

        public override void Blur(float[] pixels, float[] output, double[,] kernel)
        {
            
            byte[] bytes = output.ToByteArray();
            int size = imageHeight * imageWidth;
            gaussianBlur(bytes, size, imageWidth, startIndex+1,endIndex-1);
            pixels = bytes.ToFloatArray();
            
        }
    }
}
