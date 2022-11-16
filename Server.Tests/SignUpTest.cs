using Common.Requests.Post;

namespace Server.Tests
{
    public class SignUpTest
    {
        [Fact]
        public void Test()
        {

            SignUpRequest req =new(new Common.Services.TokenGenerator()) { Username="Nick"};
            
        }
    }
}
