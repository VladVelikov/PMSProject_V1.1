using Microsoft.EntityFrameworkCore;
using PMS.Data.Repository.Interfaces;

namespace PMS.Data.Repository
{
    public class GenericRepository<T, I>: IRepository<T, I>
        where T : class
    {
        private readonly PMSDbContext context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(PMSDbContext _context)
        {
            this.context = _context;
            this.dbSet = context.Set<T>();  
        }


        public async Task<bool> AddAsync(T item)
        {
            await dbSet.AddAsync(item);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> items)
        {
            await dbSet.AddRangeAsync(items);
            await context.SaveChangesAsync();
            return true;
        }       

        public IQueryable<T> GetAllAsQueryable()
        {
            return dbSet.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetByCompositeIdAsync(params I[] id)
        {
            return await dbSet.FindAsync(id[0], id[1]);
        }

        public async Task<T> GetByIdAsync(I id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<bool> DeleteByIdAsync(I id)
        {
            T item = await GetByIdAsync(id);
            dbSet.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> DeleteByCompositeIdAsync(params I[] id)
        {
            T item = await GetByCompositeIdAsync(id);
            dbSet.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveItemAsync(T item)
        {
            dbSet.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRangeAsync(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items);
            await context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateAsync(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRange(IEnumerable<T> itemList)
        {
            dbSet.AttachRange(itemList);
            foreach (var item in itemList)
            {
               context.Entry(item).State = EntityState.Modified;
            }
            await context.SaveChangesAsync();
            return true;
        }

    }
}
