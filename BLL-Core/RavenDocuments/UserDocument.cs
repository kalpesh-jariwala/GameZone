using BLL_Core.Domain;
using BLL_Core.Infrastructure;
using BLL_Core.Infrastructure.ExtensionMethods;
using BLL_Core.ModelClassess.Account;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Core.RavenDocuments
{
    [Serializable]
    public class UserDocument
    {
        //Amjad - implement the Register-SendConfirmEmail-Finalise with this?
        #region constructors
        //Don't make any new constructors please! - BM
        public UserDocument() : this(IdUtilities.GenerateRNGCryptoIdForRaven("User"))
        {
            UserDevices = new List<UserMultipleDevices>();
            ProfilePhotos = new List<Photo>();
            LinkedAccountIds = new List<string>();

        }

        public UserDocument(string id)
        {
            Id = id;
            UserDevices = new List<UserMultipleDevices>();
            ProfilePhotos = new List<Photo>();
            LinkedAccountIds = new List<string>();
        }
        #endregion
        #region properties
        public string Id { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string IdStripped { get { return Id.StripCollectionName(); } }

        public string Name { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
        public string County { get; set; }
        public string Borough { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
      
        public string Password { get; set; }
        public string Salt { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime PasswordResetTokenExpiration { get; set; }
        public List<Photo> ProfilePhotos { get; set; }
        public bool EmailNotify { get; set; }
        public List<UserMultipleDevices> UserDevices { get; set; }
        public DateTime? AppFirstDownload { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public virtual ICollection<FlexOAuthAccount> OAuthAccounts { get; set; }
        //new fields
        public string Bio { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string Linkedin { get; set; }

        public string Location { get; set; }

        public List<String> LinkedAccountIds { get; set; }
        public bool OptInProduct { get; set; }

     
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<string> RelatedAccountIds { get; set; }
        public DateTime DateCreated { get; set; }

        public string CreatedFrom { get; set; }

        #endregion

        #region Sub-Classes
        public class UserMultipleDevices
        {
            public string DeviceOSType { get; set; }
            public string DeviceUDID { get; set; }
            public string DeviceToken { get; set; }
            public string ApplicationId { get; set; }
        }
        #endregion

        #region methods
        private bool SameInfo(UserDocument otherUser, bool ignoreEmail = false)
        {
            if (ignoreEmail) //only compares on email fields
                return false;
            if (otherUser == null)
                return false;
            if (otherUser.Username == Username)
                return true;
            if ((otherUser.Username == Username || otherUser.EmailAddress == EmailAddress || otherUser.EmailAddress == Username) &&
                otherUser.DateOfBirth == DateOfBirth && otherUser.Name.EqualContent(Name))
                return true;
            return false;
        }
        private bool SimilarInfo(UserDocument otherUser, bool ignoreEmail = false, int typoTolerance = 5)
        {
            //if ignoreEmail = true, just compares name and DoB
            if (otherUser == null)
                return false;
            if (!ignoreEmail && otherUser.Username == Username)
                return true;
            if (!ignoreEmail && otherUser.Username.DamerauLevenshteinDistance(Username) < typoTolerance)
                return true;
            if (!(otherUser.DateOfBirth == DateOfBirth || otherUser.DateOfBirth == null || DateOfBirth == null)) //both set and not equal
                return false;
            if (!otherUser.Name.EqualContent(Name) && otherUser.Name.DamerauLevenshteinDistance(Name) > typoTolerance)
                return false;
            if (!ignoreEmail)
            {
                var preAtOtherEmail = otherUser.EmailAddress?.Split('@').First();
                var preAtEmail = EmailAddress?.Split('@').First();
                var preAtOtherUsername = otherUser.Username?.Split('@').First();
                var preAtUsername = Username?.Split('@').First();
                var similarEmail = false;
                if (otherUser.EmailAddress == EmailAddress || otherUser.EmailAddress == Username ||
                    Username == otherUser.EmailAddress)
                    similarEmail = true;
                if (!similarEmail && preAtOtherEmail != null && ((preAtEmail != null &&
                    preAtOtherEmail.DamerauLevenshteinDistance(preAtEmail) < typoTolerance) || (preAtUsername != null &&
                    preAtOtherEmail.DamerauLevenshteinDistance(preAtUsername) < typoTolerance)))
                    similarEmail = true;
                if (!similarEmail && preAtEmail != null && preAtOtherUsername != null &&
                    preAtEmail.DamerauLevenshteinDistance(preAtOtherUsername) < typoTolerance)
                    similarEmail = true;
                if (!similarEmail)
                    return false;
            }
            return true;
        }
        public (bool sameInfo, bool similarInfo) CompareIdentifyingInfo(UserDocument otherUser, bool ignoreEmail = false)
        {
            if (this == null || otherUser == null)
                return (false, false);
            var sameInfo = SameInfo(otherUser, ignoreEmail);
            if (sameInfo)
                return (true, true);
            var similarInfo = SimilarInfo(otherUser, ignoreEmail);
            return (false, similarInfo);
        }
        #endregion
    }
}
