using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyGauss
{
    static class BlurFactory
    {
        public static BlurInterface Create(BlurType blurType, int _imageWidth, int _imageHeight, int _startIndex, int _endIndex)
        {
            switch (blurType)
            {
                case BlurType.Assembly:
                    return new BlurAssembly(_imageWidth, _imageHeight, _startIndex, _endIndex); //to implement
                case BlurType.Cs:
                    return new BlurCS(_imageWidth, _imageHeight, _startIndex, _endIndex);
                default:
                    return null;
                    
            }
        }
    }
    
    enum BlurType
    {
        Undefined,
        Assembly,
        Cs
    }
}
