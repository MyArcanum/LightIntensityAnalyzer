using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightIntensityAnalyzer.Imaging
{
    public class Image : IEnumerable<Pixel>, IEnumerator<Pixel>
    {
        //public Image(Pixel[][] pixels)
        //{
        //    H = pixels.GetLength(0);
        //    W = pixels.GetLength(1);

        //    for (var i = 0; i < H; i++)
        //    {
        //        for (var j = 0; j < W; j++)
        //        {
        //            //var pixelPosition = i * W + j;
        //            Pixels.Append(new Pixel
        //            {
        //                B = buffer[pixelPosition],
        //                G = buffer[pixelPosition + 1],
        //                R = buffer[pixelPosition + 2],
        //                X = j,
        //                Y = i
        //            });
        //        }
        //    }
        //}

        public Image(int h, IEnumerable<Pixel> pixels)
        {
            var n = pixels.Count();
            if (n % h != 0)
                throw new FormatException("Can not determine the picture width. Insufficient pixels amount according to height.");
            W = n / h;
            H = h;
            Pixels = pixels;
        }

        //public Pixel this[int i] => Pixels.ElementAt(i);

        private IEnumerable<Pixel> Pixels;
        public readonly int H;
        public readonly int W;

        #region Enumrator + Enumerable

        private int Position = -1;

        public Pixel Current
        {
            get => Pixels.ElementAt(Position);
        }

        object IEnumerator.Current
        {
            get => Current;
        }

        public IEnumerator<Pixel> GetEnumerator() => this;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            Position++;
            return Position < Pixels.Count();
        }

        public void Reset()
        {
            Position = -1;
        }

        public void Dispose()
        {  }

        #endregion
    }
}
