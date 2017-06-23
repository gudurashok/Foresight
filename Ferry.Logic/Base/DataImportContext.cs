using System;
using System.Text;
using System.IO;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.DataAccess;
using ImportAbortException = Ferry.Logic.Common.ImportAbortException;

namespace Ferry.Logic.Base
{
    public abstract class DataImportContext
    {
        #region Declarations

        private Database _soureDatabase;
        private DataContext _targetDbContext;
        private const string importDataPath = @"\ImportData";
        protected readonly CompanyPeriod companyPeriod;
        public DataImporter Importer { get; private set; }

        #endregion

        #region Constructor

        protected DataImportContext(CompanyPeriod companyPeriod)
        {
            this.companyPeriod = companyPeriod;
            initializeImport();
        }

        #endregion

        private void initializeImport()
        {
            ValidateSourceData();
            var sourceDataPath = MakeCopyOfSourceData();
            _soureDatabase = GetSourceDataContext(sourceDataPath);
            _targetDbContext = createTargetDatabaseContext();
            Importer = CreateImporterInstance(_soureDatabase, _targetDbContext, companyPeriod);
        }

        #region Public Methods

        public void PerformImport()
        {
            Exception importException = null;
            Exception finallyException = null;

            try
            {
                _targetDbContext.DeleteCompanyPeriodData(companyPeriod);
                importData();
            }
            catch (ImportAbortException)
            {
                _targetDbContext.Rollback();
                throw;
            }
            catch (Exception ex)
            {
                importException = ex;
            }
            finally
            {
                try
                {
                    if (_targetDbContext != null)
                        _targetDbContext.Close();

                    if (_soureDatabase != null)
                        _soureDatabase.Close();
                }
                catch (Exception fe)
                {
                    finallyException = fe;
                }

                if (importException != null || finallyException != null)
                    throw getPreparedException(finallyException, importException);
            }
        }

        private void importData()
        {
            Importer.Execute();
            if (Importer.IsCancelled)
                throw new ImportAbortException();

            companyPeriod.IsImported = true;
            _targetDbContext.Commit();
        }

        #endregion

        #region Internal Methods

        private Exception getPreparedException(Exception finallyException, Exception importException)
        {
            var fullMessage = new StringBuilder();
            var message = new StringBuilder();

            if (importException != null)
            {
                message.Append(importException.Message);
                fullMessage.Append(string.Format("Exception during Import: {0} \n", importException));
            }

            if (finallyException != null)
            {
                message.Append("\n\n").Append(finallyException.Message);
                fullMessage.Append(string.Format("Exception in finally: {0}", finallyException));
            }

            return new Exception(message.ToString(), new Exception(fullMessage.ToString()));
        }

        private DataImporter CreateImporterInstance(Database sourceDb,
                                DataContext targetDbContext, CompanyPeriod cp)
        {
            return new DataImporter(sourceDb, targetDbContext, cp);
        }

        private DataContext createTargetDatabaseContext()
        {
            var dbc = new DataContext(companyPeriod.Company.Group);
            dbc.BeginTransaction();
            return dbc;
        }

        protected abstract Database GetSourceDataContext(string sourceDataPath);
        protected abstract void ValidateSourceData();
        protected abstract string MakeCopyOfSourceData();

        protected string MakeCopyOfSourceDataInternal(string searchPattern)
        {
            var sourceFolder = new DirectoryInfo(companyPeriod.DataPath);
            var destinationFolder = Util.GetAppPath() + importDataPath + @"\" + sourceFolder.Name;
            deleteDestFolder(destinationFolder);
            Directory.CreateDirectory(destinationFolder);

            CopyAllFiles(searchPattern, sourceFolder, destinationFolder);
            return destinationFolder;
        }

        private void deleteDestFolder(string destFolder)
        {
            if (!Directory.Exists(destFolder))
                return;

            var targetFolder = new DirectoryInfo(destFolder);
            targetFolder.Delete(true);
        }

        protected virtual void CopyAllFiles(string searchPattern, DirectoryInfo sourceFolder, string destFolder)
        {
            var files = sourceFolder.GetFiles(searchPattern);
            foreach (var file in files)
            {
                var destFileName = destFolder + @"\" + file.Name;
                file.CopyImportSourceFile(destFileName);
            }
        }

        #endregion
    }
}
