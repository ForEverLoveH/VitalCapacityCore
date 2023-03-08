using System.Text;

namespace PLADCore.GameSystem
{
    public class ByteHelper
    {
        public static byte[] GetBytes(string parseString)
        {
            return Encoding.UTF8.GetBytes(parseString);
        }
        public static string ParseToString(byte[] utf8Bytes)
        {
            return Encoding.UTF8.GetString(utf8Bytes);
        }
    }
}