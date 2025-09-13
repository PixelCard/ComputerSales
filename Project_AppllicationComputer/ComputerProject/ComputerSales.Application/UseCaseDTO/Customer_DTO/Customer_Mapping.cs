using ComputerSales.Domain.Entity.ECustomer;

namespace ComputerSales.Application.UseCaseDTO.Customer_DTO
{
    public static class Customer_Mapping
    {
        public static Customer ToEntity(this CustomerInputDTO input)
        {
            return Customer.create(
                input.IMG,
                input.Name,
                input.Description,
                input.Date,
                input.IDAccount
            );
        }

        public static CustomerOutputDTO ToResult(this Customer e)
        {
            return new CustomerOutputDTO(
                e.CustomerID,
                e.IMG,
                e.Name,
                e.Description,
                e.Date,
                e.IDAccount 
            );
        }
    }

}
