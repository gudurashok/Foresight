using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Import;
using ScalableApps.Foresight.Logic.Properties;
using ScalableApps.Foresight.Logic.Sql;
using ScalableApps.Foresight.Logic.Report;

namespace ScalableApps.Foresight.Logic.DataAccess
{
    public class DataContext : IDisposable
    {
        #region Declarations

        private readonly Database _db;

        #endregion

        #region Constructors

        public DataContext()
            : this(Session.CompanyGroup)
        {
        }

        public DataContext(CompanyGroup companyGroup)
        {
            _db = DatabaseFactory.GetForesightDatabase(companyGroup);
            Session.CompanyGroup = GetCompanyGroupById(GetCompanyGroupIdByName(companyGroup.Name));
        }

        #endregion

        #region Company Group

        public int GetCompanyGroupIdByName(string name)
        {
            var value = _db.ExecuteScalar(SqlQueries.SelectCompanyGroupIdByName, "@name", name);

            if (value == null)
                return 0;

            return (int)value;
        }

        public CompanyGroup GetCompanyGroupById(int id)
        {
            var reader = _db.ExecuteReader(SqlQueries.SelectCompanyGroupById, "@Id", id);
            CompanyGroup group = null;
            if (reader.Read())
                group = readCompanyGroup(reader);

            reader.Close();
            return group;
        }

        private CompanyGroup readCompanyGroup(IDataReader reader)
        {
            var group = CompanyGroup.CreateNewGroup();
            group.Id = Convert.ToInt32(reader["Id"]);
            group.Name = reader["Name"].ToString();

            if (Session.CompanyGroup != null)
                group.FilePath = Session.CompanyGroup.FilePath;

            return group;
        }

        #endregion

        #region Company

        private Company getCompanyById(int id)
        {
            var reader = _db.ExecuteReader(SqlQueries.SelectCompanyById, "@Id", id);

            Company company = null;
            if (reader.Read())
                company = new Company
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Code = reader["Code"].ToString(),
                    Name = reader["Name"].ToString(),
                    Group = GetCompanyGroupById(GetCompanyGroupIdByName(Session.CompanyGroup.Name))
                };
            reader.Close();
            return company;
        }

        public IList<Company> GetCompaniesByGroupId(CompanyGroup group)
        {
            IList<Company> companies = new List<Company>();
            var reader = _db.ExecuteReader(SqlQueries.SelectCompaniesByGroupId, "@GroupId", group.Id);

            while (reader.Read())
            {
                companies.Add(new Company
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Code = reader["Code"].ToString(),
                    Name = reader["Name"].ToString(),
                    Group = group
                });
            }

