using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class HackException : Exception
    {
        public HackException(string? message) : base(message)
        {

        }
    }

}
