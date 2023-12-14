using AutoMapper.Features;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Shared.DTOs.Users.Responses;
using Thesis.Inventory.Shared.DTOs.Users.Requests;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.UserManagement.Mappings
{
    public class GeneralProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralProfile"/> class.
        /// </summary>
        public GeneralProfile()
        {

            this.CreateMap<UserEntity, UserLoginResponse>().ReverseMap();
            this.CreateMap<UserEntity, GetUserResponse>().ReverseMap();
            this.CreateMap<UserEntity, UpdateUserRequest>().ReverseMap();
            this.CreateMap<UserRegisterRequest, UserEntity>().ReverseMap();
            this.CreateMap<ChatMessageEntity, ChatRoomMessageModel>().ReverseMap();

        }
    }
}
