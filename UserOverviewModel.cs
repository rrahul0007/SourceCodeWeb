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
    /// User overview model
    /// </summary>
    public class UserOverviewModel : BaseModel<UserOverviewModel>
    {
        /// <summary>
        /// User id
        /// </summary>
        [Display(Name = "User Id")]

        public string UserId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Search result
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public IList<BlastManager.Service.Main.Common.BusinessEntity.User> SearchResult { get; set; }

        /// <summary>
        /// Pager information
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public Pager Pagers { get; set; }


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
        /// User model constructor
        /// </summary>
        public UserOverviewModel()
        {
            BlastManagerWebSettingsDropdownList = new List<SelectListItem>();
            //BlastManagerWebSettingsDropdownList.Add(new SelectListItem() { Value = "", Text = "Please select" });

            string status = null;
            foreach (RoleType en in Enum.GetValues(typeof(RoleType)))
            {
                status = Utilities.GetSystemTypeName(Constants.SystemTypeCategory.Language, (int)en);
                if (!string.IsNullOrWhiteSpace(status))
                {
                    BlastManagerWebSettingsDropdownList.Add(new SelectListItem() { Text = status, Value = Convert.ToString((int)en) });
                }
            }

        }
    }
}