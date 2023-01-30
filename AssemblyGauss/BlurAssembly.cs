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

        public override void Blur(float[] pixels, ref float[] output, double[,] kernel)
        {
            
            byte[] bytes = output.ToByteArray();
            byte[] bytes2 = output.ToByteArray();
            int size = imageHeight * imageWidth/4;
            gaussianBlur(bytes, size, imageWidth, startIndex,endIndex-1);
            output = bytes.ToFloatArray();
            bool x = pixels == output;
        }
    }
}
