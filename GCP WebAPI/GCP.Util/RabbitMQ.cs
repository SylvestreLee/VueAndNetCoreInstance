using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.ServiceModel;

namespace GCP.Util
{
    public class RabbitMQ
    {
        /// <summary>
        /// 发送消息到消息队列
        /// </summary>
        /// <param name="queue">队列名称</param>
        /// <param name="msg">消息</param>
        public static void AddMessage(string queue, string msg)
        {
            var factory = new ConnectionFactory();
            factory.HostName = GlobalContext.SystemConfig.MQIP;
            factory.Port = GlobalContext.SystemConfig.MQPort;
            factory.UserName = GlobalContext.SystemConfig.MQUserName;
            factory.Password = GlobalContext.SystemConfig.MQPassword;

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //创建一个消息队列
                    channel.QueueDeclare(queue, true, false, false, null);
                    //传递的消息内容
                    var body = Encoding.UTF8.GetBytes(msg);
                    //开始传递
                    channel.BasicPublish("", queue, null, body);
                }
            }
        }

        /// <summary>
        /// 发送消息到路由
        /// </summary>
        /// <param name="exchange">交换</param>
        /// <param name="routingKey">路由</param>
        /// <param name="body">消息</param>
        public static void AddMessage(string exchange, string routingKey, string body)
        {
            var factory = new ConnectionFactory();
            factory.HostName = GlobalContext.SystemConfig.MQIP;
            factory.Port = GlobalContext.SystemConfig.MQPort;
            factory.UserName = GlobalContext.SystemConfig.MQUserName;
            factory.Password = GlobalContext.SystemConfig.MQPassword;

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;
                    channel.BasicPublish(exchange: exchange,
                             routingKey: routingKey,
                             basicProperties: properties,
                             body: Encoding.UTF8.GetBytes(body));
                }
            }
        }
    }
}
