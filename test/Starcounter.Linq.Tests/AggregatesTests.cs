﻿using System.Linq;
using Xunit;
using static Starcounter.Linq.Tests.Utils;
using static Starcounter.Linq.DbLinq;

namespace Starcounter.Linq.Tests
{
    public class AggregatesTests
    {
        [Fact]
        public void Count()
        {
            Assert.Equal("SELECT COUNT(P) FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P",
                Sql(() => Objects<Person>().Count()));
        }

        [Fact]
        public void CountFiltering()
        {
            Assert.Equal("SELECT COUNT(P) FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" = ?))",
                Sql(() => Objects<Person>().Count(x => x.Name == "XXX")));
        }

        [Fact]
        public void CountFiltering_CalculatedValue()
        {
            var name = "XXX";
            Assert.Equal("SELECT COUNT(P) FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" = ?))",
                Sql(() => Objects<Person>().Count(x => x.Name == name)));
        }

        [Fact]
        public void Average()
        {
            Assert.Equal("SELECT AVG(P.\"Age\") FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P",
                Sql(() => Objects<Person>().Average(p => p.Age)));
        }

        [Fact]
        public void Min()
        {
            Assert.Equal("SELECT MIN(P.\"Age\") FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P",
                Sql(() => Objects<Person>().Min(p => p.Age)));
        }

        [Fact]
        public void Max()
        {
            Assert.Equal("SELECT MAX(P.\"Age\") FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P",
                Sql(() => Objects<Person>().Max(p => p.Age)));
        }

        [Fact]
        public void Sum()
        {
            Assert.Equal("SELECT SUM(P.\"Age\") FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P",
                Sql(() => Objects<Person>().Sum(p => p.Age)));
        }

        [Fact]
        public void Sum_ReservedWordField()
        {
            Assert.Equal("SELECT SUM(P.\"Limit\") FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P",
                Sql(() => Objects<Person>().Sum(p => p.Limit)));
        }
    }
}