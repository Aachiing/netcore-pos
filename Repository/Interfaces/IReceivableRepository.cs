using Sales_Inventory.Models.DTO;

namespace Sales_Inventory.Repository.Interfaces
{
    public interface IReceivableRepository
    {
        Task<IEnumerable<OrderHeaderDTO>> List();
        Task<IEnumerable<ReceivableDTO>> PaymentHistory(int order_id);
        Task<IEnumerable<OrderDetailsDTO>> OrderDetails(int order_id);
        Task AddPaymentHistory(ReceivableDTO dto);
        Task UpdatePayment(ReceivableDTO dto);
    }
}
