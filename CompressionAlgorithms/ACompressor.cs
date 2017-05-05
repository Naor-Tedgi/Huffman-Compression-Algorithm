using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSrc.Model.CompressionAlgorithms
{
     public abstract class ACompressor : ICompressor
    {
        public abstract byte[] Compress(byte[] data);

        public abstract byte[] Decompress(byte[] compressedData);
    }
}