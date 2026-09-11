namespace Business.Features.Generic.Queries.GetAll
{
    public class GenericGetAllResponse<TEntity>
    {
        public List<TEntity>? entity { get; set; }
    }
}
