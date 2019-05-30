using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;

namespace Contracts.Implementation.Tests.Extensions
{
    public static class TestExtensions
    {
        // https://www.loganfranken.com/blog/517/mocking-dbset-queries-in-ef6/
        public static DbSet<T> AsDbSet<T>(this IEnumerable<T> enumerable) where T : class
        {
            var queryable = enumerable.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return dbSet.Object;
        }
    }
}