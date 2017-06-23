using System.IO;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.Common
{
    public static class IOExtensions
    {
        public static void CopyImportSourceFile(this FileInfo fileInfo, string destFileName)
        {
            try
            {
                fileInfo.CopyTo(destFileName, true);
            }
            catch (IOException ex)
            {
                throw getImportSourceFileCopyException(fileInfo, ex);
            }
        }

        private static ValidationException getImportSourceFileCopyException(FileInfo fi,
                                                IOException innerException)
        {
            return new ValidationException(
                            string.Format(Resources.CouldNotImportDataSourceIsInUse,
                                            fi.DirectoryName, innerException.Message));
        }
    }
}
