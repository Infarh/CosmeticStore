using CosmeticStore.Domain.Entities;
using CosmeticStore.Interfaces.Base.Repositories;

namespace CosmeticStore.Interfaces.Repositories;

public interface ICustomersRepository : IRepository<Customer>
{
    Task<Customer?> FindByName(string Name, CancellationToken Cancel = default);
}