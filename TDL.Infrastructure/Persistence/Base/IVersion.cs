namespace TDL.Infrastructure.Persistence.Base
{
    public interface IVersion
    {
        long RowVersion { get; set; }
    }
}
