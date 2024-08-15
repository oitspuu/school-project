using Base;
using DAL.EF;
using Test.DAL;

namespace Test.BLL;

public class TestUoW : UnitOfWork<TestDbContext>
{
    private readonly TestDbContext _context;
    private readonly DomainDalMapper<TestEntity, TestEntity> _mapper;
    private TestEntityRepository? _repository;


    public TestUoW(TestDbContext context, DomainDalMapper<TestEntity, TestEntity> mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public TestEntityRepository Repository => _repository ?? new TestEntityRepository(_context, _mapper);

}