using Glab.Domains.Models.Roles;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Glab.Infrastructures.Storages.RolesStorages
{
    public class RoleStorage : IRoleStorage
    {
        private readonly string connectionString;
        private const string selectRolesQuery = "select * from dbo.ROLES";
        private const string selectRoleByIdQuery = "SELECT * FROM ROLES WHERE RoleId = @aRoleId";
        private const string selectRoleByNameQuery = "SELECT * FROM ROLES WHERE RoleName = @aRoleName";

        public RoleStorage(IConfiguration configuration) =>
       connectionString = configuration.GetConnectionString("db_aa5c49_rachediradouane");

        private static Role getRoleFromDataRow(DataRow row)
        {
            return new()
            {
                RoleId = (string)row["RoleId"],
                RoleName = (string)row["RoleName"],
         
            };
        }

        public async Task<List<Role>> SelectRoles()
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(selectRolesQuery, connection);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            return (from DataRow row in ds.Rows select getRoleFromDataRow(row)).ToList();
        }
        public async Task<Role?> SelectRoleById(string roleId)
        {
            await using var connection = new SqlConnection(connectionString);

            SqlCommand cmd = new(selectRoleByIdQuery, connection);
            cmd.Parameters.AddWithValue("@aRoleId", roleId);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            return ds.Rows.Count == 0 ? null : getRoleFromDataRow(ds.Rows[0]);
        }

        public async Task<Role?> SelectRoleByName(string roleName)
        {
            await using var connection = new SqlConnection(connectionString);

            SqlCommand cmd = new(selectRoleByNameQuery, connection);
            cmd.Parameters.AddWithValue("@aRoleName", roleName);

            DataTable ds = new();
            SqlDataAdapter da = new(cmd);

            connection.Open();
            da.Fill(ds);

            return ds.Rows.Count == 0 ? null : getRoleFromDataRow(ds.Rows[0]);
        }




    }
}
