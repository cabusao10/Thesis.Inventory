using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Product.Requests;
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.ItemManagement.Services.Commands.Product
{
    public class UpdateProduct : IRequest<Result<bool>>
    {
        public UpdateProduct(UpdateProductRequest request)
        {
            this.Request = request;
        }

        public UpdateProductRequest Request { get; }


        public class Handler : BaseService, IRequestHandler<UpdateProduct, Result<bool>>
        {
            private const string SuccessMessage = "Success updating product!";
            private const string FailedMessage1 = "Product not found.";
            private const string FailedMessage3 = "Invalid Image File.";

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
            public async Task<Result<bool>> Handle(UpdateProduct command, CancellationToken cancellationToken)
            {
                try
                {
                    var request = command.Request;

                    var product = this.ThesisUnitOfWork.Products.Entities.Where(x => x.Id == request.Id).FirstOrDefault();
                    if (product == null)
                    {
                        return Result<bool>.Fail(FailedMessage1);
                    }

                    product.Price = request.Price;
                    product.Quantity = request.Quantity;
                    product.Status = request.Status;
                    product.CategoryId = request.CategoryId;
                    product.UOMId = request.UOMId;
                    product.ProductId = request.ProductId;
                    product.ProductName = request.ProductName;
                    product.DateModified = DateTime.Now;
                    product.MinimumQuantity = request.MinimumQuantity;

                    if (request.ProductImage != null && request.ProductImage != string.Empty)
                    {
                        var imagedata = request.ProductImage.Split(",");

                      
                        var imagetype = imagedata[0];
                        if (!imagetype.Contains("jpeg") && !imagetype.Contains("png") && !imagetype.Contains("jpg"))
                        {
                            return Result<bool>.Fail(FailedMessage3);
                        }

                        var b64 = imagedata[1];
                        var imagebytes = Convert.FromBase64String(b64);

                        product.ImageType = imagetype;
                        product.Image = imagebytes;
                    }

                    await this.ThesisUnitOfWork.Save();

                    return Result<bool>.Success(true, SuccessMessage);
                }
                catch (Exception ex)
                {
                    return Result<bool>.Fail(ex.Message);
                }
            }
        }
    }
}
