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
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.UserManagement.Services.Queries
{
    public class GetProvincesQuery : IRequest<Result<IEnumerable<GetProvincesResponse>>>
    {
        public class Handler : BaseService, IRequestHandler<GetProvincesQuery, Result<IEnumerable<GetProvincesResponse>>>
        {
            private const string SuccessMessage = "Successfully retrieved provinces.";
            private const string ErrorMessage = "Provinces not found.";

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
            public async Task<Result<IEnumerable<GetProvincesResponse>>> Handle(GetProvincesQuery query, CancellationToken cancellationToken)
            {
                var provinces = await this.ThesisUnitOfWork.Provinces.GetAllAsync();

                var result = provinces.Select(x => new GetProvincesResponse { Key = x.Id.ToString(), Text = x.ProvinceName }).ToArray();
                return result.Any()
                    ? Result<IEnumerable<GetProvincesResponse>>.Success(result, SuccessMessage)
                    : Result<IEnumerable<GetProvincesResponse>>.Fail(ErrorMessage);
            }
        }
    }
}
