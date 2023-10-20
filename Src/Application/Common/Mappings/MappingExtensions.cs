using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    public static IQueryable<TDestination> GetPaginatedSource<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize, out int totalCount) where TDestination : class
        => PaginatedList<TDestination>.GetPaginatedSource(queryable, pageNumber, pageSize, out totalCount);
    public static PaginatedList<TDestination> ToPaginatedArray<TDestination>(this IEnumerable<TDestination> enumerable, int pageNumber, int pageSize, int totalCount) where TDestination : class
        => PaginatedList<TDestination>.FromPaginatedArray(enumerable, pageNumber, pageSize, totalCount);
    
}
