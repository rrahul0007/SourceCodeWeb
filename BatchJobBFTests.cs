using System;
using System.Linq;
using SingtelWPM.Service.Main.Common.BusinessEntity;
using SingtelWPM.Service.Main.Common.CommonType;
using System.Collections.Generic;
using System.Linq;

using Castle.Components.DictionaryAdapter;
using SingtelWPM.Service.Main.Common.BusinessEntity.Extends;

using Xunit;
using Xunit.Abstractions;

using Moq;

using SingtelWPM.Service.Main.Common.BusinessEntity;

namespace SingtelWPM.Service.Main.Backend.BusinessFacade.Tests
{

    public class BatchJobBFTests
    {
        [Fact]
        public void ShouldReturn2CasesForVPEmail()
        {
            var data = new FakeDataSet()
            {
                T_Case = new List<Case>()
                {
                    new Case()  //quallified case
                    {
                        CaseId = Guid.NewGuid(),
                        CaseStatus = (short) CaseStatus.PendingApproval,
                        CurrentTaskId = 2,
                        Task = new List<Task>()
                        {
                            new Task()
                            {
                                TaskId = 1,
                                TaskStatus = (short) TaskStatus.FormSubmitted,
                                TaskTypeId = (short) TaskTypeEnum.SAM
                            },

                            new Task()
                            {
                                TaskId = 2,
                                TaskStatus = (short) TaskStatus.Waiting,
                                TaskTypeId = (short) TaskTypeEnum.PM
                            }

                        }

                    },

                    new Case()  //qualified case
                    {
                        CaseId = Guid.NewGuid(),
                        CaseStatus = (short) CaseStatus.PendingApproval,
                        CurrentTaskId = 3,
                        Task = new List<Task>()
                        {
                            new Task()
                            {
                                TaskId = 3,
                                TaskStatus = (short) TaskStatus.Waiting,
                                TaskTypeId = (short) TaskTypeEnum.PM
                            }

                        }
                    },

                    new Case()  //disqulified case
                    {
                        CaseId = Guid.NewGuid(),
                        CaseStatus = (short) CaseStatus.PendingApproval,
                        CurrentTaskId = 4,
                        Task = new List<Task>()
                        {
                            new Task()
                            {
                                TaskId = 3,
                                TaskStatus = (short) TaskStatus.Started,
                                TaskTypeId = (short) TaskTypeEnum.ICE
                            }

                        }
                    },

                    new Case()  //disqulified case
                    {
                        CaseId = Guid.NewGuid(),
                        CaseStatus = (short) CaseStatus.InProgressAfterApproval,
                        CurrentTaskId = 5,
                        Task = new List<Task>()
                        {
                            new Task()
                            {
                                TaskId = 5,
                                TaskStatus = (short) TaskStatus.Started,
                                TaskTypeId = (short) TaskTypeEnum.Deployment
                            }

                        }
                    }

                }
            };

            var mockContext = MockManager.MockDbContext(data);

            var sut = new BatchJobBF(mockContext.Object);

            var caseList = sut.GetAllCasesForVPEmail();

            Assert.Equal(2, caseList.ToList().Count);

            Assert.Equal((short)CaseStatus.PendingApproval, caseList.ToList()[0].CaseStatus);

            Assert.Equal((short)TaskTypeEnum.PM, caseList.ToList()[0].Task.ToList()[1].TaskTypeId);

            Assert.Equal((short)TaskStatus.Waiting, caseList.ToList()[0].Task.ToList()[1].TaskStatus);

            Assert.Equal((short)TaskTypeEnum.PM, caseList.ToList()[1].Task.ToList()[0].TaskTypeId);

            Assert.Equal((short)TaskStatus.Waiting, caseList.ToList()[1].Task.ToList()[0].TaskStatus);
        }

