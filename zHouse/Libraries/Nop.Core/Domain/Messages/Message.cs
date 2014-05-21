using System;

namespace Nop.Core.Domain.Messages
{
    public partial class Message : BaseEntity
    {
        /// <summary>
        /// Get or set the Name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Get or set the Phone
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// Get or set the email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Get or set the title
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Get or set the body
        /// </summary>
        public string Body { get; set; }
        
        
        /// <summary>
        /// Get or set the type
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// Get or set the productid
        /// </summary>
        public int ProductId { get; set; }
        
        /// <summary>
        /// Get or set the Store Id
        /// </summary>
        public int StoreId { get; set; }
        
        /// <summary>
        /// Get or set the CustomerId
        /// </summary>
        public int CustomerId { get; set; }
        
        /// <summary>
        /// Get or set the Create on
        /// </summary>
        public DateTime CreatedOn { get; set; }
        
        /// <summary>
        /// Get or set the deleted
        /// </summary>
        public bool Deleted { get; set; }

    }
}
