using Scalable.Shared.Common;
using Scalable.Shared.Properties;

namespace Scalable.Shared.Domain
{
    public class Credentials
    {
        private string _loginName;

        public string LoginName
        {
            get { return _loginName; }
            private set
            {
                if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
                    throw new ValidationException(Resources.LoginNameCannotBeEmpty, "LoginName");

                _loginName = value;
            }
        }

        public string Password { get; private set; }

        public Credentials(string loginName, string password)
        {
            LoginName = loginName;
            Password = password;
        }

        public bool Equals(Credentials credentials)
        {
            if (credentials == null)
                return false;

            var index = string.Compare(LoginName, credentials.LoginName, true);
            if (index != 0) return false;
            if (Password != credentials.Password) return false;

            return true;
        }
    }
}
