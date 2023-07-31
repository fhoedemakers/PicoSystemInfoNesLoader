using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoNesLoader
{
    /// <summary>
    /// Object that contains properties of a connected Pico
    /// </summary>
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

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="info">Output of Picotool info -a</param>
        public RP2040(string[] info)
        {
           
            int index;
  
            index = Array.FindIndex(info, row => row.Contains("name:"));
            if ( index == -1)
            {
                throw new Exception("name: not found in picotool output.");
            }   
            ProgramName = (info[index].Split(":")[1]).Trim();

            index = Array.FindIndex(info, row => row.Contains("version:"));
            if ( index == -1)
            {
                throw new Exception("version: not found in picotool output.");
            }
            ProgramVersion = (info[index].Split(":")[1]).Trim();

            // binary start: 0x10000000
            index = Array.FindIndex(info, row => row.Contains("binary start:"));
            if ( index == -1)
            {
                throw new Exception("binary start: not found in picotool output.");
            }
            ProgramBinaryStartHex = (info[index].Split(":"))[1].Trim().Substring(2);
            ProgramBinaryStart = Int32.Parse(ProgramBinaryStartHex, System.Globalization.NumberStyles.HexNumber);

            // binary end:   0x1000c000
            index = Array.FindIndex(info, row => row.Contains("binary end:"));
            if ( index == -1)
            {
                throw new Exception("binary end: not found in picotool output.");
            }
            ProgramBinaryEndHex = (info[index].Split(":"))[1].Trim().Substring(2);
            ProgramBinaryEnd = Int32.Parse(ProgramBinaryEndHex, System.Globalization.NumberStyles.HexNumber);

            // flash size:   16384K
            index = Array.FindIndex(info, row => row.Contains("flash size:"));
            if ( index == -1)
            {
                throw new Exception("flash size: not found in picotool output.");
            }
            var  tmpStr = (info[index].Split(":"))[1].Trim();
            tmpStr = tmpStr.Substring(0, tmpStr.Length - 1);
            FlashSizeInKBytes = int.Parse(tmpStr);
            FlashSizeBytes = FlashSizeInKBytes * 1024;
        }
    }
}
