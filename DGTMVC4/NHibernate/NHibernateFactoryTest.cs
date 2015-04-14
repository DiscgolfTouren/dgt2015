using System;
using System.Collections.Generic;
using System.Globalization;
using DGTMVC4.NHibernate.Enums;
using DGTMVC4.NHibernate.Models;
using NUnit.Framework;

namespace DGTMVC4.NHibernate
{
    public class NHibernateFactoryTest
    {
        [Test]
        public void Add_Player_Read_Player_Delete_Player_Compare_Player()
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

            Player dbPlayer;

            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var id = session.Save(player);
                    
                    dbPlayer = session.Get<Player>(id);
                    session.Delete(dbPlayer);

                    transaction.Commit();
                }
            }
            Assert.AreEqual(player, dbPlayer);
        }

        [Test]
        public void Add_Holes_And_Courseconfiguration_Then_Delete_And_Compare()
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

            Courseconfiguration dbCourseconfiguration;

            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var id = session.Save(courseconfiguration);
                    dbCourseconfiguration = session.Get<Courseconfiguration>(id);
                    session.Delete(dbCourseconfiguration);
                    transaction.Commit();
                }
            }

            Assert.That(dbCourseconfiguration.Holes.Count == 2);
            Assert.AreEqual(courseconfiguration, dbCourseconfiguration);
        }

        [Test]
        public void Add_PlayerResult_For_Player()
        {
            var hole1score = new HoleScore { HoleNumber = 1, Score = 3 };
            var hole2score = new HoleScore { HoleNumber = 2, Score = 3 };

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

            Player dbPlayer;
            PlayerResult dbPlayerResult;

            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var pid = session.Save(player);
                    var prid = session.Save(playerResult);

                    dbPlayer = session.Get<Player>(pid);
                    dbPlayerResult = session.Get<PlayerResult>(prid);

                    session.Delete(dbPlayer);
                    session.Delete(dbPlayerResult);

                    transaction.Commit();
                }
            }

            Assert.AreEqual(player, dbPlayer);
            Assert.AreEqual(playerResult, dbPlayerResult);
            Assert.That(dbPlayerResult.Scores.Count == 2);
        }

        [Test]
        public void Create_Tour_With_Competitions_And_Players()
        {
            var courseconfig = new Courseconfiguration
                {
                    CourseMap = "coursemap",
                    CourseName = "first",
                    Description = "my first course"
                };
            courseconfig.AddHole(new Hole {Length = 100, Number = 1, Par = 3});
            courseconfig.AddHole(new Hole {Length = 86, Number = 2, Par = 3});
            courseconfig.AddHole(new Hole {Length = 149, Number = 3, Par = 4});

            var player1 = new Player
                {
                    Email = "email@internet.webz",
                    FirstName = "firstname",
                    LastName = "lastname",
                    Phone = "08112233"
                };

            var player2 = new Player
                {
                    Email = "whatever@webben.net",
                    FirstName = "john",
                    LastName = "doe",
                    Phone = "08334422"
                };

            var competition = new Competition {Date = DateTime.UtcNow, Name = "dgt1"};
            competition.AddPlayer(new PlayerStatus {Player = player1, Status = PlayerCompetitionStatus.Registered});
            competition.AddPlayer(new PlayerStatus {Player = player2, Status = PlayerCompetitionStatus.Payed});

            competition.AddRound(new Round {Courseconfig = courseconfig, RoundNumber = 1});
            competition.AddRound(new Round {Courseconfig = courseconfig, RoundNumber = 2});

            var tour = new Tour {Year = "2015", Description = "testTour"};
            tour.AddCompetition(competition);

            Player dbPlayer1;
            Player dbPlayer2;
            Courseconfiguration dbCourseconfig;
            Tour dbTour;
            
            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var pid1 = session.Save(player1);
                    var pid2 = session.Save(player2);
                    var cid = session.Save(courseconfig);
                    var tid = session.Save(tour);

                    dbPlayer1 = session.Get<Player>(pid1);
                    dbPlayer2 = session.Get<Player>(pid2);
                    dbCourseconfig = session.Get<Courseconfiguration>(cid);
                    dbTour = session.Get<Tour>(tid);

                    session.Delete(dbPlayer1);
                    session.Delete(dbPlayer2);
                    session.Delete(dbCourseconfig);
                    session.Delete(dbTour);

                    transaction.Commit();
                }
            }

            Assert.AreEqual(player1, dbPlayer1);
            Assert.AreEqual(player2, dbPlayer2);
            Assert.AreEqual(courseconfig, dbCourseconfig);
            Assert.AreEqual(tour, dbTour);
        }

        [Test]
        [Ignore] //Only run once manually
        public void Create_Tour_2015_With_Competitions()
        {
            var tour = new Tour
                {
                    Year = "2015",
                    Description =
                        "Premiär tour för dgt med tävlingar på Ultuna, Visättra, Eskilstuna, Lillsjön, Åkersberga"
                };

            tour.AddCompetition(new Competition
                {
                    Date = DateTime.ParseExact("2015-04-25", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Description = "Tävling #1 på touren 2015",
                    Name = "Ultuna"
                });
            tour.AddCompetition(new Competition
                {
                    Date = DateTime.ParseExact("2015-05-17", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Description = "Tävling #2 på touren 2015",
                    Name = "Visättra"
                });
            tour.AddCompetition(new Competition
                {
                    Date = DateTime.ParseExact("2015-07-25", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Description = "Tävling #3 på touren 2015",
                    Name = "Eskilstuna"
                });
            tour.AddCompetition(new Competition
                {
                    Date = DateTime.ParseExact("2015-08-15", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Description = "Tävling #4 på touren 2015",
                    Name = "Lillsjön"
                });
            tour.AddCompetition(new Competition
                {
                    Date = DateTime.ParseExact("2015-08-29", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Description = "Tävling #5 på touren 2015",
                    Name = "Åkersberga"
                });

            using (var session = NHibernateFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    session.Save(tour);
                    transaction.Commit();
                }
            }
        }
    }
}