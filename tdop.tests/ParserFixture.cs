using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace tdop.tests
{
    [TestClass]
    public class ParserFixture
    {
        [TestMethod]
        public void ParseNumber()
        {

            var p = new Parser();
            var result = p.Parse(new Token[]
                                      {
                                          new Token() {Type = "number", Value = 1}, 
                                          new Token() {Type = "(end)"},
                                      });

            result.Count.Should().Be(1);
            var n = result[0];
            n.Type.Should().Be("number");
            n.Value.Should().Be(1);
        }
    }
}
