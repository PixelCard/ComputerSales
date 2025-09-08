using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.ProductProtection_UC
{
    public class CreateProductProtection_UC
    {
        private IRespository<ProductProtection> _repoProductProtection;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public CreateProductProtection_UC(IRespository<ProductProtection> repoProductProtection,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repoProductProtection = repoProductProtection;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductProtectionOutput> HandleAsync(ProductProtectionInputcs input, CancellationToken ct)
        {
            ProductProtection productProtection = input.ToEnity();

            await _repoProductProtection.AddAsync(productProtection, ct);

            await _unitOfWorkApplication.SaveChangesAsync();

            return productProtection.ToResult();
        }
    }
}
