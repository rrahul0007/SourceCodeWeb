using System;

using Xunit;
using Xunit.Abstractions;
using SingtelWPM.Service.Main.Backend.DataAccess;
using SingtelWPM.Service.Main.Common.BusinessEntity;
using SingtelWPM.Service.Main.Common.CommonType;
using Qilin.Service.Server.DataAccess;
using System.Data.Entity;
using System.Linq;
using SingtelWPM.Service.Main.Backend.BusinessFacade.Common;

namespace SingtelWPM.Service.Main.Backend.BusinessFacade.Tests
{
    
    public class MailManagerTests
    {
        [Fact]
        public void ShouldSendHtmlEmailByMailType()
        {
            using (var uow = new UnitOfWork(SingtelWPMMainContext))
            {
                MailType mailTypeInfo = uow.CreateRepository<MailType>()
                   .Get(u => u.MailTypeGroup == (short)MailTypeGroup.MailForFormReturn && u.IsEmailActivated &&
                   u.SourceTaskTypeId == 7 && u.DestinationTaskTypeId == 14)
                   .Include(u => u.MailTypeToUserRole)
                   .SingleOrDefault();

                if (mailTypeInfo != null)
                {
                    string subject = String.Format(mailTypeInfo.EmailSubject, "Test Return");
                    string mailBody = "Test Body";

                    MailManager mailManager = new MailManager(SingtelWPMMainContext);
                    mailManager.SendHtmlEmailByMailType(mailTypeInfo, 2, subject, mailBody, new Guid("9E59B981-7DB4-4E05-9152-A4147500AB93"));
                }
            }
        }

        #region internal resource

        private SingtelWPMMainContainer _singtelWPMMainContext;

        private SingtelWPMMainContainer SingtelWPMMainContext
        {
            get
            {
                if (_singtelWPMMainContext == null)
                {
                    return new SingtelWPMMainContainer();
                }
                return _singtelWPMMainContext;
            }
        }

        #endregion
    }
}
