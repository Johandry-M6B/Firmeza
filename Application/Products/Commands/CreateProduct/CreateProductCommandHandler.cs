using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
   private readonly IProductRepository _productRepository;

   public CreateProductCommandHandler(IProductRepository productRepository)
   {
      _productRepository = productRepository;
   }

   public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
   {
      if (await _productRepository.ExistsCodeAsync(request.Code))
      {
         throw new DuplicateProductCodeException(request.Code);
      }
      
      var product = new Product(
         code: request.Code,
         name: request.Name,
         description: request.Description,
         categoryId: request.CategoryId,
         measurementId: request.MeasurementId,
         buyerPrice: request.BuyerPrice,
         salePrice: request.SalePrice,
         wholesalePrice: request.WholesalePrice,
         initialStock: request.InitialStock,
         minimumStock: request.MinimumStock,
         supplierId: request.SupplierId
      );

      if (!string.IsNullOrWhiteSpace(request.Mark) ||
          !string.IsNullOrWhiteSpace(request.Model) ||
          request.RequiredRefrigeration ||
          request.DangerousMaterial)
      {
         product.UpdatePhysicalCharacteristics(
            mark: request.Mark,
            model: request.Model,
            color: request.Color,
            weight: request.Weight,
            size: request.Size,
            requiredRefrigeration: request.RequiredRefrigeration,
            dangerousMaterial: request.DangerousMaterial
         );
      }
      
      var createProduct = await _productRepository.AddAsync(product);
      
      return createProduct.Id; 
   }   
}