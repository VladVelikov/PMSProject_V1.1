﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Data.Repository.Interfaces
{
        public interface IRepository<T, I>   /// Generic for all common cases, T= Type , I = id
        {
            Task<T> GetByIdAsync(I id);

            Task<T> GetByCompositeIdAsync(params I[] id);

            Task<IEnumerable<T>> GetAllAsync();

            IQueryable<T> GetAllAsQueryable();

            Task<bool> AddAsync(T item);

            Task<bool> AddRangeAsync(T[] items);

            Task<bool> UpdateAsync(T item);

            Task<bool> DeleteByIdAsync(I id);

            Task<bool> DeleteByCompositeIdAsync(params I[] id);
        }
    
}