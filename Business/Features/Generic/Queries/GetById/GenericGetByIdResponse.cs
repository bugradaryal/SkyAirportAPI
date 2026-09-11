namespace Business.Features.Generic.Queries.GetById
{
    public class GenericGetByIdResponse<TEntity>
    {
        public TEntity? entity { get; set; }
    }
}
