using ConnectorAccess.Service.data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Data;

namespace ConnectorAccess.Service.Services
{
    public class ReportService
    {
        private readonly ILogger<ReportService> logger;
        private readonly ConnectorDbContext context;

        public ReportService(ConnectorDbContext context, ILogger<ReportService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public DataTable GetProductReport(string description, string epc, string sku, DateTime initialDate, DateTime endDate)
        {
            if (!string.IsNullOrEmpty(epc))
            {
                epc = epc.PadLeft(24, '0');
            }

            DataTable dt = new DataTable();
            string sql;

            sql = @"
                WITH AccessEntries AS (
                    SELECT 
                        P.SKU, 
                        P.Description,
                        A.Status,
                        DATEADD(MINUTE, DATEDIFF(MINUTE, 0, A.AccessedOn), 0) AS EntryDate,
                        NULL AS ExitDate
                    FROM AccessControl A
                    INNER JOIN Product P ON P.Id = A.ProductId
                    WHERE 
                        A.Direction = 'IN'
                        AND DATEADD(MINUTE, DATEDIFF(MINUTE, 0, A.AccessedOn), 0)
                            BETWEEN DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @InitialDate), 0)
                            AND DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @EndDate), 0)
                        AND (@Description = '' OR P.Description LIKE '%' + @Description + '%')
                        AND (@EPC = '' OR P.EPC = @EPC)
                        AND (@SKU = '' OR P.SKU = @SKU)

                    UNION ALL

