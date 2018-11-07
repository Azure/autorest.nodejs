// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoRest.NodeJS.DSL
{
    [TestClass]
    public class JSDocumentationCommentTests
    {
        [TestMethod]
        public void DeprecatedWithNull()
        {
            JSBuilder builder = new JSBuilder();
            builder.DocumentationComment(comment =>
            {
                comment.Deprecated(null);
            });
            AssertEx.EqualLines("", builder);
        }

        [TestMethod]
        public void DeprecatedWithEmpty()
        {
            JSBuilder builder = new JSBuilder();
            builder.DocumentationComment(comment =>
            {
                comment.Deprecated("");
            });
            AssertEx.EqualLines(
                new[]
                {
                    "/**",
                    " * @deprecated",
                    " */",
                },
                builder);
        }

        [TestMethod]
        public void DeprecatedWithWhitespace()
        {
            JSBuilder builder = new JSBuilder();
            builder.DocumentationComment(comment =>
            {
                comment.Deprecated("\t  ");
            });
            AssertEx.EqualLines(
                new[]
                {
                    "/**",
                    " * @deprecated",
                    " */",
                },
                builder);
        }

        [TestMethod]
        public void DeprecatedWithNonEmpty()
        {
            JSBuilder builder = new JSBuilder();
            builder.DocumentationComment(comment =>
            {
                comment.Deprecated("abc");
            });
            AssertEx.EqualLines(
                new[]
                {
                    "/**",
                    " * @deprecated abc",
                    " */",
                },
                builder);
        }
    }
}
