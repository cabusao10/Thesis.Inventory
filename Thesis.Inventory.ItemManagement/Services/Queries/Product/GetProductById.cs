using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.Extensions;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Product
{
    public class GetProductById : IRequest<Result<GetProductResponse>>
    {
        public GetProductById(int id)
        {
            this.Id = id;
        }
        
        public int Id { get; set; }

        /// <summary>
        /// Get Products Handler.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<GetProductById, Result<GetProductResponse>>
        {
            private const string SuccessMessage = "Successfully retrieved products.";
            private const string ErrorMessage = "products not found.";

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
            public async Task<Result<GetProductResponse>> Handle(GetProductById query, CancellationToken cancellationToken)
            {
                var product = await this.ThesisUnitOfWork.Products.Entities.Where(x => x.Id == query.Id).FirstOrDefaultAsync();
                if(product == null)
                {
                    Result<GetProductResponse>.Fail(ErrorMessage);
                }

                var result = this.Mapper.Map<GetProductResponse>(product);

                return result != null
                    ? Result<GetProductResponse>.Success(result, SuccessMessage)
                    : Result<GetProductResponse>.Fail(ErrorMessage);
            }
        }
    }
}
