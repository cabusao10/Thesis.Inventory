using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Product.Requests;
using Thesis.Inventory.Shared.DTOs.Users.Responses;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.ItemManagement.Services.Commands.Product
{
    public class AddProductCategory : IRequest<Result<bool>>
    {
        public AddProductCategory(AddProductCategoryRequest request)
        {
            this.Request = request;
        }

        public AddProductCategoryRequest Request { get; }


        /// <summary>
        /// Handler for command.
        /// </summary>
        public class Handler : BaseService, IRequestHandler<AddProductCategory, Result<bool>>
        {
            private const string SuccessMessage = "Success creating category.";
            private const string FailedMessage = "Failed creating new category.";

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
            public async Task<Result<bool>> Handle(AddProductCategory command, CancellationToken cancellationToken)
            {
                try
                {
                    var request = command.Request;

                    var entity = this.Mapper.Map<ProductCategoryEntity>(request);
                    entity.DateDeleted = DateTime.Now;

                    await this.ThesisUnitOfWork.ProductCategories.AddAsync(entity);
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
