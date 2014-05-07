using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
     public enum OrderStatusEnum
    {
       PENDING=0,
        WAITTING=10,
       COMPLETE=20,
       CANCEL=30
    }
     public enum ProviderEnum
     { 
        PAYOO=10,
        MSERVICE=20
     }
     public enum OrderType { 
        CARD=10,
        PAYMENT=20
     }
     public enum PaymentStatus { 
        CHUATHANHTOAN=0,
        DATHANHTOAN=10,
         HUY=20
     }
}
