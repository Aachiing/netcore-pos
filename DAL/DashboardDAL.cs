using Microsoft.Data.SqlClient;
using Sales_Inventory.Helper;
using Sales_Inventory.Models.DTO;
using System.Data;

namespace Sales_Inventory.DAL
{
    public class DashboardDAL
    {
        private readonly IConfiguration _configuration;
        private readonly AdoHelper _adoHelper;

        public DashboardDAL(IConfiguration configuration)
        {
            _configuration = configuration;
            _adoHelper = new AdoHelper(_configuration);
        }
        public List<RevenueDetailsDTO> MonthlyDashboardReport()
        {
            SqlParameterCollection _params = new SqlCommand().Parameters;

            DataTable dtRead = _adoHelper.ExecuteRead(@"[dbo].[sp_MonthlyDashboardReport]", _params);

            return dtRead.ToList<RevenueDetailsDTO>();
        }
        public List<RevenueDetailsDTO> DailyDashboardReport()
        {
            SqlParameterCollection _params = new SqlCommand().Parameters;

            DataTable dtRead = _adoHelper.ExecuteRead(@"[dbo].[sp_DailyDashboardReport]", _params);

            return dtRead.ToList<RevenueDetailsDTO>();
        }
    }
}
