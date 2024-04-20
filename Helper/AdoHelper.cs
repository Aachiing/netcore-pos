using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Sales_Inventory.Helper
{
    public class AdoHelper
    {
        private readonly IConfiguration _configuration;
        private string ConnString;
        public AdoHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnString = _configuration.GetConnectionString("dbConn");
        }
        public DataTable ExecuteRead(string sProc, SqlParameterCollection oArrParam)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cnn = new SqlConnection(ConnString))
            {
                try
                {
                    cnn.Open();

                    SqlCommand cmd = new SqlCommand(sProc, cnn)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 360000
                    };

                    foreach (SqlParameter oParam in oArrParam)
                    {
                        cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    cnn.Close();
                }
                catch (SqlException sqlEx)
                {
                    string sErrMessage = "SQL Error: Number - " + sqlEx.Number + ", " + sqlEx.Message;
                    throw;
                }
                catch (Exception ex)
                {
                    string sErrMessage = ex.GetType().Name + ": " + ex.Message;
                    throw;
                }
            }

            return dt;
        }
    }
}
