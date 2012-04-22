using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.IO;
using Ninject.Activation;

namespace Budgomatic.Core.DataAccess
{
    public class SessionFactoryProvider : Provider<ISessionFactory>
    {
        private string DbFile = 
            string.IsNullOrEmpty((string)AppDomain.CurrentDomain.GetData("DataDirectory")) ?
                "budgomatic.db" :
                Path.Combine((string)AppDomain.CurrentDomain.GetData("DataDirectory"), "budgomatic.db");

        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .UsingFile(DbFile))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private void BuildSchema(Configuration config)
        {
            // delete the existing db on each run
            if (File.Exists(DbFile))
                File.Delete(DbFile);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
                .Create(false, true);
        }

        protected override ISessionFactory CreateInstance(IContext context)
        {
            return CreateSessionFactory();
        }
    }
}
