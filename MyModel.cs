using ATPproject.Huffman;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ATPproject
{

    internal class MyModel 
    {

        // Fields
        private HuffmanAlgorithm m_Huffman;
        private string Return_Statment;
        /// <summary>
        /// Cunstractur
        /// </summary>
        /// <param name="controller">IController for the MVC architectural pattern  </param>
        public MyModel()
        {
            m_Huffman = new HuffmanAlgorithm();
            Return_Statment = "";
        }
        /// <summary>
        /// Display a list of files and subfolders
        /// </summary>
        /// <param name="parameters">Command , Path</param>
        /// <returns>string with all the files listed in that directury</returns>
     
        public string DoHuf(params string[] parameters)
        {
            //    string path = parameters[1].Remove(parameters[1].IndexOf("."), parameters[1].Length - parameters[1].IndexOf("."));
            try
            {
                File.WriteAllBytes(parameters[1] + ".huf", m_Huffman.Compress(FileToByteArray(parameters[1])));
                Return_Statment = string.Format("huf compress for {0} success", parameters[1]);
            }
            catch
            {
                Return_Statment = "Error acordes using huf";
            }
            return Return_Statment;
        }
        /// <summary>
        /// DeCompressing a file using HuffmanAlgorithm
        /// </summary>
        /// <param name="parameters">Command , Path of .huf file</param>
        /// <returns>Success/Failed</returns>
        public string DoUnhuf(params string[] parameters)
        {
            try
            {
                string Path = parameters[1];
                string[] name = Path.Split('.');
                name[0] += "After_Unhuf.";
                name[0] += name[1];
                string s = parameters[1].Remove(parameters[1].Length - 4);

                File.WriteAllBytes(name[0], m_Huffman.Decompress(FileToByteArray(parameters[1])));
                Return_Statment = string.Format("unhuf Decompress for {0} success", parameters[1]);
            }
            catch
            {
                Return_Statment = "The process failed";

            }
            return Return_Statment;
        }
        /// <summary>
        /// Return the size of the File In the Givin path
        /// </summary>
        /// <param name="parameters">Command , Path</param>
        /// <returns>Return the size of the File In the Givin path</returns>
        public string DoSize(params string[] parameters)
        {
            try
            {
                long length = new System.IO.FileInfo(parameters[1]).Length;
                Return_Statment = string.Format(" {0} KB" ,length);
            }
            catch
            {
                Return_Statment = string.Format("file '{0}'. Not Found", parameters[1]);
            }
            return Return_Statment;
        }
 
   
        /// <summary>
        /// Convert A file into Byte Array
        /// </summary>
        /// <param name="parameters">Path of the file to Convert</param>
        /// <returns>Byte[]</returns>
        public byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }
        /// <summary>
        /// Auxiliary function For the Dir Command
        /// Create the Outpot
        /// </summary>
        /// <param name="parameters">name and path of the files in dest</param>
        /// <returns>Success/Failed</</returns>
        private static string ProcessFile(string path)
        {
            path = path.Replace("@", System.Environment.NewLine);
            return path;
        }
    }
}