using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPproject.Huffman
{
    internal class HChar : IComparable<HChar>
    {
        #region Fields

        
        private int m_count;
        private string m_binaryRepresentation;
        private HChar m_left;
        private HChar m_right;
        private byte m_character;
        #endregion

        #region Cunstractur

        public HChar()
        {
        }
     
        public HChar(byte character, int p)
        {
            this.m_character = character;
            this.m_count = p;
        }
        #endregion

        #region Setters

        internal void SetCount(int count)
        {
            m_count = count;
        }

        internal void SetLeft(HChar first)
        {
            this.m_left = first;
        }

        internal void SetRight(HChar second)
        {
            this.m_right = second;
        }

        internal void SetBinaryRepresentation(string bin)
        {
            m_binaryRepresentation = bin;
        }


        #endregion

        #region Setters


        public int GetHashcode()
        {
            return m_character.GetHashCode();
        }

        internal int GetCount()
        {
            return m_count;
        }

        internal byte GetCharacter()
        {
            return m_character;
        }


        internal HChar GetLeft()
        {
            return m_left;
        }

        internal HChar GetRight()
        {
            return m_right;
        }

        internal string GetBinaryRepresentation()
        {
            return m_binaryRepresentation;
        }




        #endregion

        public int CompareTo(HChar other)
        {
            return m_count.CompareTo(other.GetCount());
        }

    }
}