// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using AutoRest.Core.Extensibility;

namespace AutoRest.NodeJS
{
    public class GeneratorSettingsJs : IGeneratorSettings
    {
        /// <summary>
        /// Whether or not to generate a new package.json file.
        /// </summary>
        public bool GeneratePackageJson { get; set; } = true;

        /// <summary>
        /// Whether or not to generate a new readme.md file.
        /// </summary>
        public bool GenerateReadmeMd { get; set; } = false;

        /// <summary>
        /// Whether or not to generate the LICENSE.txt file.
        /// </summary>
        public bool GenerateLicenseTxt { get; set; } = true;

        /// <summary>
        /// The sub-folder path where source code will be generated.
        /// </summary>
        public string SourceCodeFolderPath { get; set; } = "lib";

        /// <summary>
        /// The name of the package to generate.
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// The version of the package to generate.
        /// </summary>
        public string PackageVersion { get; set; }

        /// <summary>
        /// The folder where the generated files will be output to.
        /// </summary>
        public string OutputFolder { get; set; }
    }
}