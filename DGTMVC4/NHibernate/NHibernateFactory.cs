using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

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
            _sessionFactory = Fluently.Configure()
                                      .Database(MySQLConfiguration
                                                    .Standard
                                                    .ConnectionString(cs => cs
                                                                                .Server("localhost")
                                                                                .Database("dgt")
                                                                                .Username("peterby_dgtadmin")
                                                                                .Password("dgt2015")))
                                      .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                                      .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                                      .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}