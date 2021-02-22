using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.ModelClassess.Account
{
    public class FlexOAuthAccount
    {
        /// <summary>
        ///   Gets or sets the provider.
        /// </summary>
        /// <value> The provider. </value>
        public string Provider { get; set; }

        /// <summary>
        ///   Gets or sets the provider user id.
        /// </summary>
        /// <value> The provider user id. </value>
        public string ProviderUserId { get; set; }
    }
}
