using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BLL_Core.Exception
{

    [Serializable]
    public class BaseRouteInfoException : System.Exception
    {
        public BaseRouteInfoException()
        {
        }

        public BaseRouteInfoException(string message)
            : base(message)
        {
        }

        public BaseRouteInfoException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public BaseRouteInfoException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public BaseRouteInfoException(string format, System.Exception innerException,
                                            params object[] args)
            : base(string.Format(format, args), innerException)
        {
        }

        protected BaseRouteInfoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
