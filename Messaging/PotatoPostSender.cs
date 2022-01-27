
using System.Text;
using Microsoft.Extensions.Options;
using PotatoCounter.Interfaces;
using PotatoCounter.Models;
using RabbitMQ.Client;

namespace PotatoCounter.Messaging
{
    public class PotatoPostSender : IPotatoPostSender
    {
        private string _hostname;
        private string _username;
        private string _password;
        private IConnection _connection;
        private string _queueName;

        public PotatoPostSender(IRabbitMqConfiguration rabbitMqOptions)
        {
            _queueName = rabbitMqOptions.QueueName;
            _hostname = rabbitMqOptions.Hostname;
            _username = rabbitMqOptions.UserName;
            _password = rabbitMqOptions.Password;
            CreateConnection();
        }
        private void CreateConnection(){
            try{
                var factory = new ConnectionFactory{
                    HostName = _hostname
                };
                _connection = factory.CreateConnection();
            }catch(Exception ex){
                Console.WriteLine($"Cannot connect to rabbtimq {ex.Message}");
            }
        }
        public void SendPotatoPost(Potato potato)
        {
            if(_connection.IsOpen){
                using(var channel = _connection.CreateModel()){
                    channel.QueueDeclare(queue:_queueName,
                    durable:true,
                    exclusive: false, 
                    autoDelete: false,
                    arguments:null);

                    string message = potato.ToString();

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                    routingKey: _queueName,
                    basicProperties: null,
                    body:body);
                    Console.WriteLine("Sent {0}", message);
                }
            }else{
                return;
            }
        }
    }
}