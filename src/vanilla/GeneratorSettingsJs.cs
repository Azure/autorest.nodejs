// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using AutoRest.Core.Extensibility;

namespace AutoRest.NodeJS
{
    public class GeneratorSettingsJs : IGeneratorSettings
    {
        /// <summary>
        /// Change to true if you want to generate new package.json and README.md files.
        /// </summary>
        public bool GenerateMetadata { get; set; }

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