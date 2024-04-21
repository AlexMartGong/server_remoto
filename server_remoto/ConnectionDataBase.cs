using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_remoto
{
    public class ConnectionDataBase
    {
        private MySqlConnection connection;
        private string server = "localhost";
        private string database = "db_people";
        private string user = "root";
        private string password = "root";
        private string cadenaConexion;

        public ConnectionDataBase()
        {
            cadenaConexion = "Database=" + database + "; DataSource=" + server + "; User Id=" + user + "; Password=" + password;
        }

        public MySqlConnection getConnection()
        {
            if (connection == null || !connection.Ping())
            {
                connection = new MySqlConnection(cadenaConexion);
                connection.Open();
            }
            return connection;
        }

    }
}
