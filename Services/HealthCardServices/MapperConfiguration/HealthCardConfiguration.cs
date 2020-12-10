using AutoMapper;
using Core.Models.DBModels;
using HealthCardServices.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCardServices.MapperConfiguration
{
    public class HealthCardConfiguration : Profile
    {
        public HealthCardConfiguration()
        {
            CreateMap<ChatMember, AllCardQueryResponseDomainModel>();
        }
    }
}
