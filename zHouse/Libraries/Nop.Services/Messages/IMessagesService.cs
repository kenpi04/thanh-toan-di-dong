using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Messages;

namespace Nop.Services.Messages
{
    public partial interface IMessagesService
    {
        void DeleteMessage(Message message);
        void InsertMessage(Message message);
        void UpdateMessage(Message message);
        List<Message> GetMessageByCustomerId(int CustomerId);
        List<Message> GetMessage(DateTime? FromDate, DateTime? ToDate,int Type = 0, string Key = "");
        IPagedList<Message> GetMessage(int page, int pagesize, DateTime? FromDate, DateTime? ToDate, int Type = 0, string Key = "", int CustomerId = 0);
        Message GetMessageById(int Id);
    }
}
