using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoNesLoader
{
    internal class TarInspector
    {
        byte[] data;
        public TarInspector() { }

        int parseOct(int index, int count)
        {
            int v = 0;
            while (count > 0)
            {
                byte ch = data[index++];
                if (ch == 0 || ch == 0x20)
                {
                    return v;
                }

                if (ch < '0' || ch >= '8')
                {
                    return 0;
                }
                v <<= 3;
                v |= ch - '0';
                --count;
            }
            return v;
        }
        private bool isAll0(int index, int s)
        {
            while (s > 0)
            {
                if (data[index++] != 0)
                {
                    return false;
                }
                s--;
            }
            return true;
        }
        public TarInspector(string fileName)
        {
            data = File.ReadAllBytes(fileName);
            int index = 0;
            int size = 0;
            int totalsize = 0;
            byte[] bytes = new byte[100];
            while (true)
            {
                if (isAll0(index, 1024))
                {
                    break;
                }
                int tmpIndex = index + 257;
                if (data[tmpIndex] != 'u' ||
                    data[tmpIndex + 1] != 's' ||
                     data[tmpIndex + 2] != 't' ||
                      data[tmpIndex + 3] != 'a' ||
                       data[tmpIndex + 4] != 'r')
                {
                    throw new Exception("Not a tar file");
                }
                if ((size = parseOct(index + 124, 12)) == 0)
                {
                    throw new Exception("parseTAR: invalid size");
                }
                Buffer.BlockCopy(data, index, bytes, 0, bytes.Length);
                var filename = System.Text.Encoding.Default.GetString(bytes).TrimEnd((Char)0);
                var itemSize = (size + 512 + 511) & ~511;
                Debug.Print("{0} - f:{1} - a:{2}", filename, size,itemSize);
                totalsize += itemSize;
                index += itemSize;
            }
            totalsize += 1024;
            Debug.Print("Totalsize {0}", totalsize);
        }
    }
}
