using System.ComponentModel.Composition;
using WebMatrix.WebData;
using Demo.Web.Contracts;

namespace Demo.Web.Client.Adapters
{
    [Export(typeof(ISecurityAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class SecurityAdapter : ISecurityAdapter
    {
        public void Initialize()
        {
            if (WebSecurity.Initialized) return;
            WebSecurity.InitializeDatabaseConnection("DemoDbConnection", "Customer", "CustomerId", "LoginEmail", autoCreateTables: true);
        }

        public void Register(string loginEmail, string password, object propertyValues)
        {
            WebSecurity.CreateUserAndAccount(loginEmail, password, propertyValues);
        }
        
        public bool Login(string loginEmail, string password, bool rememberMe)
        {
            return WebSecurity.Login(loginEmail, password, persistCookie: rememberMe);
        }

        public bool ChangePassword(string loginEmail, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(loginEmail, oldPassword, newPassword);
        }

        public bool UserExists(string loginEmail)
        {
            return WebSecurity.UserExists(loginEmail);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }
    }
}
