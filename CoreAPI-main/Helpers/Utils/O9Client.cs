using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.Util;
using System;
using WebApi.Helpers.Services;
using WebApi.Helpers.Common;
using System.Net;
using System.IO;

namespace WebApi.Helpers.Utils
{
    public class O9Client
    {
        public static PooledActiveMQ activeMQ;
        public static O9MemCached memCached;
        public static CoreConfig coreConfig;
        public static string O9_GLOBAL_SEPARATE_ADDRESS { get; private set; }
        public static string OP_MCKEY_FMAP { get; private set; }
        public static string Comcode { get; set; }
        private static string m_userName;
        private static string m_passWord;
        public static string m_urlRequest;
        private static string m_requestQueueName;
        private static string m_requestQueueNameASynchronize;
        private static string m_replyQueueName;
        private static string m_replyNotificationQueueName;
        private static string m_functionRequestID;
        private static string m_functionReplyID;
        private static string m_functionAsynchronizeRequestID;
        private static string m_functionAsynchronizeReplyID;
        private static int m_messageClienTimeOut;
        private static int m_sizeSessionID;
        public static bool isInit { get; private set; } = false;
        private ActiveMQConnection m_connection;
        private Session m_messageSession;
        private MessageProducer m_messageProducer;
        private ActiveMQDestination m_destination;
        private ActiveMQDestination m_destinationAsynchronize;
        private ActiveMQTextMessage m_message;
        private ActiveMQDestination m_destinationReply;
        private MessageConsumer m_messageConsumerReply;

        public static void Init(string ipmemcached, AppSettings app)
        {
            memCached = new O9MemCached(ipmemcached);
            coreConfig = app.Configure;
            InitParam();
            if (string.IsNullOrEmpty(m_urlRequest)) return;
            activeMQ = new PooledActiveMQ(m_userName, m_passWord, m_urlRequest, 5, app.PoolConnection);
            activeMQ.Init();
            GlobalVariable.TIME_UPDATE_TXDT = coreConfig.WKDTimes;
            if (!string.IsNullOrEmpty(m_urlRequest)) isInit = true;
        }

        public static void InitParam()
        {
            O9_GLOBAL_SEPARATE_ADDRESS = memCached.GetValue("OP_SERVER_MESSAGE_SEPARATE_ADDRESS");
            OP_MCKEY_FMAP = memCached.GetValue("OP_MCKEY_FMAP");
            m_userName = memCached.GetValue("OP_CLIENT_MESSAGE_USERNAME");
            m_passWord = memCached.GetValue("OP_CLIENT_MESSAGE_PASSWORD");
            m_userName = O9Encrypt.Decrypt(m_userName);
            m_passWord = O9Encrypt.Decrypt(m_passWord);
            m_urlRequest = memCached.GetValue("OP_CLIENT_MESSAGE_BROKER_URL");
            m_requestQueueName = memCached.GetValue("OP_CLIENT_MESSAGE_QUEUE_NAME");
            m_requestQueueNameASynchronize = memCached.GetValue("OP_CLIENT_MESSAGE_ASYNCHRONIZE_QUEUE_NAME");
            m_replyQueueName = memCached.GetValue("OP_CLIENT_MESSAGE_REPLY_NAME");
            m_replyQueueName = m_replyQueueName + O9_GLOBAL_SEPARATE_ADDRESS;
            m_replyNotificationQueueName = memCached.GetValue("OP_CLIENT_MESSAGE_NOTIFICATION_REPLY_NAME");
            m_functionRequestID = memCached.GetValue("OP_CLIENT_FUNCTION_REQUEST_ID");
            m_functionReplyID = memCached.GetValue("OP_CLIENT_FUNCTION_REPLY_ID");
            m_functionAsynchronizeRequestID = memCached.GetValue("OP_CLIENT_FUNCTION_ASYNCHRONIZE_REQUEST_ID");
            m_functionAsynchronizeReplyID = memCached.GetValue("OP_CLIENT_FUNCTION_ASYNCHRONIZE_REPLY_ID");
            int.TryParse(memCached.GetValue("OP_CLIENT_MESSAGE_CLIENT_TIMEOUT"),out m_messageClienTimeOut);
            int.TryParse(memCached.GetValue("OP_SIZE_OF_SESSIONID"),out m_sizeSessionID);
        }

