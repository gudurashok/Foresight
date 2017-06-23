using System.Text;
using Scalable.Shared.Common;
using Scalable.Shared.Properties;

namespace Scalable.Shared.DataAccess
{
    public class Database
    {
        #region Properties

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ValidationException(Resources.DatabaseNameCannotBeEmpty, "Name");

                _name = value;
            }
        }

        public string Password { get; private set; }

        #endregion

        #region Constructors

        public Database(string name)
            : this(name, "")
        {
        }

        public Database(string name, string password)
        {
            Name = name;
            Password = password;
        }

        #endregion

        #region Public Members

        public string GetSqlCeConnectionString()
        {
            var result = new StringBuilder("Data Source=" + Name);

            if (isPasswordProtected())
                result.Append(";Password=" + Password);

            return result.ToString();
        }

        #endregion

        #region Public Members

        private bool isPasswordProtected()
        {
            return !string.IsNullOrEmpty(Password);
        }

        #endregion
    }
}
