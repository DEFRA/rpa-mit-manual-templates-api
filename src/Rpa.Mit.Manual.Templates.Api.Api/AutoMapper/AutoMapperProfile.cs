using Rpa.Mit.Manual.Templates.Api.Api.Models;
using Rpa.Mit.Manual.Templates.Api.Core.Entities;

using AutoMapper;

namespace Rpa.Mit.Manual.Templates.Api.Api.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ItemRequest, Item>();
    }
}
