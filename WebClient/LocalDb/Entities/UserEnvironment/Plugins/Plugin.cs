using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.LocalDb.Entities.UserEnvironment.Plugins
{
    abstract class Plugin
    {
        protected static readonly HttpClient client = new() { Timeout=new TimeSpan(0,20,0)};
    }
}
