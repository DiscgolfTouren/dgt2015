using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Web.Configuration;

namespace DGTMVC4.NHibernate
{
    public class NHibernateFactory
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
            var dbServer = WebConfigurationManager.AppSettings["DBServer"];
            var dbName = WebConfigurationManager.AppSettings["DBName"];
            var dbUsername = WebConfigurationManager.AppSettings["DBUsername"];
            var dbPassword = WebConfigurationManager.AppSettings["DBPassword"];
            _sessionFactory = Fluently.Configure()
                                      .Database(MySQLConfiguration
                                                    .Standard
                                                    .ConnectionString(cs => cs
                                                                                .Server(dbServer)
                                                                                .Database(dbName)
                                                                                .Username(dbUsername)
                                                                                .Password(dbPassword)))
                                      .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                                      .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                                      .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}