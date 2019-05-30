namespace Persistence
{
    public interface IStoreContextFactory
    {
        IStoreContext CreateContext();
    }
}