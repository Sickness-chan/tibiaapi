using System;
using System.Text;
using Tibia.Objects;

namespace Tibia.Packets
{
    /// <summary>
    /// Class for building and parsing packets.
    /// </summary>
    public class PacketBuilder
    {
        public const int MaxLength = 8096;
        private byte[] data;
        private PacketType type;
        private int index = 0;

        #region Properties
        /// <summary>
        /// Get/Set the unencrypted bytes associated with this packetbuilder object.
        /// </summary>
        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// Get/Set the type of the packet (specified in the third byte of the data).
        /// </summary>
        public PacketType Type
        {
            get { return type; }
            set
            {
                type = value;
                if (data != null && data.Length > 3)
                {
                    data[0] = (byte)type;
                }
            }
        }

        /// <summary>
        /// Get/Set the current index in this packet. Set is the same as Seek(int).
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PacketBuilder()
        {
            data = new byte[MaxLength];
        }

        /// <summary>
        /// Start building a packet of the desired type.
        /// </summary>
        /// <param name="type"></param>
        public PacketBuilder(PacketType type)
            : this()
        {
            Type = type;
            index++;
        }

        /// <summary>
        /// Start parsing the given packet.
        /// </summary>
        /// <param name="packet"></param>
        public PacketBuilder(byte[] packet) : this(packet, 0, packet.Length) { }

        /// <summary>
        /// Start parsing the given packet.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="start"></param>
        public PacketBuilder(byte[] packet, int start) : this(packet, start, packet.Length - start) { }

        /// <summary>
        /// Start parsing the given packet.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public PacketBuilder(byte[] packet, int start, int length)
            : this()
        {
            Array.Copy(packet, start, data, 0, length);
        }
        #endregion

        #region Add
        /// <summary>
        /// Add a byte at the current index and advance.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int AddByte(byte b)
        {
            return AddBytes(new byte[] { b });
        }

        /// <summary>
        /// Add an array of bytes at the current index and advance.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int AddBytes(byte[] b)
        {
            return AddBytes(b, b.Length);
        }

        /// <summary>
        /// Add an array of bytes at the current index and advance.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public int AddBytes(byte[] b, int length)
        {
            return AddBytes(b, 0, length);
        }

        /// <summary>
        /// Add an array of bytes at the current index and advance.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="sourceIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public int AddBytes(byte[] b, int sourceIndex, int length)
        {
            Array.Copy(b, sourceIndex, data, index, length);
            index += length;
            return length;
        }

        /// <summary>
        /// Add an "integer" (aka. ushort) at the current index and advance.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int AddInt(int i)
        {
            return AddBytes(BitConverter.GetBytes((ushort)i));
        }

        /// <summary>
        /// Add a "long" (aka. 4 byte int) at the current index and advance.
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public int AddLong(int l)
        {
            return AddBytes(BitConverter.GetBytes(l));
        }

        /// <summary>
        /// Add a string at the current index and advance.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int AddString(string s)
        {
            return AddString(s, s.Length);
        }

        /// <summary>
        /// Add part of a string at the current index and advance.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public int AddString(string s, int length)
        {
            return AddBytes(Encoding.ASCII.GetBytes(s));
        }

        /// <summary>
        /// Add a location object at the current index and advance.
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        public int AddLocation(Location loc)
        {
            AddBytes(BitConverter.GetBytes((ushort)loc.X));
            AddBytes(BitConverter.GetBytes((ushort)loc.Y));
            return AddByte((byte)loc.Z);
        }
        #endregion

        #region Get
        /// <summary>
        /// Get the byte at the current index and advance.
        /// </summary>
        /// <returns></returns>
        public byte GetByte()
        {
            return data[index++];
        }

        /// <summary>
        /// Get an array of bytes at the current index and advance.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] GetBytes(int length)
        {
            byte[] b = new byte[length];
            Array.Copy(data, index, b, 0, length);
            index += length;
            return b;
        }

        /// <summary>
        /// Get an "int" (aka. 2 byte unsigned short) at the current index and advance.
        /// </summary>
        /// <returns></returns>
        public ushort GetInt()
        {
            ushort i = BitConverter.ToUInt16(data, index);
            index += 2;
            return i;
        }

        /// <summary>
        /// Get a "long" (aka. 4 byte integer) at the current index and advance.
        /// </summary>
        /// <returns></returns>
        public int GetLong()
        {
            int l = BitConverter.ToInt32(data, index);
            index += 4;
            return l;
        }

        /// <summary>
        /// Get a string at the current index and advance.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetString(int length)
        {
            string s = Encoding.ASCII.GetString(data, index, length);
            index += length;
            return s;
        }

        /// <summary>
        /// Get a location object at the current index and advance.
        /// </summary>
        /// <returns></returns>
        public Location GetLocation()
        {
            Location loc = new Location();
            loc.X = GetInt();
            loc.Y = GetInt();
            loc.Z = GetByte();
            return loc;
        }

        public Item GetItem()
        {
            return null;
        }

        /// <summary>
        /// Get the completed packet with the two byte length header attached.
        /// </summary>
        /// <returns></returns>
        public byte[] GetPacket()
        {
            byte[] b = new byte[index + 2];
            Array.Copy(BitConverter.GetBytes((short)index), b, 2);
            Array.Copy(data, 0, b, 2, index);
            return b;
        }
        #endregion

        #region Peek
        /// <summary>
        /// Get the byte at the current index.
        /// </summary>
        /// <returns></returns>
        public byte PeekByte()
        {
            return data[index++];
        }

        /// <summary>
        /// Get an array of bytes at the current index.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] PeekBytes(int length)
        {
            byte[] b = new byte[length];
            Array.Copy(data, index, b, 0, length);
            return b;
        }

        /// <summary>
        /// Get an "int" (aka. 2 byte unsigned short) at the current index.
        /// </summary>
        /// <returns></returns>
        public ushort PeekInt()
        {
            ushort i = BitConverter.ToUInt16(data, index);
            return i;
        }

        /// <summary>
        /// Get a "long" (aka. 4 byte integer) at the current index.
        /// </summary>
        /// <returns></returns>
        public int PeekLong()
        {
            int l = BitConverter.ToInt32(data, index);
            return l;
        }

        /// <summary>
        /// Get a string at the current index.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string PeekString(int length)
        {
            string s = Encoding.ASCII.GetString(data, index, length);
            return s;
        }

        /// <summary>
        /// Get a location object at the current index.
        /// </summary>
        /// <returns></returns>
        public Location PeekLocation()
        {
            Location loc = new Location();
            loc.X = GetInt();
            loc.Y = GetInt();
            loc.Z = GetByte();
            index -= 5;
            return loc;
        }
        #endregion

        #region Control
        /// <summary>
        /// Move the index to the specified value. Same as setting Index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int Seek(int index)
        {
            this.index = index;
            return index;
        }

        /// <summary>
        /// Skip the index ahead the specified amount of bytes.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public int Skip(int length)
        {
            index += length;
            return index;
        }
        #endregion
    }
}