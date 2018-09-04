// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using Xunit;

namespace AutoRest.NodeJS
{
    public class GeneratorSettingsJsTests
    {
        [Fact]
        public void UpdatePackageVersionWithNullPackageName()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            Assert.Null(settings.PackageName);
            Assert.Null(settings.PackageVersion);

            settings.UpdatePackageVersion();

            Assert.Null(settings.PackageName);
            Assert.Null(settings.PackageVersion);
        }

        [Fact]
        public void UpdatePackageVersionWithEmptyPackageName()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "";
            Assert.Null(settings.PackageVersion);

            settings.UpdatePackageVersion();

            Assert.Equal("", settings.PackageName);
            Assert.Null(settings.PackageVersion);
        }

        [Fact]
        public void UpdatePackageVersionWithNonExistingPackageName()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "idontexistandyoucantinstallme";
            Assert.Null(settings.PackageVersion);

            settings.UpdatePackageVersion();

            Assert.Equal("idontexistandyoucantinstallme", settings.PackageName);
            Assert.Equal("1.0.0", settings.PackageVersion);
        }

        [Fact]
        public void UpdatePackageVersionWithNonExistingPackageNameAndPackageVersionSpecified()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "idontexistandyoucantinstallme";
            settings.PackageVersion = "2.3.4";

            settings.UpdatePackageVersion();

            Assert.Equal("idontexistandyoucantinstallme", settings.PackageName);
            Assert.Equal("2.3.4", settings.PackageVersion);
        }

        [Fact]
        public void UpdatePackageVersionWithExistingPackageName()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "npm";
            Assert.Null(settings.PackageVersion);

            settings.UpdatePackageVersion();

            Assert.Equal("npm", settings.PackageName);
            Assert.NotNull(settings.PackageVersion);
            Assert.NotEmpty(settings.PackageVersion);
        }

        [Fact]
        public void UpdatePackageVersionWithExistingPackageNameAndPackageVersionSpecified()
        {
            GeneratorSettingsJs settings = new GeneratorSettingsJs();
            settings.PackageName = "npm";
            settings.PackageVersion = "1.2.3";

            settings.UpdatePackageVersion();

            Assert.Equal("npm", settings.PackageName);
            Assert.Equal("1.2.3", settings.PackageVersion);
        }
    }
}
