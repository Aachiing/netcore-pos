using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IPaymentRepository
    {
        Task PostCashPayment(OrderHeaderDTO dto);
        Task<ReceivableDTO> PostCreditPayment(OrderHeaderDTO dto);
    }
}
