// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoRest.NodeJS
{
    [TestClass]
    public class GeneratorSettingsJsTests
    {
        [TestMethod]
        public void UpdatePackageVersionWithNullPackageName()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            Assert.IsNull(settings.PackageName);
            Assert.IsNull(settings.PackageVersion);

            settings.UpdatePackageVersion();

            Assert.IsNull(settings.PackageName);
            Assert.IsNull(settings.PackageVersion);
        }

        [TestMethod]
        public void UpdatePackageVersionWithEmptyPackageName()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "";
            Assert.IsNull(settings.PackageVersion);

            settings.UpdatePackageVersion();

            Assert.AreEqual("", settings.PackageName);
            Assert.IsNull(settings.PackageVersion);
        }

        [TestMethod]
        public void UpdatePackageVersionWithNonExistingPackageName()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "idontexistandyoucantinstallme";
            Assert.IsNull(settings.PackageVersion);

            settings.UpdatePackageVersion();

            Assert.AreEqual("idontexistandyoucantinstallme", settings.PackageName);
            Assert.AreEqual("1.0.0", settings.PackageVersion);
        }

        [TestMethod]
        public void UpdatePackageVersionWithNonExistingPackageNameAndPackageVersionSpecified()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "idontexistandyoucantinstallme";
            settings.PackageVersion = "2.3.4";

            settings.UpdatePackageVersion();

            Assert.AreEqual("idontexistandyoucantinstallme", settings.PackageName);
            Assert.AreEqual("2.3.4", settings.PackageVersion);
        }

        [TestMethod]
        public void UpdatePackageVersionWithExistingPackageName()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "npm";
            Assert.IsNull(settings.PackageVersion);

            settings.UpdatePackageVersion();

            Assert.AreEqual("npm", settings.PackageName);
            Assert.IsNotNull(settings.PackageVersion);
            Assert.AreNotEqual("", settings.PackageVersion);
        }

        [TestMethod]
        public void UpdatePackageVersionWithExistingPackageNameAndPackageVersionSpecified()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "npm";
            settings.PackageVersion = "1.2.3";

            settings.UpdatePackageVersion();

            Assert.AreEqual("npm", settings.PackageName);
            Assert.AreEqual("1.2.3", settings.PackageVersion);
        }
    }
}
