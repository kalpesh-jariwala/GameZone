using BLL_Core.Infrastructure.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.ModelClassess
{
    public class BaseInput
    {
        public BaseInput()
        {
            Errors = new List<string>();
            PageNumber = 1;
            PageSize = 5;
        }



        public string Id { get; set; }
        public string RavenId
        {
            get { return Id.AddCollectionName("CommunityDocuments"); }

        }

        public string Slug { get; set; }
        public string InnerId { get; set; }
        public string InnerName { get; set; }
        public string ViewTab { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Username { get; set; }
        public string MemberId { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public string NameFilter { get; set; }
        public string SearchTerm { get; set; }
    }
}
