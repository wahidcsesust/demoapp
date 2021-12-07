using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Data.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public GenericRepository(ApplicationDbContext _context)
        {
            this.context = _context;
            entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await entities.ToListAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<T>> GetAllActive()
        {
            return await entities.Where(t => !t.IsDeleted).ToListAsync();
        }
        public async Task<T> Get(long id)
        {
            try
            {
                return await entities.SingleOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            await context.SaveChangesAsync();
        }
        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.ModifiedDate = DateTime.Now;
            await context.SaveChangesAsync();
        }
        public async Task Save(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.Id == 0)
            {
                entities.Add(entity);
            }
            entity.CleanBeforeSave();
            await context.SaveChangesAsync();
        }
        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
