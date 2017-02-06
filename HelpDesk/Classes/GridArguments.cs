using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QA
{
    public class GridArguments
    {
        public bool displayPageTotals { get; set; }

        public decimal sumJDEForecast { get; set; }
        public decimal sumJDEForecast_Pages { get; set; }
        public decimal sumJDEAdj { get; set; }
        public decimal sumJDEAdj_Pages { get; set; }
        public decimal sumPYAdj { get; set; }
        public decimal sumPYAdj_Pages { get; set; }
        public decimal sumFrozenQty { get; set; }
        public decimal sumFrozenQty_Pages { get; set; }
        public decimal sumSalesHistory { get; set; }
        public decimal sumSalesHistory_Pages { get; set; }
        public decimal sumAdjSalesForecast { get; set; }
        public decimal sumAdjSalesForecast_Pages { get; set; }
        public decimal sumCurrentProdBuild { get; set; }
        public decimal sumCurrentProdBuild_Pages { get; set; }

        public string strSumJDEAdj_Pages { get; set; }

        //public string PRC { get; set; }
        //public string DRQJ { get; set; }
        //public string productManager { get; set; }
        //public string family { get; set; }
        //public string planner { get; set; }
        //public string scheduleCode { get; set; }
        
        
        //public string topXPercent { get; set; }
        //public bool qtyNotZero { get; set; }
        //public bool diffOnly { get; set; }
        //public bool frozenOnly { get; set; }

        public void Clear()
        {
            sumAdjSalesForecast = 0;
            sumAdjSalesForecast_Pages = 0;
            sumFrozenQty = 0;
            sumFrozenQty_Pages = 0;
            sumJDEAdj = 0;
            sumJDEAdj_Pages = 0;
            sumJDEForecast = 0;
            sumJDEForecast_Pages = 0;
            sumPYAdj = 0;
            sumPYAdj_Pages = 0;
            sumSalesHistory = 0;
            sumSalesHistory_Pages = 0;
            sumCurrentProdBuild = 0;
            sumCurrentProdBuild_Pages = 0;
        }
    }
}