using System;
using DGTMVC4.NHibernate.Models;
using NUnit.Framework;

namespace DGTMVC4.NHibernate
{
    public class NHibernateFactoryTest
    {
         [Test]
         public void Add_Player()
         {
             using (var session = NHibernateFactory.OpenSession())
             {
                 using (var transaction = session.BeginTransaction())
                 {
                     var player = new Player
                         {
                             FirstName = "Mikael",
                             LastName = "Edvardsson",
                             Email = "mikaedva@gmail.com",
                             PdgaNumber = "12089",
                             Phone = "0708616094",
                             RatingDate = DateTime.UtcNow
                         };
                     session.Save(player);
                     transaction.Commit();
                 }
             }
         }
    }
}