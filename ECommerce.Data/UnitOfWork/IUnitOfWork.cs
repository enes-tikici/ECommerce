using ECommerce.Core.Entities;
using ECommerce.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        //IRepository<User> Users { get; }
        //IRepository<Product> Products { get; }
        //IRepository<Category> Categories { get; }
        //IRepository<Cart> Carts { get; }
        //IRepository<CartItem> CartItems { get; }
        //IRepository<Order> Orders { get; }
        //IRepository<OrderDetail> OrderDetails { get; }

        // Generic repository erişimi
        IRepository<T> GetRepository<T>() where T : BaseEntity;

        Task<int> CompleteAsync(); // SaveChangesAsync
        Task BeginTransactionAsync(); // Transaction başlat
        Task CommitAsync();           // Commit
        Task RollbackAsync();         // Rollback
    }
}
