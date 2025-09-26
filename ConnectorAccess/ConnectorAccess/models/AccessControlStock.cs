using System;
using System.Data;
using System.Data.SqlClient;

namespace ConnectorAccess.models
{
    class AccessControlStock
    {
        protected static readonly Logger Logger = new Logger();

        public static DataTable GetAllStock(string description, string sku, DateTime initialDate, DateTime endDate)
        {
            DataTable dt = new DataTable();

            string sql = @"
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
                        AND P.DeletedOn IS NULL
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
                using (var cn = new SqlConnection(Conn.strConn))
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
                Logger.Error("Erro GenerateProductReport", ex);
            }

            return dt;
        }
    }
}
