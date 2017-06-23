using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scalable.Shared.DataAccess
{
    public class RavenDbManger : IDisposable
    {
        //public DocumentStore Db;
        //public string DbName { get; set; }

        ////public IObjectContainer Db;

        //public DbContext(string dbName)
        //{
        //    DbName = dbName;
        //    Db = new DocumentStore { Url = "http://localhost:8080" };
        //    Db.Initialize();
        //    Db.DatabaseCommands.EnsureDatabaseExists(dbName);
        //    //Db = Db4oEmbedded.OpenFile(@"E:\TestDB.dbo");
        //}

        //public IDocumentSession OpenSession()
        //{
        //    return Db.OpenSession(DbName);
        //}
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
