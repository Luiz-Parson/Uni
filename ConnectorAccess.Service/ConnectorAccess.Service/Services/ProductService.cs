using ConnectorAccess.Service.data;
using ConnectorAccess.Service.models;
using ConnectorAccess.Service.models.dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConnectorAccess.Service.Services
{
    public class ProductService
    {
        private readonly ConnectorDbContext context;
        private readonly ILogger<ProductService> logger;
        private readonly string connectionString;

        public ProductService(ConnectorDbContext context, ILogger<ProductService> logger, IConfiguration configuration)
        {
            this.context = context;
            this.logger = logger;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Product GetProductByEpc(string epc)
        {
            return context.Product.FirstOrDefault(p => p.EPC == epc && p.DeletedOn == null);
        }

        public List<Product> GetAllProducts()
        {
            return context.Product.Where(p => p.DeletedOn == null).ToList();
        }

        public List<SKUAndDescriptionDTO> GetAllSKUsAndDescriptions()
        {
            return context.Product
                          .Where(p => p.DeletedOn == null)
                          .Select(p => new SKUAndDescriptionDTO
                          {
                              SKU = p.SKU,
                              Description = p.Description
                          })
                          .Distinct()
                          .ToList();
        }

        public DataTable GenerateExclusionReport(string description, string epc, string sku, DateTime initialDate, DateTime endDate)
        {
            if (!string.IsNullOrEmpty(epc))
            {
                epc = epc.PadLeft(24, '0');
            }

            DataTable dt = new DataTable();

            string sql = @"
                SELECT 
                    P.Description,
                    P.SKU,
                    P.EPC,
                    E.Category,
                    (
                        SELECT COUNT(*) 
                        FROM ExclusionControl EC
                        INNER JOIN Product PP ON EC.ProductId = PP.Id
                        WHERE PP.EPC = P.EPC
                    ) AS QuantityOfDeletes,
                    E.ExcludedOn
                FROM ExclusionControl E
                INNER JOIN Product P ON P.Id = E.ProductId
                WHERE 
                    E.ExcludedOn BETWEEN @InitialDate AND @EndDate
                    AND (@Description IS NULL OR P.Description LIKE '%' + @Description + '%')
                    AND (@EPC IS NULL OR P.EPC = @EPC)
                    AND (@SKU IS NULL OR P.SKU = @SKU)
                ORDER BY 
                    E.ExcludedOn DESC;";

            try
            {
                using (var cn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Description", string.IsNullOrWhiteSpace(description) ? (object)DBNull.Value : description);
                        cmd.Parameters.AddWithValue("@EPC", string.IsNullOrWhiteSpace(epc) ? (object)DBNull.Value : epc);
                        cmd.Parameters.AddWithValue("@SKU", string.IsNullOrWhiteSpace(sku) ? (object)DBNull.Value : sku);
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
                logger.LogError(ex, "Erro ao gerar relatório de exclusões.");
            }

            return dt;
        }

        public Product AddProduct(string description, string sku, string epc, string createdBy)
        {
            var product = new Product
            {
                Description = description,
                SKU = sku,
                EPC = epc,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = createdBy,
                UpdatedOn = DateTime.Now
            };

            context.Product.Add(product);
            context.SaveChanges();

            return product;
        }

        public List<Product> AddMultipleProducts(List<ProductDTO> productDtos)
        {
            var products = productDtos.Select(productDto => new Product
            {
                Description = productDto.Description,
                SKU = productDto.SKU,
                EPC = productDto.EPC,
                CreatedBy = productDto.CreatedBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = productDto.CreatedBy,
                UpdatedOn = DateTime.Now
            }).ToList();

            context.Product.AddRange(products);
            context.SaveChanges();

            return products;
        }


        public Product UpdateProductById(int id, string description, string sku, string epc, string updatedBy)
        {
            var product = context.Product.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                throw new Exception("Produto não encontrado!");
            }

            product.Description = description;
            product.SKU = sku;
            product.EPC = epc;
            product.UpdatedBy = updatedBy;
            product.UpdatedOn = DateTime.Now;

            context.SaveChanges();

            return product;
        }

        public Product DeleteProductById(int id, string deletedBy)
        {
            var product = context.Product.FirstOrDefault(p => p.Id == id && p.DeletedOn == null);

            if (product == null)
            {
                throw new Exception("Produto não encontrado ou já foi excluído!");
            }

            product.DeletedOn = DateTime.Now;
            product.DeletedBy = deletedBy;
            product.UpdatedBy = deletedBy;
            product.UpdatedOn = DateTime.Now;

            context.SaveChanges();

            return product;
        }

        public List<Product> DeleteMultipleProducts(List<int> ids, string deletedBy)
        {
            var products = context.Product
                                  .Where(p => ids.Contains(p.Id) && p.DeletedOn == null)
                                  .ToList();

            if (products.Count == 0)
            {
                throw new Exception("Nenhum produto válido encontrado para exclusão!");
            }

            var now = DateTime.Now;

            foreach (var product in products)
            {
                product.DeletedOn = now;
                product.DeletedBy = deletedBy;
                product.UpdatedOn = now;
                product.UpdatedBy = deletedBy;
            }

            context.SaveChanges();

            return products;
        }
    }
}
