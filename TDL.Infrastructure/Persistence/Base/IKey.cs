namespace TDL.Infrastructure.Persistence.Base
{
    public interface IKey<TKey>
    {
        TKey Id { get; set; }
    }
}
