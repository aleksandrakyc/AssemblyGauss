using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGauss
{
    public abstract class BlurInterface
    {
        //stuff needed to execute effect
        protected int imageWidth;
        protected int imageHeight;
        protected int startIndex;
        protected int endIndex;
        //protected double[,] kernel;
        //constructor
        public BlurInterface(int _imageWidth, int _imageHeight, int _startIndex, int _endIndex)
        {
            this.imageWidth = _imageWidth;
            this.imageHeight = _imageHeight;
            this.startIndex = _startIndex;
            this.endIndex = _endIndex;

        }
        //method executing effect
        public abstract void Blur(float[] pixels, ref float[] output, double[,] kernel);
    }
}
