using System;
using System.Collections.Generic;
using System.Data;
using ScalableApps.Foresight.Logic.Business;
using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Sql;
using CommandType = ScalableApps.Foresight.Logic.Common.CommandType;

namespace ScalableApps.Foresight.Logic.DataAccess
{
    public abstract class ForesightDatabase : IDisposable
    {
        #region Declarations

        protected Database db;

        #endregion

        #region Common

        public void Dispose()
        {
            db.Close();
        }

        #endregion

        #region Login

        public void CheckPassword(int role, string password)
        {
            new PasswordRules(getForesightLoginPassword(role))
                                .Check(password);
        }

        private string getForesightLoginPassword(int role)
        {
            var value = db.ExecuteScalar(SqlQueries.SelectLoginPassword, "@Role", role);
            return value == DBNull.Value ? "" : value.ToString();
        }

        #endregion

        #region Company Group

        public bool IsCompanyGroupNameExist(CompanyGroup companyGroup)
        {
            var cmd = db.CreateCommand(SqlQueries.SelectCountByCompanyGroupName);
            db.AddParameterWithValue(cmd, "@name", companyGroup.Name);
            db.AddParameterWithValue(cmd, "@id", companyGroup.Id);
            var value = cmd.ExecuteScalar();
            return Convert.ToInt32(value) > 0;
        }

        public IEnumerable<CompanyGroup> GetCompanyGroups()
        {
            var result = new List<CompanyGroup>();
            var rdr = db.ExecuteReader(SqlQueries.SelectAllCompanyGroups);
            while (rdr.Read())
                result.Add(readCompanyGroup(rdr));

            rdr.Close();
            return result;
        }

        private CompanyGroup readCompanyGroup(IDataReader rdr)
        {
            var companyGroup = CompanyGroup.CreateNewGroup();
            companyGroup.Name = rdr["Name"].ToString();
            companyGroup.FilePath = rdr["DatabaseName"].ToString();
            companyGroup.Id = Convert.ToInt16(rdr["Id"]);
            return companyGroup;
        }

        public void SaveCompanyGroup(CompanyGroup companyGroup)
        {
            var cmd = db.CreateCommand(SqlQueries.ForesightInsertCompanyGroup);
            db.AddParameterWithValue(cmd, "@Name", companyGroup.Name);
            db.AddParameterWithValue(cmd, "@DatabaseName", companyGroup.FilePath);
            cmd.ExecuteNonQuery();
            companyGroup.Id = db.GetGeneratedIdentityValue();
        }

        public void UpdateCompanyGroup(CompanyGroup companyGroup)
        {
            var cmd = db.CreateCommand(SqlQueries.ForesightUpdateCompanyGroup);
            db.AddParameterWithValue(cmd, "@Id", companyGroup.Id);
            db.AddParameterWithValue(cmd, "@Name", companyGroup.Name);
            db.AddParameterWithValue(cmd, "@DatabaseName", companyGroup.FilePath);
            cmd.ExecuteNonQuery();
        }

        public void DeleteCompanyGroup(CompanyGroup companyGroup)
        {
            db.ExecuteNonQuery(SqlQueries.ForesightDeleteCompanyGroup, "@Id", companyGroup.Id);
        }

        #endregion

        #region Command

        public Command GetCommandByNr(int nr)
        {
            var rdr = db.ExecuteReader(SqlQueries.SelectCommandByNr, "@nr", nr);
            Command ri = null;

            if (rdr.Read())
                ri = readCommand(rdr);

            rdr.Close();

            if (ri == null)
                throw new ValidationException(string.Format("Command Nr: {0} not found.", nr));

            return ri;
        }

        public IEnumerable<Command> GetAllCommands()
        {
            var result = new List<Command>();
            var rdr = db.ExecuteReader(SqlQueries.SelectAllCommands);
            while (rdr.Read())
                result.Add(readCommand(rdr));

            rdr.Close();
            return result;
        }

        private Command readCommand(IDataReader rdr)
        {
            var cmd = new Command();
            cmd.Id = Convert.ToInt32(rdr["Id"]);
            cmd.Nr = Convert.ToInt32(rdr["Nr"]);
            cmd.Name = rdr["Name"].ToString();
            cmd.Title = rdr["Title"].ToString();
            cmd.Description = rdr["Description"].ToString();
            cmd.UIControlName = rdr["UIControlName"].ToString();
            cmd.IsActive = Convert.ToBoolean(rdr["IsActive"]);
            cmd.Type = (CommandType)rdr["Type"];
            cmd.Properties = readCommandPropsByCommandtNr(cmd.Id);
            return cmd;
        }

        private IList<CommandProp> readCommandPropsByCommandtNr(int commandId)
        {
            var result = new List<CommandProp>();
            var rdr = db.ExecuteReader(SqlQueries.SelectCommandPropsByCommandId,
                                                    "@commandId", commandId);
            while (rdr.Read())
                result.Add(new CommandProp
                               {
                                   Id = Convert.ToInt32(rdr["Id"]),
                                   PropName = rdr["PropName"].ToString(),
                                   PropValue = rdr["PropValue"].ToString()
                               });

            rdr.Close();
            return result;
        }

        #endregion

        #region Error Log

        public void InsertError(ErrorMessage error)
        {
            var cmd = db.CreateCommand(SqlQueries.InsertError);
            db.AddParameterWithValue(cmd, "@DateTime", error.DateTime);
            db.AddParameterWithValue(cmd, "@Text", error.Text);
            cmd.ExecuteNonQuery();
            error.Id = db.GetGeneratedIdentityValue();
        }

        //private int getExceptionTextLength(string text)
        //{

        //    //exception.ToString().Substring(0, getExceptionTextLength(exception.ToString()))
        //    const int sqlTableNVarCharColumnLength = 4000;

        //    if (text.Length > sqlTableNVarCharColumnLength)
        //        return sqlTableNVarCharColumnLength;
        //    else
        //        return text.Length;
        //}

        #endregion

        #region Chart Of Account Mapper

        public IList<ChartOfAccountMapper> GetChartOfAccountsMapper()
        {
            var result = new List<ChartOfAccountMapper>();
            var rdr = db.ExecuteReader(SqlQueries.SelectChartOfAccountsMapper);
            while (rdr.Read())
                result.Add(readChartOfAccountMapper(rdr));

            rdr.Close();
            return result;
        }

        private ChartOfAccountMapper readChartOfAccountMapper(IDataReader rdr)
        {
            var coaMapper = new ChartOfAccountMapper();
            coaMapper.Id = Convert.ToInt32(rdr["Id"]);
            coaMapper.ChartOfAccountId = Convert.ToInt32(rdr["ChartOfAccountId"]);
            coaMapper.ChartOfAccountName = rdr["ChartOfAccountName"].ToString();
            coaMapper.EasyCode = Util.ConvertDbNullToString(rdr["EasyCode"]);
            coaMapper.McsCode = Util.ConvertDbNullToString(rdr["McsCode"]);
            coaMapper.TcsCode = Util.ConvertDbNullToString(rdr["TcsCode"]);
            return coaMapper;
        }

        #endregion

        #region DbScript

        public string GetDbScript()
        {
            return db.ExecuteScalar(SqlQueries.SelectDbScript).ToString();
        }

        #endregion
    }
}
