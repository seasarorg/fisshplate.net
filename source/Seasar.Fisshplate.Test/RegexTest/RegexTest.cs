using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Text.RegularExpressions;

namespace Seasar.Fisshplate.Test.RegexTest
{
    [TestFixture]
    public class RegexTest
    {
        private string foreachStr = @"^\s*#foreach\s+(\S+)\s*:\s*(\S+)(\s+index\s*=\s*(\S+))*(\s+max\s*=\s*(\S+))*\s*$";
        [Test]
        public void TestIteratorBlockRegex()
        {
            
            Regex regex = new Regex(foreachStr);

            Match m = regex.Match("#foreach foo : fooList index = idx max = 10");

            Assert.AreEqual<int>(7, m.Groups.Count);
            Assert.AreEqual<string>("foo", m.Groups[1].Value);
            Assert.AreEqual<string>("fooList", m.Groups[2].Value);
            Assert.AreEqual<string>("idx", m.Groups[4].Value);
            Assert.AreEqual<string>("10", m.Groups[6].Value);

        }
        [Test]
        public void TestIteratorBlockRegexWithOutIndex()
        {
            Regex regex = new Regex(foreachStr);

            Match m = regex.Match(" #foreach foo:fooList max= 10");

            Assert.AreEqual<int>(7, m.Groups.Count);
            Assert.AreEqual<string>("foo", m.Groups[1].Value);
            Assert.AreEqual<string>("fooList", m.Groups[2].Value);
            Assert.AreEqual<string>("", m.Groups[4].Value);
            Assert.AreEqual<string>("10", m.Groups[6].Value);
        }
        [Test]
        public void TestIteratorBlockRegexWithOutMax()
        {
            Regex regex = new Regex(foreachStr);

            Match m = regex.Match("#foreach foo :fooList index =idx ");

            Assert.AreEqual<int>(7, m.Groups.Count);
            Assert.AreEqual<string>("foo", m.Groups[1].Value);
            Assert.AreEqual<string>("fooList", m.Groups[2].Value);
            Assert.AreEqual<string>("idx", m.Groups[4].Value);
            Assert.AreEqual<string>("", m.Groups[6].Value);
        }
        [Test]
        public void TestIteratorBlockRegexWithOutIndexMax()
        {
            Regex regex = new Regex(foreachStr);

            Match m = regex.Match(" #foreach foo: fooList ");

            Assert.AreEqual<int>(7, m.Groups.Count);
            Assert.AreEqual<string>("foo", m.Groups[1].Value);
            Assert.AreEqual<string>("fooList", m.Groups[2].Value);
            Assert.AreEqual<string>("", m.Groups[4].Value);
            Assert.AreEqual<string>("", m.Groups[6].Value);
        }

        private string picStr = @"^\s*#picture\s*\(\s*\S+\s+cell\s*=\s*\S+\s+row\s*=\s*\S+\s*\)";

        [Test]
        public void TestPictuireRegex()
        {
            Regex regex = new Regex(picStr);
            Match m = regex.Match(" #picture(${data.picture!} cell=1 row=1) ");

            Assert.IsTrue(m.Success);
            

        }

    }
}
