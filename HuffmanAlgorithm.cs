using ATPproject.Model.CompressionAlgorithms;
using DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ATPproject.Huffman
{
    internal class HuffmanAlgorithm : ACompressor
    {
        // Constants
        private Dictionary<int, HChar> charFrequency;
        private int node_counter;
        private BinaryHeap<HChar> PriorityQueue;

        /// <summary>
        /// Cunstractur
        /// </summary>
        public HuffmanAlgorithm()
        {
            charFrequency = new Dictionary<int, HChar>();
            node_counter = 0;
            PriorityQueue = new BinaryHeap<HChar>();
        }
        /// <summary>
        /// Count the amount of time a certen token apper in the Byte[] data
        ///
        /// </summary>
        /// <param name="data">Arry of byte of a given file</param>
        /// <returns>Return Dictinury with a Count As a Key and HChar As a Value</returns>
        private Dictionary<int, HChar> Get_Frequency(byte[] data)
        {
            Dictionary<int, HChar> charFrequency = new Dictionary<int, HChar>();
            HChar hchar;
            foreach (byte character in data)
            {
                if (!charFrequency.ContainsKey(character))
                {
                    hchar = new HChar(character, 1);
                    node_counter++;
                    charFrequency.Add(character, hchar);
                }
                else
                {
                    hchar = charFrequency[character];
                    hchar.SetCount(hchar.GetCount() + 1);
                }
            }

            return charFrequency;
        }
        /// <summary>
        /// Building the Huffman Tree Using A BinaryHeap
        /// </summary>
        /// <param name="charFrequency">Dictunery with the amount of every Val</param>
        /// <returns>The Huffman Tree </returns>
        private BinaryHeap<HChar> CreatePriorityQueue(Dictionary<int, HChar> charFrequency)
        {
            BinaryHeap<HChar> PriorityQueue = new BinaryHeap<HChar>();
            foreach (KeyValuePair<int, HChar> entry in charFrequency)
            {
                PriorityQueue.Add(entry.Value);
            }
            HChar first;
            HChar second;
            HChar newOne;
            while (PriorityQueue.Count > 1)
            {
                first = PriorityQueue.Remove();
                second = PriorityQueue.Remove();
                newOne = new HChar((byte)(first.GetCharacter() + second.GetCharacter()), first.GetCount() + second.GetCount());
                node_counter++;
                newOne.SetLeft(first);
                newOne.SetRight(second);
                PriorityQueue.Add(newOne);
            }
            return PriorityQueue;
        }


        /// <summary>
        /// Set the Binary Representation of a certen node in the huffman tree
        /// Using Recurse Better using on the Root
        /// </summary>
        /// <param name="node">A node on the Tree</param>
        /// <param name="bin"></param>
        private void SetBinRepDFS(HChar node, string bin)
        {
            node.SetBinaryRepresentation(bin);
            if (node.GetLeft() != null)
            {
                SetBinRepDFS(node.GetLeft(), node.GetBinaryRepresentation() + "0");
            } if (node.GetRight() != null)
            {
                SetBinRepDFS(node.GetRight(), node.GetBinaryRepresentation() + "1");
            }
        }
        /// <summary>
        /// The Compress Method Using Huffman Algorithm Tree
        /// Create a coded byte[] from the data Given
        /// </summary>
        /// <param name="data">File To Compress</param>
        /// <returns>coded byte[] from the data Given</returns>
        public override byte[] Compress(byte[] data)
        {
            charFrequency = Get_Frequency(data);
            BinaryHeap<HChar> PriorityQueue = CreatePriorityQueue(charFrequency);
            HChar root = PriorityQueue.Remove();
            SetBinRepDFS(root, "");

            //adding the paris to array
            //110100110000111
            ArrayList dictinary = new ArrayList();
            foreach (KeyValuePair<int, HChar> entry in charFrequency)
            {
                dictinary.Add(entry.Key);
                dictinary.Add(entry.Value.GetBinaryRepresentation());
            }

            // {1} size of dictinury
            byte[] dicSize = new byte[4];

            int m_idicSize = charFrequency.Count;
            dicSize = BitConverter.GetBytes(m_idicSize);

            // {2}
            byte[] m_arrDic, m_tempL, m_tempVal, m_tempKey;
            m_arrDic = new byte[0];
            foreach (var item in charFrequency)
            {
                m_tempKey = BitConverter.GetBytes(item.Value.GetCharacter());//6

                m_tempL = BitConverter.GetBytes(item.Value.GetBinaryRepresentation().Length);

                m_tempVal = GetBytes(item.Value.GetBinaryRepresentation());//0010

                m_arrDic = Combine(m_arrDic, m_tempKey, m_tempL, m_tempVal);
            }

            // {3}

            string totrans = "";
            for (int i = 0; i < data.Length; i++)
            {
                totrans += charFrequency[data[i]].GetBinaryRepresentation();
            }
            byte[] lang = new byte[4];

            lang = BitConverter.GetBytes(totrans.Length);

            byte[] arrByteCode = to_binary(totrans);
            //uinon {1}{2}{3}
            return Combine(dicSize, m_arrDic, lang, arrByteCode);
        }
        /// <summary>
        /// Auxiliary function Convert a string into Byte[]
        /// </summary>
        /// <param name="str">string of Binary term</param>
        /// <returns>Byte[] with the same Val As the string</returns>
        private static byte[] to_binary(string str)
        {
            List<byte> list = new List<byte>();
            string s = "";

            for (int i = 0; i < str.Length; i = i + 8)
            {
                try
                {
                    s = str.Substring(i, 8);
                }
                catch
                {
                    s = str.Substring(i, str.Length - i);
                    while (s.Length < 0)
                        s = "0" + s;
                }
                byte b = (byte)Convert.ToInt32(s, 2);
                list.Add(b);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Get The size of a given string in byte
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>Size of string in byte</returns>
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// Return a String From a Byte[]
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <returns>string</returns>
        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        /// <summary>
        /// Merge for All Amount of arrays 
        /// </summary>
        /// <param name="arrays">Amount of arryas</param>
        /// <returns>Byte[] of the combain of all  the params </returns>
        private byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
        /// <summary>
        /// The DeCompress Method Using Huffman Algorithm Tree
        /// Create a decoded byte[] from the data Given
        /// </summary>
        /// <param name="data">File To DeCompress</param>
        /// <returns>decoded byte[] from the data Given</returns>
        public override byte[] Decompress(byte[] compressedData)
        {
            //extrcting thr size of dic
            byte[] m_arrTempInt = new byte[4];
            int dicSize = BitConverter.ToInt32(compressedData, 0);

            //extrcting th dic, by int and string
            int m_tempKey;
            int length;
            byte[] strTemp = new byte[1];

            string m_sTemp;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            int postion = 4;
            for (int i = 0; i < dicSize; i++)
            {
                m_tempKey = BitConverter.ToInt16(compressedData, postion);
                postion += 2;
                length = BitConverter.ToInt32(compressedData, postion);
                postion += 4;
                strTemp = new byte[length * 2];
                m_sTemp = BitConverter.ToString(compressedData, postion, length * 2);
                Array.Copy(compressedData, postion, strTemp, 0, length * 2);
                m_sTemp = GetString(strTemp);
                postion += length * 2;
                dictionary.Add(m_sTemp, m_tempKey);
            }
            m_tempKey = BitConverter.ToInt32(compressedData, postion);
            postion += 4;
            strTemp = new byte[compressedData.Length - postion];

            Array.Copy(compressedData, postion, strTemp, 0, compressedData.Length - postion);
            string j = "";
            //string ToTrans = GetString(strTemp);
            string to_translate = "";
            for (int i = 0; i < strTemp.Length - 1; i++)
            {
                j = Convert.ToString(Convert.ToInt32(strTemp[i]), 2);
                while (j.Length < 8)
                {
                    j = "0" + j;
                }
                to_translate = to_translate + j;
            }

            // to_translate = to_translate + Convert.ToString(Convert.ToInt32(strTemp[strTemp.Length - 1]), 2);
            //translating the string
            j = Convert.ToString(Convert.ToInt32(strTemp[strTemp.Length - 1]), 2);
            while (to_translate.Length + j.Length < m_tempKey)
            { j = "0" + j; }
            to_translate = to_translate + j;
            string temp = "";
            List<byte> sol = new List<byte>();
            for (int i = 0; i < to_translate.Length; i++)
            {
                temp += to_translate[i];
                if (dictionary.ContainsKey(temp))
                {
                    byte b = (byte)dictionary[temp];
                    sol.Add(b);
                    temp = "";
                }
            }

            return sol.ToArray();
        }
    }
}