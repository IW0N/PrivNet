using System.Net;
using System.Net.Sockets;
using System.Text;
using WebClientAPI.LocalServer;

LocalServer server = new(42364);
server.Listen();
