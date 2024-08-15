using Microsoft.AspNetCore.Mvc;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IReportRepository
    {
        Task<string> DailyCashSalesReport(DateTime dateFrom, DateTime dateTo);
        Task<string> DailyCreditSalesReport(DateTime dateFrom, DateTime dateTo);
        Task<string> DailyExpensesReport(DateTime dateFrom, DateTime dateTo);
        Task<string> InventoryHistoryReport(DateTime dateFrom, DateTime dateTo);
    }
}
