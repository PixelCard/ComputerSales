using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Repositories.Respository_ImplementationInterface
{
    public class EfRepository<TEntity> : IRespository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<TEntity> _set;
        private readonly PropertyInfo? _keyProp;

        public EfRepository(AppDbContext db)
        {
            _db = db;
            _set = _db.Set<TEntity>();
            _keyProp = FindKeyProperty(); // "Id" hoặc "{TypeName}Id"
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            await _set.AddAsync(entity, ct);
            return entity;
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
            => _set.AddRangeAsync(entities, ct);

        public async Task<TEntity?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (_keyProp is null) return null;
            // _set.FindAsync chỉ chạy tốt khi biết key; nhưng vì không chỉ định key qua model builder ở đây,
            // ta fallback về FirstOrDefault với Expression theo keyProp
            var param = Expression.Parameter(typeof(TEntity), "e");
            var left = Expression.Property(param, _keyProp);
            var right = Expression.Constant(Convert.ChangeType(id, _keyProp.PropertyType));
            var body = Expression.Equal(left, right);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, param);

            return await _set.FirstOrDefaultAsync(lambda, ct);
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => _set.FirstOrDefaultAsync(predicate, ct);

        public async Task<List<TEntity>> ListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes = null,
            int? skip = null, int? take = null,
            CancellationToken ct = default)
        {
            IQueryable<TEntity> q = _set.AsQueryable();
            if (includes != null) q = includes(q);
            if (predicate != null) q = q.Where(predicate);
            if (orderBy != null) q = orderBy(q);
            if (skip.HasValue) q = q.Skip(skip.Value);
            if (take.HasValue) q = q.Take(take.Value);
            return await q.ToListAsync(ct);
        }

        public void Update(TEntity entity) => _set.Update(entity);
        public void Remove(TEntity entity) => _set.Remove(entity);
        private static PropertyInfo? FindKeyProperty()
        {
            var t = typeof(TEntity);
            // Ưu tiên "Id"
            var p = t.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p != null) return p;
            // Thử "{TypeName}Id" (ví dụ: ProductOverviewId)
            var name = t.Name + "Id";
            return t.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        }

        public async Task UpdateByIdAsync(object id, Action<TEntity> updateAction, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);
            if (entity == null) throw new KeyNotFoundException($"{typeof(TEntity).Name} id={id} not found");

            updateAction(entity);      // áp thay đổi
            _set.Update(entity);
        }

        public async Task DeleteByIdAsync(object id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);
            if (entity == null) return;   // hoặc throw tuỳ bạn
            _set.Remove(entity);
        }
    }
}
