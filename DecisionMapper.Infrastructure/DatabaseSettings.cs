namespace DecisionMapper.Infrastructure
{
    public class DatabaseSettings
    {
        public DatabaseSettings(string connectionString, string database)
        {
            ConnectionString = connectionString;
            Database = database;
        }

        public string ConnectionString { get; private set; }
        public string Database { get; private set; }
    }
}
