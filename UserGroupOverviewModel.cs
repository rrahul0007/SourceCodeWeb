using BlastManagerWEB.Common;
using BlastManager.Service.Main.Common.BusinessEntity;
using Qilin.Web.Core.Models;
using Qilin.Core;
using Qilin.Web.Helper.Validators;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using BlastManager.Service.Main.Common.CommonMainType;
using BlastManager.Service.Main.Common.ServiceProxy;
using System;
namespace BlastManagerWEB.Models.Users
{
    /// <summary>
    /// UserGroupOverviewModel
    /// </summary>
    public class UserGroupOverviewModel : BaseModel<UserGroupOverviewModel>
    {
        /// <summary>
        /// User group name
        /// </summary>
        [Display(Name = "User Group Name")]
        [QilinStringMaxLength(MaximumLength = 100,
            ErrorMessageResourceName = "The max length is {0}.")]
        public string UserGroupName { get; set; }

        /// <summary>
        /// Role type id
        /// <remarks>Actually we can use Range to validate</remarks>
        /// </summary>
        [Display(Name = "Role Type")]
        public int? RoleTypeId { get; set; }
        /// <summary>
        /// Role type list
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public List<SelectListItem> RoleTypeList { get; private set; }

        /// <summary>
        /// Search result
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public ICollection<UserRole> SearchResult { get; set; }

        /// <summary>
        /// Pager information
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public Pager Pagers { get; set; }

        /// <summary>
        /// Language
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public int Language { get; set; }

        /// <summary>
        /// LanguageTypeName
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string LanguageTypeName { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string UserName { get; set; }

        


        /// <summary>
        /// User group model constructor
        /// </summary>
        public UserGroupOverviewModel()
        {

            RoleTypeList = new List<SelectListItem>();
            RoleTypeList.Add(new SelectListItem() { Value = "", Text = "All" });
            using (var proxy = new CommonProxy())
            {
                List<SystemType> List = proxy.GetSystemTypebyRoleType(Constants.SystemTypeCategory.RoleType);
                foreach (SystemType item in List)
                {
                    RoleTypeList.Add(new SelectListItem() { Value = Convert.ToString(item.SystemTypeId), Text = item.DisplayName });
                }
            }
        }
    }
}