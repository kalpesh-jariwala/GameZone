using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace BLL_Core.Infrastructure.ExtensionMethods
{
    public static class AutomapperExtensions
    {
        /// <summary>
        /// <strong>MapTo - Maps Source Type to Destination Type </strong>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source">No need to pass the parameter, it will take from <c>this</c>.</param>
        /// <returns><strong>TDestination Type</strong></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            //TSource source;
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(source);
        }
    }
}

