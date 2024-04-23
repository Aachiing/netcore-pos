using Microsoft.Data.SqlClient;
using Sales_Inventory.Helper;
using Sales_Inventory.Models.DTO;
using System.Data;

namespace Sales_Inventory.DAL
{
    public class ReportDAL
    {
        private readonly IConfiguration _configuration;
        private readonly AdoHelper _adoHelper;

        public ReportDAL(IConfiguration configuration)
        {
            _configuration = configuration;
            _adoHelper = new AdoHelper(_configuration);
        }

        public List<DailySalesReportDTO> ListDailyCashSalesReport(DateTime dateFrom, DateTime dateTo)
        {
            SqlParameterCollection _params = new SqlCommand().Parameters;
            _params.AddWithValue("@dateFrom", dateFrom);
            _params.AddWithValue("@dateTo", dateTo);

            DataTable dtRead = _adoHelper.ExecuteRead(@"[dbo].[sp_DailyCashReport]", _params);

            return dtRead.ToList<DailySalesReportDTO>();
        }

        public List<DailySalesReportDTO> ListDailyCreditSalesReport(DateTime dateFrom, DateTime dateTo)
        {
            SqlParameterCollection _params = new SqlCommand().Parameters;
            _params.AddWithValue("@dateFrom", dateFrom);
            _params.AddWithValue("@dateTo", dateTo);

            DataTable dtRead = _adoHelper.ExecuteRead(@"[dbo].[sp_DailyCreditReport]", _params);

            return dtRead.ToList<DailySalesReportDTO>();
        }

        public List<DailyExpensesReportDTO> ListDailyExpensesReport(DateTime dateFrom, DateTime dateTo)
        {
            SqlParameterCollection _params = new SqlCommand().Parameters;
            _params.AddWithValue("@dateFrom", dateFrom);
            _params.AddWithValue("@dateTo", dateTo);

            DataTable dtRead = _adoHelper.ExecuteRead(@"[dbo].[sp_DailyExpensesReport]", _params);

            return dtRead.ToList<DailyExpensesReportDTO>();
        }
    }
}
