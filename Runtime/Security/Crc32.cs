using System.Linq;
using System.Text;

namespace CipherSphere.Runtime.Security
{
    internal static class Crc32
    {
        private static readonly uint[] Table = new uint[256];

        static Crc32()
        {
            const uint polynomial = 0xedb88320;
            for (uint i = 0; i < 256; i++)
            {
                var crc = i;
                for (var j = 0; j < 8; j++)
                {
                    crc = (crc & 1) == 1 ? (crc >> 1) ^ polynomial : crc >> 1;
                }
                Table[i] = crc;
            }
        }

        public static string Compute(string source)
        {
            var input = Encoding.UTF8.GetBytes(source);
            var crc = input.Aggregate(0xffffffff, (current, b) => (current >> 8) ^ Table[(current & 0xff) ^ b]);
            return (~crc).ToString("x8"); // ゼロ埋めの8桁の16進数
        }
    }
}