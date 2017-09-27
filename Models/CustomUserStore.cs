using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace sensoresapp.Models
{
    public class CustomUserSore<T> : IUserStore<T> where T : ApplicationUser
    {
        public Task CreateAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T user)
        {
            throw new NotImplementedException();
        }

        
        void IDisposable.Dispose()
        {
            // throw new NotImplementedException();

        }


    }
}