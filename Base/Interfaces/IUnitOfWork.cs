using System.Threading.Tasks;

namespace Base.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();

    void Clear();
}