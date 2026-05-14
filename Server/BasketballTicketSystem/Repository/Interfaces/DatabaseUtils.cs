//using System.Configuration;
//using MySqlConnector;

//public static class DatabaseUtils
//{
//    public static string GetConnectionString()
//    {
//        // This reads the "BasketballDB" string from your App.config
//        return ConfigurationManager.ConnectionStrings["BasketballTicketsDB"].ConnectionString;
//    }

//    public static MySqlConnection GetConnection()
//    {
//        return new MySqlConnection(GetConnectionString());
//    }
//}

//Useless now, since using ORM