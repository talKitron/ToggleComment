using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToggleComment.Codes;

namespace Test.ToggleComment.Codes
{
    /// <summary>
    /// Test class for <see cref="BlockCommentPattern"/>.
    /// </summary>
    [TestClass]
    public class BlockCommentPatternTest
    {
        [TestMethod]
        public void IsCommentTest()
        {
            var pattern = new BlockCommentPattern("/*", "*/");

            Assert.IsTrue(pattern.IsComment("/**/"));
            Assert.IsTrue(pattern.IsComment(" /**/"));
            Assert.IsTrue(pattern.IsComment("/**/ "));
            Assert.IsTrue(pattern.IsComment("/*hoge*/"));
            Assert.IsTrue(pattern.IsComment(string.Join(Environment.NewLine, "/*", "hoge", "*/")));

            Assert.IsFalse(pattern.IsComment("/*"));
            Assert.IsFalse(pattern.IsComment("*/"));

            Assert.IsFalse(pattern.IsComment("/ /"));
            Assert.IsFalse(pattern.IsComment("/ *"));
            Assert.IsFalse(pattern.IsComment("* /"));
            Assert.IsFalse(pattern.IsComment("* *"));

            Assert.IsFalse(pattern.IsComment("* */"));
            Assert.IsFalse(pattern.IsComment("/* *"));

            Assert.IsFalse(pattern.IsComment(string.Empty));
            Assert.IsFalse(pattern.IsComment(" "));
            Assert.IsFalse(pattern.IsComment(Environment.NewLine));
        }
    }
}
