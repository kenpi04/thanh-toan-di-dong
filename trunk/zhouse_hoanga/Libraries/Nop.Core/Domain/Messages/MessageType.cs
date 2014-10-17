using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Messages
{
    public enum MessageType
    {
        EmailAFriend = 5,
        WrongNews = 10,
        Contact = 15,
        Book=20,
        Support=30,
        Error=40,
        Feedback=50
    }
}
