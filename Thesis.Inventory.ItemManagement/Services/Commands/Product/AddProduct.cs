using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Product.Requests;
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.ItemManagement.Services.Commands.Product
{
    public class AddProduct : IRequest<Result<GetProductResponse>>
    {
        public AddProduct(AddProductRequest request)
        {
            this.Request = request;
        }
        public AddProductRequest Request { get; }

        public class Handler : BaseService, IRequestHandler<AddProduct, Result<GetProductResponse>>
        {
            private const string SuccessMessage = "Success creating new product.";
            private const string FailedMessage1 = "Failed creating new category.";
            private const string FailedMessage2 = "There is already an existing product with that Id.";
            private const string FailedMessage3 = "Invalid Image file.";

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="thesisUnitOfWork">User unit of work.</param>
            /// <param name="mapper">THe mapper.</param>
            public Handler(IThesisUnitOfWork thesisUnitOfWork, IMapper mapper, JwtSettings jwtSettings)
                : base(thesisUnitOfWork, mapper, jwtSettings)
            {
            }

            /// <inheritdoc/>
            public async Task<Result<GetProductResponse>> Handle(AddProduct command, CancellationToken cancellationToken)
            {
                try
                {
                    var request = command.Request;

                    var existingProduct = this.ThesisUnitOfWork.Products.Entities.Where(x => x.ProductId == request.ProductId).FirstOrDefault();
                    if (existingProduct != null)
                    {
                        return Result<GetProductResponse>.Fail(FailedMessage2);
                    }

                  
                    var entity = this.Mapper.Map<ProductEntity>(request);
                    if (request.ProductImage != null && request.ProductImage != string.Empty)
                    {
                        var imagedata = request.ProductImage.Split(",");
                        var imagetype = imagedata[0];

                        if(!imagetype.Contains("jpeg") && !imagetype.Contains("png") && !imagetype.Contains("jpg"))
                        {
                            return Result<GetProductResponse>.Fail(FailedMessage3);
                        }

                        var b64 = imagedata[1];
                        var imagebytes = Convert.FromBase64String(b64);

                        entity.ImageType = imagetype;
                        entity.Image = imagebytes;
                    }


                    entity.DateDeleted = DateTime.Now;

                    await this.ThesisUnitOfWork.Products.AddAsync(entity);
                    await this.ThesisUnitOfWork.Save();

                    var response = this.Mapper.Map<GetProductResponse>(entity);

                    return Result<GetProductResponse>.Success(response, SuccessMessage);
                }
                catch (Exception ex)
                {
                    return Result<GetProductResponse>.Fail(ex.Message);
                }
            }
        }
    }
}
