using ConnectorAccess.Service.models.dtos;
using ConnectorAccess.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConnectorAccess.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductController(ProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("getByEpc/{epc}")]
        public IActionResult GetProductByEpc(string epc)
        {
            var product = productService.GetProductByEpc(epc);

            if (product == null)
            {
                return NotFound("Produto não encontrado para o EPC informado.");
            }

            return Ok(product);
        }

        [HttpGet("getAll")]
        public IActionResult GetAllProducts()
        {
            var products = productService.GetAllProducts();

            if (products == null || !products.Any())
            {
                return NotFound("Nenhum produto encontrado.");
            }

            return Ok(products);
        }

        [HttpGet("getAllSKUsAndDescriptions")]
        public IActionResult GetAllSKUsAndDescriptions()
        {
            var skus = productService.GetAllSKUsAndDescriptions();

            if (skus == null || !skus.Any())
            {
                return NotFound("Nenhum SKU encontrado.");
            }

            return Ok(skus);
        }

        [HttpPost("getAllExclusions")]
        public IActionResult GetExclusionReport([FromBody] ExclusionReportRequestDTO exclusionReportRequestDTO)
        {
            DataTable report = productService.GenerateExclusionReport(
                exclusionReportRequestDTO.Description,
                exclusionReportRequestDTO.EPC,
                exclusionReportRequestDTO.SKU,
                exclusionReportRequestDTO.InitialDate,
                exclusionReportRequestDTO.EndDate
            );

            string result = JsonConvert.SerializeObject(report);
            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult AddProduct([FromBody] ProductDTO productDto)
        {
            var product = productService.AddProduct(
                productDto.Description,
                productDto.SKU,
                productDto.EPC,
                productDto.CreatedBy
            );

            return Ok(product);
        }

        [HttpPost("add-multiple")]
        public IActionResult AddMultipleProducts([FromBody] List<ProductDTO> productDtos)
        {
            var addedProducts = productService.AddMultipleProducts(productDtos);
            return Ok(addedProducts);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateProductById(int id, [FromBody] ProductUpdateDTO productDto)
        {
            var updatedProduct = productService.UpdateProductById(
                id,
                productDto.Description,
                productDto.SKU,
                productDto.EPC,
                productDto.UpdatedBy
            );

            return Ok(updatedProduct);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteProductById(int id, [FromQuery] string deletedBy)
        {
            try
            {
                var deletedProduct = productService.DeleteProductById(id, deletedBy);
                return Ok(new
                {
                    message = "Produto excluído com sucesso!",
                    product = deletedProduct
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("delete-multiple")]
        public IActionResult DeleteMultipleProducts([FromBody] DeleteMultipleDTO dto)
        {
            try
            {
                var deletedProducts = productService.DeleteMultipleProducts(dto.Ids, dto.DeletedBy);
                return Ok(new
                {
                    message = $"{deletedProducts.Count} produtos excluídos com sucesso.",
                    products = deletedProducts
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