        [Fact]
        public void ShouldSendEmailToVPWith2Cases()
        {
            List<Case> caseList = new List<Case>();

            //initialize two cases
            Case case1 = new Case()  //quallified case
            {
                CaseId = Guid.NewGuid(),
                CaseStatus = (short)CaseStatus.PendingApproval,
                BizGroupId = 2, //GAM
                ReferenceNo = "G1617/0001",
                CurrentTaskId = 2,
                Task = new List<Task>()
                                    {
                                        new Task()
                                        {
                                            TaskId = 1,
                                            TaskStatus = (short) TaskStatus.FormSubmitted,
                                            TaskTypeId = (short) TaskTypeEnum.SAM

                                        },

                                        new Task()
                                        {
                                            TaskId = 2,
                                            TaskStatus = (short) TaskStatus.Waiting,
                                            TaskTypeId = (short) TaskTypeEnum.PM
                                        }

                                    }

            };

            caseList.Add(case1);

            Case case2 = new Case()  //quallified case
            {
                CaseId = Guid.NewGuid(),
                CaseStatus = (short)CaseStatus.PendingApproval,
                BizGroupId = 2, //GAM
                ReferenceNo = "G1617/0002",
                CurrentTaskId = 3,
                Task = new List<Task>()
                        {
                            new Task()
                            {
                                TaskId = 3,
                                TaskStatus = (short) TaskStatus.Waiting,
                                TaskTypeId = (short) TaskTypeEnum.PM
                            }

                        }

            };

            caseList.Add(case2);

            var data = new FakeDataSet()
            {
                T_MailType = new List<MailType>()
                {
                    new MailType()
                    {
                        MailTypeId = 1,
                        MailTypeGroup = 1,  //MailForFormSubmission
                        MailTypeName = "CRCI submit form to ICE",
                        SourceTaskTypeId = 1,
                        DestinationTaskTypeId = 2,
                        IsEmailActivated = true,
                        EmailSubject = "",
                        EmailBodyTemplate = "",
                        MailTypeToUserRole = new List<MailTypeToUserRole>()
                        {
                            new MailTypeToUserRole()
                            {
                                MailTypeId = 1,
                                UserRoleId = 1,
                                MailSendType = 1
                            }
                        }
                    },

                    new MailType()
                    {
                        MailTypeId = 6,
                        MailTypeGroup = 11,  //MailForConsolidatedCaseToVP
                        MailTypeName = "Consolidated request list to VP for approval",
                        SourceTaskTypeId = null,
                        DestinationTaskTypeId = null,
                        IsEmailActivated = true,
                        EmailSubject = "WPM – Pending VP Approval",
                        EmailBodyTemplate = "WPM – Pending VP Approval {0}",
                        MailTypeToUserRole = new List<MailTypeToUserRole>()
                        {
                            new MailTypeToUserRole()
                            {
                                MailTypeId = 6,
                                UserRoleId = 2,
                                MailSendType = 1
                            }
                        }
                    }
                },

                T_User = new List<User>()
                {
                    new User()
                    {
                        UserId = "GAMVP",
                        UserDisplayName = "GAM VP",
                        Email = "zujiang@ncs.com.sg",
                        IsDeleted = false,
                        IsDisabled = false,
                        UserRoles = new List<UserRole>()
                        {
                            new UserRole()
                            {
                                UserRoleId = 2
                            }
                        },
                        BizGroup = new List<BizGroup>()
                        {
                            new BizGroup()
                            {
                                BizGroupId = 2  //GAM
                            }
                        }

                    }
                },

                T_SystemParameter = new List<SystemParameter>()
                {
                    new SystemParameter()
                    {
                        SystemParameterId = "SmtpServerAddress",
                        Value = "10.70.190.52"
                    },

                    new SystemParameter()
                    {
                        SystemParameterId = "SmtpServerPort",
                        Value = "25"
                    },

                    new SystemParameter()
                    {
                        SystemParameterId = "SenderEmailAddress",
                        Value = "singtelwpm@singtel.com.sg"
                    },
                }
            };

            var mockContext = MockManager.MockDbContext(data);

            var sut = new BatchJobBF(mockContext.Object);

            var result = sut.SendConsolidatedEmailToVP(caseList, 2);

            Assert.Equal(true, result);

        }

        [Fact]
        public void ShouldSendEmailToVP()
        {
            var batchJobBF = new BatchJobBF();
            IQueryable<Case> caseList = batchJobBF.GetAllCasesForVPEmail();
                        
            int groupSize = 15; 

            //group the cases by its BizGroupId
            var caseGroupsByBizGroupId = caseList.GroupBy(c => new { c.BizGroupId, c.FiscalYearId });
            if (caseGroupsByBizGroupId == null)
                return;

            foreach (var casesUnderSameBizGroup in caseGroupsByBizGroupId)
            {
                int bizGroupId = casesUnderSameBizGroup.ToList()[0].BizGroupId;
                var caseGroups = casesUnderSameBizGroup.Select((x, i) => new { Item = x, Index = i })
                                            .GroupBy(x => x.Index / groupSize, x => x.Item);

                foreach (var caseListForSingleEmail in caseGroups)   //each caseListForSingleEmail contains 15 cases
                {
                    bool result = batchJobBF.SendConsolidatedEmailToVP(caseListForSingleEmail.ToList(), bizGroupId);
                }

                //var caseGroupsByFiscalYearId = casesUnderSameBizGroup.GroupBy(c => c.FiscalYearId);

                //foreach (var casesUnderSameBizGroupFiscalYear in caseGroupsByFiscalYearId)
                //{
                //    //group the cases into multiple groups, each group will have only 15 cases, the last group will have 15 or lesser cases
                //    var caseGroups = casesUnderSameBizGroupFiscalYear.Select((x, i) => new { Item = x, Index = i })
                //                            .GroupBy(x => x.Index / groupSize, x => x.Item);

                //    foreach (var caseListForSingleEmail in caseGroups)   //each caseListForSingleEmail contains 15 cases
                //    {
                //        bool result = batchJobBF.SendConsolidatedEmailToVP(caseListForSingleEmail.ToList(), bizGroupId);
                //    }
                //}
                    
            }
        }


        [Fact]
        public void ShouldSendSubmitFormReminderToDeployment()
        {
            var batchJobBF = new BatchJobBF();            
            int result = batchJobBF.SendSubmitFormReminderToDeployment();
            Assert.Equal(1, result);
        }
    }
}
