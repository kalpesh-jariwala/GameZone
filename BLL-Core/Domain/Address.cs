using BLL_Core.Infrastructure.ExtensionMethods;
using OpenActive.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.Domain
{
    public class Address
    {
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        /// <summary>
        /// To cater for US / Australia...large countries 
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// e.g Surrey, Nashville County..
        /// </summary>
        public string Region { get; set; }
        public string PostCode { get; set; }
        /// <summary>
        /// The country, expressed as a two-letter ISO 3166-1 alpha-2 country code.
        /// </summary>
        public string Country { get; set; }

        public override string ToString()
        {
            return StreetAddress + Environment.NewLine +
                   Region
                   + Environment.NewLine + (State.IsNotNullOrEmpty() ? string.Empty : State + Environment.NewLine)
                       + PostCode + Environment.NewLine + Country;
        }
        public PostalAddress ToPostalAddress()
        {
            if (this == null)
                return null;
            return new PostalAddress
            {
                AddressCountry = Country,
                StreetAddress = StreetAddress,
                AddressLocality = Locality,
                AddressRegion = Region,
                PostalCode = PostCode
            };
        }
    }
}
