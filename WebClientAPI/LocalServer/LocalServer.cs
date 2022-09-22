using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebClientAPI.APIRequests;
using Newtonsoft.Json;
using Common.Requests;
using WebClientAPI.APIResponse;
using Common.Extensions;
using System.Reflection.Emit;
using Newtonsoft.Json.Linq;

namespace WebClientAPI.LocalServer
{
    public class LocalServer
    {
        APIManuals manuals = new();
        IPEndPoint EndPoint { get; }
        Socket ListenSocket { get; }
        public LocalServer(int port)
        {
            var (endPoint, socket) = CreateSocket(port);
            EndPoint = endPoint;
            ListenSocket = socket;
        }
        
        public void Listen()
        {
            try
            {
                BindSocket(ListenSocket, EndPoint);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = ListenSocket.Accept();
                    Console.WriteLine("Соединение установлено!");
                    int dataSize = GetDataSize(handler);
                    var data = GetData(handler, dataSize);
                    
                    APIRequest request = Parse(data);
                    dynamic result=manuals[request.Request].InvokeManual(request);
                    byte[] pack=PackResult(result);
                    Console.WriteLine("Отправляю ответ...");
                    handler.Send(pack);
                    Console.WriteLine("Ответ отправлен!");
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        (IPEndPoint endPoint, Socket socket) CreateSocket(int port)
        {
            var ipAdress = IPAddress.Parse("127.0.0.1");
            IPEndPoint EndPoint = new IPEndPoint(ipAdress, port);
            Socket ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return (EndPoint, ListenSocket);
        }
        
        void BindSocket(Socket ListenSocket, IPEndPoint EndPoint)
        {
            // связываем сокет с локальной точкой, по которой будем принимать данные
            ListenSocket.Bind(EndPoint);

            // начинаем прослушивание
            ListenSocket.Listen(1);
        }
        int GetDataSize(Socket handler)
        {
            const byte intSize = 4;
            byte[] byteSize = new byte[intSize];
            
                handler.Receive(byteSize);
            
            return BitConverter.ToInt32(byteSize);
        }
        byte[] GetData(Socket handler, int dataSize)
        {
            byte[] data = new byte[dataSize]; // буфер для получаемых данных

            do
                handler.Receive(data);
            while (handler.Available > 0);
            return data;
        }
        dynamic Parse(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            JObject baseRequest = (JObject)JsonConvert.DeserializeObject(json);
            var type = APIContext.ApiRequestTypes[(string)baseRequest["Request"]];

            dynamic obj=baseRequest.ToObject(type);
            
            return obj;
        }
      
        byte[] PackResult(dynamic result)
        {
            string message = JsonConvert.SerializeObject(result);
            var data = Encoding.UTF8.GetBytes(message);
            byte[] response = new byte[4 + data.Length];
            BitConverter.GetBytes(data.Length).CopyTo(response, 0);
            data.CopyTo(response, 4);
            return response;
        }
    }
}
