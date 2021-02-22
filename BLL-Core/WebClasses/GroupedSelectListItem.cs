using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace BLL_Core.WebClasses
{
    public class GroupedSelectListItem : SelectListItem
    {
        public string GroupKey { get; set; }
        public string GroupName { get; set; }
    }
}
