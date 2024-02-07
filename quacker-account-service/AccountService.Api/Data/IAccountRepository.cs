using AccountService.Api.Models;
using System.Threading.Tasks;

namespace AccountService.Api.Data
{
    public interface IAccountRepository
    {
        void AddAccount(Account account);
    }
}