using Project.Infrastructure.Model;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Project.CrossCutting.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace Project.Infrastructure
{
    public class UnitOfWork
    {
        private readonly ProjectContext _context;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(ProjectContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IRequestDetail RequestDetail => _serviceProvider.GetService<IRequestDetail>();
        // public Repository Repository => _serviceProvider.GetService<Repository>();

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public IDbContextTransaction BeginTransactionSync()
        {
            return _context.Database.BeginTransaction();
        }

        public void CommitSync()
        {
            _context.SaveChanges();
        }
    }
}