using Newtonsoft.Json;
using ZaminEducation.Domain.Configurations;

namespace ZaminEducation.Service.Extensions
{
    public static class CollectionExtension
    {
        public static IEnumerable<T> ToPageList<T>(this IEnumerable<T> source, PaginationParmas @params)
        {
            var metaData = new PaginationMetaData(source.Count(), @params);

            var json = JsonConvert.SerializeObject(metaData);

            // davomi bor...
            // HttpContextHelper help qilishi kerak

            return @params.PageSize > 0 && @params.PageIndex >= 0
                ? source.Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize)
                : source;
        }
    }
}
