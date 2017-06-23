using System.IO;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.DataAccess;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.Common
{
    internal abstract class RulesBase
    {
        protected void Check(IValidator rule)
        {
            var result = rule.validate();
            if (!result.IsValid())
                throw new ValidationException(result);
        }
    }

    internal class PasswordRules : RulesBase
    {
        private readonly string _foresightPassword;

        public PasswordRules(string foresightPassword)
        {
            _foresightPassword = foresightPassword;
        }

        public void Check(string password)
        {
            Check(new MatchPasswordRule(_foresightPassword, password));
        }

        private class MatchPasswordRule : IValidator
        {
            private readonly string _foresightPassword;
            private readonly string _userPassword;

            public MatchPasswordRule(string foresightPassword, string userPassword)
            {
                _foresightPassword = foresightPassword;
                _userPassword = userPassword;
            }

            public ValidationResult validate()
            {
                var result = new ValidationResult();
                if (!_foresightPassword.Equals(_userPassword))
                    result.AddError(Resources.IncorrectPassword);

                return result;
            }
        }
    }

    internal class ValueRequiredRule : IValidator
    {
        private readonly string _fieldName;
        private readonly string _value;

        public ValueRequiredRule(string fieldName, string value)
        {
            _fieldName = fieldName;
            _value = value;
        }

        public ValidationResult validate()
        {
            var result = new ValidationResult();
            if (string.IsNullOrEmpty(_value))
                result.AddError(_fieldName + " Required");

            return result;
        }
    }

    internal class FilePathShouldNotExistRule : IValidator
    {
        private readonly string _path;

        public FilePathShouldNotExistRule(string path)
        {
            _path = path;
        }

        public ValidationResult validate()
        {
            var result = new ValidationResult();
            if (File.Exists(_path))
                result.AddError(string.Format(Resources.CompanyGroupAlreadyExists, _path));

            return result;
        }
    }

    internal class SaveCompanyGroupRules : RulesBase
    {
        public void Check(ForesightDatabase db, CompanyGroup companyGroup)
        {
            Check(new ValueRequiredRule("Name", companyGroup.Name));
            Check(new CompanyGroupNameShoudNotExistRule(db, companyGroup));
        }

        private class CompanyGroupNameShoudNotExistRule : IValidator
        {
            private readonly ForesightDatabase _db;
            private readonly CompanyGroup _companyGroup;

            public CompanyGroupNameShoudNotExistRule(ForesightDatabase db, CompanyGroup companyGroup)
            {
                _db = db;
                _companyGroup = companyGroup;
            }

            public ValidationResult validate()
            {
                var result = new ValidationResult();
                if (_db.IsCompanyGroupNameExist(_companyGroup))
                    result.AddError("This company group already exist");

                return result;
            }
        }
    }

    internal class SqlCeSaveCompanyGroupRules : RulesBase
    {
        public void Check(CompanyGroup companyGroup)
        {
            if (companyGroup.IsNew())
                Check(new FilePathShouldNotExistRule(companyGroup.FilePath));
        }
    }

    //internal class SaveAccountRules : RulesBase
    //{
    //    public void Check(Account account)
    //    {
    //        base.Check(new ValueRequiredRule("Name", account.Name));
    //    }
    //}

    //internal class DeleteAccountRules : RulesBase
    //{
    //    private SqlCeCommand cmd = null;

    //    public DeleteAccountRules(SqlCeCommand cmd)
    //    {
    //        this.cmd = cmd;
    //    }

    //    public void Check(Account account)
    //    {
    //        base.Check(new TransactionsShoudNotExistRule(cmd, account));
    //    }

    //    private class TransactionsShoudNotExistRule : IValidator
    //    {
    //        private SqlCeCommand cmd = null;
    //        private Account account;

    //        public TransactionsShoudNotExistRule(SqlCeCommand cmd, Account account)
    //        {
    //            this.cmd = cmd;
    //            this.account = account;
    //        }

    //        public ValidationResult validate()
    //        {
    //            var result = new ValidationResult();
    //            if (areTransactionsExist(account))
    //                result.AddError("Transactions exit. Cannot delete.");

    //            return result;
    //        }

    //        private bool areTransactionsExist(Account account)
    //        {
    //            cmd.CommandText = SqlQueries.ReadTransactionCountOfAccountId;
    //            cmd.Parameters.Add("@AccountId", account.Id);
    //            object value = cmd.ExecuteScalar();
    //            return Convert.ToInt32(value) > 0;
    //        }
    //    }
    //}

    //internal class SaveTransactionRules : RulesBase
    //{
    //    public void Check(Transaction trans)
    //    {
    //        base.Check(new AmountRequiredRule(trans.Amount));
    //        base.Check(new CreditOrDebitEntryRequiredRule(trans));
    //    }

    //    private class AmountRequiredRule : IValidator
    //    {
    //        private decimal amount;

    //        public AmountRequiredRule(decimal amount)
    //        {
    //            this.amount = amount;
    //        }

    //        public ValidationResult validate()
    //        {
    //            var result = new ValidationResult();
    //            if (amount <= 0)
    //                result.AddError("Amount cannot be zero");

    //            return result;
    //        }
    //    }

    //    private class CreditOrDebitEntryRequiredRule : IValidator
    //    {
    //        private Transaction trans;

    //        public CreditOrDebitEntryRequiredRule(Transaction trans)
    //        {
    //            this.trans = trans;
    //        }

    //        public ValidationResult validate()
    //        {
    //            var result = new ValidationResult();
    //            if (!trans.HasCreditEntry() && !trans.HasDebitEntry())
    //                result.AddError("Either CR or DB entry required.");

    //            return result;
    //        }
    //    }
    //}
}
