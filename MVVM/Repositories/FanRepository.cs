using Furia_FanHub.MVVM.Models;
using SQLite;

namespace Furia_FanHub.MVVM.Repositories
{
    public class FanRepository
    {
        private SQLiteAsyncConnection _database;

        public FanRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<FanProfile>().Wait();
        }

        public Task<int> SaveFanAsync(FanProfile fan) => _database.InsertAsync(fan);

        public Task<int> UpdateFanAsync(FanProfile fan) => _database.UpdateAsync(fan);

        public Task<FanProfile> GetFanByIdAsync(int id) => _database.Table<FanProfile>()
                                                                    .Where(f => f.Id == id)
                                                                    .FirstOrDefaultAsync();
        
        public Task<List<FanProfile>> GetAllFansAsync() => _database.Table<FanProfile>()
                                                                    .ToListAsync();

        public Task<FanProfile> GetFanByEmailAsync(string email) => _database.Table<FanProfile>()
                                                                    .Where(f => f.Email == email)
                                                                    .FirstOrDefaultAsync();

        public Task<FanProfile> GetFanByCPFAsync(string cpf) => _database.Table<FanProfile>()
                                                                    .Where(f => f.CPF == cpf)
                                                                    .FirstOrDefaultAsync();

        public Task<FanProfile> GetFanByLoginAsync(string email, string senha) => _database.Table<FanProfile>()
                                                                    .Where(f => f.Email == email && f.Senha == senha)
                                                                    .FirstOrDefaultAsync();

        public Task<int> DeleteFanAsync(FanProfile fan)
        {
            return _database.DeleteAsync(fan);
        }
    }
}
