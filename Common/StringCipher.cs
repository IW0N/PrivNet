using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Cipher<T>:WebCipher
    {
        public T Content { get; }
        public Cipher(T content)
        {
            Content = content;
        }
    }
}
