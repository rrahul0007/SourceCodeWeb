using BlastManagerWEB.Common;
using BlastManager.Service.Main.Common.BusinessEntity;
using Qilin.Web.Core.Models;
using Qilin.Core;
using Qilin.Core.Security;
using Qilin.Web.Helper.Validators;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using BlastManager.Service.Main.Common.CommonMainType;
using BlastManager.Service.Main.Common.ServiceProxy;
using System;
using System.Resources;
namespace BlastManagerWEB.Models.Users
{
    /// <summary>
    /// UserGroupDetailModel
    /// </summary>
    public class UserGroupDetailModel : BaseModel<UserGroupDetailModel>
    {
     
        /// <summary>
        /// User group id
        /// </summary>
        [Display(Name = "User Group Id")]
        [QilinDataType(Qilin.Web.Helper.Validators.DataType.Integer,
            ErrorMessageResourceName = "Invalid value.")]
        public int? UserGroupId { get; set; }

        /// <summary>
        /// User group name
        /// </summary>
        //[Display(Name = "UserGroupName", ResourceType = typeof(Resources.Resource))]
        //[QilinRequired(AllowWhiteSpace = false,
        //    ErrorMessageResourceName = "This field is required."),]
        //[QilinStringMaxLength(MaximumLength = 100,
        //    ErrorMessageResourceName = "The max length is {0}.")]
        //public string UserGroupName { get; set; }

        //[Display(Name = "UserGroupName", ResourceType = typeof(Properties.Resources))]
        //[Display(true,false,"UserGroupName","",)]

        /// <summary>
        /// User group name
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// Role type id
        /// <remarks>Actually we can use Range to validate</remarks>
        /// </summary>
        //[Display(Name = "RoleType"/*, ResourceType = typeof(Resources.Resource)*/)]
        //[QilinRequired(AllowWhiteSpace = false,
        //    ErrorMessageResourceName = "This field is required.")]
        //[QilinDataType(Qilin.Web.Helper.Validators.DataType.Integer,
        //    ErrorMessageResourceName = "Invalid value.")]
        public int? RoleTypeId { get; set; }
        /// <summary>
        /// Role type list
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public List<SelectListItem> RoleTypeList { get; private set; }

        /// <summary>
        /// Description
        /// </summary>
        //[Display(Name = "Description"/*, ResourceType = typeof(Resources.Resource)*/)]
        //[QilinRequired(AllowWhiteSpace = false,
        //    ErrorMessageResourceName = "This field is required.")]
        //[QilinStringMaxLength(MaximumLength = 500,
        //    ErrorMessageResourceName = "The max length is {0}.")]
        public string Description { get; set; }

        /// <summary>
        /// Created by
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string CreatedBy { get; set; }


        /// <summary>
        /// User_GuidID
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public Guid User_GuidID { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string Created { get; set; }

        /// <summary>
        /// Updated by
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Updated date
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public string Updated { get; set; }

        /// <summary>
        /// User list
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public ICollection<BlastManager.Service.Main.Common.BusinessEntity.User> UserList { get; set; }


        /// <summary>
        /// User group detail model constructor
        /// </summary>
        public UserGroupDetailModel()
        {
            RoleTypeList = new List<SelectListItem>();
            RoleTypeList.Add(new SelectListItem() { Value = "", Text = "Please select" });

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