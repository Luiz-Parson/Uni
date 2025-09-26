using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ConnectorAccess.Service.Services
{
    public class AccessControlDay
    {
        private readonly ILogger<AccessControlDay> _logger;
        private readonly string _connectionString;

        public AccessControlDay(ILogger<AccessControlDay> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public DataTable GenerateProductReport(string description, string epc, string sku, DateTime initialDate, DateTime endDate)
        {
            if (!string.IsNullOrEmpty(epc))
            {
                epc = epc.PadLeft(24, '0');
            }

            DataTable dt = new DataTable();

            string sql = @"
            SELECT * FROM (
                SELECT 
                    P.Description,
                    P.SKU,
                    P.EPC,
                    A.AccessedOn AS EntryDate,
                    CAST(NULL AS DATETIME) AS ExitDate
                FROM AccessControl A
                INNER JOIN Product P ON P.Id = A.ProductId
                WHERE 
                    A.Direction = 'IN'
                    AND A.AccessedOn BETWEEN @InitialDate AND @EndDate
                    AND (ISNULL(@Description, '') = '' OR P.Description LIKE '%' + @Description + '%')
                    AND (ISNULL(@EPC, '') = '' OR P.EPC = @EPC)
                    AND (ISNULL(@SKU, '') = '' OR P.SKU = @SKU)

                UNION ALL

                SELECT 
                    P.Description,
                    P.SKU,
                    P.EPC,
                    CAST(NULL AS DATETIME) AS EntryDate,
                    A.AccessedOn AS ExitDate
                FROM AccessControl A
                INNER JOIN Product P ON P.Id = A.ProductId
                WHERE 
                    A.Direction = 'OUT'
                    AND A.AccessedOn BETWEEN @InitialDate AND @EndDate
                    AND (ISNULL(@Description, '') = '' OR P.Description LIKE '%' + @Description + '%')
                    AND (ISNULL(@EPC, '') = '' OR P.EPC = @EPC)
                    AND (ISNULL(@SKU, '') = '' OR P.SKU = @SKU)
            ) AS CombinedResult
            ORDER BY COALESCE(EntryDate, ExitDate);";

            try
            {
                using (var cn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EPC", (object)epc ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SKU", (object)sku ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@InitialDate", initialDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        cn.Open();
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar relatório de produtos.");
            }

            return dt;
        }

        public string SaveReportAsCsv(DataTable dataTable)
        {
            string filePath = Path.Combine(Path.GetTempPath(), $"Relatorio_{DateTime.Now:yyyyMMddHHmmss}.csv");

            StringBuilder csv = new StringBuilder();

            string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            csv.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                csv.AppendLine(string.Join(";", fields));
            }

            File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
            return filePath;
        }
    }
}
