using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoNesLoader
{
    internal class RP2040
    {
        public string ProgramName { get; set; }
        public string ProgramVersion { get; set; }

        public string ProgramBinaryStartHex { get; set; }
        public string ProgramBinaryEndHex { get; set; }
        public long ProgramBinaryStart { get; set; }   
        public long ProgramBinaryEnd { get; set; }

        public long FlashSizeInKBytes  { get; set; }
        public long FlashSizeBytes { get; set; }

        public RP2040(string[] info)
        {
            ProgramName= (info[1].Split(":")[1]).Trim();
            ProgramVersion= (info[2].Split(":")[1]).Trim();
            ProgramBinaryStartHex = (info[4].Split(":"))[1].Trim().Substring(2);
            ProgramBinaryStart = Int32.Parse(ProgramBinaryStartHex, System.Globalization.NumberStyles.HexNumber);
            ProgramBinaryEndHex = (info[5].Split(":"))[1].Trim().Substring(2);
            ProgramBinaryEnd = Int32.Parse(ProgramBinaryEndHex, System.Globalization.NumberStyles.HexNumber);
            var  tmpStr = (info[19].Split(":"))[1].Trim();
            tmpStr = tmpStr.Substring(0, tmpStr.Length - 1);
            FlashSizeInKBytes = int.Parse(tmpStr);
            FlashSizeBytes = FlashSizeInKBytes * 1024;
        }
    }
}
