using System.Security.Cryptography;
using System.Text;
using MySqlConnector;
using Persistence;

namespace DAL
{
    public class StaffDAL
    {
        private string query = "";
        private MySqlConnection connection = DbConfig.GetConnection();

        internal Staff GetStaff(MySqlDataReader reader)
        {
            Staff staff = new Staff();
            staff.StaffID = reader.GetInt32("staff_id");
            staff.StaffName = reader.GetString("staff_name");
            staff.UserName = reader.GetString("user_name");
            staff.Password = reader.GetString("password");
            staff.RoleID = reader.GetInt32("role_id");
            staff.StaffStatus = reader.GetInt32("status");
            return staff;
        }
        public Staff GetStaffByID(int staffID)
        {
            Staff staff = new Staff();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = "SELECT * FROM staffs WHERE staff_id = @staffid;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@staffid", staffID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    staff = GetStaff(reader);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return staff;
        }
        public Staff GetAccount(string userName)
        {
            Staff staff = new Staff();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = "SELECT * FROM staffs WHERE user_name = @username;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@username", userName);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    staff = GetStaff(reader);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return staff;
        }
        public string CreateMD5(string input)
        {

            // Creates an instance of the default implementation of the MD5 hash algorithm.
            using (var md5Hash = MD5.Create())
            {
                // Byte array representation of source string
                var sourceBytes = Encoding.UTF8.GetBytes(input);

                // Generate hash value(Byte Array) for input data
                var hashBytes = md5Hash.ComputeHash(sourceBytes);

                // Convert hash byte array to string
                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hash;
            }
        }
    }
}