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
        private static extern void gaussianBlur(float[] input, float[] kernel, int width, int start, int end);

        public override void Blur(float[] pixels, ref float[] output, double[,] kernel)
        {

            //byte[] bytes = output.ToByteArray();
            //byte[] bytes2 = output.ToByteArray();
            //int size = imageHeight * imageWidth/4;

            var asmKernel = this.To1DArray(kernel);

            gaussianBlur(output, asmKernel, imageWidth, startIndex,endIndex);
            
            bool x = pixels == output;
        }

        float[] To1DArray(double[,] input)
        {
            // Step 1: get total size of 2D array, and allocate 1D array.
            float[] result = new float[12];

            float corner = (float)input[0, 0];
            float side = (float)input[1, 0];
            float center = (float)input[1, 1];

            
            // Step 2: copy 2D array elements into a 1D array.
            
            for (int i = 0; i < 4; i++)
            {
                result[i] = corner;
                result[i+4] = side;
                result[i+8] = center;
            }
            // Step 3: return the new array.
            return result;
        }
    }
}
