using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToggleComment.Codes;

namespace Test.ToggleComment.Codes
{
    /// <summary>
    /// Test class for <see cref="LineCommentPattern"/>.
    /// </summary>
    [TestClass]
    public class LineCommentPatternTest
    {
        [TestMethod]
        public void IsCommentTest()
        {
            var pattern = new LineCommentPattern("//");

            Assert.IsTrue(pattern.IsComment("//"));
            Assert.IsTrue(pattern.IsComment("///"));
            Assert.IsTrue(pattern.IsComment(" //"));
            Assert.IsTrue(pattern.IsComment("\t//"));
            Assert.IsTrue(pattern.IsComment("\t //"));

            Assert.IsFalse(pattern.IsComment("/"));
            Assert.IsFalse(pattern.IsComment("hoge//"));

            Assert.IsTrue(pattern.IsComment(string.Join(Environment.NewLine, "//", "//")));
            Assert.IsTrue(pattern.IsComment(string.Join(Environment.NewLine, "//", string.Empty, "//")));
            Assert.IsFalse(pattern.IsComment(string.Join(Environment.NewLine, "//", "hoge", "//")));

            Assert.IsFalse(pattern.IsComment(string.Empty));
            Assert.IsFalse(pattern.IsComment(" "));
            Assert.IsFalse(pattern.IsComment(Environment.NewLine));
        }
    }
}
