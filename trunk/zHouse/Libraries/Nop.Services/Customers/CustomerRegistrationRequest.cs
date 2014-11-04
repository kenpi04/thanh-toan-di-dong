using Nop.Core.Domain.Customers;

namespace Nop.Services.Customers
{
    public class CustomerRegistrationRequest
    {
        public Customer Customer { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public bool IsApproved { get; set; }
        public string PhoneNumber { get; set; }

        public CustomerRegistrationRequest(Customer customer, string email, string username,
            string password, 
            PasswordFormat passwordFormat,
            bool isApproved = true, string phoneNumber="")
        {
            this.Customer = customer;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
            this.PhoneNumber = phoneNumber;
        }

        //public bool IsValid  
        //{
        //    get 
        //    {
        //        return (!CommonHelper.AreNullOrEmpty(
        //                    this.Email,
        //                    this.Password
        //                    ));
        //    }
        //}
    }
}
