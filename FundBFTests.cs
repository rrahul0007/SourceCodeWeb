using System;
using Xunit;
using Xunit.Abstractions;
using SingtelWPM.Service.Main.Common.BusinessEntity;
using SingtelWPM.Service.Main.Common.CommonType;
using System.Linq;
using System.Collections.Generic;
using Qilin.Service.Core.Context;
using Qilin.Core;

namespace SingtelWPM.Service.Main.Backend.BusinessFacade.Tests
{


    #region Fund Related Test Cases
    public class FundBFTests
    {


        [Fact]

        #region Save Budget
        public void ShouldSaveBudget()
        {
            FundBF _Proxy = new FundBF();
            Budget BudgetInfo = prepareBudgetInfo();
            bool result = _Proxy.SaveBudget(BudgetInfo);
            Assert.Equal(true, result);
        }
        #endregion

        [Fact]
        #region ShouldUpdateBudget

        public void ShouldUpdateBudget()
        {
            int FiscalYearId = 1;
            QilinApplicationContext.Current["_User_Id"] = "sysadmin";
            decimal BudgetAmount = 1000;
            FundBF _Proxy = new FundBF();
            Budget Info = _Proxy.GetBudgetDetailByFiscalYearID(FiscalYearId);
            int returnCode = _Proxy.UpdateBudget(Info, BudgetAmount);
            Assert.Equal(1, returnCode);
        }

        #endregion

        [Fact]

        #region ShouldSubmitFundTransfer

        public void ShouldSubmitFundTransfer()
        {
            int FiscalYearId = 1;
            int TransferFrom = 1;
            int TransferTo = 2;
            int CapaxTotal = 0;
            decimal TransferAmount = 10;

            QilinApplicationContext.Current["_User_Id"] = "sysadmin";
            FundBF _Proxy = new FundBF();
            var TransferFromInfo = new BudgeAllocation();
            var TransferToInfo = new BudgeAllocation();
            Budget info = _Proxy.GetBudgetDetailByFiscalYearID(FiscalYearId);

            TransferFromInfo = _Proxy.GetBudgetAllocationdataByBizGroupID(TransferFrom, FiscalYearId);
            TransferToInfo = _Proxy.GetBudgetAllocationdataByBizGroupID(TransferTo, FiscalYearId);
            int ReturnCode = _Proxy.SubmitFundTransfer(info, TransferFromInfo, TransferToInfo, CapaxTotal, TransferAmount);
            Assert.Equal(1, ReturnCode);

        }

        #endregion



            #region Private Method

        private Budget prepareBudgetInfo()
        {
            #region prepareBudgetInfo

                Budget BudgetInfo = new Budget();

                QilinApplicationContext.Current["_User_Id"] = "sysadmin";
                BudgetInfo.FiscalYearId = 1;
                BudgetInfo.BudgetAmount = 120;
                BudgetInfo.CreatedBy = StringHelper.ToString(QilinApplicationContext.Current["_User_Id"]);

               return BudgetInfo;
            #endregion

        }

        #endregion

    }
}


#endregion