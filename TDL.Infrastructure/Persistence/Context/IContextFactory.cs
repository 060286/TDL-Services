namespace TDL.Infrastructure.Persistence.Context
{
    public interface IContextFactory<out TContext> where TContext : BaseDbContext
    {
        TContext Create();
    }
}
