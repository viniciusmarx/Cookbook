using AutoMapper;
using CommomTestUtilities.Encryption;
using Cookbook.Application.Services.AutoMapper;

namespace CommomTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncripter = IdEncripterBuilder.Build();

        return new MapperConfiguration(options => options.AddProfile(new AutoMapping(idEncripter))).CreateMapper();
    }
}
