namespace Persistence
{
    public class StoreContextFactory : IStoreContextFactory
    {
        public IStoreContext CreateContext()
        {
            return new StoreContext();
        }
    }
}