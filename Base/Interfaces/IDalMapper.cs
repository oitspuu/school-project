namespace Base.Interfaces;

public interface IDalMapper<TLeftObject, TRightObject>
    where TLeftObject : class
    where TRightObject : class
{
    TLeftObject? Map(TRightObject? input);
    TRightObject? Map(TLeftObject? input);
}