﻿using System.Linq;
using Xunit;
using static Starcounter.Linq.DbLinq;
using static Starcounter.Linq.Tests.Utils;
namespace Starcounter.Linq.Tests
{
    public class WhereTests
    {

        [Fact]
        public void WhereEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" = ?))",
                Sql(() => Objects<Person>().Where(p => p.Name == "XXX")));
        }

        [Fact]
        public void MultipleWhereEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE ((P.\"Name\" = ?)) AND ((P.\"Age\" = ?))",
                Sql(() => Objects<Person>().Where(p => p.Name == "XXX").Where(p => p.Age == 1)));
        }

        [Fact]
        public void WhereOrEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (((P.\"Name\" = ?) OR (P.\"Age\" = ?)))",
                Sql(() => Objects<Person>().Where(p => p.Name == "XXX" || p.Age == 1)));
        }

        [Fact]
        public void WhereAndEquals()
        {
            Assert.Equal("SELECT P FROM \"Starcounter\".\"Linq\".\"Tests\".\"Person\" P WHERE (((P.\"Name\" = ?) AND (P.\"Age\" = ?)))",
                Sql(() => Objects<Person>().Where(p => p.Name == "XXX" && p.Age == 1)));
        }
    }
}
