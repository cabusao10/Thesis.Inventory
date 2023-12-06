﻿using AspNetCoreHero.Results;
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
using Thesis.Inventory.Shared.DTOs.Product.Response;

namespace Thesis.Inventory.ItemManagement.Services.Commands.Product
{
    public class ArchiveProduct : IRequest<Result<bool>>
    {
        public ArchiveProduct(int id)
        {
            this.ProductId = id;
        }

        public int ProductId { get; }

        public class Handler : BaseService, IRequestHandler<ArchiveProduct, Result<bool>>
        {
            private const string SuccessMessage = "Success archiving product!";
            private const string FailedMessage1 = "Product not found.";

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
            public async Task<Result<bool>> Handle(ArchiveProduct command, CancellationToken cancellationToken)
            {
                try
                {
                    var product = this.ThesisUnitOfWork.Products.Entities.Where(x=> x.Id ==  command.ProductId).FirstOrDefault();
                    if(product == null)
                    {
                        return Result<bool>.Fail(FailedMessage1);
                    }

                    product.Status = Shared.Enums.ProductStatusType.Archived;
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
