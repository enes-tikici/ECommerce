using ECommerce.Core.Entities;
using ECommerce.Data.Context;
using ECommerce.Data.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceDbContext _context;
        private IDbContextTransaction? _transaction;
        public IRepository<User> Users { get; }
        public IRepository<Product> Products { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Cart> Carts { get; }
        public IRepository<CartItem> CartItems { get; }
        public IRepository<Order> Orders { get; }
        public IRepository<OrderDetail> OrderDetails { get; }

        public UnitOfWork(ECommerceDbContext context)
        {
            _context = context;

            Users = new Repository<User>(_context);
            Products = new Repository<Product>(_context);
            Categories = new Repository<Category>(_context);
            Carts = new Repository<Cart>(_context);
            CartItems = new Repository<CartItem>(_context);
            Orders = new Repository<Order>(_context);
            OrderDetails = new Repository<OrderDetail>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }


        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
               await _transaction.CommitAsync();
               await _transaction.DisposeAsync();

                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();

                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
