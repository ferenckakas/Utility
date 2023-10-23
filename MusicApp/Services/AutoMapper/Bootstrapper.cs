using AutoMapper;
using Services.AutoMapper.Profiles;

namespace Services.AutoMapper
{
    public class Bootstrapper
    {
        public static IMapper Mapper { get; set; }

        static Bootstrapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SearchResultProfile>());

            Mapper = config.CreateMapper();
        }
    }
}
