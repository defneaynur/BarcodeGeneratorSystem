using BarcodeGeneratorSystem.Api.Services.Base;
using BarcodeGeneratorSystem.Domain.Models.DatabaseModel;
using Dapper;
using System.Data;

namespace BarcodeGeneratorSystem.Api.Services.Processor
{
    public interface IUserProcessors
    {
        Task<Users> RegisterUserAsync(Users user);
        Task<Users> GetUserWithUserNameAsync(string username);
        Task<Users> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(long id);
        Task<IEnumerable<Users>> GetUsersAsync();
    }

    public class UserProcessors(IDbConnection _dbConnection) : IUserProcessors
    {
        /// <summary>
        /// This method return Users
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Users>> GetUsersAsync()
        {
            const string query = "SELECT * FROM Users WHERE IsDeleted = 0 OR IsDeleted IS NULL";

            var result = await _dbConnection.QueryAsync<Users>(query);

            return result;
        }

        /// <summary>
        /// This method run for user register operation
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Users> RegisterUserAsync(Users user)
        {
            const string query = @"
                    INSERT INTO Users (UserName, Password, Email, Name, Creator, IsDeleted)
                    VALUES (@UserName, @Password, @Email, @Name, @Creator, @IsDeleted)";

            var result = await _dbConnection.ExecuteAsync(query, user);

            return user;
        }

        /// <summary>
        /// This method run for update user operation
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Users> UpdateUserAsync(Users user)
        {
            var isChangedUser = user.Password != GetUserWithUserIdAsync(user.Id).Result.Password;
            if (isChangedUser)
                user.Password = Utility.HashPassword(user.Password);

            const string query = @"UPDATE Users SET UserName = @UserName, Password = @Password, Email = @Email, Name = @Name, Changer = @Changer, Changed = @Changed WHERE Id = @Id";

            var result = await _dbConnection.ExecuteAsync(query, user);

            return user;
        }

        /// <summary>
        /// This method run for delete user operation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAsync(long id)
        {
            const string query = @"UPDATE Users SET IsDeleted = 1 WHERE Id = @Id";

            var result = await _dbConnection.ExecuteAsync(query, new { Id = id });

            return result == 1;
        }

        /// <summary>
        /// This method return user filtered with username 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<Users> GetUserWithUserNameAsync(string username)
        {
            const string query = "SELECT * FROM Users WHERE Username = @UserName";

            var user = await _dbConnection.QueryFirstOrDefaultAsync<Users>(query, new { Username = username });

            return user;
        }

        /// <summary>
        /// This method return user filtered with userid 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Users> GetUserWithUserIdAsync(long id)
        {
            const string query = "SELECT * FROM Users WHERE Id = @Id";

            var user = await _dbConnection.QueryFirstOrDefaultAsync<Users>(query, new { Id = id });

            return user;
        }

    }

}
