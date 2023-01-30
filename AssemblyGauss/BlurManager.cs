using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AssemblyGauss
{
    class BlurManager
    {
        private BitmapSource oldBitmap;
        private int numberOfThreads;
        private float[] allPixels;
        private float[] output;
        private List<BlurInterface> blurInterfaces = new List<BlurInterface>();
        private List<Task> tasks = new List<Task>();

        public BlurManager(BitmapSource oldBitmap, BlurType type, int numberOfThreads)
        {
            this.oldBitmap = oldBitmap;
            this.numberOfThreads = numberOfThreads;
            double[,] kernel;
            allPixels = oldBitmap.ToBmpBGRArray();
            output = allPixels;
            //add to time?
            kernel = BlurCS.GaussianBlurKernelDefault(3, 1);
            
            //something wrong with the thread assigning - use modulo

            int onePiece = countHeight(numberOfThreads, oldBitmap.PixelHeight);
            for(int i = 0; i<numberOfThreads; i++)
            {
                int partNum = i;
                int pieceBegin = partNum * onePiece;
                int pieceEnd = (partNum + 1) * onePiece;
                if (partNum + 1 == numberOfThreads)
                    pieceEnd = oldBitmap.PixelHeight-1;
                if (partNum == 0)
                    pieceBegin = 1;

                blurInterfaces.Add(BlurFactory.Create(
                type,
                oldBitmap.PixelWidth,
                oldBitmap.PixelHeight,
                pieceBegin,
                pieceEnd
                ));
                tasks.Add(new Task(() => blurInterfaces[partNum].Blur(allPixels, ref output, kernel)));
            }
        }
        private int countPiece(int numberOfThreads, int height, int width)
        {
            int piece = height*width / numberOfThreads;
            while (piece % (oldBitmap.Format.BitsPerPixel / 8) != 0)
                piece++;
            return piece;
        }

        private int countHeight(int numberOfThreads, int height)
        {
            return height/numberOfThreads;
        }
        public BitmapSource Run(out System.TimeSpan time)
        {
            //stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Parallel.ForEach(tasks, (task) => task.Start());
            Task.WaitAll(tasks.ToArray());

            stopwatch.Stop();
            time = stopwatch.Elapsed;

            return output.BmpBGRArrayToImage(oldBitmap.PixelWidth,
                oldBitmap.PixelHeight, oldBitmap.Format);
        }
    }
}
