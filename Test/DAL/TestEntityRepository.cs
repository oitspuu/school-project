using Base;
using Base.Interfaces;

namespace Test.DAL;

public class TestEntityRepository : BaseEntityRepository<TestEntity, TestEntity, TestDbContext>
{
    public TestEntityRepository(TestDbContext dbContext, IDalMapper<TestEntity, TestEntity> mapper) : base(dbContext, mapper)
    {
    }
}