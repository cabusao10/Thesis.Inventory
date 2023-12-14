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

namespace Thesis.Inventory.ItemManagement.Services.Commands.Product
{
    public class AddUOM : IRequest<Result<bool>>
    {
        public AddUOM(AddUomRequest request)
        {
            this.Request = request;
        }
        public AddUomRequest Request { get; set; }

        public class Handler : BaseService, IRequestHandler<AddUOM, Result<bool>>
        {
            private const string SuccessMessage = "Success creating UOM.";
            private const string FailedMessage = "Failed creating new UOM.";

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
            public async Task<Result<bool>> Handle(AddUOM command, CancellationToken cancellationToken)
            {
                try
                {
                    var request = command.Request;

                    var entity = this.Mapper.Map<UOMEntity>(request);
                    entity.DateDeleted = DateTime.Now;

                    await this.ThesisUnitOfWork.UOM.AddAsync(entity);
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
