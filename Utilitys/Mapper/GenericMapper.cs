using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;

namespace Utilitys.Mapper
{
    public class GenericMapper : IMapper
    {
        public static List<TypePair> typePairs = new();
        private IMapper mapperContainer;


        public TDestination Map<TDestination, TSource>(TSource source, string? ignore = null)
        {
            Config<TDestination, TSource>(3, ignore);
            return mapperContainer.Map<TSource, TDestination>(source);
        }
        public IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, string? ignore = null)
        {
            Config<TDestination, TSource>(3, ignore);
            return mapperContainer.Map< IList<TSource>, IList<TDestination>>(source);
        }
        public TDestination Map<TDestination>(object source, string? ignore = null)
        {
            Config<TDestination, object>(3, ignore);
            return mapperContainer.Map<TDestination>(source);
        }
        public IList<TDestination> Map<TDestination>(IList<object> source, string? ignore = null)
        {
            Config<TDestination, object>(3, ignore);
            return mapperContainer.Map<IList<TDestination>>(source);
        }
        protected void Config<TDestination, TSource>(int depth = 3, string? ignore = null)
        {
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));
            if (typePairs.Any(x => x.DestinationType == typePair.DestinationType && x.SourceType == typePair.SourceType) && ignore is null)
                return;
            typePairs.Add(typePair);
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var item in typePairs)
                {
                    if (ignore is not null)
                    {
                        cfg.CreateMap(item.SourceType, item.DestinationType).MaxDepth(depth).ForMember(ignore, x => x.Ignore()).ReverseMap();
                    }
                    else
                    {
                        cfg.CreateMap(item.SourceType, item.DestinationType).MaxDepth(depth).ReverseMap();
                    }
                }
            });
            mapperContainer = config.CreateMapper();
        }
    }
}
