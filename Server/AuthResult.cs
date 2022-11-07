using Common;
using Common.Requests;
using Common.Requests.Base;

namespace Server
{
    public class AuthResult<T> where T:WebCipher
    {
        public bool Authenticated { get; init; }
        public T Request { get; init; }
        public string AliasId { get; init; }
        public static implicit operator AuthResult<T>(bool input)
        {
            if (!input)
                return new()
                {
                    Authenticated = false,
                    Request = null,
                    AliasId = null
                };
            else
                throw new ArgumentException("It can converts from 'false' value only!");
            
        }
        public static implicit operator bool(AuthResult<T> input) => input.Authenticated;
    }
}
