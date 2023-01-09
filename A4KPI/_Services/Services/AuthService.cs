using Microsoft.EntityFrameworkCore;
using A4KPI.Data;
using A4KPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4KPI.Helpers;
using A4KPI._Repositories.Interface;
using A4KPI._Services.Interface;

namespace A4KPI._Services.Services
{
  
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _repo;

        public AuthService(
            IAccountRepository repo
            )
        {
            _repo = repo;
        }

        public async Task<bool> CheckLock(string username)
        {
            var account = await _repo.FindAll(x => x.Username == username).FirstOrDefaultAsync();

            if (account == null)
                return false;
            
            return account.IsLock;

        }

        public async Task<Account> Login(string username, string password)
        {
            var account = await _repo.FindAll(x => x.Username == username).FirstOrDefaultAsync();

            if (account == null)
                return null;
            if (account.Password.ToDecrypt() == password)
                return account;
            return null;

        }

        public async Task<bool> CheckPassword(string username, string password)
        {
            var account = await _repo.FindAll(x => x.Username == username).FirstOrDefaultAsync();

            if (account == null)
                return false;
            if (account.Password.ToDecrypt() == password)
                return true;
            return false;

        }

        public async Task<Account> LoginAnonymous(string username)
        {
            var account = await _repo.FindAll(x => x.Username == username).FirstOrDefaultAsync();
            if (account == null)
                return null;
            return account;

        }

    }
}
