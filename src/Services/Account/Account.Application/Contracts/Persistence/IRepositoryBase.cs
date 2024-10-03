using System.Linq.Expressions;
using Account.Domain.Common;

namespace Account.Application.Contracts.Persistence;

public interface IRepositoryBase<T> where T : EntityBase
    {
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    }