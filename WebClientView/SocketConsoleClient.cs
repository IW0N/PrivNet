using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebClientAPI.APIRequests;
using Common.Requests;
using Newtonsoft.Json;
using WebClientAPI.APIResponse;

namespace WebClientView
{
    internal class SocketClient
    {
        static Dictionary<string, Type> TypeOutputTable = new()
        {
            {"/signUp",typeof(SignUpOutput) }
        };
        public int Port { get; }
        const string address = "127.0.0.1";
        public SocketClient(int port)
        {
            Port = port;

        }
        byte[] BuildRequest<T>(T data) where T:APIRequest
        {
            string dataJson = JsonConvert.SerializeObject(data);
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataJson);
            byte[] request = new byte[4+dataBytes.Length];
            byte[] dataSize = BitConverter.GetBytes(dataBytes.Length);
            dataSize.CopyTo(request, 0);
            dataBytes.CopyTo(request, 4);
            return request;
        }
        public Tout WriteAPIRequest<T,Tout>(T data) 
            where T:APIRequest 
            where Tout:APIOutput
        {
            string request=data.Request;
            byte[] byteRequest = BuildRequest(data);
            var response=Write(byteRequest,request);
            return (Tout)response;
        }
        Socket ConnectToServer()
        {
            IPEndPoint ipPoint = new(IPAddress.Parse(address), Port);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);
            return socket;
        }
        APIOutput Write(byte[] data,string request)
        {
            APIOutput output=null;
            try
            {
                Socket socket = ConnectToServer();
                socket.Send(data);
                // получаем ответ
                dynamic apiResponse=ReadOutput(request,socket);
                output = apiResponse;
                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return output;
            
        }
        dynamic ReadOutput(string request,Socket socket)
        {
            var type = TypeOutputTable[request];
            byte[] buffer = ReadOutputBytes(socket);
            string json =Encoding.UTF8.GetString(buffer);
            dynamic obj = JsonConvert.DeserializeObject(json,type);
            return obj;
        }
        int GetDataSize(Socket socket)
        {
            var data = new byte[4]; // буфер для ответа
          
            socket.Receive(data, data.Length, 0);
            return BitConverter.ToInt32(data);
        }
        byte[] ReadOutputBytes(Socket socket)
        {
            int bufferSize = GetDataSize(socket);
            var data = new byte[bufferSize]; // буфер для ответа
            do
                socket.Receive(data, data.Length, 0);
            while (socket.Available > 0);
            return data;

        }
    }
}
