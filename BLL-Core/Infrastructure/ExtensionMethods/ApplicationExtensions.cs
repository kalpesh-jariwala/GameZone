using BLL_Core.Exception;
using BLL_Core.ModelClassess;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class ApplicationExtensions
    {
        public static int GetNumberOfRounds(this int numberofparticipants)
        {
            return (int)(Math.Ceiling(Math.Log(numberofparticipants, 2)));
        }

        public static void ValidateRouteInfo(this BaseInput routeInfo)
        {
            routeInfo.ValidateRouteInfo(false);
        }

        public static void ValidateRouteInfo(this BaseInput routeInfo, bool throwIfNullInnerId, bool throwIfNullUsername=true)
        {
            if (routeInfo.Id.IsNullOrEmpty())
            {
                throw new BaseRouteInfoException("No Community Id");
            }
            if (routeInfo.Slug.IsNullOrEmpty())
            {
                throw new BaseRouteInfoException("No Community Slug");
                
            }
            if (throwIfNullUsername && routeInfo.Username.IsNullOrEmpty())
            {
                throw new BaseRouteInfoException("No Username");
            }
            if (throwIfNullInnerId && routeInfo.InnerId.IsNullOrEmpty())
            {
                throw new BaseRouteInfoException("No Community Inner Id");
            }
        }

        public static T Clone<T>(this T source)
        {
            if (!typeof (T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }

        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(T));
                dcs.WriteObject(stream, a);
                stream.Position = 0;
                return (T)dcs.ReadObject(stream);
            }
        }

    }
}
