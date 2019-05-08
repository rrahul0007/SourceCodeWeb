using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BlastManagerWEB.Models.Account;
using BlastManager.Service.Main.Common.CommonMainType;
using BlastManagerWEB.Common;
using BlastManager.Service.Main.Common.BusinessEntity;
using BlastManager.Service.Main.Backend.DataAccess;
using BlastManager.Service.Main.Common.ServiceProxy;
using System.Web.Security;
using Qilin.Web.Core.Context;
using Qilin.Web.Core.Models;
using BlastManagerWEB.Models.Users;
using System.Text;
using Qilin.Core.Web;
using Qilin.Web.Core.Controllers;
using Qilin.Core.Security;
using Qilin.Core;
using System.Globalization;
using System.Threading;
using BlastManagerWEB.Filters;
namespace BlastManagerWEB.Controllers.UsersController
{
    /// <summary>
    /// UsersController
    /// </summary>
     [AllowAnonymous]
    public class UsersController : Controller
    {
     
        /// <summary>
        /// Go to logon page
        /// </summary>

        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Controllers.User_UserGroup)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_UserGroup)]
        public ActionResult UserGroup(UserGroupOverviewModel model, int? start)
        {
       

            if (model == null)
            {
                model = new UserGroupOverviewModel();
            }

            User userInfo = @Utilities.GetCurrentUserInfo();
            if (userInfo != null)
            {
                model.UserName = userInfo.UserName;
                model.Language = userInfo.Language;
            }
         
            else
            {
                return HttpNotFound();
            }
            using (var Commonmproxy = new CommonProxy())
            {
                SystemType sp = Commonmproxy.GetSystemTypebyKeyType(userInfo.Language);
                if (sp != null)
                {
                    model.LanguageTypeName = sp.TypeName;
                }

                else
                {
                    model.LanguageTypeName = null;
                }

            }

            int total;
     
            using (var proxy = new UserProxy())
            {


                foreach (UserRole UserRole in userInfo.UserRoles)
                {
                    if (UserRole.RoleType == (int)RoleType.UserAdmin)
                    {
                        model.SearchResult = proxy.GetUserGroupList(start,
                                            Constants.PageSize,
                                            out total,
                                            Convert.ToString(model.UserGroupName),
                                            UserRole.RoleType);
                        model.Pagers = new Pager((start.HasValue ? start.Value : 0), total, Constants.PageSize);

                    }

                    else if (UserRole.RoleType == (int)RoleType.SystemAdmin)
                    {

                        model.SearchResult = proxy.GetUserGroupList(start,
                            Constants.PageSize,
                            out total,
                            Convert.ToString(model.UserGroupName),
                            model.RoleTypeId);
                        model.Pagers = new Pager((start.HasValue ? start.Value : 0), total, Constants.PageSize);

                    }
                }





            }
            return View(model);
          

  
    }




        /// <summary>
        /// Create user group
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Controllers.Create_User_Group)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.Create_User_Group)]
        public ActionResult CreateUserGroup()
        {
            UserGroupDetailModel model = new UserGroupDetailModel();
       
            return View(model);
        }
    
        /// <summary>
        /// Create user group data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(Constants.Controllers.Create_User_Group)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.Create_User_Group)]
        public ActionResult CreateUserGroupData(UserGroupDetailModel model)
        {
            // Validate
            if (ModelState.IsValid)
            {
                List<SiteMap> siteMapList = new List<SiteMap>();
                List<SiteMap> siteMapRoleList = new List<SiteMap>();
                SiteMap list = new SiteMap();
                UserRole userRole = null;
                using (var proxy = new UserProxy())
                {
                    if (proxy.CheckUserGroupExist(model.UserGroupName))
                    {
                        ModelState.AddModelError("User Group Name", Convert.ToString(Resources.Res.RES_Error_Unique));
                    }
                    else
                    {
                        using (var _proxy = new UserProxy())
                        {

                        if ((Utilities.MaxRoleType == RoleType.SystemAdmin) && (Utilities.MaxRoleType != 0))
                        {
                                if (Convert.ToInt32(model.RoleTypeId) == Convert.ToInt32(RoleType.SystemAdmin))
                                {
                                    siteMapList = _proxy.GetSiteMapByApplicationId(1);
                                }
                                if (Convert.ToInt32(model.RoleTypeId) == Convert.ToInt32(RoleType.UserAdmin))
                                {

                                    siteMapRoleList = _proxy.GetSiteMapByApplicationId(1);
                                    foreach (var UserAdminSitmap in siteMapRoleList)
                                    {
                                        if ((UserAdminSitmap.SiteMapTitle == Constants.Controllers.Users) || (UserAdminSitmap.SiteMapTitle == Constants.Controllers.Sitmap_Organization) || (UserAdminSitmap.SiteMapTitle == Constants.Controllers.UserAccount)
                                           || (UserAdminSitmap.SiteMapTitle == Constants.Controllers.UserGroup))
                                        {
                                            
                                               list = _proxy.GetSiteMapBySiteMapById(UserAdminSitmap.SiteMapId);
                                            siteMapList.Add(list);
                                        }
                                    }
                                }
                                else if (Convert.ToInt32(model.RoleTypeId) == Convert.ToInt32(RoleType.NormalUser))
                                {
                                    siteMapRoleList = _proxy.GetSiteMapByApplicationId(1);
                                    foreach (var UserAdminSitmap in siteMapRoleList)
                                    {
                                        if ((UserAdminSitmap.SiteMapTitle == Constants.Controllers.Users) || (UserAdminSitmap.SiteMapTitle == Constants.Controllers.UserAccount))
                                        {

                                            list = _proxy.GetSiteMapBySiteMapById(UserAdminSitmap.SiteMapId);
                                            siteMapList.Add(list);
                                        }
                                    }

                                }

                                else if (Convert.ToInt32(model.RoleTypeId) == Convert.ToInt32(RoleType.RigUser))
                                {
                                    siteMapRoleList = _proxy.GetSiteMapByApplicationId(1);
                                    foreach (var UserAdminSitmap in siteMapRoleList)
                                    {
                                        if ((UserAdminSitmap.SiteMapTitle == Constants.Controllers.Users) || (UserAdminSitmap.SiteMapTitle == Constants.Controllers.UserAccount))
                                        {

                                            list = _proxy.GetSiteMapBySiteMapById(UserAdminSitmap.SiteMapId);
                                            siteMapList.Add(list);
                                        }
                                    }

                                }
                            }
                            }

                        User user = @Utilities.GetCurrentUserInfo();
                        
                        userRole = new UserRole()
                        {
                            Description = Convert.ToString(model.Description),
                            RoleName = Convert.ToString(model.UserGroupName),
                            RoleType = model.RoleTypeId.Value,
                            IsADGroup = false,
                            SiteMaps = siteMapList,
                            CreatedBy = Convert.ToString(user.UserName),
                            CreatedDate = DateTime.Now,
                            UpdatedBy = null,
                            UpdatedDate = null,
                        };
                        proxy.CreateUserGroup(userRole,user);
                       
                        model.ShowSuccessMessage = true;
                        model.SuccessDirectionPath = string.Format("{0}/{1}", Constants.Controllers.User, Constants.Controllers.User_UserGroup);
                        model.SuccessMessage = Convert.ToString(Resources.Res.RES_Success);
                   
                    }
                }
            }
            return View("CreateUserGroup", model);

        }

        /// <summary>
        /// Manage user group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Controllers.User_ManageUserGroup)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_ManageUserGroup)]
        public ActionResult ManageUserGroup(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            else
            {
                int? ugId = Convert.ToInt32(id);
                if (ugId.HasValue)
                {
                    UserRole userRole = null;
                    using (var proxy = new UserProxy())
                    {
                        userRole = proxy.GetUserGroupById(ugId.Value);
                    }
                    UserGroupDetailModel model = new UserGroupDetailModel();
                    if (userRole != null)
                    {
                        model.Created = Convert.ToString(userRole.CreatedDate);
                        model.Description = userRole.Description;
                        model.CreatedBy = Utilities.GetUserDisplayName(userRole.CreatedBy);
                        model.RoleTypeId = userRole.RoleType;
                        model.UserGroupId = userRole.UserRoleId;
                        model.UserGroupName = userRole.RoleName;
                        model.UserList = userRole.Users.Where(u => u.IsActive == 1).ToList();

                        if (model.UpdatedBy != null && model.Updated != null)
                        {
                            model.UpdatedBy = Utilities.GetUserDisplayName(userRole.UpdatedBy);
                            model.Updated = StringHelper.ToString(userRole.UpdatedDate, Constants.ShowDateTime);
                        }
                        else
                        {
                            model.UpdatedBy = Utilities.GetUserDisplayName(userRole.UpdatedBy);
                            model.Updated = StringHelper.ToString(userRole.UpdatedDate, Constants.ShowDateTime);
                        }
                    }
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        /// <summary>
        /// Manage user group data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(Constants.Controllers.User_ManageUserGroup)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_ManageUserGroup)]
        public ActionResult ManageUserGroupData(UserGroupDetailModel model)
        {
            // Validate
            if (ModelState.IsValid)
            {
                UserRole userRole = null;
                using (var proxy = new UserProxy())
                {
                    userRole = proxy.GetUserGroupById(model.UserGroupId.Value);
                    User user = @Utilities.GetCurrentUserInfo();
                    if (userRole != null)
                    {
                        model.Created = Convert.ToString(userRole.CreatedDate);
                        model.CreatedBy = Utilities.GetUserDisplayName(userRole.CreatedBy);
                        model.Updated = string.IsNullOrWhiteSpace(userRole.UpdatedBy) ? string.Empty : Convert.ToString(userRole.UpdatedDate);
                        model.UpdatedBy = Utilities.GetUserDisplayName(userRole.UpdatedBy);
                        model.UserList = userRole.Users.ToList();
                        if (proxy.CheckUserGroupExist(model.UserGroupName) && !StringHelper.Compare(userRole.RoleName, model.UserGroupName))
                        {
                            ModelState.AddModelError("User Group Name", Convert.ToString(Resources.Res.RES_Error_Unique));
                        }
                        else
                        {
                            userRole = new UserRole()
                            {
                                UserRoleId = model.UserGroupId.Value,
                                Description = Convert.ToString(model.Description),
                                RoleName = Convert.ToString(model.UserGroupName),
                                RoleType = model.RoleTypeId.Value,
                                UpdatedBy = Convert.ToString(user.UserName),
                                UpdatedDate = DateTime.Now,
                                CreatedDate = Convert.ToDateTime(model.Created),
                                CreatedBy = Utilities.GetUserDisplayName(model.CreatedBy),
                            };
                            proxy.UpdateUserGroup(userRole);

                            model.ShowSuccessMessage = true;
                            model.SuccessDirectionPath = string.Format("{0}/{1}", Constants.Controllers.User, Constants.Controllers.User_UserGroup);
                            model.SuccessMessage = Convert.ToString(Resources.Res.RES_Success);
                        }
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }

            return View("ManageUserGroup", model);
        }


        /// <summary>
        /// Delete user group
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        [ActionName(Constants.Controllers.User_DeleteUserGroup)]
        public string DeleteUserGroup(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("No user group id");
            }
            else
            {
                int? ugId = Convert.ToInt32(id);
                if (ugId.HasValue)
                {
                    UserRole userRole = null;
                    using (var proxy = new UserProxy())
                    {
                        userRole = proxy.GetUserGroupById(ugId.Value);
                        if (userRole != null)
                        {
                            if (userRole.Users != null && userRole.Users.Count > 0)
                            {
                                proxy.DeleteUserGroup(ugId.Value);
                                return string.Empty;
                       
                            }
                            else
                            {
                                // Cannot delete
                                return "Cannot delete, has user";
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid user id");
                        }
                    }
                }
                else
                {
                    throw new Exception("Invalid user id");
                }
            }
        }


        /// <summary>
        /// User list
        /// </summary>
        /// <param name="model"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Controllers.User_UserAccount)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_UserAccount)]
        public ActionResult UserAccount(UserOverviewModel model, int? start)
        {

            if (model == null)
            {
                model = new UserOverviewModel();
            }
            int total;
            User userInfo = @Utilities.GetCurrentUserInfo();

     
            using (var proxy = new UserProxy())
            {
                foreach (UserRole UserRole in userInfo.UserRoles)
                {
                    if (UserRole.RoleType == (int)RoleType.UserAdmin)
                    {
                        using (var Organizationproxy = new OrganizationProxy())
                        {
                            
                                model.SearchResult = proxy.GetUserListByUserAdmin(start,
                               Constants.PageSize,
                               out total,
                                Convert.ToString(model.UserId),
                               (userInfo.Organisation_GUID),
                               Constants.WindowsAuthentication).ToList();
                                model.Pagers = new Pager((start.HasValue ? start.Value : 0), total, Constants.PageSize);


                        }
                    }

                    else  if (UserRole.RoleType == (int)RoleType.SystemAdmin)
                    {
                         model.SearchResult = proxy.GetUserList(start,
                          Constants.PageSize,
                         out total,
                         Convert.ToString(model.UserId),
                         Convert.ToString(model.UserName),
                          Constants.WindowsAuthentication).ToList();
                        model.Pagers = new Pager((start.HasValue ? start.Value : 0), total, Constants.PageSize);

                    }
                    else if (UserRole.RoleType == (int)RoleType.NormalUser)
                    {
                        model.SearchResult = proxy.GetUserList(start,
                         Constants.PageSize,
                        out total,
                          Convert.ToString(model.UserId),
                        Convert.ToString(userInfo.UserName),
                         Constants.WindowsAuthentication).ToList();
                        model.Pagers = new Pager((start.HasValue ? start.Value : 0), total, Constants.PageSize);

                    }
                    else if (UserRole.RoleType == (int)RoleType.RigUser)
                    {
                        model.SearchResult = proxy.GetUserList(start,
                         Constants.PageSize,
                        out total,
                        Convert.ToString(model.UserId),
                        Convert.ToString(userInfo.UserName),
                         Constants.WindowsAuthentication).ToList();
                        model.Pagers = new Pager((start.HasValue ? start.Value : 0), total, Constants.PageSize);

                    }
                }

            }
            return View(model);
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Controllers.User_CreateUserAccount)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_CreateUserAccount)]
        public ActionResult CreateUserAccount()
        {
            UserDetailModel model = new UserDetailModel();
            return View(model);
        }

        /// <summary>
        /// Create user data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(Constants.Controllers.User_CreateUserAccount)]
        public ActionResult CreateUserAccountData(UserDetailModel model)
        {
            // Validate
            if (ModelState.IsValid)
            {
                User user = null;
                UserActivity UserActivity = null;
                int[] selectedUGIds = null;
                using (var proxy = new UserProxy())
                {
                                    user = proxy.GetUserById(model.UserId);
                                    if (user != null)
                                    {
                                       ModelState.AddModelError("User Id", Convert.ToString(Resources.Res.RES_Error_Unique));
                                    }
                                    else
                                    {
                                        User userInfo = @Utilities.GetCurrentUserInfo();
                                        user = new User()
                                        {
                                            Email = Convert.ToString(model.Email),
                                            MobileNumber = Convert.ToString(model.OfficeNumber),
                                            Password =model.Password,
                                            IsDeptAdmin = Convert.ToString( model.IsDeptAdmin),
                                            UserDisplayName = Convert.ToString(model.UserName),
                                            Organisation_GUID = model.OrganizationGUID,
                                            Language = model.BlastManagerWebSettingID,
                                            UserName = model.UserId,
                                           LastName = model.UserLastName,
                                           IsActive = Convert.ToByte(model.IsActive),
                                             CreatedBy = Convert.ToString(userInfo.UserName),
                                            CreatedDate = DateTime.Now,
                                            UpdatedBy = null,
                                            UpdatedDate = null,

                                        };

                        UserActivity = new UserActivity()
                        {
                            IsActive = Convert.ToBoolean(model.IsActive),
                            TimeStamp = DateTime.Now,

                        };
                    }
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrWhiteSpace(model.SelectedUserGroupIdList))
                        {
                            user.UserRoles = new List<UserRole>();
                            selectedUGIds = model.SelectedUserGroupIdList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(p => int.Parse(p)).ToArray();
                            foreach (var ugId in selectedUGIds)
                            {
                                user.UserRoles.Add(new UserRole() { UserRoleId = ugId });
                            }
                        }
                        proxy.CreateUser(user, UserActivity);

                        model.ShowSuccessMessage = true;
                        model.SuccessDirectionPath = string.Format("{0}/{1}", Constants.Controllers.User, Constants.Controllers.User_UserGroup);
                        model.SuccessMessage = Convert.ToString(Resources.Res.RES_Success);
                    }
                }
            }

            return View("CreateUserAccount", model);
        }

        /// <summary>
        /// Manage user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Controllers.User_ManageUserAccount)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_ManageUserAccount)]
        public ActionResult ManageUserAccount(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }
            else
            {
                User user = null;
                string uid = id.Replace(";", "\\");
                using (var proxy = new UserProxy())
                {
                    user = proxy.GetUserById(uid);
                }
                UserDetailModel model = new UserDetailModel();
                if (user != null)
                {
                        model.Email = user.Email;
                        model.OrganizationGUID = user.Organisation_GUID;
                        model.OfficeNumber = user.MobileNumber;
                        model.UserId = user.UserName;
                       model.IsDeptAdmin = Convert.ToBoolean(user.IsDeptAdmin);
                        model.BlastManagerWebSettingID = user.Language;
                        model.WindowsAccount = user.UserName;
                        model.UserName = user.UserDisplayName;
                        model.SelectedUserGroupIdList += string.Join(";", user.UserRoles.Select(p => p.UserRoleId).ToArray());
                        model.CreatedBy = Utilities.GetUserDisplayName(user.CreatedBy);
                        model.IsActive = Convert.ToBoolean(user.IsActive);
                        model.CreatedDate = StringHelper.ToString(user.CreatedDate, Constants.ShowDateTime);
                    if (model.UpdatedBy != null && model.UpdatedDate != null)
                    {
                        model.UpdatedBy = Utilities.GetUserDisplayName(user.UpdatedBy);
                        model.UpdatedDate = StringHelper.ToString(user.UpdatedDate, Constants.ShowDateTime);
                    }
                    else
                    {
                        model.UpdatedBy = Utilities.GetUserDisplayName(user.UpdatedBy);
                        model.UpdatedDate = StringHelper.ToString(user.UpdatedDate, Constants.ShowDateTime);
                    }


                }
                else
                {

                }
                return View(model);
            }

            }


        /// <summary>
        /// Manage user data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(Constants.Controllers.User_ManageUserAccount)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_ManageUserAccount)]
        public ActionResult ManageUserAccountData(UserDetailModel model)
        {
            // Validate
            if (ModelState.IsValid)
            {
                User user = null;
                int[] selectedUGIds = null;
                using (var proxy = new UserProxy())
                {
                    
                       user = proxy.GetUserById(model.UserId);
                    User userInfo = @Utilities.GetCurrentUserInfo();
                    if (user != null)
                         {
                                user = new User()
                                {
                                    CreatedDate = user.CreatedDate,
                                    UpdatedBy = Convert.ToString(userInfo.UserName),
                                    UpdatedDate = DateTime.Now,
                                    CreatedBy = user.CreatedBy,
                                     Email = model.Email,
                                    Organisation_GUID = model.OrganizationGUID,
                                    MobileNumber = model.OfficeNumber,
                                    UserName = model.UserName,
                                    Language = model.BlastManagerWebSettingID,
                                    GUID_User = user.GUID_User,
                                    IsDeptAdmin = Convert.ToString(model.IsDeptAdmin),   
                                    IsActive = Convert.ToByte(model.IsActive),
                                    UserDisplayName = user.UserDisplayName,
                                };

                        model.SelectedUserGroupIdList += string.Join(";", user.UserRoles.Select(p => p.UserRoleId).ToArray());
            

                            if (!string.IsNullOrWhiteSpace(model.SelectedUserGroupIdList))
                            {
                                user.UserRoles = new List<UserRole>();
                                selectedUGIds = model.SelectedUserGroupIdList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(p => NumericHelper.ParseInt(p).Value).ToArray();
                                foreach (var ugId in selectedUGIds)
                                {
                                    user.UserRoles.Add(new UserRole() { UserRoleId = ugId });
                                }
                            }
                            proxy.UpdateUser(user);

                            model.ShowSuccessMessage = true;
                            model.SuccessDirectionPath = StringHelper.Format("{0}/{1}", Constants.Controllers.User, Constants.Controllers.User_UserAccount);
                        model.SuccessMessage = Convert.ToString(Resources.Res.RES_Success);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }

            return View("ManageUserAccount", model);
        }


        /// <summary>
        /// Access Control
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Controllers.User_AccessControl)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_AccessControl)]
        public ActionResult AccessControl()
        {
            StringBuilder strData = new StringBuilder();
            AccessControlOverviewModel model = new AccessControlOverviewModel();
            List<SiteMap> siteMapList = new List<SiteMap>();


            model.SiteMapJsonList = new List<AccessControlOverviewModel.JsonSiteMap>();



            using (var proxy = new UserProxy())
            {
                siteMapList = proxy.GetSiteMapByApplicationId(1);
            }
            //Generate Site Map string.
            foreach (SiteMap siteMap in siteMapList.Where(f => f.MapPath.IndexOf("*", StringComparison.InvariantCultureIgnoreCase) == -1))
            {
                model.SiteMapJsonList.Add(new AccessControlOverviewModel.JsonSiteMap
                {
                    SiteMapId = siteMap.SiteMapId,
                    ParentSiteMapId = siteMap.ParentId.HasValue ? siteMap.ParentId.Value : siteMap.SiteMapId,
                    SiteMapTitle = siteMap.SiteMapTitle,
                    SiteMapUrl = Url.Action(Constants.Controllers.User_ManageAccessControl, Constants.Controllers.User, new { id = siteMap.SiteMapId }),
                    Open = Convert.ToString(false).ToLower()
                });
            }
            return View(model);
        }

        /// <summary>
        /// Manage Access Control
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Controllers.User_ManageAccessControl)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_ManageAccessControl)]
        public ActionResult ManageAccessControl(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            AccessControlDetailModel model = new AccessControlDetailModel();
            using (var egProxy = new UserProxy())
            {
                var eg = egProxy.GetSiteMapById(NumericHelper.ParseInt(id).Value);
                if (eg == null)
                {
                    return HttpNotFound();
                }
                if (eg.ParentId != null && eg.ParentId.HasValue)
                {
                    var parent = egProxy.GetSiteMapById(eg.ParentId.Value);
                    model.SiteMapName = eg.Description + "/" + eg.Description;
                }
                else
                {
                    model.SiteMapName = eg.Description;
                }
                model.Description = eg.Description;
                model.Depth = eg.Depth;
                model.MinimumRoleType = eg.MinimumRoleType;
                model.SiteMapId = NumericHelper.ParseInt(id).Value;
                model.UserGroups = eg.UserRoles.ToList();
                model.SetUserRoleList((int)eg.MinimumRoleType);
            }

            using (var proxy = new CommonProxy())
            {
                var ht = proxy.GetAllSystemTypes();
                Dictionary<string, string> dic = new Dictionary<string, string>();

                foreach (RoleType en in Enum.GetValues(typeof(RoleType)))
                {
                    if (en != RoleType.None)
                    {
                        string key = StringHelper.Format("{0}.{1}", Constants.SystemTypeCategory.RoleType, (int)en);
                        dic.Add((StringHelper.ToString((int)en)), ht[key] as string);
                    }
                }

                model.DicRoleType = dic;
            }
            return View(model);
        }

        /// <summary>
        /// Manage Access Control
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(Constants.Controllers.User_ManageAccessControl)]
        [AuthorizationFilter(SitemapId = Constants.SitemapID.User_ManageAccessControl)]
        public ActionResult ManageAccessControlData(AccessControlDetailModel model)
        {
            // Reset model error
            model.Reset();
            SiteMap eg = new SiteMap();

            using (var proxy = new CommonProxy())
            {
                var ht = proxy.GetAllSystemTypes();
                Dictionary<string, string> dic = new Dictionary<string, string>();

                foreach (RoleType en in Enum.GetValues(typeof(RoleType)))
                {
                    if (en != RoleType.None)
                    {
                        string key = StringHelper.Format("{0}.{1}", Constants.SystemTypeCategory.RoleType, (int)en);
                        dic.Add((StringHelper.ToString((int)en)), ht[key] as string);
                    }
                }

                model.DicRoleType = dic;
            }
            using (var egProxy = new UserProxy())
            {
                eg = egProxy.GetSiteMapById(model.SiteMapId);
                if (eg == null)
                {
                    return HttpNotFound();
                }

                // They will not post back
                if (eg.ParentId != null && eg.ParentId.HasValue)
                {
                    model.SiteMapName = eg.Description + "/" + eg.Description;
                }
                else
                {
                    model.SiteMapName = eg.Description;
                }
                model.Description = eg.Description;
                model.Depth = eg.Depth;
                model.MinimumRoleType = eg.MinimumRoleType;
                model.SiteMapId = eg.SiteMapId;
                model.UserGroups = eg.UserRoles.ToList();
                model.SetUserRoleList((int)eg.MinimumRoleType);
            }

            // Validate
            if (ModelState.IsValid)
            {
                var selectedUGId = model.UserRoleIdList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(p => NumericHelper.ParseInt(p).Value).ToList();
                eg.UserRoles = new List<UserRole>();

                using (var proxy = new UserProxy())
                {
                    foreach (int id in selectedUGId)
                    {
                        eg.UserRoles.Add(proxy.GetUserGroupById(id));
                    }
                }
                using (var egProxy = new UserProxy())
                {
                    SiteMap sitemap = egProxy.UpdateAccessControl(eg);
                    model.UserGroups = sitemap.UserRoles.ToList();
                }

                model.ShowSuccessMessage = true;
                model.SuccessDirectionPath = StringHelper.Format("{0}/{1}", Constants.Controllers.User, Constants.Controllers.User_AccessControl);
                model.SuccessMessage = Convert.ToString(Resources.Res.RES_Success);
            }
            return View(model);
        }


    }
}