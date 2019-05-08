using BlastManagerWEB.Common;
using BlastManager.Service.Main.Common.BusinessEntity;
using Qilin.Web.Core.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using BlastManager.Service.Main.Common.CommonMainType;
using System;
using BlastManager.Service.Main.Common.ServiceProxy;
using System.Collections;
using System.Linq;
namespace BlastManagerWEB.Models.Users
{
    /// <summary>
    /// User detail model
    /// </summary>
    public class UserDetailModel : BaseModel<UserDetailModel>
    {
        /// <summary>
        /// Check if windows authentication
        /// </summary>
        public bool WindowsAuth
        {
            get
            {
                return Constants.WindowsAuthentication;
            }
        }

        /// <summary>
        /// Check if need password
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public bool NeedPassword
        {
            get
            {
                return string.IsNullOrWhiteSpace(UserId) && !Constants.WindowsAuthentication;
            }
        }

        /// <summary>
        /// Windows Account
        /// </summary>
        [Display(Name = "Windows Account")]
        public string WindowsAccount { get; set; }

        private string _userId;
        /// <summary>
        /// User id
        /// </summary>
        [Display(Name = "User Id")]
        public string UserId
        {
            get
            {
                if (Constants.WindowsAuthentication)
                {
                    // Hack this, or some validations will be failed
                    return "windows";
                }
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        /// <summary>
        /// User name
        /// </summary>
        [Display(Name = "User Display Name")]
        public string UserName { get; set; }





        /// <summary>
        /// UserLastName
        /// </summary>
        [Display(Name = "User Last Name")]
        public string UserLastName { get; set; }

        /// <summary>
        /// Is AD User flag
        /// </summary>
        [Display(Name = "Is AD User")]
        public bool IsADUser { get; set; }

        /*
         * 
         * Check password:
         * 1. Required
         * 2. Min length is 8, should be configured in system parameters
         * 3. Max length is 30, should be configured in system parameters
         * 4. Cannot be same with user id
         * 5. Cannot be same with old password
         * 6. New Password must be same with Confirm Password
         * 7. Must meet password policy
         * 8. Cannot find in top 10 password history, check by controller
         * 
         */

        private System.Security.SecureString _password = null;
        private System.Security.SecureString _confirmPassword = null;

        /// <summary>
        /// Password, should ignore if manage user
        /// </summary>
        [Display(Name = "Password")]
        public string Password
        {
            get
            {
                return Utilities.RetrieveString(_password);
            }
            set
            {
                _password = Utilities.ConvertToSecureString(value);
            }
        }

        /// <summary>
        /// Confirm password
        /// </summary>
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword
        {
            get
            {
                return Utilities.RetrieveString(_confirmPassword);
            }
            set
            {
                _confirmPassword = Utilities.ConvertToSecureString(value);
            }
        }

        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Office Number
        /// </summary>
        [Display(Name = "Office Number")]
        public string OfficeNumber { get; set; }

        /// <summary>
        /// Is Department Admin
        /// </summary>
        [Display(Name = "Is Department Admin")]
        public bool IsDeptAdmin { get; set; }

        /// <summary>
        /// Change password next login
        /// </summary>
        [Display(Name = "Change Password Next Login")]
        public bool ChangePasswordNextLogin { get; set; }

        /// <summary>
        /// Password never expire
        /// </summary>
        [Display(Name = "Password Never Expire")]
        public bool PasswordNeverExpire { get; set; }



        /// <summary>
        /// IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Created by
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
         [Display(Name = "UserCreatedDate")]
        [System.Runtime.Serialization.IgnoreDataMember]
        public string CreatedDate { get; set; }

        /// <summary>
        /// Updated by
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Updated date
        /// </summary>
        [Display(Name = "UserUpdatedDate")]
        [System.Runtime.Serialization.IgnoreDataMember]
     
        public string UpdatedDate { get; set; }


        /// <summary>
        /// Language
        /// </summary>
        [Display(Name = "Language")]
        [System.Runtime.Serialization.IgnoreDataMember]

        public int Language { get; set; }

        /// <summary>
        /// Updated date
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public System.Guid OrganizationGUID { get; set; }


        /// <summary>
        /// User group list
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public IList<UserRole> UserGroupList { get; set; }

        /// <summary>
        /// Selected user group id list
        /// </summary>
        public string SelectedUserGroupIdList { get; set; }

        /// <summary>
        /// OrganizationDropdownList drop down list
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public List<SelectListItem> OrganizationDropdownList { get; set; }


        /// <summary>
        /// OrganizationDropdownList drop down list
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public List<SelectListItem> BlastManagerWebSettingsDropdownList { get; set; }

        /// <summary>
        /// Role type id
        /// <remarks>Actually we can use Range to validate</remarks>
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public int BlastManagerWebSettingID { get; set; }

        /// <summary>
        /// User detail model constructor
        /// </summary>
        public UserDetailModel()
        {
            ChangePasswordNextLogin = true;
            using (var proxy = new UserProxy())
            {
                OrganizationDropdownList = new List<SelectListItem>();
                UserGroupList = proxy.GetAllUserGroupList().ToList();
                OrganizationDropdownList.Add(new SelectListItem() { Value = "", Text = "Please select Organisation", Selected = true });
                List<Organisation> List = proxy.GetAllOrganizationList().ToList();
                foreach (Organisation item in List)
                {
                    OrganizationDropdownList.Add(new SelectListItem() { Value = Convert.ToString(item.Organisation_GUID), Text = item.OrganisationName });
                }

            }

            BlastManagerWebSettingsDropdownList = new List<SelectListItem>();

            using (var proxy = new CommonProxy())
            {
                BlastManagerWebSettingsDropdownList.Add(new SelectListItem() { Value = "", Text = "Please select Language", Selected = true });
                List<SystemType> List = proxy.GetSystemTypebyLanguage(Constants.SystemTypeCategory.Language);
                foreach (SystemType item in List)
                {
                    BlastManagerWebSettingsDropdownList.Add(new SelectListItem() { Value = Convert.ToString(item.TypeKey), Text = item.DisplayName });
                }
            }
            //foreach (RoleType en in Enum.GetValues(typeof(RoleType)))
            //{
            //    status = Utilities.GetSystemTypeName(Constants.SystemTypeCategory.Language, (int)en);
            //    if (!string.IsNullOrWhiteSpace(status))
            //    {
            //        BlastManagerWebSettingsDropdownList.Add(new SelectListItem() { Text = status, Value = Convert.ToString((int)en) });
            //    }
            //}
        }
    }
}

