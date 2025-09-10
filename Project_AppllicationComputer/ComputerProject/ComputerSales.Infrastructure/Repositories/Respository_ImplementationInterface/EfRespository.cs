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
        private readonly PropertyInfo? _keyProp; //property khóa chính (primary key) của entity TEntity

        public EfRepository(AppDbContext db)
        {
            _db = db;
            _set = _db.Set<TEntity>();
            _keyProp = FindKeyPropertyFromEfModel(db)  // lấy từ EF metadata
            ?? throw new InvalidOperationException(
                $"Primary key for entity {typeof(TEntity).Name} not found. " +
                $"Ensure it has a key (HasKey or [Key])."); 
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

            //Đoạn code tạo Lambda Expresion dựa trên _keyProp (ID)
            var param = Expression.Parameter(typeof(TEntity), "e");
            // e => ...

            var left = Expression.Property(param, _keyProp);
            //  e.KeyProperty 

            var right = Expression.Constant(Convert.ChangeType(id, _keyProp.PropertyType));
            // hằng số id được convert sang đúng kiểu của key property

            var body = Expression.Equal(left, right);
            // e.KeyProperty == id

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, param);
            // e => e.KeyProperty == id

            /*
              Ví dụ:

                TEntity = Product

                _keyProp = Product.ProductId

                id = 5

                => Lambda se la : e => e.ProductID == 5
            */

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



        private static PropertyInfo? FindKeyPropertyFromEfModel(DbContext db)
        {
            var entityType = db.Model.FindEntityType(typeof(TEntity));

            var pk = entityType?.FindPrimaryKey();

            if (pk == null) return null;

            if (pk.Properties.Count != 1)
                throw new NotSupportedException(
                    $"Entity {typeof(TEntity).Name} has composite key; this repository supports single key only.");

            var keyName = pk.Properties[0].Name;           // ví dụ: "ProtectionProductId"

            return typeof(TEntity).GetProperty(keyName)
                   ?? throw new InvalidOperationException(
                       $"Key property '{keyName}' not found on {typeof(TEntity).Name}.");
        }
    }
}
