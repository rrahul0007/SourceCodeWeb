
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
namespace BlastManagerWEB.Models.Users
{
    /// <summary>
    /// Access Control Overview Model
    /// </summary>
    public class AccessControlOverviewModel : BaseModel<AccessControlOverviewModel>
    {

        /// <summary>
        /// Site map JSON object list
        /// </summary>
        public List<JsonSiteMap> SiteMapJsonList { get; set; }


        /// <summary>
        /// Site map JSON class (in format used by ztree)
        /// </summary>
        [DataContract]
        public class JsonSiteMap
        {
            /// <summary>
            /// Site map Id
            /// </summary>
            [DataMember(Name = "id")]
            public int SiteMapId { get; set; }

            /// <summary>
            /// Parent Site map Id
            /// </summary>
            [DataMember(Name = "pId")]
            public int ParentSiteMapId { get; set; }

            /// <summary>
            /// Site map name
            /// </summary>
            [DataMember(Name = "name")]
            public string SiteMapTitle { get; set; }

            /// <summary>
            /// URL to manage acess control for this site map
            /// </summary>
            [DataMember(Name = "url")]
            public string SiteMapUrl { get; set; }

            /// <summary>
            /// Open flag (for ztree)
            /// </summary>
            [DataMember(Name = "open")]
            public string Open { get; set; }
        }

        /// <summary>
        /// Serialize JsonSiteMap list as JSON string
        /// </summary>
        public string GetSiteMapsAsJsonString()
        {
            var serializer = new DataContractJsonSerializer(SiteMapJsonList.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, SiteMapJsonList);
                stream.Position = 0;
                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}