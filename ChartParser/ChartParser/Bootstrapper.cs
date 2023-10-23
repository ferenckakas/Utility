using AutoMapper;
using Common.Models;
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartParser
{
    public static class Bootstrapper
    {
        public static IMapper Mapper { get; set; }

        public static void Run()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Track, SongTitle>());

            Mapper = config.CreateMapper();
        }
    }
}
