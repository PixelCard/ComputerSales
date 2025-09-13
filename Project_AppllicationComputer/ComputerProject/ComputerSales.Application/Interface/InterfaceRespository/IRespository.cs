using System.Linq.Expressions;

namespace ComputerSales.Application.Interface.InterfaceRespository
{
    public interface IRespository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default); //Create

        Task<TEntity?> GetByIdAsync(object id, CancellationToken ct = default); //Get by ID

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

        Task<List<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            int? skip = null, int? take = null,
            CancellationToken ct = default);

        void Update(TEntity entity); //Update = Entity
        void Remove(TEntity entity); //Delete = Entity


        //Delete = ID //Update = ID
        Task UpdateByIdAsync(object id, Action<TEntity> updateAction, CancellationToken ct = default);
        Task DeleteByIdAsync(object id, CancellationToken ct = default);
    }
}
