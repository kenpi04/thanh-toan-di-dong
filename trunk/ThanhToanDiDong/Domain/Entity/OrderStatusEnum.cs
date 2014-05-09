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
        PROCESSING=10,
       COMPLETE=20,
       CANCEL=30
    }
     public enum ProviderEnum
     { 
        PAYOO=10,
        MSERVICE=20
     }
     public enum OrderType { 
        TOPUP=10,
         CARD=20,
        BILLING=30,

     }
     public enum PaymentStatus { 
        CHUATHANHTOAN=0,
        DATHANHTOAN=10,
         HUY=20
     }
}
