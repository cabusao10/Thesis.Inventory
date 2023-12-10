using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.DTOs;
using Thesis.Inventory.Shared.DTOs.Users.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Thesis.Inventory.UserManagement.Services.Queries
{
    public class GetActiveContacts : IRequest<Result<IEnumerable<GetActiveContactsResponse>>>
    {
        public GetActiveContacts(ClaimsPrincipal claims)
        {
            this.Claims = claims;
        }
        public ClaimsPrincipal Claims { get; }

        public class Handler : BaseService, IRequestHandler<GetActiveContacts, Result<IEnumerable<GetActiveContactsResponse>>>
        {
            private const string SuccessMessage = "Successfully retrieving contacts.";
            private const string ErrorMessage = "Contacts not found.";
            private const string FailedMessage2 = "UserN not found.";

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
            public async Task<Result<IEnumerable<GetActiveContactsResponse>>> Handle(GetActiveContacts query, CancellationToken cancellationToken)
            {
                var chatrooms = await this.ThesisUnitOfWork.ChatRooms.GetAllAsync();

                if (query.Claims.Identity == null) return Result<IEnumerable<GetActiveContactsResponse>>.Fail(FailedMessage2);

                var admin_name = query.Claims.Identity.Name;

                var admin = this.ThesisUnitOfWork.Users.Entities.Where(x => x.Username == admin_name).FirstOrDefault();

                if (admin == null)
                {
                    return Result<IEnumerable<GetActiveContactsResponse>>.Fail(FailedMessage2);
                }


                var chatroom_members_id = chatrooms.Select(x => x.Name.Split("-")).SelectMany(x => x)
                    .Select(x => Convert.ToInt32(x))
                    .Where(x => x != admin.Id)
                    .ToArray();

                var chatroom_members_info = this.ThesisUnitOfWork.Users.Entities.Where(x => chatroom_members_id.Contains(x.Id)).ToArray();

                var result = chatroom_members_info.Select(x => new GetActiveContactsResponse
                {
                    Fullname = x.Fullname,
                    UserId = x.Id,
                    Username = x.Username
                });


                return result != null
                    ? Result<IEnumerable<GetActiveContactsResponse>>.Success(result, SuccessMessage)
                    : Result<IEnumerable<GetActiveContactsResponse>>.Fail(ErrorMessage);
            }
        }
    }
}
