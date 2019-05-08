using BlastManagerWEB.Common;
using BlastManager.Service.Main.Common.BusinessEntity;
using Qilin.Web.Core.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using BlastManager.Service.Main.Common.CommonMainType;
using System;
using System.IO;
using Qilin.Core;
using Qilin.Core.Security;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using BlastManager.Service.Main.Common.ServiceProxy;
using System.Linq;

namespace BlastManagerWEB.Models.Users
{
    /// <summary>
    /// Access Control Detail Model
    /// </summary>
    public class AccessControlDetailModel : BaseModel<AccessControlDetailModel>
    {

        /// <summary>
        /// Site Map Name
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string SiteMapName { get; set; }

        /// <summary>
        /// Site Map Description
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string Description { get; set; }

        /// <summary>
        /// Site Map depth
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public int Depth { get; set; }

        /// <summary>
        /// Site Map minimum role type
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public int MinimumRoleType { get; set; }

        /// <summary>
        /// Current Site Map Id
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public int SiteMapId { get; set; }

        /// <summary>
        /// Dictionary of user role type enum to user role type name
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public Dictionary<string, string> DicRoleType { get; set; }

        /// <summary>
        /// List of user groups
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public List<UserRole> UserGroups { get; set; }

        /// <summary>
        /// Selected user role id
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public int? SelectedUserRoleId { get; set; }

        /// <summary>
        /// User role drop down list
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public List<SelectListItem> UserRoleDropdownList { get; set; }

        /// <summary>
        /// Dictionary of user role id to UserRole entity
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public Dictionary<string, UserRole> DicUserRole { get; set; }

        /// <summary>
        /// Selected user role id list
        /// </summary>
        public string UserRoleIdList { get; set; }

        /// <summary>
        /// OrganizationDropdownList drop down list
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public List<SelectListItem> OrganizationDropdownList { get; set; }

        /// <summary>
        /// Updated date
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public System.Guid OrganizationGUID { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AccessControlDetailModel()
        {
            UserRoleDropdownList = new List<SelectListItem>();
            DicUserRole = new Dictionary<string, UserRole>();
        }

        /// <summary>
        /// Populate the user role id to UserRole entity dictionary
        /// </summary>
        public void SetUserRoleList(int roleType)
        {
            UserRoleDropdownList.Add(new SelectListItem() { Value = "", Text = "Please select user group", Selected = true });
            using (var proxy = new UserProxy())
            {
                List<UserRole> List = proxy.GetAllUserGroupList().Where(ud => ud.RoleType <= roleType).ToList();
                foreach (UserRole item in List)
                {
                    UserRoleDropdownList.Add(new SelectListItem() { Value = StringHelper.ToString(item.UserRoleId), Text = item.RoleName });
                    DicUserRole.Add(StringHelper.ToString(item.UserRoleId), item);
                }
            }
            using (var proxy = new UserProxy())
            {
                OrganizationDropdownList = new List<SelectListItem>();
                OrganizationDropdownList.Add(new SelectListItem() { Value = "", Text = "Please select Organisation", Selected = true });
                List<Organisation> List = proxy.GetAllOrganizationList().ToList();
                foreach (Organisation item in List)
                {
                    OrganizationDropdownList.Add(new SelectListItem() { Value = Convert.ToString(item.Organisation_GUID), Text = item.OrganisationName });
                }

            }
        }

       
    }
}