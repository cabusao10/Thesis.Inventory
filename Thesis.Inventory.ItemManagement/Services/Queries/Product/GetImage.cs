using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.Extensions;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs.Product.Response;
using Thesis.Inventory.Shared.DTOs;
using Microsoft.EntityFrameworkCore.Internal;

namespace Thesis.Inventory.ItemManagement.Services.Queries.Product
{
    public class GetImage : IRequest<byte[]>
    {
        public GetImage(int productid)
        {
            this.Productid = productid;
        }
        public int Productid { get; set; }

        public class Handler : BaseService, IRequestHandler<GetImage, byte[]>
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
            public async Task<byte[]> Handle(GetImage query, CancellationToken cancellationToken)
            {
                var product = this.ThesisUnitOfWork.Products.Entities
               .Where(x => x.Id == query.Productid).FirstOrDefault();
                if (product == null)
                {
                    return new byte[0];
                }

                return product.Image;
            }
        }
    }
}
