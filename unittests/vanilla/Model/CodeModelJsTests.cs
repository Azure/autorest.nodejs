// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using Xunit;

namespace AutoRest.NodeJS.Model
{
    public class CodeModelJsTests
    {
        [Fact]
        public void HomePageUrlWithNullOutputFolder()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = null;
            Assert.Equal("https://github.com/azure/azure-sdk-for-node", codeModel.HomePageUrl);
        }

        [Fact]
        public void HomePageUrlWithEmptyOutputFolder()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = "";
            Assert.Equal("https://github.com/azure/azure-sdk-for-node", codeModel.HomePageUrl);
        }

        [Fact]
        public void HomePageUrlWithOutputFolderThatDoesntContainAzureSdkForNode()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = "test/azure/generated/StorageManagementClient";
            Assert.Equal("https://github.com/azure/azure-sdk-for-node", codeModel.HomePageUrl);
        }

        [Fact]
        public void HomePageUrlWithBackslashOutputFolderThatContainsAzureSdkForNode()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = "C:\\Users\\daschult\\Sources\\azure-sdk-for-node\\lib\\services\\batchManagement";
            Assert.Equal("https://github.com/azure/azure-sdk-for-node/lib/services/batchManagement", codeModel.HomePageUrl);
        }

        [Fact]
        public void HomePageUrlWithForwardSlashOutputFolderThatContainsAzureSdkForNode()
        {
            CodeModelJs codeModel = new CodeModelJs();
            codeModel.OutputFolder = "C:/Users/daschult/Sources/azure-sdk-for-node/lib/services/batchManagement";
            Assert.Equal("https://github.com/azure/azure-sdk-for-node/lib/services/batchManagement", codeModel.HomePageUrl);
        }
    }
}
