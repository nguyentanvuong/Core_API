using Apache.NMS.Util;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;

namespace WebApi.Helpers.Utils
{
    public class O9Compression
    {
        public static string GetTextFromCompressBytes(byte[] _content)
        {
            string str = string.Empty;
            try
            {
                if (_content != null)
                    str = new EndianBinaryReader((Stream)new InflaterInputStream((Stream)new MemoryStream(_content))).ReadString32();
                return str;
            }
            catch (Exception)
            {
                return str;
            }
        }

        public static byte[] SetCompressText(string txt)
        {
            byte[] numArray = (byte[])null;
            if (txt != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream((Stream)ms);
                new EndianBinaryWriter((Stream)deflaterOutputStream).WriteString32(txt);
                deflaterOutputStream.Close();
                numArray = ms.ToArray();
                }
            }
            return numArray;
        }
    }
}
