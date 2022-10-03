using Apache.NMS.ActiveMQ;

namespace WebApi.Helpers.Services
{
    public class ActiveMQConnection
    {
        public Connection connection { get; set; }
        public int Queue { get; }
        public bool IsInit { get; set; } = false;
        private string UserName { get; set; }
        private string Password { get; set; }

        public ActiveMQConnection(string userName, string password, int queue)
        {
            UserName = userName;
            Password = password;
            Queue = queue;
        }

        public void Init(ConnectionFactory connectionFactory)
        {
            connection = (Connection)connectionFactory.CreateConnection(UserName, Password);
            IsInit = true;
        }

        public bool IsStart()
        {
            if (connection != null) return connection.IsStarted;
            return false;
        }

        public void Start()
        {
            if (connection != null && !connection.IsStarted) connection.Start();
        }
    }
}
