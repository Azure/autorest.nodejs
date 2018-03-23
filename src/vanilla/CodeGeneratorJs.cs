// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoRest.Core;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.NodeJS.Model;
using AutoRest.NodeJS.vanilla.Templates;
using static AutoRest.Core.Utilities.DependencyInjection;

namespace AutoRest.NodeJS
{
    public class CodeGeneratorJs : CodeGenerator
    {
        private const string ClientRuntimePackage = "ms-rest version 2.0.0";

        public override string ImplementationFileExtension => ".js";

        public override string UsageInstructions => $"The {ClientRuntimePackage} or higher npm package is required to execute the generated code.";

        private static bool ReadSettingBool(string settingName, bool defaultValue = false)
        {
            bool settingValue;
            if (!bool.TryParse(Settings.Instance.Host.GetValue(settingName).Result, out settingValue))
            {
                settingValue = defaultValue;
            }
            return settingValue;
        }

        /// <summary>
        ///     Generate NodeJS client code 
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <returns></returns>
        public override async Task Generate(CodeModel cm)
        {
            GeneratorSettingsJs generatorSettings = Singleton<GeneratorSettingsJs>.Instance;

            var codeModel = cm as CodeModelJs;
            if (codeModel == null)
            {
                throw new InvalidCastException("CodeModel is not a NodeJS code model.");
            }

            codeModel.PackageName = generatorSettings.PackageName;
            codeModel.PackageVersion = generatorSettings.PackageVersion;

            // Service client
            var serviceClientTemplate = new ServiceClientTemplate {Model = codeModel};
            await Write(serviceClientTemplate, codeModel.Name.ToCamelCase() + ".js");

            var serviceClientTemplateTS = new ServiceClientTemplateTS {Model = codeModel};
            await Write(serviceClientTemplateTS, codeModel.Name.ToCamelCase() + ".d.ts");

            //Models
            if (codeModel.ModelTypes.Any())
            {
                var modelIndexTemplate = new ModelIndexTemplate {Model = codeModel};
                await Write(modelIndexTemplate, Path.Combine("models", "index.js"));

                var modelIndexTemplateTS = new ModelIndexTemplateTS {Model = codeModel};
                await Write(modelIndexTemplateTS, Path.Combine("models", "index.d.ts"));

                foreach (var modelType in codeModel.ModelTemplateModels)
                {
                    var modelTemplate = new ModelTemplate {Model = modelType};
                    await Write(modelTemplate, Path.Combine("models", modelType.NameAsFileName.ToCamelCase() + ".js"));
                }
            }

            //MethodGroups
            if (codeModel.MethodGroupModels.Any())
            {
                var methodGroupIndexTemplate = new MethodGroupIndexTemplate {Model = codeModel};
                await Write(methodGroupIndexTemplate, Path.Combine("operations", "index.js"));

                var methodGroupIndexTemplateTS = new MethodGroupIndexTemplateTS {Model = codeModel};
                await Write(methodGroupIndexTemplateTS, Path.Combine("operations", "index.d.ts"));

                foreach (var methodGroupModel in codeModel.MethodGroupModels)
                {
                    var methodGroupTemplate = new MethodGroupTemplate {Model = methodGroupModel};
                    await Write(methodGroupTemplate, Path.Combine("operations", methodGroupModel.TypeName.ToCamelCase() + ".js"));
                }
            }

            if (generatorSettings.GeneratePackageJson)
            {
                var packageJson = new PackageJson { Model = codeModel };
                await Write(packageJson, "package.json").ConfigureAwait(false);
            }

            if (generatorSettings.GenerateReadmeMd)
            {
                var readme = new ReadmeTemplate { Model = codeModel };
                await Write(readme, "README.md").ConfigureAwait(false);
            }

            if (generatorSettings.GenerateLicense)
            {
                LicenseTemplate license = new LicenseTemplate { Model = codeModel };
                await Write(license, "LICENSE.txt").ConfigureAwait(false);
            }
        }
    }
}
