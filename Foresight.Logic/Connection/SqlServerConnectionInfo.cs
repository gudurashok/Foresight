using System.Text;

namespace ScalableApps.Foresight.Logic.Connection
{
    public class SqlServerConnectionInfo : IDbConnectionInfo
    {
        #region Internal Declarations

        private readonly string _serverName;
        private readonly string _userId;
        private readonly string _password;
        private readonly string _initialCatalog;
        private readonly bool _useIntegratedSecurity;
        private readonly bool _isFullConnectionStringSpecified;

        #endregion

        #region Constructors

        public SqlServerConnectionInfo(string connectionString)
        {
            _serverName = connectionString;
            _isFullConnectionStringSpecified = true;
        }

        public SqlServerConnectionInfo(string serverName, string initialCatalog)
            : this(serverName, initialCatalog, true, null, null)
        {

        }

        public SqlServerConnectionInfo(string serverName, string initialCatalog,
                                        string userId, string password)
            : this(serverName, initialCatalog, false, userId, password)
        {

        }

        private SqlServerConnectionInfo(string serverName, string initialCatalog,
                                        bool useIntegratedSecurity,
                                        string userId, string password)
        {
            _serverName = serverName;
            _initialCatalog = initialCatalog;
            _useIntegratedSecurity = useIntegratedSecurity;
            _userId = userId;
            _password = password;
        }

        #endregion

        #region Public Members

        public string GetConnectionString()
        {
            if (_isFullConnectionStringSpecified)
                return _serverName;

            var result = new StringBuilder();
            result.Append("; Data Source=" + _serverName);

            if (!string.IsNullOrEmpty(_initialCatalog))
                result.Append("; Initial Catalog=" + _initialCatalog);

            if (_useIntegratedSecurity)
            {
                result.Append("; Integrated Security=True");
            }
            else
            {
                result.Append("; User ID=" + _userId);
                result.Append("; Password=" + _password);
            }

            result.Append("; MultipleActiveResultSets=True");

            return result.ToString();
        }

        #endregion
    }
}
