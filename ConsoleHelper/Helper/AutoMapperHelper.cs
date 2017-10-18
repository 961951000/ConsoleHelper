using AutoMapper;
using ConsoleHelper.Models;
using ConsoleHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelper.Helper
{
    public class AutoMapperHelper
    {
        public static BookViewModel Run(Book book)
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());
            var model = Mapper.Map<BookViewModel>(book);

            return model;
        }
    }

    public static class AutoMapperConfig
    {
        public static IMapper RegisterMappings()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Book, BookViewModel>().ForMember(dest => dest.Author, opts => opts.MapFrom(src => src.Author.Name));
            });
            var mapper = config.CreateMapper();

            return mapper;
        }
    }

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookViewModel>().ForMember(dest => dest.Author, opts => opts.MapFrom(src => src.Author.Name));
            //CreateMap<Book, BookViewModel>();
        }
    }
}
