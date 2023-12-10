using AspNetCoreHero.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Authentication.Configurations;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.UserManagement.Services.Queries
{
    public class GetAllMessages : IRequest<Result<IEnumerable<ChatRoomMessageModel>>>
    {
        public GetAllMessages(string username , ClaimsPrincipal claims)
        {
            this.Claims = claims;
            this.TargetUser = username;
        }
        public string TargetUser { get;  }
        public ClaimsPrincipal Claims { get; }

        public class Handler : BaseService, IRequestHandler<GetAllMessages, Result<IEnumerable<ChatRoomMessageModel>>>
        {
            private const string SuccessMessage = "Success getting all the messages.";
            private const string FailedMessage = "Failed to verify user.";

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
            public async Task<Result<IEnumerable<ChatRoomMessageModel>>> Handle(GetAllMessages query, CancellationToken cancellationToken)
            {
                try
                {

                    if (query.Claims.Identity == null) return Result<IEnumerable<ChatRoomMessageModel>>.Fail(FailedMessage);

                    

                    var user = this.ThesisUnitOfWork.Users.Entities
                        .Where(x => x.Username == query.Claims.Identity.Name).FirstOrDefault();

                    if (user == null)
                    {
                        return Result<IEnumerable<ChatRoomMessageModel>>.Fail(FailedMessage);
                    }

                    var target_user = this.ThesisUnitOfWork.Users.Entities
                       .Where(x => x.Username == query.TargetUser).FirstOrDefault();

                    if (target_user == null)
                    {
                        return Result<IEnumerable<ChatRoomMessageModel>>.Fail(FailedMessage);
                    }


                    var roomName = $"{String.Join("-", new[] { user.Id, target_user.Id }.OrderBy(x => x).ToArray())}";

                    var room = await this.ThesisUnitOfWork.ChatRooms.Entities.Where(x => x.Name == roomName).FirstOrDefaultAsync();
                    
                    if(room!= null)
                    {
                        var messages = room.Messages.ToList();
                        var results = this.Mapper.Map<ChatRoomMessageModel[]>(messages);
                        return Result<IEnumerable<ChatRoomMessageModel>>.Success(results, SuccessMessage);

                    }
                    else
                    {
                        return Result<IEnumerable<ChatRoomMessageModel>>.Success(new ChatRoomMessageModel[0], SuccessMessage);
                    }
                  

                }
                catch (Exception ex)
                {
                    return Result<IEnumerable<ChatRoomMessageModel>>.Fail(ex.Message);
                }
            }
        }
    }
}