                    SELECT 
                        P.SKU, 
                        P.Description, 
                        A.Status,
                        NULL AS EntryDate,
                        DATEADD(MINUTE, DATEDIFF(MINUTE, 0, A.AccessedOn), 0) AS ExitDate
                    FROM AccessControl A
                    INNER JOIN Product P ON P.Id = A.ProductId
                    WHERE 
                        A.Direction = 'OUT'
                        AND DATEADD(MINUTE, DATEDIFF(MINUTE, 0, A.AccessedOn), 0)
                            BETWEEN DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @InitialDate), 0)
                            AND DATEADD(MINUTE, DATEDIFF(MINUTE, 0, @EndDate), 0)
                        AND (@Description = '' OR P.Description LIKE '%' + @Description + '%')
                        AND (@EPC = '' OR P.EPC = @EPC)
                        AND (@SKU = '' OR P.SKU = @SKU)
                )
                SELECT 
                    Description, 
                    SKU,
                    Status,
                    COUNT(*) AS Quantidade, 
                    MAX(ExitDate) AS ExitDate,
                    MIN(EntryDate) AS EntryDate
                FROM AccessEntries
                GROUP BY SKU, Description, Status, EntryDate, ExitDate
                ORDER BY COALESCE(EntryDate, ExitDate);";

            try
            {
                using (var cn = (SqlConnection)context.Database.GetDbConnection())
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
                logger.LogError(ex, "Erro GetProductReport");
            }

            return dt;
        }

        public DataTable GetAllEntrysAndExits(string description, string epc, string sku, DateTime initialDate, DateTime endDate)
        {
            if (!string.IsNullOrEmpty(epc))
            {
                epc = epc.PadLeft(24, '0');
            }

            DataTable dt = new DataTable();
            string sql;

            sql = @"
                WITH UltimaMovimentacao AS (
                    SELECT
                        P.EPC,
                        A.Direction,
                        ROW_NUMBER() OVER (
                            PARTITION BY P.EPC
                            ORDER BY A.AccessedOn DESC
                        ) AS RowNum
                    FROM AccessControl A
                    INNER JOIN Product P ON P.Id = A.ProductId
                    WHERE
                        A.AccessedOn BETWEEN @InitialDate AND @EndDate
                        AND (ISNULL(@Description, '') = '' OR P.Description LIKE '%' + @Description + '%')
                        AND (ISNULL(@EPC, '') = '' OR P.EPC = @EPC)
                        AND (ISNULL(@SKU, '') = '' OR P.SKU = @SKU)
                )
                SELECT
                    SUM(CASE WHEN Direction = 'IN'  THEN 1 ELSE 0 END) AS TotalEntradas,
                    SUM(CASE WHEN Direction = 'OUT' THEN 1 ELSE 0 END) AS TotalSaidas
                FROM UltimaMovimentacao
                WHERE RowNum = 1;";

            try
            {
                using (var cn = (SqlConnection)context.Database.GetDbConnection())
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
                logger.LogError(ex, "Erro GetAllEntrysAndExits");
            }

            return dt;
        }

        public long GetSequence()
        {
            string sql = "SELECT NEXT VALUE FOR dbo.GlobalCounter;";

            try
            {
                using (var cn = (SqlConnection)context.Database.GetDbConnection())
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cn.Open();
                        long sequenceValue = (long)cmd.ExecuteScalar();
                        cn.Close();
                        return sequenceValue;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao obter valor da sequência");
                return -1;
            }
        }

        public DataTable GetAllStock(string description, string sku, DateTime initialDate, DateTime endDate)
        {
            DataTable dt = new DataTable();
            string sql;

            sql = @"
                WITH UltimaMovimentacao AS (
                    SELECT
                        P.Description,
                        P.SKU,
                        A.Direction,
                        ROW_NUMBER() OVER (
                            PARTITION BY P.EPC
                            ORDER BY A.AccessedOn DESC
                        ) AS RowNum
                    FROM AccessControl A
                    INNER JOIN Product P ON P.Id = A.ProductId
                    WHERE
                        A.AccessedOn BETWEEN @InitialDate AND @EndDate
                        AND (ISNULL(@Description, '') = '' OR P.Description LIKE '%' + @Description + '%')
                        AND (ISNULL(@SKU, '') = '' OR P.SKU = @SKU)
                )
                SELECT
                    Description,
                    SKU,
                    SUM(CASE WHEN Direction = 'OUT' THEN 1 ELSE 0 END) AS QuantidadeLaundry,
                    SUM(CASE WHEN Direction = 'IN'  THEN 1 ELSE 0 END) AS QuantidadeStock,
                    SUM(CASE WHEN Direction = 'IN' THEN 1 ELSE 0 END) + SUM(CASE WHEN Direction = 'OUT' THEN 1 ELSE 0 END) AS Total
                FROM UltimaMovimentacao
                WHERE RowNum = 1
                GROUP BY Description, SKU;";

            try
            {
                using (var cn = (SqlConnection)context.Database.GetDbConnection())
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
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
                logger.LogError(ex, "Erro GetAllStock");
            }

            return dt;
        }

        public DataTable GetValidityReport(string description, string epc, string sku)
        {
            DataTable dt = new DataTable();

            string sql;

            sql = @"
                SELECT
                    P.Description,
                    P.SKU,
                    P.EPC,
                    CAST(RIGHT(P.EPC, 8) AS DATE) AS ValidityDate
                FROM Product P
                WHERE
                    P.DeletedOn IS NULL
                    AND (ISNULL(@Description, '') = '' OR P.Description LIKE '%' + @Description + '%')
                    AND (ISNULL(@SKU, '') = '' OR P.SKU = @SKU)
                    AND (ISNULL(@EPC, '') = '' OR P.EPC = @EPC)
                ORDER BY ValidityDate";

            try
            {
                using (var cn = (SqlConnection)context.Database.GetDbConnection())
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@EPC", (object)epc ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SKU", (object)sku ?? DBNull.Value);

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
                logger.LogError(ex, "Erro GetValidityReport");
            }

            return dt;
        }

        public DataTable GetLiveReport(string description, string epc, string sku, DateTime initialDate, DateTime endDate)
        {
            DataTable dt = new DataTable();

            string sql;

            sql = @"
                SELECT
                    P.Description,
                    P.SKU,
                    P.EPC,
                    G.AccessedOn
                FROM GeneralControl G
                INNER JOIN Product P ON P.Id = G.ProductId
                WHERE
                    G.AccessedOn BETWEEN @InitialDate AND @EndDate
                    AND (ISNULL(@Description, '') = '' OR P.Description LIKE '%' + @Description + '%')
                    AND (ISNULL(@EPC, '') = '' OR P.EPC = @EPC)
                    AND (ISNULL(@SKU, '') = '' OR P.SKU = @SKU)
                ORDER BY
                    G.AccessedOn ASC";

            try
            {
                using (var cn = (SqlConnection)context.Database.GetDbConnection())
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
                logger.LogError(ex, "Erro GetLiveReport");
            }

            return dt;
        }
    }
}
