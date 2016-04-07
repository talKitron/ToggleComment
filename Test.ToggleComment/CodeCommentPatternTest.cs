using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToggleComment;

namespace Test.ToggleComment
{
    /// <summary>
    /// <see cref="CodeCommentPattern"/>のテストクラスです。
    /// </summary>
    [TestClass]
    public class CodeCommentPatternTest
    {
        [TestMethod]
        public void IsCommentTest_LineComment()
        {
            var pattern = new CodeCommentPattern("//");

            Assert.IsTrue(pattern.IsComment("//"));
            Assert.IsTrue(pattern.IsComment("///"));
            Assert.IsTrue(pattern.IsComment(" //"));
            Assert.IsTrue(pattern.IsComment("\t//"));
            Assert.IsTrue(pattern.IsComment("\t //"));

            Assert.IsFalse(pattern.IsComment("/"));
            Assert.IsFalse(pattern.IsComment("hoge//"));

            Assert.IsTrue(pattern.IsComment(@"
//
//"));

            Assert.IsTrue(pattern.IsComment(@"
//

//"));

            Assert.IsFalse(pattern.IsComment(@"
//
a
//"));

            Assert.IsFalse(pattern.IsComment(string.Empty));
            Assert.IsFalse(pattern.IsComment(" "));
            Assert.IsFalse(pattern.IsComment(@"
"));
        }

        [TestMethod]
        public void IsCommentTest_BlockComment()
        {
            var pattern = new CodeCommentPattern("/*", "*/");

            Assert.IsTrue(pattern.IsComment("/**/"));
            Assert.IsTrue(pattern.IsComment(" /**/"));
            Assert.IsTrue(pattern.IsComment("/**/ "));
            Assert.IsTrue(pattern.IsComment("/*hoge*/"));

            Assert.IsTrue(pattern.IsComment(@"/*

*/"));

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
            Assert.IsFalse(pattern.IsComment(@"
"));
        }
    }
}
