using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoNesLoader
{

    public class NesRom
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

        public byte[] RomContents { get; private set; }

        public RomType ValidRom { get; private set; }

        public string Name { get; private set; }

        public NesRom(string fileName)
        {
            ValidRom = RomType.Valid;
            Name = Path.GetFileName(fileName);
            RomContents = File.ReadAllBytes(fileName);
            if (RomContents[0] == 'N' &&
                RomContents[1] == 'E' &&
                RomContents[2] == 'S' &&
                RomContents[3] == 0x1A)
            {
                int mapper = RomContents[6] >> 4;
                var valid = ValidMappers.Where(x => x == mapper);
                if (valid.Count() > 0)
                {
                    ValidRom = RomType.InvalidMapper;
                }


            } else
            {
                ValidRom = RomType.NoRom;
            }


        }
    }
}
