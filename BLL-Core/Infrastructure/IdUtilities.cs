using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL_Core.Infrastructure
{
    public class IdUtilities
    {
        private static int seedCounter = new Random().Next();

        [ThreadStatic]
        private static Random rng;

        public static Random Instance
        {
            get
            {
                if (rng == null)
                {
                    int seed = Interlocked.Increment(ref seedCounter);
                    rng = new Random(seed);
                }
                return rng;
            }
        }

        /// <summary>
        /// http://www.codeproject.com/Articles/14403/Generating-Unique-Keys-in-Net
        /// slighly altered to have longer random length but only lower case 
        /// </summary>
        /// <returns></returns>
        public static string GenerateRNGCryptoId()
        {

            int maxSize = 15;
            int minSize = 10;
            char[] chars = new char[36];
            string a;

            //a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            a = "abcdefghijklmnopqrstuvwxyz1234567890";
            chars = a.ToCharArray();
            //int size = maxSize;
            int size = Instance.Next(minSize, maxSize);
            byte[] data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            //size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }
        public static string GenerateRNGCryptoIdForRaven(string type)
        {
            return string.Format("{0}/{1}", type, GenerateRNGCryptoId());
        }
        public static string GenerateIdFromGuid()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }


        public static Guid GenerateComb()
        {
            byte[] destinationArray = Guid.NewGuid().ToByteArray();
            var time = new DateTime(0x76c, 1, 1);
            DateTime now = DateTime.Now;
            var span = new TimeSpan(now.Ticks - time.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes = BitConverter.GetBytes(span.Days);
            byte[] array = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes);
            Array.Reverse(array);
            Array.Copy(bytes, bytes.Length - 2, destinationArray, destinationArray.Length - 6, 2);
            Array.Copy(array, array.Length - 4, destinationArray, destinationArray.Length - 4, 4);
            return new Guid(destinationArray);
        }

        public static string GenerateCombForRaven(string type)
        {
            byte[] destinationArray = Guid.NewGuid().ToByteArray();
            var time = new DateTime(0x76c, 1, 1);
            DateTime now = DateTime.Now;
            var span = new TimeSpan(now.Ticks - time.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes = BitConverter.GetBytes(span.Days);
            byte[] array = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes);
            Array.Reverse(array);
            Array.Copy(bytes, bytes.Length - 2, destinationArray, destinationArray.Length - 6, 2);
            Array.Copy(array, array.Length - 4, destinationArray, destinationArray.Length - 4, 4);
            return string.Format("{0}/{1}", type, new Guid(destinationArray).ToString());
        }

        public static string GenerateDateId()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmssffff");
        }

        public static string GenerateDateIdForRaven(string type)
        {
            return string.Format("{0}/{1}", type, DateTime.Now.ToString("yyyyMMddHHmmssffff"));
        }

        //http://ayende.com/blog/4643/createsequetialuuid-answer
        private static int sequentialUuidCounter;
    }
}
