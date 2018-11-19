// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoRest.NodeJS
{
    [TestClass]
    public class ClientModelExtensionsTests
    {
        [TestMethod]
        public void CreateRegexPatternConstraintValueWithNull()
        {
            Assert.AreEqual("", ClientModelExtensions.CreateRegexPatternConstraintValue(null));
        }

        [TestMethod]
        public void CreateRegexPatternConstraintValueWithEmpty()
        {
            Assert.AreEqual("", ClientModelExtensions.CreateRegexPatternConstraintValue(""));
        }

        [TestMethod]
        public void CreateRegexPatternConstraintValueWithWhitespace()
        {
            Assert.AreEqual("/ \t /", ClientModelExtensions.CreateRegexPatternConstraintValue(" \t "));
        }

        [TestMethod]
        public void CreateRegexPatternConstraintValueWithUnescapedForwardSlash()
        {
            Assert.AreEqual(
                "/^([0-9]{1,3}\\.){3}[0-9]{1,3}(\\/([0-9]|[1-2][0-9]|3[0-2]))?$/",
                ClientModelExtensions.CreateRegexPatternConstraintValue(
                    "^([0-9]{1,3}\\.){3}[0-9]{1,3}(/([0-9]|[1-2][0-9]|3[0-2]))?$"));
        }

        [TestMethod]
        public void CreateRegexPatternConstraintValueWithEscapedForwardSlash()
        {
            Assert.AreEqual(
                "/^([0-9]{1,3}\\.){3}[0-9]{1,3}(\\/([0-9]|[1-2][0-9]|3[0-2]))?$/",
                ClientModelExtensions.CreateRegexPatternConstraintValue(
                    "^([0-9]{1,3}\\.){3}[0-9]{1,3}(\\/([0-9]|[1-2][0-9]|3[0-2]))?$"));
        }
    }
}