        public string SendString(string text, string functionID, string userId, string sessionID, EnmCacheAction enmCacheAction, EnmSendTypeOption sendType, MsgPriority priority)
        {
            string strReturn = String.Empty;
            try
            {
                m_connection = O9Client.activeMQ.GetConnection();
                if (m_connection != null)
                {
                    if (!m_connection.IsStart()) m_connection.Start();
                    m_messageSession = (Session)m_connection.connection.CreateSession();
                    m_destination = (ActiveMQDestination)SessionUtil.GetDestination(m_messageSession, m_requestQueueName);
                    m_destinationAsynchronize = (ActiveMQDestination)SessionUtil.GetDestination(m_messageSession, m_requestQueueNameASynchronize);
                    m_destinationReply = (ActiveMQDestination)SessionUtil.GetDestination(m_messageSession, O9Client.coreConfig.GetReplyString(m_replyQueueName, O9_GLOBAL_SEPARATE_ADDRESS, m_connection.Queue));
                    m_messageConsumerReply = (MessageConsumer)m_messageSession.CreateConsumer(m_destinationReply);
                    m_messageProducer = (MessageProducer)m_messageSession.CreateProducer();
                    m_message = (ActiveMQTextMessage)m_messageSession.CreateTextMessage();

                    if (sendType == EnmSendTypeOption.Synchronize)
                        m_message.NMSCorrelationID = m_functionRequestID + "-" + Guid.NewGuid().ToString() + O9Encrypt.GenerateRandomString();
                    else if (sendType == EnmSendTypeOption.AsSynchronize)
                        m_message.NMSCorrelationID = m_functionAsynchronizeRequestID + "-" + Guid.NewGuid().ToString();

                    string strSession = "";
                    string strIsCaching = "N";

                    strSession = sessionID.PadRight(m_sizeSessionID);
                    m_message.Content = O9Compression.SetCompressText(strIsCaching + strSession + text);

                    m_message.UserID = userId;
                    m_message.NMSPriority = priority;
                    m_message.NMSType = functionID;
                    m_message.ReplyTo = m_destinationReply;
                    m_messageProducer.DeliveryMode = MsgDeliveryMode.NonPersistent;


                    if (sendType == EnmSendTypeOption.Synchronize)
                        m_messageProducer.Send(m_destination, m_message);
                    else if (sendType == EnmSendTypeOption.AsSynchronize)
                        m_messageProducer.Send(m_destinationAsynchronize, m_message);

                    IMessage receivedMsg = null;
                    bool flag = false;

                    while (flag == false)
                    {
                        receivedMsg = m_messageConsumerReply.Receive(TimeSpan.FromSeconds(m_messageClienTimeOut));
                        if (receivedMsg == null)
                            break;
                        else if (receivedMsg != null && receivedMsg.NMSCorrelationID.Equals(m_message.NMSCorrelationID))
                        {
                            flag = true;
                        }
                    }

                    if (receivedMsg != null)
                    {
                        m_message = (ActiveMQTextMessage)receivedMsg;
                        strReturn = O9Compression.GetTextFromCompressBytes(m_message.Content);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (m_messageProducer != null) m_messageProducer.Close();
                if (m_messageConsumerReply != null) m_messageConsumerReply.Close();
                if (m_destination != null) m_destination.Dispose();
                if (m_destinationAsynchronize != null) m_destinationAsynchronize.Dispose();
                if (m_destinationReply != null) m_destinationReply.Dispose();
                if (m_messageSession != null) m_messageSession.Close();
                if (m_connection != null) activeMQ.ReleaseConnection(m_connection);
            }
            return strReturn;
        }
    }
}