            reader.Close();
            return companies;
        }

        private void insertCompany(Company company)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertCompany);
            _db.AddParameterWithValue(cmd, "@Code", company.Code);
            _db.AddParameterWithValue(cmd, "@Name", company.Name);
            _db.AddParameterWithValue(cmd, "@GroupId", GetCompanyGroupIdByName(company.Group.Name));
            cmd.ExecuteNonQuery();
            company.Id = _db.GetGeneratedIdentityValue();
        }

        public void DeleteCompany(Company company)
        {
            if (isCompanyInUse(company))
                throw new ValidationException("This company is in use. Cannot delete");

            _db.ExecuteNonQuery(SqlQueries.DeleteCompany, "@Id", company.Id);
        }

        private bool isCompanyInUse(Company company)
        {
            return Convert.ToInt32(_db.ExecuteScalar(SqlQueries.SelectCompanyCountFromCompanyPeriod, "@CompanyId", company.Id)) > 0;
        }

        #endregion

        #region Company Period

        public CompanyPeriod GetCompanyPeriodByCompanyAndPeriodId(int companyId, int periodId)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectCompanyPeriodByCompanyAndPeriodId);
            _db.AddParameterWithValue(cmd, "@CompanyId", companyId);
            _db.AddParameterWithValue(cmd, "@PeriodId", periodId);
            var value = cmd.ExecuteScalar();

            if (value == DBNull.Value)
                return null;

            return GetCompanyPeriodById(Convert.ToInt32(value));
        }

        public void ClearUnfinishedImports(string whereClause)
        {
            var cmd = _db.CreateCommand(SqlQueries.ClearUnfinishedCompanyPeriodImports + whereClause);
            cmd.ExecuteNonQuery();
        }

        public void UpdateIsCompanyPeriodImporting(CompanyPeriod cp, bool value, int processId)
        {
            var cmd = _db.CreateCommand(SqlQueries.UpdateCompanyPeriodIsImporting);
            addCompanyPeriodParams(cmd, cp);
            _db.AddParameterWithValue(cmd, "@isImporting", value);
            _db.AddParameterWithValue(cmd, "@processId", processId);
            cmd.ExecuteNonQuery();
        }

        public void CheckIsCompanyPeriodImported(CompanyPeriod cp)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectCompanyIsImported);
            addCompanyPeriodParams(cmd, cp);
            cp.IsImported = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public bool IsCompanyPeriodImporting(CompanyPeriod cp)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectCompanyIsImporting);
            addCompanyPeriodParams(cmd, cp);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public void SaveCompanyPeriod(CompanyPeriod cp)
        {
            if (isCompanyPeriodExist(cp))
                throw new ValidationException(Resources.PeriodAlreadyExists);

            if (cp.Company.IsNew())
                insertCompany(cp.Company);

            insertCompanyPeriod(cp);
        }

        private void insertCompanyPeriod(CompanyPeriod cp)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertCompanyPeriod);
            addCompanyPeriodParams(cmd, cp);
            _db.AddParameterWithValue(cmd, "@DataPath", cp.DataPath);
            _db.AddParameterWithValue(cmd, "@SourceDataProvider", cp.SourceDataProvider);
            cmd.ExecuteNonQuery();
            cp.Id = _db.GetGeneratedIdentityValue();
        }

        public void SaveCompanyPeriod(CompanyPeriod oldCp, CompanyPeriod newCp)
        {
            if (isCompanyPeriodExist(newCp))
                throw new ValidationException(Resources.PeriodAlreadyExists);

            try
            {
                _db.BeginTransaction();

                if (newCp.Company.IsNew())
                    insertCompany(newCp.Company);

                updateCompanyPeriod(oldCp, newCp);
                setCompanyPeriodColumnValue(oldCp, newCp);
                _db.Commit();
            }
            catch
            {
                _db.Rollback();
                throw;
            }
        }

        private void updateCompanyPeriod(CompanyPeriod oldCp, CompanyPeriod newCp)
        {
            var cmd = _db.CreateCommand(SqlQueries.UpdateCompanyPeriod);
            _db.AddParameterWithValue(cmd, "@NewCompanyId", newCp.Company.Id);
            _db.AddParameterWithValue(cmd, "@NewPeriodId", newCp.Period.Id);
            _db.AddParameterWithValue(cmd, "@CompanyId", oldCp.Company.Id);
            _db.AddParameterWithValue(cmd, "@PeriodId", oldCp.Period.Id);
            cmd.ExecuteNonQuery();
        }

        private bool isCompanyPeriodExist(CompanyPeriod cp)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectCountOfCompanyPeriod);
            addCompanyPeriodParams(cmd, cp);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private void addCompanyPeriodParams(IDbCommand cmd, CompanyPeriod cp)
        {
            _db.AddParameterWithValue(cmd, "@CompanyId", cp.Company.Id);
            _db.AddParameterWithValue(cmd, "@PeriodId", cp.Period.Id);
        }

        private bool isCompanyNameExist(string companyName)
        {
            var value = _db.ExecuteScalar(SqlQueries.SelectCompanyByName, "@Name", companyName);
            return Convert.ToInt16(value) > 0;
        }

        public int GetCompanyPeriodByNameAndFinPeriod(string name, DatePeriod period)
        {
            var cmd = _db.CreateCommand();
            cmd.CommandText = string.Format(SqlQueries.SelectCompanyPeriodIDByNameAndFinPeriod);
            _db.AddParameterWithValue(cmd, "@companyName", name);
            _db.AddParameterWithValue(cmd, "@FinancialFrom", period.FinancialFrom);
            _db.AddParameterWithValue(cmd, "@FinancialTo", period.FinancialTo);
            var value = cmd.ExecuteScalar();
            return (value == DBNull.Value ? 0 : Convert.ToInt32(value));
        }

        public CompanyPeriod GetCompanyPeriodById(int id)
        {
            var reader = _db.ExecuteReader(string.Format(SqlQueries.SelectCompanyPeriodsBy,
                                                                    " WHERE cp.Id = @Id"), "@Id", id);
            CompanyPeriod cp = null;

            if (reader.Read())
                cp = readCompanyPeriod(reader);

            reader.Close();
            return cp;
        }

        public IList<CompanyPeriod> GetCompanyPeriods()
        {
            return GetCompanyPeriods(false);
        }

        public IList<CompanyPeriod> GetCompanyPeriods(bool importedOnly)
        {
            var result = new List<CompanyPeriod>();
            var reader = _db.ExecuteReader(string.Format(SqlQueries.SelectCompanyPeriodsBy,
                                                    getSelectCompanyPeriodByFilter(importedOnly)));

            while (reader.Read())
                result.Add(readCompanyPeriod(reader));

            reader.Close();
            return result;
        }

        private string getSelectCompanyPeriodByFilter(bool importedOnly)
        {
            return importedOnly ? " WHERE cp.IsImported = 1" : "";
        }

        private CompanyPeriod readCompanyPeriod(IDataReader reader)
        {
            var period = new CompanyPeriod();
            period.Company = getCompanyById(Convert.ToInt16(reader["CompanyId"]));
            period.Id = Convert.ToInt16(reader["Id"]);
            period.Period = new DatePeriod();
            period.Period.Id = Convert.ToInt16(reader["periodId"]);
            period.Period.Name = reader["Name"].ToString();
            period.Period.FinancialFrom = Convert.ToDateTime(reader["FinancialFrom"]);
            period.Period.FinancialTo = Convert.ToDateTime(reader["FinancialTo"]);
            period.DataPath = reader["DataPath"].ToString();
            period.SourceDataProvider = (SourceDataProvider)Convert.ToInt16(reader["SourceDataProvider"]);
            period.IsImported = reader["IsImported"] != DBNull.Value && Convert.ToBoolean(reader["IsImported"]);
            return period;
        }

        public void DeleteCompanyPeriod(CompanyPeriod cp)
        {
            try
            {
                var cmd = _db.CreateCommand();
                _db.BeginTransaction();

                if (cp.IsImported)
                {
                    var did = new DeleteImportedData(_db, cmd, cp);
                    did.Delete();
                }

                deleteCompanyPeriod(cmd, cp.Id);

                _db.Commit();
            }
            catch
            {
                if (_db != null)
                    _db.Rollback();

                throw;
            }
        }

        private void deleteCompanyPeriod(IDbCommand cmd, int id)
        {
            cmd.CommandText = SqlQueries.DeleteCompanyPeriod;
            _db.AddParameterWithValue(cmd, "@Id", id);
            cmd.ExecuteNonQuery();
        }

        private void setCompanyPeriodColumnValue(CompanyPeriod oldCp, CompanyPeriod newCp)
        {
            var rdr = getTransTableList();
            var cmd = createChangeCompanyPeriodColumnCommand();

            while (rdr.Read())
            {
                cmd.CommandText = string.Format(SqlQueries.ChangeTransTablesCompanyPeriod, rdr["TableName"]);
                ((IDbDataParameter)cmd.Parameters["@NewCompanyId"]).Value = newCp.Company.Id;
                ((IDbDataParameter)cmd.Parameters["@NewPeriodId"]).Value = newCp.Period.Id;
                ((IDbDataParameter)cmd.Parameters["@CompanyId"]).Value = oldCp.Company.Id;
                ((IDbDataParameter)cmd.Parameters["@PeriodId"]).Value = oldCp.Period.Id;
                cmd.ExecuteNonQuery();
            }

            rdr.Close();
        }

        private IDbCommand createChangeCompanyPeriodColumnCommand()
        {
            var cmd = _db.CreateCommand();
            _db.AddParameterWithValue(cmd, "@NewCompanyId", null);
            _db.AddParameterWithValue(cmd, "@NewPeriodId", null);
            _db.AddParameterWithValue(cmd, "@CompanyId", null);
            _db.AddParameterWithValue(cmd, "@PeriodId", null);
            return cmd;
        }

        private IDataReader getTransTableList()
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectTransTables);
            return cmd.ExecuteReader();
        }

        public void SetCompanyPeriodColumnValue(CompanyPeriod companyPeriod)
        {
            var rdr = getTransTableList();
            var cmd = createUpdateCompanyPeriodColumnCommand();

            while (rdr.Read())
            {
                cmd.CommandText = string.Format(SqlQueries.UpdateTransTablesCompanyPeriod, rdr["TableName"]);
                ((IDbDataParameter)cmd.Parameters["@CompanyPeriodId"]).Value = companyPeriod.Id;
                ((IDbDataParameter)cmd.Parameters["@CompanyId"]).Value = companyPeriod.Company.Id;
                ((IDbDataParameter)cmd.Parameters["@PeriodId"]).Value = companyPeriod.Period.Id;
                cmd.ExecuteNonQuery();
            }

            rdr.Close();
        }

        private IDbCommand createUpdateCompanyPeriodColumnCommand()
        {
            var cmd = _db.CreateCommand();
            _db.AddParameterWithValue(cmd, "@CompanyPeriodId", null);
            _db.AddParameterWithValue(cmd, "@CompanyId", null);
            _db.AddParameterWithValue(cmd, "@PeriodId", null);
            return cmd;
        }

        public void DeleteCompanyPeriodLedgerData(CompanyPeriod companyPeriod)
        {
            var cmd = _db.CreateCommand(string.Format(SqlQueries.DeleteTransTablePeriodData, "AccountLedger"));

            _db.AddParameterWithValue(cmd, "@CompanyId", companyPeriod.Company.Id);
            _db.AddParameterWithValue(cmd, "@PeriodId", companyPeriod.Period.Id);
            cmd.ExecuteNonQuery();

            cmd.CommandText = string.Format(SqlQueries.DeleteTransTablePeriodData, "ItemLedger");
            cmd.ExecuteNonQuery();
        }

        public void DeleteCompanyPeriodData(CompanyPeriod companyPeriod)
        {
            if (!companyPeriod.IsImported)
                return;

            var cmd = _db.CreateCommand();
            var did = new DeleteImportedData(_db, cmd, companyPeriod);
            did.Delete();
        }

        public void SetCompanyPeriodIsImported(CompanyPeriod companyPeriod, bool isImported)
        {
            var cmd = _db.CreateCommand();
            cmd.CommandText = SqlQueries.UpdateCompanyPeriodSetIsImportedFlagValue;
            _db.AddParameterWithValue(cmd, "@companyId", companyPeriod.Company.Id);
            _db.AddParameterWithValue(cmd, "@periodId", companyPeriod.Period.Id);
            _db.AddParameterWithValue(cmd, "@isImported", isImported);
            cmd.ExecuteNonQuery();
        }

        #endregion

        #region DatePeriod

        public void AddDatePeriod(DatePeriod period)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertDatePeriod);
            _db.AddParameterWithValue(cmd, "@PeriodName", period.GetNameFromFinancialPeriod());
            _db.AddParameterWithValue(cmd, "@FinFrom", period.FinancialFrom);
            _db.AddParameterWithValue(cmd, "@FinTo", period.FinancialTo);
            period.SetAssessmentPeriodFromFinancialPeriod();
            _db.AddParameterWithValue(cmd, "@AssesmentFrom", period.AssesmentFrom);
            _db.AddParameterWithValue(cmd, "@AssesmentTo", period.AssesmentTo);
            cmd.ExecuteNonQuery();
            period.Id = _db.GetGeneratedIdentityValue();
        }

        public DatePeriod GetDatePeriodById(int id)
        {
            return readDatePeriod(_db.ExecuteReader(SqlQueries.SelectDatePeriodById, "@Id", id));
        }

        public DatePeriod GetDatePeriodByFinPeriod(DatePeriod period)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectDatePeriodByFinPeriod);
            _db.AddParameterWithValue(cmd, "@FinFrom", period.FinancialFrom);
            _db.AddParameterWithValue(cmd, "@FinTo", period.FinancialTo);
            return readDatePeriod(cmd.ExecuteReader());
        }

        private DatePeriod readDatePeriod(IDataReader reader)
        {
            DatePeriod result = null;
            if (reader.Read())
                result = fillDatePeriod(reader);

            reader.Close();
            return result;
        }

        public IList<DatePeriod> GetDatePeriods()
        {
            var periods = new List<DatePeriod>();
            var reader = _db.ExecuteReader(SqlQueries.SelectDatePeriods);
            while (reader.Read())
            {
                var dp = fillDatePeriod(reader);
                periods.Add(dp);
            }
            reader.Close();
            return periods;
        }

        private DatePeriod fillDatePeriod(IDataReader reader)
        {
            var dp = new DatePeriod();
            dp.Id = Convert.ToInt16(reader["Id"]);
            dp.Name = reader["Name"].ToString();
            dp.FinancialFrom = Convert.ToDateTime(reader["FinancialFrom"]);
            dp.FinancialTo = Convert.ToDateTime(reader["FinancialTo"]);
            return dp;
        }

        #endregion

        #region Account Opening Balance

        public void SaveAccountOpeningBalance(AccountOpeningBalance aob)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertAccountOpeningBalance);
            setAccountOpeningBalanceParams(cmd, aob);
            cmd.ExecuteNonQuery();
        }

        private void setAccountOpeningBalanceParams(IDbCommand cmd, AccountOpeningBalance aob)
        {
            _db.AddParameterWithValue(cmd, "@accountId", aob.Account.Id);
            _db.AddParameterWithValue(cmd, "@date", aob.Date);
            _db.AddParameterWithValue(cmd, "@amount", aob.Amount);
        }

        #endregion

        #region Sale Invoice

        public void SaveSaleInvoice(SaleInvoice invoice)
        {
            var id = saveSaleInvoiceHeader(invoice.Header);
            invoice.SetIdentityValue(id);
            if (invoice.HeaderEx != null)
                saveSaleInvoiceHeaderEx(invoice.HeaderEx);

            foreach (var line in invoice.Lines)
                saveSaleInvoiceLine(line);

            foreach (var term in invoice.Terms)
                saveSaleInvoiceTerm(term);
        }

        private int saveSaleInvoiceHeader(SaleInvoiceHeader header)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertSaleInvoiceHeader);
            setSaleInvoiceHeaderParams(cmd, header);
            cmd.ExecuteNonQuery();
            return _db.GetGeneratedIdentityValue();
        }

        private void setSaleInvoiceHeaderParams(IDbCommand cmd, SaleInvoiceHeader header)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", header.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@documentNr", header.DocumentNr);
            _db.AddParameterWithValue(cmd, "@date", header.Date);
            _db.AddParameterWithValue(cmd, "@accountId", header.Account.Id);
            _db.AddParameterWithValue(cmd, "@brokerId", header.BrokerId);
            _db.AddParameterWithValue(cmd, "@brokerageAmount", header.BrokerageAmount);
            _db.AddParameterWithValue(cmd, "@through", Util.ConvertToDbValue(header.Through));
            _db.AddParameterWithValue(cmd, "@vehicleId", header.VehicleId);
            _db.AddParameterWithValue(cmd, "@transport", Util.ConvertToDbValue(header.Transport));
            _db.AddParameterWithValue(cmd, "@referenceDocNr", Util.ConvertToDbValue(header.ReferenceDocNr));
            _db.AddParameterWithValue(cmd, "@orderId", header.OrderId);
            _db.AddParameterWithValue(cmd, "@discountPct", header.DiscountPct);
            _db.AddParameterWithValue(cmd, "@amount", header.Amount);
            _db.AddParameterWithValue(cmd, "@isAdjusted", header.IsAdjusted);
            _db.AddParameterWithValue(cmd, "@saleTaxColumnId", header.SaleTaxColumnId);
            _db.AddParameterWithValue(cmd, "@notes", header.Notes);
        }

        private void saveSaleInvoiceHeaderEx(SaleInvoiceHeaderEx headerEx)
        {
            if (headerEx == null)
                return;

            var cmd = _db.CreateCommand(SqlQueries.InsertSaleInvoiceHeaderEx);
            setSaleInvoiceExParams(cmd, headerEx);
            cmd.ExecuteNonQuery();
        }

        private void setSaleInvoiceExParams(IDbCommand cmd, SaleInvoiceHeaderEx headerEx)
        {
            _db.AddParameterWithValue(cmd, "@invoiceId", headerEx.InvoiceId);
            _db.AddParameterWithValue(cmd, "@shipToName", Util.ConvertToDbValue(headerEx.ShipToName));
            _db.AddParameterWithValue(cmd, "@shipToAddressLine1", Util.ConvertToDbValue(headerEx.ShipToAddressLine1));
            _db.AddParameterWithValue(cmd, "@shipToAddressLine2", Util.ConvertToDbValue(headerEx.ShipToAddressLine2));
            _db.AddParameterWithValue(cmd, "@shipToCity", Util.ConvertToDbValue(headerEx.ShipToCity));
        }

        private void saveSaleInvoiceLine(SaleInvoiceLine line)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertSaleInvoiceLine);
            setSaleInvoiceLineParams(cmd, line);
            cmd.ExecuteNonQuery();
        }

        private void setSaleInvoiceLineParams(IDbCommand cmd, SaleInvoiceLine line)
        {
            _db.AddParameterWithValue(cmd, "@InvoiceId", line.InvoiceId);
            _db.AddParameterWithValue(cmd, "@LineNr", line.LineNr);
            _db.AddParameterWithValue(cmd, "@ItemId", line.Item.Id);
            _db.AddParameterWithValue(cmd, "@ItemDescription", Util.ConvertToDbValue(line.ItemDescription));
            _db.AddParameterWithValue(cmd, "@Quantity1", line.Quantity1);
            _db.AddParameterWithValue(cmd, "@Packing", line.Packing);
            _db.AddParameterWithValue(cmd, "@Quantity2", line.Quantity2);
            _db.AddParameterWithValue(cmd, "@Quantity3", line.Quantity3);
            _db.AddParameterWithValue(cmd, "@Price", line.Price);
            _db.AddParameterWithValue(cmd, "@DiscountPct", line.DiscountPct);
            _db.AddParameterWithValue(cmd, "@Amount", line.Amount);
        }

        private void saveSaleInvoiceTerm(SaleInvoiceTerm term)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertSaleInvoiceTerm);
            setSaleInvoiceTermParams(cmd, term);
            cmd.ExecuteNonQuery();
        }

        private void setSaleInvoiceTermParams(IDbCommand cmd, SaleInvoiceTerm term)
        {
            _db.AddParameterWithValue(cmd, "@InvoiceId", term.InvoiceId);
            _db.AddParameterWithValue(cmd, "@TermId", term.TermId);
            _db.AddParameterWithValue(cmd, "@Description", term.Description);
            _db.AddParameterWithValue(cmd, "@Percentage", term.Percentage);
            _db.AddParameterWithValue(cmd, "@Amount", term.Amount);
            _db.AddParameterWithValue(cmd, "@AccountId", term.Account.Id);
        }

        #endregion

        #region Purchase Invoice

        public void SavePurchaseInvoice(PurchaseInvoice invoice)
        {
            var id = savePurchaseHeader(invoice.Header);
            invoice.SetIdentityValue(id);

            foreach (var line in invoice.Lines)
                savePurchaseInvoiceLine(line);

            foreach (var term in invoice.Terms)
                savePurchaseInvoiceTerm(term);
        }

        private int savePurchaseHeader(PurchaseInvoiceHeader header)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertPurchaseInvoiceHeader);
            setPurchaseInvoiceHeaderParams(cmd, header);
            cmd.ExecuteNonQuery();
            return _db.GetGeneratedIdentityValue();
        }

        private void setPurchaseInvoiceHeaderParams(IDbCommand cmd, PurchaseInvoiceHeader header)
        {
            cmd.Parameters.Clear();
            _db.AddParameterWithValue(cmd, "@daybookId", header.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@documentNr", header.DocumentNr);
            _db.AddParameterWithValue(cmd, "@date", header.Date);
            _db.AddParameterWithValue(cmd, "@accountId", header.Account.Id);
            _db.AddParameterWithValue(cmd, "@brokerId", header.BrokerId);
            _db.AddParameterWithValue(cmd, "@brokerageAmount", header.BrokerageAmount);
            _db.AddParameterWithValue(cmd, "@through", Util.ConvertToDbValue(header.Through));
            _db.AddParameterWithValue(cmd, "@transport", Util.ConvertToDbValue(header.Transport));
            _db.AddParameterWithValue(cmd, "@referenceDocNr", header.ReferenceDocNr);
            _db.AddParameterWithValue(cmd, "@orderId", header.OrderId);
            _db.AddParameterWithValue(cmd, "@discountPct", header.DiscountPct);
            _db.AddParameterWithValue(cmd, "@amount", header.Amount);
            _db.AddParameterWithValue(cmd, "@isAdjusted", header.IsAdjusted);
            _db.AddParameterWithValue(cmd, "@saleTaxColumnId", header.SaleTaxColumnId);
            _db.AddParameterWithValue(cmd, "@notes", header.Notes);
        }

        private void savePurchaseInvoiceLine(PurchaseInvoiceLine line)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertPurchaseInvoiceLine);
            setPurchaseInvoiceLineParams(cmd, line);
            cmd.ExecuteNonQuery();
        }

        private void setPurchaseInvoiceLineParams(IDbCommand cmd, PurchaseInvoiceLine line)
        {
            _db.AddParameterWithValue(cmd, "@InvoiceId", line.InvoiceId);
            _db.AddParameterWithValue(cmd, "@LineNr", line.LineNr);
            _db.AddParameterWithValue(cmd, "@ItemId", line.Item.Id);
            _db.AddParameterWithValue(cmd, "@ItemDescription", Util.ConvertToDbValue(line.ItemDescription));
            _db.AddParameterWithValue(cmd, "@Quantity1", line.Quantity1);
            _db.AddParameterWithValue(cmd, "@Packing", line.Packing);
            _db.AddParameterWithValue(cmd, "@Quantity2", line.Quantity2);
            _db.AddParameterWithValue(cmd, "@Quantity3", line.Quantity3);
            _db.AddParameterWithValue(cmd, "@Cost", line.Cost);
            _db.AddParameterWithValue(cmd, "@DiscountPct", line.DiscountPct);
            _db.AddParameterWithValue(cmd, "@Amount", line.Amount);
        }

        private void savePurchaseInvoiceTerm(PurchaseInvoiceTerm term)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertPurchaseInvoiceTerm);
            setPurchaseInvoiceTermParams(cmd, term);
            cmd.ExecuteNonQuery();
        }

        private void setPurchaseInvoiceTermParams(IDbCommand cmd, PurchaseInvoiceTerm term)
        {
            _db.AddParameterWithValue(cmd, "@InvoiceId", term.InvoiceId);
            _db.AddParameterWithValue(cmd, "@TermId", term.TermId);
            _db.AddParameterWithValue(cmd, "@Description", term.Description);
            _db.AddParameterWithValue(cmd, "@Percentage", term.Percentage);
            _db.AddParameterWithValue(cmd, "@Amount", term.Amount);
            _db.AddParameterWithValue(cmd, "@AccountId", term.Account.Id);
        }

        #endregion

        #region Cash

        public void SaveCashTransaction(CashTransaction cash)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertCashTransaction);
            setCashTransactionParams(cmd, cash);
            cmd.ExecuteNonQuery();
        }

        private void setCashTransactionParams(IDbCommand cmd, CashTransaction cash)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", cash.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@documentNr", cash.DocumentNr);
            _db.AddParameterWithValue(cmd, "@date", cash.Date);
            _db.AddParameterWithValue(cmd, "@accountId", cash.Account.Id);
            _db.AddParameterWithValue(cmd, "@brokerId", cash.BrokerId);
            _db.AddParameterWithValue(cmd, "@amount", cash.Amount);
            _db.AddParameterWithValue(cmd, "@txnType", cash.TxnType);
            _db.AddParameterWithValue(cmd, "@isAdjusted", cash.IsAdjusted);
            _db.AddParameterWithValue(cmd, "@notes", cash.Notes);
        }

        #endregion

        #region Bank

        public void SaveBankPayment(BankPayment payment)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertBankPayment);
            setBankPaymentParams(cmd, payment);
            cmd.ExecuteNonQuery();
        }

        private void setBankPaymentParams(IDbCommand cmd, BankPayment payment)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", payment.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@documentNr", payment.DocumentNr);
            _db.AddParameterWithValue(cmd, "@date", payment.Date);
            _db.AddParameterWithValue(cmd, "@accountId", payment.Account.Id);
            _db.AddParameterWithValue(cmd, "@brokerId", payment.BrokerId);
            _db.AddParameterWithValue(cmd, "@chequeNr", Util.ConvertToDbValue(payment.ChequeNr));
            _db.AddParameterWithValue(cmd, "@amount", payment.Amount);
            _db.AddParameterWithValue(cmd, "@isAdjusted", payment.IsAdjusted);
            _db.AddParameterWithValue(cmd, "@notes", Util.ConvertToDbValue(payment.Notes));
            _db.AddParameterWithValue(cmd, "@isRealised", payment.IsRealised);
        }

        public void SaveBankReceipt(BankReceipt receipt)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertBankReceipt);
            setBankReceiptParams(cmd, receipt);
            cmd.ExecuteNonQuery();
        }

        private void setBankReceiptParams(IDbCommand cmd, BankReceipt receipt)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", receipt.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@documentNr", receipt.DocumentNr);
            _db.AddParameterWithValue(cmd, "@date", receipt.Date);
            _db.AddParameterWithValue(cmd, "@accountId", receipt.Account.Id);
            _db.AddParameterWithValue(cmd, "@brokerId", receipt.BrokerId);
            _db.AddParameterWithValue(cmd, "@chequeNr", Util.ConvertToDbValue(receipt.ChequeNr));
            _db.AddParameterWithValue(cmd, "@amount", receipt.Amount);
            _db.AddParameterWithValue(cmd, "@bankBranchName", Util.ConvertToDbValue(receipt.BankBranchName));
            _db.AddParameterWithValue(cmd, "@isAdjusted", receipt.IsAdjusted);
            _db.AddParameterWithValue(cmd, "@notes", Util.ConvertToDbValue(receipt.Notes));
            _db.AddParameterWithValue(cmd, "@isRealised", receipt.IsRealised);
        }

        #endregion

        #region Credit Note

        public void SaveCreditNote(CreditNote note)
        {
            var id = saveCreditNoteHeader(note.Header);
            note.SetIdentityValue(id);

            foreach (var line in note.Lines)
                saveCreditNoteLine(line);
        }

        private int saveCreditNoteHeader(CreditNoteHeader header)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertCreditNoteHeader);
            setTransParams(cmd, header);
            cmd.ExecuteNonQuery();
            return _db.GetGeneratedIdentityValue();
        }

        private void saveCreditNoteLine(CreditNoteLine line)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertCreditNoteLine);
            setCreditNoteLineParams(cmd, line);
            cmd.ExecuteNonQuery();
        }

        private void setCreditNoteLineParams(IDbCommand cmd, CreditNoteLine line)
        {
            _db.AddParameterWithValue(cmd, "@NoteId", line.NoteId);
            _db.AddParameterWithValue(cmd, "@LineNr", line.LineNr);
            _db.AddParameterWithValue(cmd, "@ItemId", line.Item.Id);
            _db.AddParameterWithValue(cmd, "@ItemDescription", Util.ConvertToDbValue(line.ItemDescription));
            _db.AddParameterWithValue(cmd, "@Quantity1", line.Quantity1);
            _db.AddParameterWithValue(cmd, "@Packing", line.Packing);
            _db.AddParameterWithValue(cmd, "@Quantity2", line.Quantity2);
            _db.AddParameterWithValue(cmd, "@Quantity3", line.Quantity3);
            _db.AddParameterWithValue(cmd, "@Cost", line.Cost);
            _db.AddParameterWithValue(cmd, "@DiscountPct", line.DiscountPct);
            _db.AddParameterWithValue(cmd, "@Amount", line.Amount);
        }

        #endregion

        #region Debit Note

        public void SaveDebitNote(DebitNote note)
        {
            var id = saveDebitNoteHeader(note.Header);
            note.SetIdentityValue(id);

            foreach (var line in note.Lines)
                saveDebitNoteLine(line);
        }

        private int saveDebitNoteHeader(DebitNoteHeader header)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertDebitNoteHeader);
            setTransParams(cmd, header);
            cmd.ExecuteNonQuery();
            return _db.GetGeneratedIdentityValue();
        }

        private void saveDebitNoteLine(DebitNoteLine line)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertDebitNoteLine);
            setDebitNoteLineParams(cmd, line);
            cmd.ExecuteNonQuery();
        }

        private void setDebitNoteLineParams(IDbCommand cmd, DebitNoteLine line)
        {
            _db.AddParameterWithValue(cmd, "@NoteId", line.NoteId);
            _db.AddParameterWithValue(cmd, "@LineNr", line.LineNr);
            _db.AddParameterWithValue(cmd, "@ItemId", line.Item.Id);
            _db.AddParameterWithValue(cmd, "@ItemDescription", Util.ConvertToDbValue(line.ItemDescription));
            _db.AddParameterWithValue(cmd, "@Quantity1", line.Quantity1);
            _db.AddParameterWithValue(cmd, "@Packing", line.Packing);
            _db.AddParameterWithValue(cmd, "@Quantity2", line.Quantity2);
            _db.AddParameterWithValue(cmd, "@Quantity3", line.Quantity3);
            _db.AddParameterWithValue(cmd, "@Price", line.Price);
            _db.AddParameterWithValue(cmd, "@DiscountPct", line.DiscountPct);
            _db.AddParameterWithValue(cmd, "@Amount", line.Amount);
        }

        #endregion

        #region Transaction Common

        private void setTransParams(IDbCommand cmd, TransactionHeader t)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", t.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@documentNr", t.DocumentNr);
            _db.AddParameterWithValue(cmd, "@date", t.Date);
            _db.AddParameterWithValue(cmd, "@accountId", t.Account.Id);
            _db.AddParameterWithValue(cmd, "@brokerId", t.BrokerId);
            _db.AddParameterWithValue(cmd, "@amount", t.Amount);
            _db.AddParameterWithValue(cmd, "@isAdjusted", t.IsAdjusted);
            _db.AddParameterWithValue(cmd, "@notes", t.Notes);
        }

        #endregion

        #region Journal Voucher

        public void SaveJournalVoucher(JournalVoucher jv)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertJournalVoucher);
            setJournalVoucherParams(cmd, jv);
            cmd.ExecuteNonQuery();
        }

        private void setJournalVoucherParams(IDbCommand cmd, JournalVoucher jv)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", jv.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@documentNr", jv.DocumentNr);
            _db.AddParameterWithValue(cmd, "@accountId", jv.Account.Id);
            _db.AddParameterWithValue(cmd, "@date", jv.Date);
            _db.AddParameterWithValue(cmd, "@amount", jv.Amount);
            _db.AddParameterWithValue(cmd, "@txnType", jv.TxnType);
            _db.AddParameterWithValue(cmd, "@isAdjusted", jv.IsAdjusted);
            _db.AddParameterWithValue(cmd, "@notes", Util.ConvertToDbValue(jv.Notes));
        }

        #endregion

        #region Item Lot

        public ItemLot GetItemLotByLotNrLineNr(string lotNr, int lineNr)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectLotByLotNrAndLineNr);
            _db.AddParameterWithValue(cmd, "@lotNr", lotNr);
            _db.AddParameterWithValue(cmd, "@lineNr", lineNr);
            var rdr = cmd.ExecuteReader();
            var result = readItemLotInfo(rdr);
            rdr.Close();
            return result;
        }

        private ItemLot readItemLotInfo(IDataReader rdr)
        {
            if (!rdr.Read())
                return null;

            var lot = new ItemLot();
            lot.Id = Convert.ToInt32(rdr["Id"]);
            lot.LotNr = rdr["LotNr"].ToString();
            lot.Date = Convert.ToDateTime(rdr["Date"]);
            lot.Account = GetAccountById(Convert.ToInt32(rdr["AccountId"]));
            lot.LineNr = Convert.ToInt32(rdr["LineNr"]);
            lot.Item = GetItemById(Convert.ToInt32(rdr["ItemId"]));
            lot.Quantity1 = Convert.ToDouble(rdr["Quantity1"]);
            lot.Packing = Convert.ToDouble(rdr["Packing"]);
            lot.Quantity2 = Convert.ToDouble(rdr["Quantity2"]);
            lot.Quantity3 = Convert.ToDouble(rdr["Quantity3"]);
            lot.IsClosed = rdr["IsClosed"] != DBNull.Value && Convert.ToBoolean(rdr["IsClosed"]);
            return lot;
        }

        public void SaveItemLot(ItemLot lot)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertItemLot);
            setItemLotParams(cmd, lot);
            cmd.ExecuteNonQuery();
        }

        private void setItemLotParams(IDbCommand cmd, ItemLot lot)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", 0);
            _db.AddParameterWithValue(cmd, "@LotNr", lot.LotNr);
            _db.AddParameterWithValue(cmd, "@Date", lot.Date);
            _db.AddParameterWithValue(cmd, "@AccountId", lot.Account.Id);
            _db.AddParameterWithValue(cmd, "@LineNr", lot.LineNr);
            _db.AddParameterWithValue(cmd, "@ItemId", lot.Item.Id);
            _db.AddParameterWithValue(cmd, "@Quantity1", lot.Quantity1);
            _db.AddParameterWithValue(cmd, "@Packing", lot.Packing);
            _db.AddParameterWithValue(cmd, "@Quantity2", lot.Quantity2);
            _db.AddParameterWithValue(cmd, "@Quantity3", lot.Quantity3);
        }

        #endregion

        #region Inventory Issue

        public InventoryIssue GetInventoryIssueByDocNr(string documentNr)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectInventoryIssueByDocNr);
            _db.AddParameterWithValue(cmd, "@documentNr", documentNr);
            var rdr = cmd.ExecuteReader();
            var result = readInventoryIssue(rdr);
            rdr.Close();
            return result;
        }

        private InventoryIssue readInventoryIssue(IDataReader rdr)
        {
            if (!rdr.Read())
                return null;

            var issue = new InventoryIssue();
            issue.Id = Convert.ToInt32(rdr["Id"]);
            issue.DocumentNr = rdr["DocumentNr"].ToString();
            issue.Date = Convert.ToDateTime(rdr["Date"]);
            issue.LotId = Convert.ToInt32(rdr["LotId"]);
            issue.Account = GetAccountById(Convert.ToInt32(rdr["AccountId"]));
            issue.Quantity1 = Convert.ToDouble(rdr["Quantity1"]);
            issue.Quantity2 = Convert.ToDouble(rdr["Quantity2"]);
            issue.Quantity3 = Convert.ToDouble(rdr["Quantity3"]);
            return issue;
        }

        public void SaveInventoryIssue(InventoryIssue issue)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertInventoryIssue);
            setInventoryIssueParams(cmd, issue);
            cmd.ExecuteNonQuery();
        }

        private void setInventoryIssueParams(IDbCommand cmd, InventoryIssue issue)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", issue.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@DocumentNr", issue.DocumentNr);
            _db.AddParameterWithValue(cmd, "@Date", issue.Date);
            _db.AddParameterWithValue(cmd, "@LotId", issue.LotId);
            _db.AddParameterWithValue(cmd, "@AccountId", issue.Account.Id);
            _db.AddParameterWithValue(cmd, "@Quantity1", issue.Quantity1);
            _db.AddParameterWithValue(cmd, "@Quantity2", issue.Quantity2);
            _db.AddParameterWithValue(cmd, "@Quantity3", issue.Quantity3);
        }

        #endregion

        #region Inventory Receive

        public void SaveInventoryReceive(InventoryReceive receive)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertInventoryReceive);
            setInventoryReceiveParams(cmd, receive);
            cmd.ExecuteNonQuery();
        }

        private void setInventoryReceiveParams(IDbCommand cmd, InventoryReceive receive)
        {
            cmd.Parameters.Clear();
            _db.AddParameterWithValue(cmd, "@daybookId", 0);
            _db.AddParameterWithValue(cmd, "@DocumentNr", receive.DocumentNr);
            _db.AddParameterWithValue(cmd, "@Date", receive.Date);
            _db.AddParameterWithValue(cmd, "@IssueId", receive.Issue.Id);
            _db.AddParameterWithValue(cmd, "@LineNr", receive.LineNr);
            _db.AddParameterWithValue(cmd, "@ItemId", receive.Item.Id);
            _db.AddParameterWithValue(cmd, "@Quantity1", receive.Quantity1);
            _db.AddParameterWithValue(cmd, "@Packing", receive.Packing);
            _db.AddParameterWithValue(cmd, "@Quantity2", receive.Quantity2);
            _db.AddParameterWithValue(cmd, "@Quantity3", receive.Quantity3);
        }

        #endregion

        #region Misc. Material Issue

        public void SaveMiscMaterialIssue(MiscMaterialIssue issue)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertMiscInventoryIssue);
            setMiscMaterialIssueParams(cmd, issue);
            cmd.ExecuteNonQuery();
        }

        private void setMiscMaterialIssueParams(IDbCommand cmd, MiscMaterialIssue issue)
        {
            _db.AddParameterWithValue(cmd, "@daybookId", issue.Daybook.Id);
            _db.AddParameterWithValue(cmd, "@DocumentNr", issue.DocumentNr);
            _db.AddParameterWithValue(cmd, "@Date", issue.Date);
            _db.AddParameterWithValue(cmd, "@LineNr", issue.LineNr);
            _db.AddParameterWithValue(cmd, "@ItemId", issue.Item.Id);
            _db.AddParameterWithValue(cmd, "@Quantity1", issue.Quantity1);
            _db.AddParameterWithValue(cmd, "@Quantity2", issue.Quantity2);
            _db.AddParameterWithValue(cmd, "@Quantity3", issue.Quantity3);
        }

        #endregion

        #region Account Ledger

        public IDataReader ReadLedgerData(AccountTransTables tran, int cpId)
        {
            if (tran.TransName == "Opening")
                return _db.ExecuteReader(SqlQueries.SelectMonthlyOpeningBalanceByAccount, "@companyPeriodId", cpId);

            return _db.ExecuteReader(string.Format(SqlQueries.SelectMonthlySumOfTransByAccount,
                              tran.TableName, getTransDimensionFilter(tran)), "@companyPeriodId", cpId);
        }

        private string getTransDimensionFilter(AccountTransTables trans)
        {
            return string.IsNullOrEmpty(trans.Filter) ? "" : " AND " + trans.Filter;
        }

        public void InsertAccountLedger(AccountMonthlyLedger account)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertAccountLedger);
            _db.AddParameterWithValue(cmd, "@accountId", account.AccountId);
            _db.AddParameterWithValue(cmd, "@chartOfAccountId", account.ChartOfAccountId);
            _db.AddParameterWithValue(cmd, "@month", account.Month);
            _db.AddParameterWithValue(cmd, "@opening", account.Opening);
            _db.AddParameterWithValue(cmd, "@sale", account.Sale);
            _db.AddParameterWithValue(cmd, "@cashPayment", account.CashPayment);
            _db.AddParameterWithValue(cmd, "@bankPayment", account.BankPayment);
            _db.AddParameterWithValue(cmd, "@debitNote", account.DebitNote);
            _db.AddParameterWithValue(cmd, "@debitJV", account.DebitJV);
            _db.AddParameterWithValue(cmd, "@purchase", account.Purchase);
            _db.AddParameterWithValue(cmd, "@cashReceipt", account.CashReceipt);
            _db.AddParameterWithValue(cmd, "@bankReceipt", account.BankReceipt);
            _db.AddParameterWithValue(cmd, "@creditNote", account.CreditNote);
            _db.AddParameterWithValue(cmd, "@creditJV", account.CreditJV);
            _db.AddParameterWithValue(cmd, "@periodId", account.CompanyPeriod.Period.Id);
            _db.AddParameterWithValue(cmd, "@companyId", account.CompanyPeriod.Company.Id);
            _db.AddParameterWithValue(cmd, "@companyPeriodId", account.CompanyPeriod.Id);
            cmd.ExecuteNonQuery();
            account.Id = _db.GetGeneratedIdentityValue();
        }

        #endregion

        #region Account

        public decimal GetAccountOpeningBalance(int id)
        {
            var value = _db.ExecuteScalar(string.Format(
                                            ReportQueries.SelectAccountOpeningBalance, ""),
                                            "@accountId", id);
            return value == DBNull.Value ? 0 : Convert.ToDecimal(value);
        }

        public decimal GetAccountOpeningBalance(int accountId, int periodId)
        {
            var cmd = _db.CreateCommand(string.Format(
                                            ReportQueries.SelectAccountOpeningBalance,
                                            "AND PeriodId = @periodId"));

            _db.AddParameterWithValue(cmd, "@accountId", accountId);
            _db.AddParameterWithValue(cmd, "@periodId", periodId);
            var value = cmd.ExecuteScalar();
            return value == DBNull.Value ? 0 : Convert.ToDecimal(value);
        }

        public IList<KeyValuePair<int, decimal>> GetAccountBalances(bool partyGrouping)
        {
            var result = new List<KeyValuePair<int, decimal>>();
            var rdr = _db.ExecuteReader(string.Format(SqlQueries.SelectAllAccountBalances, getIdentityColumn(partyGrouping)));
            while (rdr.Read())
            {
                result.Add(new KeyValuePair<int, decimal>(
                                Convert.ToInt32(rdr["AccountId"]),
                                Convert.ToDecimal(rdr["Amount"]))
                          );
            }

            rdr.Close();
            return result;
        }


        private string getIdentityColumn(bool partyGrouping)
        {
            return partyGrouping ? "GroupId" : "Id";
        }

        public Account GetAccountById(int id)
        {
            return GetAccountById(id, false);
        }

        public Account GetAccountById(int id, bool populateBalance)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectAccountById);
            _db.AddParameterWithValue(cmd, "@id", id);
            var act = readAccountHelper(cmd.ExecuteReader());

            if (populateBalance)
                act.BalanceAmount = GetAccountOpeningBalance(id);

            return act;
        }

        public Account GetAccountByNameAndAddress(Account account)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectAccountByNameAndAddress);
            _db.AddParameterWithValue(cmd, "@name", Util.ConvertToDbValue(account.Name));
            _db.AddParameterWithValue(cmd, "@addressLine1", Util.ConvertToDbValue(account.AddressLine1));
            _db.AddParameterWithValue(cmd, "@addressLine2", Util.ConvertToDbValue(account.AddressLine2));
            _db.AddParameterWithValue(cmd, "@city", Util.ConvertToDbValue(account.City));
            _db.AddParameterWithValue(cmd, "@state", Util.ConvertToDbValue(account.State));
            _db.AddParameterWithValue(cmd, "@pin", Util.ConvertToDbValue(account.Pin));
            return readAccountHelper(cmd.ExecuteReader());
        }

        private Account readAccountHelper(IDataReader rdr)
        {
            var result = readAccount(rdr);
            rdr.Close();
            return result;
        }

        private Account readAccount(IDataReader rdr)
        {
            if (!rdr.Read())
                return null;

            var account = new Account();

            if (hasAccountGroup(rdr))
                account.Group = GetAccountById(Convert.ToInt32(rdr["GroupId"]));

            account.ChartOfAccount = GetChartOfAccountById(Convert.ToInt32(rdr["ChartOfAccountId"]));
            fillAccount(rdr, account);
            rdr.Close();
            return account;
        }

        private void fillAccount(IDataReader rdr, Account account)
        {
            account.Id = Convert.ToInt32(rdr["Id"]);
            account.Name = rdr["name"].ToString();
            account.AddressLine1 = rdr["addressLine1"].ToString();
            account.AddressLine2 = rdr["addressLine2"].ToString();
            account.City = rdr["city"].ToString();
            account.State = rdr["state"].ToString();
            account.Pin = rdr["pin"].ToString();
            account.IsActive = Convert.ToBoolean(rdr["IsActive"]);
        }

        private bool hasAccountGroup(IDataReader rdr)
        {
            return Convert.ToInt32(rdr["GroupId"]) != Convert.ToInt32(rdr["Id"]);
        }

        public void SaveAccount(Account account)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertAccount);
            setAccountParams(cmd, account);
            cmd.ExecuteNonQuery();
            account.Id = _db.GetGeneratedIdentityValue();
            if (account.Group == null)
                updateAccountGroupId(account.Id);
        }

        private void setAccountParams(IDbCommand cmd, Account account)
        {
            _db.AddParameterWithValue(cmd, "@groupId", getAccountGroupId(account));
            _db.AddParameterWithValue(cmd, "@chartOfAccountId", account.ChartOfAccount.Id);
            _db.AddParameterWithValue(cmd, "@name", Util.ConvertToDbValue(account.Name));
            _db.AddParameterWithValue(cmd, "@addressLine1", Util.ConvertToDbValue(account.AddressLine1));
            _db.AddParameterWithValue(cmd, "@addressLine2", Util.ConvertToDbValue(account.AddressLine2));
            _db.AddParameterWithValue(cmd, "@city", Util.ConvertToDbValue(account.City));
            _db.AddParameterWithValue(cmd, "@state", Util.ConvertToDbValue(account.State));
            _db.AddParameterWithValue(cmd, "@pin", Util.ConvertToDbValue(account.Pin));
            _db.AddParameterWithValue(cmd, "@contactId", account.ContactId);
            _db.AddParameterWithValue(cmd, "@isActive", account.IsActive);
        }

        private int? getAccountGroupId(Account account)
        {
            if (account.Group == null)
                return null;

            return account.Group.Id;
        }

        private void updateAccountGroupId(int id)
        {
            _db.ExecuteNonQuery(SqlQueries.UpdateAccountGroupId, "@Id", id);
        }

        public IList<Account> GetAccounts(AccountTypes accountTypes, bool partyGrouping)
        {
            var result = new List<Account>();
            var reader = _db.ExecuteReader(getSelectAccountsQuery(accountTypes, partyGrouping));

            while (reader.Read())
                result.Add(readAccountInfo(reader));

            reader.Close();
            return result;
        }

        private string getSelectAccountsQuery(AccountTypes accountTypes, bool partyGrouping)
        {
            var sb = new StringBuilder();
            var coaId = ChartOfAccount.GetChartOfAccountIds(accountTypes);

            if (!string.IsNullOrEmpty(coaId))
                sb.Append(string.Format(" ca.Nr {0} ", coaId));

            if (partyGrouping)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(" AND ");

                sb.Append(" a.Id = a.GroupId ");
            }

            if (!string.IsNullOrEmpty(sb.ToString()))
                sb.Append(" AND ");

            sb.Append(" a.Id NOT IN (SELECT AccountId FROM Daybook) ");

            var result = "";
            if (sb.Length > 0)
                result = " WHERE " + sb;

            return string.Format(SqlQueries.SelectAllAccounts, result);
        }

        private Account readAccountInfo(IDataReader rdr)
        {
            var account = new Account();
            fillAccount(rdr, account);
            return account;
        }

        #endregion

        #region Chart of Account

        public IList<ChartOfAccount> GetChartOfAccounts()
        {
            var result = new List<ChartOfAccount>();
            var rdr = _db.ExecuteReader(SqlQueries.SelectAllChartOfAccounts);
            while (rdr.Read())
                result.Add(readChartOfAccount(rdr));

            rdr.Close();
            return result;
        }

        public ChartOfAccount GetChartOfAccountById(int id)
        {
            var cmd = _db.CreateCommand();
            cmd.CommandText = SqlQueries.SelectChartOfAccountById;
            cmd.Parameters.Clear();
            _db.AddParameterWithValue(cmd, "@Id", id);
            return readChartOfAccountHelper(cmd.ExecuteReader());
        }

        public ChartOfAccount GetChartOfAccountByNr(string nr)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectChartOfAccountByNr);
            _db.AddParameterWithValue(cmd, "@nr", nr);
            return readChartOfAccountHelper(cmd.ExecuteReader());
        }

        public List<TrialBalance> GetTrialBalances()
        {
            var result = new List<TrialBalance>();
            var rdr = _db.ExecuteReader(
                            string.Format(ReportQueries.SelectAllTrailBalances,
                                AccountTransTables.GetCreditAmountExpr(),
                                AccountTransTables.GetDebitAmountExpr()));
            while (rdr.Read())
                result.Add(readTrialBalanceData(rdr));

            rdr.Close();
            return result;
        }

        private TrialBalance readTrialBalanceData(IDataReader rdr)
        {
            return new TrialBalance()
            {
                AccountId = Convert.ToInt32(Util.ConvertDbNull(rdr["AccountId"])),
                ChartOfAccountName = rdr["ChartOfAccountName"].ToString(),
                AccountName = rdr["AccountName"].ToString(),
                Opening = Convert.ToDecimal(Util.ConvertDbNull(rdr["Opening"])),
                TransactionCredit = Convert.ToDecimal(Util.ConvertDbNull(rdr["CrAmt"])),
                TransactionDebit = Convert.ToDecimal(Util.ConvertDbNull(rdr["DbAmt"])),
                Closing = Convert.ToDecimal(Util.ConvertDbNull(rdr["Closing"]))
            };
        }

        private ChartOfAccount readChartOfAccountHelper(IDataReader rdr)
        {
            if (!rdr.Read())
                return null;

            var result = readChartOfAccount(rdr);
            rdr.Close();
            return result;
        }

        private ChartOfAccount readChartOfAccount(IDataReader rdr)
        {
            var coa = new ChartOfAccount();
            coa.Id = Convert.ToInt32(rdr["Id"]);
            coa.Nr = Convert.ToInt32(rdr["Nr"]);

            var parentId = Convert.ToInt32(rdr["ParentId"]);
            if (parentId != 0)
                coa.Parent = getChartOfAccountParent(parentId);

            coa.Name = rdr["Name"].ToString();
            //coa.Sorting = Convert.ToInt32(rdr["Sorting"]); //TODO: Analysis sorting type from different providers and assign
            return coa;
        }

        private ChartOfAccount getChartOfAccountParent(int parentId)
        {
            var rdr = _db.ExecuteReader(SqlQueries.SelectChartOfAccountByParentId, "@parentId", parentId);
            return readChartOfAccountHelper(rdr);
        }

        public void SaveChartOfAccount(ChartOfAccount coa)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertChartOfAccount);
            setChartOfAccountParams(cmd, coa);
            cmd.ExecuteNonQuery();
            coa.Id = _db.GetGeneratedIdentityValue();
        }

        private void setChartOfAccountParams(IDbCommand cmd, ChartOfAccount coa)
        {
            _db.AddParameterWithValue(cmd, "@nr", coa.Nr);
            _db.AddParameterWithValue(cmd, "@parentId", getChartOfAccountParentId(coa));
            _db.AddParameterWithValue(cmd, "@name", coa.Name);
        }

        private int getChartOfAccountParentId(ChartOfAccount coa)
        {
            return coa.Parent == null ? 0 : coa.Parent.Id;
        }

        public IList<int> GetChartOfAccountIDsFor(int parentId)
        {
            var result = new List<int>();
            result.Add(parentId);
            var rdr = _db.ExecuteReader(SqlQueries.SelectChartOfAccountIdsByParentId, "@parentId", parentId);
            while (rdr.Read())
            {
                var id = Convert.ToInt32(rdr["Id"]);
                result.Add(id);
                result.AddRange(GetChartOfAccountIDsFor(id));
            }

            rdr.Close();
            return result.Distinct().ToList();
        }

        #endregion

        #region Daybook

        public IList<Daybook> GetDaybooks()
        {
            var daybooks = new List<Daybook>();
            var rdr = _db.ExecuteReader(SqlQueries.SelectAllDaybooks);
            while (rdr.Read())
                daybooks.Add(readDaybook(rdr));

            rdr.Close();
            return daybooks;
        }

        public Daybook GetDaybookByCode(string code)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectDaybookByCode);
            _db.AddParameterWithValue(cmd, "@code", code);
            return readDaybookHelper(cmd.ExecuteReader());
        }

        private Daybook readDaybookHelper(IDataReader rdr)
        {
            Daybook daybook = null;
            if (rdr.Read())
                daybook = readDaybook(rdr);

            rdr.Close();
            return daybook;
        }

        private Daybook readDaybook(IDataReader rdr)
        {
            var book = new Daybook();
            book.Id = Convert.ToInt32(rdr["Id"]);
            book.Type = (DaybookType)Convert.ToInt32(rdr["Type"]);
            book.Code = rdr["Code"].ToString();
            book.Name = rdr["Name"].ToString();

            if (book.Type != DaybookType.JournalVoucher)
                book.Account = GetAccountById(Convert.ToInt32(rdr["AccountId"]));

            return book;
        }

        public void SaveDaybook(Daybook book)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertDaybook);
            setDaybookParams(cmd, book);
            cmd.ExecuteNonQuery();
            book.Id = _db.GetGeneratedIdentityValue();
        }

        private void setDaybookParams(IDbCommand cmd, Daybook book)
        {
            _db.AddParameterWithValue(cmd, "@type", (int)book.Type);
            _db.AddParameterWithValue(cmd, "@code", book.Code);
            _db.AddParameterWithValue(cmd, "@name", book.Name);
            _db.AddParameterWithValue(cmd, "@accountId", getDaybookAccountId(book));
        }

        private int getDaybookAccountId(Daybook book)
        {
            return book.Type == DaybookType.JournalVoucher ? 0 : book.Account.Id;
        }

        public int GetDaybookIdOfAccount(int accountId)
        {
            var value = _db.ExecuteScalar(SqlQueries.SelectDaybookIdOfAccount, "@accountId", accountId);
            return value != null ? Convert.ToInt32(value) : 0;
        }

        public Daybook GetDaybookOfAccount(int accountId)
        {
            var reader = _db.ExecuteReader(SqlQueries.SelectDaybookOfAccount, "@accountId", accountId);
            return readDaybookHelper(reader);
        }

        #endregion

        #region Item

        public Item GetItemById(int id)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectItemById);
            _db.AddParameterWithValue(cmd, "@id", id);
            return readItemHelper(cmd.ExecuteReader());
        }

        public Item GetItemByName(string name)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectItemByName);
            _db.AddParameterWithValue(cmd, "@name", name);
            return readItemHelper(cmd.ExecuteReader());
        }

        private Item readItemHelper(IDataReader rdr)
        {
            var result = readItem(rdr);
            rdr.Close();
            return result;
        }

        private Item readItem(IDataReader rdr)
        {
            if (!rdr.Read())
                return null;

            var item = new Item();
            item.Id = Convert.ToInt32(rdr["Id"]);
            item.Group = GetItemGroupById(Convert.ToInt32(rdr["GroupId"]));
            item.Name = rdr["Name"].ToString();
            item.ShortName = rdr["ShortName"].ToString();
            item.Category = GetItemCategoryById(Convert.ToInt32(rdr["CategoryId"]));
            item.HasBom = Convert.ToBoolean(rdr["HasBom"]);
            item.IsActive = Convert.ToBoolean(rdr["IsActive"]);
            return item;
        }

        public void SaveItem(Item item)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertItem);
            setItemParams(cmd, item);
            cmd.ExecuteNonQuery();
            item.Id = _db.GetGeneratedIdentityValue();
        }

        private void setItemParams(IDbCommand cmd, Item item)
        {
            _db.AddParameterWithValue(cmd, "@groupId", item.Group.Id);
            _db.AddParameterWithValue(cmd, "@name", item.Name);
            _db.AddParameterWithValue(cmd, "@shortName", Util.ConvertToDbValue(item.ShortName));
            _db.AddParameterWithValue(cmd, "@categoryId", item.Category.Id);
            _db.AddParameterWithValue(cmd, "@hasBom", item.HasBom);
            _db.AddParameterWithValue(cmd, "@isActive", item.IsActive);
        }

        #endregion

        #region Item Category

        public ItemCategory GetItemCategoryById(int id)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectItemCategoryById);
            _db.AddParameterWithValue(cmd, "@id", id);
            return readItemCategoryHelper(cmd.ExecuteReader());
        }

        public ItemCategory GetItemCategoryByName(string name)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectItemCategoryByName);
            _db.AddParameterWithValue(cmd, "@name", name);
            return readItemCategoryHelper(cmd.ExecuteReader());
        }

        private ItemCategory readItemCategoryHelper(IDataReader rdr)
        {
            var result = readItemCategory(rdr);
            rdr.Close();
            return result;
        }

        private ItemCategory readItemCategory(IDataReader rdr)
        {
            if (!rdr.Read())
                return null;

            var ic = new ItemCategory();
            ic.Id = Convert.ToInt32(rdr["Id"]);
            ic.Name = rdr["Name"].ToString();
            return ic;
        }

        public void SaveItemCategory(ItemCategory ic)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertItemCategory);
            _db.AddParameterWithValue(cmd, "@name", ic.Name);
            cmd.ExecuteNonQuery();
            ic.Id = _db.GetGeneratedIdentityValue();
        }

        #endregion

        #region Item Group

        public ItemGroup GetItemGroupById(int id)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectItemGroupById);
            _db.AddParameterWithValue(cmd, "@id", id);
            return readItemGroupHelper(cmd.ExecuteReader());
        }

        public ItemGroup GetItemGroupByName(string name)
        {
            var cmd = _db.CreateCommand(SqlQueries.SelectItemGroupByName);
            _db.AddParameterWithValue(cmd, "@name", name);
            return readItemGroupHelper(cmd.ExecuteReader());
        }

        private ItemGroup readItemGroupHelper(IDataReader rdr)
        {
            var result = readItemGroup(rdr);
            rdr.Close();
            return result;
        }

        private ItemGroup readItemGroup(IDataReader rdr)
        {
            if (!rdr.Read())
                return null;

            var ig = new ItemGroup();
            ig.Id = Convert.ToInt32(rdr["Id"]);
            ig.Name = rdr["Name"].ToString();
            ig.ParentId = Convert.ToInt32(rdr["ParentId"]);
            return ig;
        }

        public void SaveItemGroup(ItemGroup ig)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertItemGroup);
            setItemGroupParams(cmd, ig);
            cmd.ExecuteNonQuery();
            ig.Id = _db.GetGeneratedIdentityValue();
        }

        private void setItemGroupParams(IDbCommand cmd, ItemGroup ig)
        {
            _db.AddParameterWithValue(cmd, "@parentId", ig.ParentId);
            _db.AddParameterWithValue(cmd, "@name", ig.Name);
        }

        #endregion

        #region Item Ledger

        public IDataReader ReadItemLedger(ItemTransTables tran, int cpId)
        {
            return _db.ExecuteReader(string.Format(
                SqlQueries.SelectMonthlySumOfTransByItemAndAccount,
                tran.HeaderTableName, tran.DetailTableName), "@companyPeriodId", cpId);
        }

        public void InsertItemLedger(ItemMonthlyLedger item)
        {
            var cmd = _db.CreateCommand(SqlQueries.InsertItemLedger);
            _db.AddParameterWithValue(cmd, "@itemId", item.ItemId);
            _db.AddParameterWithValue(cmd, "@accountId", item.AccountId);
            _db.AddParameterWithValue(cmd, "@chartOfAccountId", item.ChartOfAccountId);
            _db.AddParameterWithValue(cmd, "@month", item.Month);
            _db.AddParameterWithValue(cmd, "@sale", item.Sale);
            _db.AddParameterWithValue(cmd, "@purchase", item.Purchase);
            _db.AddParameterWithValue(cmd, "@periodId", item.CompanyPeriod.Period.Id);
            _db.AddParameterWithValue(cmd, "@companyId", item.CompanyPeriod.Company.Id);
            _db.AddParameterWithValue(cmd, "@companyPeriodId", item.CompanyPeriod.Id);
            cmd.ExecuteNonQuery();
            item.Id = _db.GetGeneratedIdentityValue();
        }

        #endregion

        #region Database Context

        public void BeginTransaction()
        {
            _db.BeginTransaction();
        }

        public void Commit()
        {
            _db.Commit();
        }

        public void Rollback()
        {
            _db.Rollback();
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            _db.Close();
        }

        #endregion
    }
}
