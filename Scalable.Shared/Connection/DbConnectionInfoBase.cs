using System.IO;
using Scalable.Shared.Common;

namespace Scalable.Shared.Connection
{
    public abstract class DbConnectionInfoBase
    {
        #region Declarations

        protected readonly string _path;

        #endregion

        #region Public Members

        protected DbConnectionInfoBase(string path)
        {
            _path = path;
        }

        #endregion

        #region Internal Members

        protected void checkPathExits()
        {
            if (!Directory.Exists(_path))
                throw new ValidationException(string.Format("Data folder {0} doesn't exist.", _path));
        }

        #endregion
    }
}
