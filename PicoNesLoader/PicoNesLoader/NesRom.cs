using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoNesLoader
{

    public class NesRom : IEquatable<NesRom>, IComparable<NesRom>
    {
        public enum RomType { Valid, NoRom, InvalidMapper };

        private readonly int[] ValidMappers = {0,
        1,
        2,
        3,
        4,
        6,
        7,
        8,
        9,
        10,
        11,
        13,
        15,
        16,
        17,
        18,
        19,
        21,
        22,
        23,
        24,
        25,
        26,
        32,
        33,
        34,
        40,
        41,
        42,
        43,
        44,
        45,
        46,
        47,
        48,
        49,
        50,
        51,
        57,
        58,
        60,
        61,
        62,
        64,
        65,
        66,
        67,
        68,
        69,
        70,
        71,
        72,
        73,
        74,
        75,
        76,
        77,
        78,
        79,
        80,
        82,
        83,
        86,
        87,
        88,
        89,
        90,
        91,
        92,
        93,
        94,
        95,
        96,
        97,
        100,
        101,
        105,
        107,
        108,
        109,
        110,
        112,
        113,
        114,
        115,
        116,
        117,
        118,
        119,
        122,
        133,
        134,
        135,
        140,
        151,
        160,
        180,
        181,
        182,
        183,
        185,
        187,
        188,
        189,
        191,
        193,
        194,
        200,
        201,
        202,
        222,
        225,
        226,
        227,
        228,
        229,
        230,
        231,
        232,
        233,
        234,
        235,
        236,
        240,
        241,
        242,
        243,
        244,
        245,
        246,
        248,
        249,
        251,
        252,
        255 };

        public int Mapper { get; set; }
        public long SizeInBytes { get; set; }

        public long SizeinKBytes { get { return SizeInBytes / 1024; } }

        public long SizeInTar
        {
            get
            {
                return (SizeInBytes + 512 + 511) & ~511;
            }
        }

        public RomType ValidRom { get; set; }

        public string Name { get; set; }

        public string FullpathName { get; set; }
        public NesRom(string fileName)
        {
            ValidRom = RomType.NoRom;
            Name = Path.GetFileName(fileName);
            FullpathName = fileName;


            // = File.ReadAllBytes(fileName);
            FileInfo fi = new FileInfo(fileName);
            SizeInBytes = fi.Length;
            byte[] RomHeader = new byte[7];
            if (SizeInBytes > RomHeader.Length)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var bytes_read = fs.Read(RomHeader, 0, RomHeader.Length);
                    if (bytes_read == RomHeader.Length)
                    {
                        if (RomHeader[0] == 'N' &&
                            RomHeader[1] == 'E' &&
                            RomHeader[2] == 'S' &&
                            RomHeader[3] == 0x1A)
                        {
                            Mapper = RomHeader[6] >> 4;
                            var valid = ValidMappers.Where(x => x == Mapper);
                            if (valid.Count() == 0)
                            {
                                ValidRom = RomType.InvalidMapper;
                            }
                            else
                            {
                                ValidRom = RomType.Valid;
                            }
                        }
                    }
                }

            }
        }
        public NesRom()
        {

        }

        public bool Equals(NesRom? other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return FullpathName.Equals(other.FullpathName);
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashProductName = FullpathName == null ? 0 : FullpathName.GetHashCode();
            //Calculate the hash code for the product.
            return hashProductName;
        }

        public int CompareTo(NesRom? other)
        {
            return FullpathName.CompareTo(other.FullpathName);
        }
    }

}
