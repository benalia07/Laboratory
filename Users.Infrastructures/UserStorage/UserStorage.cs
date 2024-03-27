using System.Data;

using GLAB.Domains.Models.Users;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Users.Infra.Storages
{
    public class UserStorage : IUserStorage
    {
        private string connectionString;

        private const string insertUserCommand =
            "Insert USERS(UserId, UserName, Password) VALUES(@aId, @aUserName, @aPassword)";
        
        private const string SelectUserByIdCommand =
            "Select * from USERS where UserId = @aUserId";
       
        private const string SelectUserPasswordCommand =
                "Select Password from USERS where UserId = @aUserId";
       
        private const string SelectUserByUserNameCommand =
            "Select * from USERS where UserName = @aUserName";
        
       
        
        public UserStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Redouane");
        }

        public async ValueTask<User?> SelectUserById(string userId)
        
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(SelectUserByIdCommand, connection);
            cmd.Parameters.AddWithValue("@aUserId", userId);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);
            
            if (ds.Rows.Count == 0)
                return null;

            return User.Create(
                (string)ds.Rows[0]["UserId"],
                (string)ds.Rows[0]["UserName"],
                (UserState)ds.Rows[0]["State"]
            );
            
        }

        public async ValueTask<string> SelectUserPassword(string userId)
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(SelectUserPasswordCommand, connection);
            cmd.Parameters.AddWithValue("@aUserId", userId);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            if (ds.Rows.Count == 0)
                return default;
            
            return (string)ds.Rows[0]["Password"];
        }

        public async ValueTask<User> SelectUserByUserName(string userName)
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(SelectUserByUserNameCommand, connection);
            cmd.Parameters.AddWithValue("@aUserName", userName);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);
            
            connection.Open();
            
            da.Fill(ds);

            if (ds.Rows.Count == 0)
                return null;
            
           var UserId= (string)ds.Rows[0]["UserId"];
           var UserName = (string)ds.Rows[0]["UserName"];
           var state= (UserState)ds.Rows[0]["Status"];
          
            return User.Create(
               UserId,UserName,state
            );
        }

        public async ValueTask<bool> InsertUser(User user)
        {
            await using var connection = new SqlConnection(connectionString);

            SqlCommand cmd = new(insertUserCommand, connection);
            cmd.Parameters.AddWithValue("@aId", user.UserId);
            cmd.Parameters.AddWithValue("@aUserName", user.UserName);
            cmd.Parameters.AddWithValue("@aPassword", user.Password);

            connection.Open();
            
            int insertedRows = await cmd.ExecuteNonQueryAsync();
            return (insertedRows > 0);
        }

       


        private const string selectUserRolesCommand = "select Roles.RoleId , Roles.RoleName from dbo.Roles roles" +
                                                      " join dbo.MembersRoles memberRoles ON " +
                                                      "memberRoles.RoleId=roles.RoleId " +
                                                      "where memberRoles.MemberId=@aMemberId";
        
        public async Task<List<ApplicationRole>> SelectUserRoles(string userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(selectUserRolesCommand, connection);
                cmd.Parameters.AddWithValue("@aMemberId", userId);
                await connection.OpenAsync();
                
                DataTable db = new DataTable();
                SqlDataAdapter da = new(cmd);

                da.Fill(db);
                List<ApplicationRole> roles = new List<ApplicationRole>();
                
                foreach (DataRow row in db.Rows)
                {
                    ApplicationRole userRole = new
                        ApplicationRole((string)row["RoleId"],(string)row["RoleName"]);
                    roles.Add(userRole);
                }
                
                
             
                return roles;
            }
        }
        
       
        
        
    }
}