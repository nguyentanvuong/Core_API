using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Concurrent;
using System.Threading;
namespace WebApi.Helpers.Services
{
    public class PooledActiveMQ
    {
        private ConnectionFactory _connectionFactory;
        private readonly BlockingCollection<ActiveMQConnection> _connection;
        public string Username {get; set;}
        public string Password { get; set; }
        public string Urlrequest { get; set; }
        public bool isAllowDirect { get; set; } = false;
        public int TimeOut { get; set; } = 60;
        public int MinConnection { get; set; } = 0;
        public int MaxConnection { get; set; } = 20;

        private int queueCount = 1;
        private int IsusingCount = 0;

        public PooledActiveMQ(string username, string password, string urlrequest)
        {
            Username = username;
            Password = password;
            Urlrequest = urlrequest;
            _connection = new BlockingCollection<ActiveMQConnection>(MaxConnection) ;
        }

        public PooledActiveMQ(string username, string password, string urlrequest, int minConnect, int maxConnect)
        {
            Username = username;
            Password = password;
            Urlrequest = urlrequest;
            MinConnection = minConnect;
            MaxConnection = maxConnect;
            _connection = new BlockingCollection<ActiveMQConnection>(MaxConnection);
        }

        public void Init()
        {
            ActiveMQConnection m_connect;
            _connectionFactory = new ConnectionFactory(Urlrequest);
            for (int i = 0; i < MaxConnection; i++)
            {
                m_connect = InitConnection();
                if (i <= MinConnection) m_connect.Init(_connectionFactory);
                _connection.Add(m_connect);
            }
        }

        public ActiveMQConnection InitConnection()
        {
            ActiveMQConnection m_connect;
            m_connect = new ActiveMQConnection(Username, Password, queueCount);
            Interlocked.Increment(ref queueCount);
            return m_connect;
        }


        public ActiveMQConnection GetConnection()
        {
            ActiveMQConnection m_connect;

            if (_connection.TryTake(out m_connect, TimeSpan.FromSeconds(TimeOut)))
            {
                Interlocked.Increment(ref IsusingCount);
                if (!m_connect.IsInit) m_connect.Init(_connectionFactory);
                return m_connect;
            }
            else
            {
                m_connect = (isAllowDirect) ? InitConnection() : null;
                if (m_connect != null) m_connect.Init(_connectionFactory);
            }
            return null;
        }

        public void ReleaseConnection(ActiveMQConnection con)
        {
            if (con != null)
            {
                if (IsusingCount > 0)
                {
                    if (_connection.TryAdd(con)) Interlocked.Decrement(ref IsusingCount);
                }
            }
        }

        public int TotalCount()
        {
            return this._connection.Count + this.IsusingCount;
        }

        public int AvailableCount()
        {
            return this._connection.Count;
        }

        public int InUsingCount()
        {
            return this.IsusingCount;
        }
    }
}
