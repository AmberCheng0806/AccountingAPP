using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳APP.Models.DTOs;
using 記帳APP.Models;
using System.Runtime.CompilerServices;

namespace 記帳APP.Util
{
    //internal static class Mapper<T1, T2>
    //{
    //    private static IMapper mapper;
    //    private static Action<IMappingExpression<T1, T2>> action;
    //    static Mapper()
    //    {
    //        var config = new MapperConfiguration(cfg =>
    //                 cfg.CreateMap<T1, T2>()
    //          );
    //        mapper = config.CreateMapper();
    //    }

    //    public static T2 Map(T1 source)
    //    {
    //        return mapper.Map<T1, T2>(source);
    //    }
    //    public static T2 Map(T1 source, Action<IMappingExpression<T1, T2>> action)
    //    {
    //        var config = new MapperConfiguration(cfg =>
    //              {
    //                  var mappingExpression = cfg.CreateMap<T1, T2>();
    //                  action?.Invoke(mappingExpression);
    //              }
    //          );
    //        var mapper2 = config.CreateMapper();
    //        return mapper2.Map<T1, T2>(source);
    //    }
    //}


    internal static class Mapper
    {
        public static T2 Map<T1, T2>(T1 source, Action<IMappingExpression<T1, T2>> action = null)
        {
            var config = new MapperConfiguration(cfg =>
                {
                    var mappingExpression = cfg.CreateMap<T1, T2>();
                    action?.Invoke(mappingExpression);
                }
            );
            var mapper2 = config.CreateMapper();
            return mapper2.Map<T1, T2>(source);
        }
    }
}
