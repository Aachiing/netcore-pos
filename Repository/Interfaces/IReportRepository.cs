using Microsoft.AspNetCore.Mvc;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IReportRepository
    {
        Task<string> DailyCashSalesReport();
        Task<string> DailyCreditSalesReport();
    }
}
