namespace Demo.Web.Models
{
    public class AccountRegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string LoginEmail { get; set; }
        public string Street { get; set; }
        public string Hausnumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CreditCard { get; set; }
        public string ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
    }
}
