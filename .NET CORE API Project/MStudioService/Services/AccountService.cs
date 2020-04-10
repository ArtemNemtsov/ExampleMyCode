using DBContext.Connect;
using DBContext.Models;
using MediaStudio.Core;
using MediaStudioService.Core.Classes;
using MediaStudioService.ModelBulder;
using MediaStudioService.Models.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStudioService.AccountService
{
    public class AccountService
    {
        private readonly MediaStudioContext postgres;
        private readonly IConfiguration Configuration;

        public object AccountManager { get; private set; }

        public AccountService(MediaStudioContext context, IConfiguration Configuration)
        {
            postgres = context;
            this.Configuration = Configuration;
        }

        public async Task<Responce> TryCreateAccountAsync(InputAccount inputAccount)
        {
            var login = inputAccount.Login;
            try
            {
                var typeAccount = await postgres.TypeAccount.FirstOrDefaultAsync(w => w.NameTtype == inputAccount.Role);

                if (LoginExists(login)) throw new InvalidOperationException($"Пользователь с логином {login} уже существует!");

                if(typeAccount == null) throw new InvalidOperationException($"Тип учетной записи {inputAccount.Role} не найден в базе даных!");

                var newAccount = AccountBulder.Create(typeAccount.IdTypeAccount,  inputAccount);
                postgres.Account.Add(newAccount);
                await postgres.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return RespоnceManager.CreateError(ex);
            }
            return RespоnceManager.CreateSucces($"Пользователь {login} успешно создан!");
        }

        public Responce GetToken(InputAccount inputAccount)
        {
            var login = inputAccount.Login;
            string access_token;
            try
            {
                if (!AccountExists(inputAccount)) throw new InvalidOperationException("Invalid username or password!");

                JWTManager tokenManager = new JWTManager(Configuration);
                var typeAccount = GetTypeAccount(login);
                access_token = tokenManager.BuldToken(typeAccount, login);
            }
            catch (Exception ex)
            {
                return RespоnceManager.CreateError(ex);
            }
            return RespоnceManager.CreateSucces(access_token);
        }

        private bool AccountExists(InputAccount inputAccount)
        {
            var login = inputAccount.Login;
            var password = inputAccount.Password;
            return postgres.Account.Any(a => a.Login == login && a.Password == password );
        }

        private bool LoginExists(string username)
        {
            return postgres.Account.Any(a => a.Login == username);
        }

        private async Task<TypeAccount> GetTypeAccount(string username)
        {
            var idTypeAcount = postgres.Account.Where(a => a.Login == username).FirstOrDefault();
            return await postgres.TypeAccount.FindAsync(idTypeAcount.IdTypeAccount);
        }
    }
}
