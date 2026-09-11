namespace Business.Features.Generic
{
    public interface IConvertToEntity<TEntity>
    {
        TEntity ToEntity();
    }
}
