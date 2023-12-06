using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Product
{
    public class GetProductUom : IRequest<Result<IEnumerable<GetUomResponse>>>
    {
        public GetProductUom()
        {
            
        }
        public class Handler : BaseService, IRequestHandler<GetProductUom, Result<IEnumerable<GetUomResponse>>>
        {
            private const string SuccessMessage = "Successfully retrieved UOM.";
            private const string ErrorMessage = "UOM not found.";

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
            public async Task<Result<IEnumerable<GetUomResponse>>> Handle(GetProductUom query, CancellationToken cancellationToken)
            {
                var uoms = await this.ThesisUnitOfWork.UOM.GetAllAsync();

                var result = this.Mapper.Map<IEnumerable<GetUomResponse>>(uoms);
                return result.Any()
                    ? Result<IEnumerable<GetUomResponse>>.Success(result, SuccessMessage)
                    : Result<IEnumerable<GetUomResponse>>.Fail(ErrorMessage);
            }
        }
    }
}
