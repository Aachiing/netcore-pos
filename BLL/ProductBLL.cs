using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;
using System.Runtime.CompilerServices;

namespace Sales_Inventory.BLL
{
    public class ProductBLL
    {
        private readonly IProductRepository _productrepository;

        public ProductBLL(IProductRepository productrepository)
        {
            _productrepository = productrepository;
        }

        public async Task<ResponseDTO> AddProduct(ProductDTO dto)
        {
            bool existing_product = await _productrepository.ExistingProduct(dto);
            try
            {
                if (existing_product)
                {
                    return new ResponseDTO
                    {
                        StatusCode = 400,
                        Message = "Product already exist!"
                    };
                }

                _productrepository.Create(dto);
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    StatusCode = 400,
                    Message = ex.Message
                };
            }

            return new ResponseDTO
            {
                StatusCode = 200,
                Message = "Product added!"
            };
        }


    }
}

