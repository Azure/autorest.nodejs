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
        public bool GeneratePackageJson { get; set; }

        /// <summary>
        /// Whether or not to generate a new readme.md file.
        /// </summary>
        public bool GenerateReadmeMd { get; set; }

        /// <summary>
        /// Whether or not to generate the LICENSE.txt file.
        /// </summary>
        public bool GenerateLicenseTxt { get; set; }

        /// <summary>
        /// The name of the package to generate.
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// The version of the package to generate.
        /// </summary>
        public string PackageVersion { get; set; }
    }
}