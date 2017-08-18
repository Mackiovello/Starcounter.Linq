﻿using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;
namespace Starcounter.Linq.Tests
{
    public class LinqTests
    {

        //[Fact]
        //public void Run()
        //{
        //    Assert.Equal("SELECT E FROM Starcounter.Linq.Tests.Employee E WHERE (((E.Department.Company.Name = ?) AND (E.Age > ?))) FETCH 1",
        //        Sql(() => Objects<Employee>().FirstOrDefault(p => p.Department.Company.Name == "XXX" && p.Age > 123)));
        //}

        [Fact]
        public void ValueEquals()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name = ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name == "XXX")));
        }

        [Fact]
        public void ValueGreaterThan()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Age > ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Age > 123)));
        }

        [Fact]
        public void ValueGreaterOrEqualTo()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Age >= ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Age >= 123)));
        }

        [Fact]
        public void StringContains()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name LIKE '%' || ? || '%')) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.Contains("XXX"))));
        }

        [Fact]
        public void StringStartsWith()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name LIKE ? || '%')) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.StartsWith("XXX"))));
        }

        [Fact]
        public void StringEndsWith()
        {
            Assert.Equal("SELECT P FROM Starcounter.Linq.Tests.Person P WHERE ((P.Name LIKE '%' || ?)) FETCH 1",
                Sql(() => Objects<Person>().FirstOrDefault(p => p.Name.EndsWith("XXX"))));
        }
    }
}