using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.LocalDb.Entities.UserEnvironment.Plugins
{
    class PluginCollection:List<object>
    {
        public T? Find<T>() where T:class
        {
            foreach (var plugin in this)
            {
                if (plugin is T val)
                    return val;
            }
            return null;
        }
        public void UpdatePlugin<T>(Func<T> pluginBuilder) where T:class
        {
            T plugin = Find<T>();
            if (plugin != null)
                Remove(plugin);
            var newPlugin = pluginBuilder.Invoke();
            Add(newPlugin);
        }
    }
}
