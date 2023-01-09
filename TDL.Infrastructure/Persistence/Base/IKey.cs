namespace TDL.Infrastructure.Persistence.Base
{
    internal interface IKey<TKey>
    {
        TKey Id { get; set; }
    }
}
