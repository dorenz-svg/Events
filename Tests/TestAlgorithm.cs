using Events.Infrastructure;
using Events.Model;
using Events.Models.Request;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class TestAlgorithm
    {
        [Fact]
        public void TestGetDate()
        {
            //arrange
            var service = new Algorithm();
            var dates = MockDates();
            //act
            var result = service.FindConvenientDate(dates);
            //assert
            Assert.Equal((DateTime.Parse("2021-11-20T21:00"), DateTime.Parse("2021-11-20T22:00")), result);
        }
        private List<UserDates> MockDates()
        {
            List<UserDates> userDates = new();
            userDates.Add(new UserDates
            {
                UserName = "Петя",
                Dates = new List<Date> {
            new Date{DateStart=DateTime.Parse("2021-11-20T18:00") ,DateEnd=DateTime.Parse("2021-11-20T22:00")},
            new Date{DateStart=DateTime.Parse("2021-11-21T18:00") ,DateEnd=DateTime.Parse("2021-11-21T22:00")},
            new Date{DateStart=DateTime.Parse("2021-11-27T18:00") ,DateEnd=DateTime.Parse("2021-11-27T22:00")},
            new Date{DateStart=DateTime.Parse("2021-11-28T18:00") ,DateEnd=DateTime.Parse("2021-11-28T22:00")},
            }
            });

            userDates.Add(new UserDates
            {
                UserName = "Вася",
                Dates = new List<Date> {
            new Date{DateStart=DateTime.Parse("2021-11-19T20:00") ,DateEnd=DateTime.Parse("2021-11-19T23:00")},
            new Date{DateStart=DateTime.Parse("2021-11-20T19:00") ,DateEnd=DateTime.Parse("2021-11-20T22:00")},
            new Date{DateStart=DateTime.Parse("2021-11-26T21:00") ,DateEnd=DateTime.Parse("2021-11-26T23:00")},
            }
            });

            userDates.Add(new UserDates
            {
                UserName = "Серега",
                Dates = new List<Date> {
            new Date{DateStart=DateTime.Parse("2021-11-18T21:00") ,DateEnd=DateTime.Parse("2021-11-18T23:00")},
            new Date{DateStart=DateTime.Parse("2021-11-19T19:00") ,DateEnd=DateTime.Parse("2021-11-19T22:00")},
            new Date{DateStart=DateTime.Parse("2021-11-20T21:00") ,DateEnd=DateTime.Parse("2021-11-20T23:00")},
            new Date{DateStart=DateTime.Parse("2021-11-21T19:00") ,DateEnd=DateTime.Parse("2021-11-21T23:00")},
            }
            });
            return userDates;
        }
    }
}
