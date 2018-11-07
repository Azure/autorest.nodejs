// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoRest.NodeJS.DSL
{
    [TestClass]

    public class JSParameterTests
    {
        [TestMethod]
        public void Constructor()
        {
            JSParameter parameter = new JSParameter("parameterName", "MyType", "description");

            Assert.AreEqual("MyType", parameter.Type);
        }
    }
}
