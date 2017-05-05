using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSrc.Model.CompressionAlgorithms
{
    public interface ICompressor
    {
       
        // interface of all Compressor Algorithm
      
    
        /// <summary>
        /// Compress Mathod
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Array of byte</returns>
   
        byte[] Compress(byte[] data);
        /// <summary>
        /// DeCompress Mathod
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Array of byte</returns>
        byte[] Decompress(byte[] compressedData);
    }
}