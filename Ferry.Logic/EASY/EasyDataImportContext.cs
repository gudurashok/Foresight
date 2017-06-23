using System.Collections.Generic;
using System.IO;
using Ferry.Logic.Base;
using Ferry.Logic.Connection;
using Ferry.Logic.Properties;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;

namespace Ferry.Logic.EASY
{
    internal class EasyDataImportContext : DataImportContext
    {
        private const string easyFileExtension = "ASK";
        private const string easyFileSearchPattern = "*." + easyFileExtension;

        internal EasyDataImportContext(CompanyPeriod companyPeriod)
            : base(companyPeriod)
        {
        }

        protected override void ValidateSourceData()
        {
            if (Directory.GetFiles(companyPeriod.DataPath, "TXN_FILE.ASK").Length == 0)
                throw new ValidationException(string.Format(Resources.InvalidSourceDataPath, companyPeriod.DataPath));
        }

        protected override string MakeCopyOfSourceData()
        {
            var destinationFolder = new DirectoryInfo(MakeCopyOfSourceDataInternal(easyFileSearchPattern));
            RenameFileExtToDbf(destinationFolder, destinationFolder.GetFiles(easyFileSearchPattern));
            return destinationFolder.FullName;
        }

        private void RenameFileExtToDbf(DirectoryInfo destinationFolder, IEnumerable<FileInfo> files)
        {
            foreach (var file in files)
            {
                var destFileName = destinationFolder + @"\" + file.Name.Replace("." + easyFileExtension, ".dbf");
                file.MoveTo(destFileName); // Rename
            }
        }

        protected override Database GetSourceDataContext(string sourceDataPath)
        {
            var connInfo = SourceDbConnInfoFactory.GetConnectionInfo(sourceDataPath);
            return DatabaseFactory.GetSourceDatabase(connInfo);
        }
    }
}
