using System;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
namespace Passenger.Infrastructure.Services
{
    public class FirebirdService : IFirebirdService
    {
        public FirebirdService()
        {
        }

        public async Task TestowePolaczenieAsync()
        {
            FbConnection conn = new FbConnection();  
            using (conn = new FbConnection(@"Server=localhost;User=SYSDBA;Password=masterkey;Database=D:\testowa_baza.fdb"))
            {
                conn.Open();
                using (FbCommand cmd = conn.CreateCommand())
                {
                    // first create the table for testing
                    cmd.CommandText = "recreate table GUID_test (guid char(16) character set octets)";
                    cmd.ExecuteNonQuery();
                }
                using (FbCommand cmd = conn.CreateCommand())
                {
                    // working with GUID
                    cmd.CommandText = "insert into GUID_test values (@guid)";
                    // classic way, works good
                    cmd.Parameters.Add("@guid", FbDbType.Char, 16).Value = Guid.NewGuid().ToByteArray();
                    // another way, maybe better readability, but same result
                    cmd.Parameters.Add("@guid", FbDbType.Guid).Value = Guid.NewGuid();
                    cmd.ExecuteNonQuery();
                }
                using (FbCommand cmd = conn.CreateCommand())
                {
                    // drop it, it has no real application
                    cmd.CommandText = "drop table GUID_test";
                    cmd.ExecuteNonQuery();
                }
            }
            await Task.CompletedTask;
        }
        
    }
}