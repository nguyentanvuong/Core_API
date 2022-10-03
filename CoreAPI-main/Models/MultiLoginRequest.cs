using System.Collections.Generic;

namespace WebApi.Models
{
    public class MultiLoginRequest
    {
        [CoreRequired]
        public List<LoginRequest> User { get; set; }

    }
}
