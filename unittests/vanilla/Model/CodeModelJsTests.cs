// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoRest.NodeJS.Model
{
    [TestClass]
    public class CodeModelJsTests
    {
        [TestMethod]
        public void HomePageUrlWithNullOutputFolder()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = null;
            Assert.AreEqual("https://github.com/azure/azure-sdk-for-node", codeModel.HomePageUrl);
        }

        [TestMethod]
        public void HomePageUrlWithEmptyOutputFolder()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = "";
            Assert.AreEqual("https://github.com/azure/azure-sdk-for-node", codeModel.HomePageUrl);
        }

        [TestMethod]
        public void HomePageUrlWithOutputFolderThatDoesntContainAzureSdkForNode()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = "test/azure/generated/StorageManagementClient";
            Assert.AreEqual("https://github.com/azure/azure-sdk-for-node", codeModel.HomePageUrl);
        }

        [TestMethod]
        public void HomePageUrlWithBackslashOutputFolderThatContainsAzureSdkForNode()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = "C:\\Users\\daschult\\Sources\\azure-sdk-for-node\\lib\\services\\batchManagement";
            Assert.AreEqual("https://github.com/azure/azure-sdk-for-node/tree/master/lib/services/batchManagement", codeModel.HomePageUrl);
        }

        [TestMethod]
        public void HomePageUrlWithForwardSlashOutputFolderThatContainsAzureSdkForNode()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = "C:/Users/daschult/Sources/azure-sdk-for-node/lib/services/batchManagement";
            Assert.AreEqual("https://github.com/azure/azure-sdk-for-node/tree/master/lib/services/batchManagement", codeModel.HomePageUrl);
        }
    }
}
