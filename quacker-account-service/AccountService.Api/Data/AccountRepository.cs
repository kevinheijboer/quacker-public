using AccountService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Api.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddAccount(Account account)
        {
             _context.Accounts.Add(account);
             _context.SaveChanges();
        }
    }
}
