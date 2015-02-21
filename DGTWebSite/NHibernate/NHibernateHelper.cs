using DGTWebSite.Entity;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace DGTWebSite.NHibernate
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InizializeFacotory();

                return _sessionFactory;
            }
        }

        private static void InizializeFacotory()
        {
            _sessionFactory = Fluently.Configure()
                                      .Database(MySQLConfiguration
                                                    .Standard
                                                    .ConnectionString(cs => cs
                                                                                .Server("localhost")
                                                                                .Database("database")
                                                                                .Username("dbusername")
                                                                                .Password("dbpassword")))
                                      .Mappings(x => x.FluentMappings.AddFromAssemblyOf<Competition>())
                                      .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                                      .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}