using AutoMapper;
using Base.Interfaces;

namespace DAL.EF;

public class DomainDalMapper<TLeftObject, TRightObject> : IDalMapper<TLeftObject, TRightObject>
where TLeftObject : class
where TRightObject : class
{
    private readonly IMapper _mapper;
    
    public DomainDalMapper(IMapper mapper)
    {
        _mapper = mapper;
    }
    public TLeftObject? Map(TRightObject? input)
    {
        return _mapper.Map<TLeftObject>(input);
    }

    public TRightObject? Map(TLeftObject? input)
    {
        return _mapper.Map<TRightObject>(input);
    }
}