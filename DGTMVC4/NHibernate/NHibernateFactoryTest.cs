using System;
using System.Collections.Generic;
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

        [Test]
        public void Add_Holes_And_Courseconfiguration()
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    
                    var hole1 = new Hole
                        {
                            Number = 1,
                            Par = 3,
                            Length = 100,
                            Description = "rakt med ob till höger"
                        };

                    var hole2 = new Hole
                        {
                            Number = 2,
                            Par = 4,
                            Length = 153,
                            Description = "dogleg vänster, mandatory passera trädet 35m fram på vänster sida"
                        };
                    
                    var courseconfiguration = new Courseconfiguration
                        {
                            CourseName = "TestBanan"
                        };

                    courseconfiguration.AddHole(hole1);
                    courseconfiguration.AddHole(hole2);

                    session.Save(courseconfiguration);
                    transaction.Commit();
                }
            }
        }

        [Test]
        public void Add_PlayerResult_For_Player()
        {
            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var hole1score = new HoleScore {HoleNumber = 1, Score = 3};
                    var hole2score = new HoleScore {HoleNumber = 2, Score = 3};

                    var player = new Player
                        {
                            FirstName = "Mikael",
                            LastName = "Edvardsson"
                        };

                    var playerResult = new PlayerResult
                        {
                            Player = player,
                            Penalties = 0,
                            Place = 1
                        };

                    playerResult.AddHoleScore(hole1score);
                    playerResult.AddHoleScore(hole2score);

                    session.Save(player);
                    session.Save(playerResult);
                    transaction.Commit();
                }
            }

        }
    }
}