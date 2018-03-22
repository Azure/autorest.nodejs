// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.NodeJS.azure.Templates;
using AutoRest.NodeJS.Azure.Model;
using AutoRest.NodeJS.vanilla.Templates;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AutoRest.Core.Utilities.DependencyInjection;

namespace AutoRest.NodeJS.Azure
{
    public class CodeGeneratorJsa : CodeGeneratorJs
    {
        private const string ClientRuntimePackage = "ms-rest-azure version 2.0.0";

        public override string UsageInstructions => $"The {ClientRuntimePackage} or higher npm package is required to execute the generated code.";

        public override string ImplementationFileExtension => ".js";


        /// <summary>
        /// Generate Azure NodeJS client code 
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public override async Task Generate(CodeModel cm)
        {
            GeneratorSettingsJs generatorSettings = Singleton<GeneratorSettingsJs>.Instance;

            var codeModel = cm as CodeModelJsa;
            if (codeModel == null)
            {
                throw new InvalidCastException("CodeModel is not a Azure NodeJS code model.");
            }

            codeModel.PackageName = generatorSettings.PackageName;
            codeModel.PackageVersion = generatorSettings.PackageVersion;

            // Service client
            var serviceClientTemplate = new AzureServiceClientTemplate { Model = codeModel };
            await Write(serviceClientTemplate, codeModel.Name.ToCamelCase() + ".js");

            var serviceClientTemplateTS = new AzureServiceClientTemplateTS { Model = codeModel, };
            await Write(serviceClientTemplateTS, codeModel.Name.ToCamelCase() + ".d.ts");

            var modelIndexTemplate = new AzureModelIndexTemplate { Model = codeModel };
            await Write(modelIndexTemplate, Path.Combine("models", "index.js"));

            var modelIndexTemplateTS = new AzureModelIndexTemplateTS { Model = codeModel };
            await Write(modelIndexTemplateTS, Path.Combine("models", "index.d.ts"));

            //Models
            if (codeModel.ModelTemplateModels.Any())
            {
                // Paged Models
                foreach (var pageModel in codeModel.PageTemplateModels)
                {
                    var pageTemplate = new PageModelTemplate { Model = pageModel };
                    await Write(pageTemplate, Path.Combine("models", pageModel.Name.ToCamelCase() + ".js"));
                }
                
                foreach (var modelType in codeModel.ModelTemplateModels)
                {
                    var modelTemplate = new ModelTemplate { Model = modelType };
                    await Write(modelTemplate, Path.Combine("models", modelType.NameAsFileName.ToCamelCase() + ".js"));
                }
            }

            //MethodGroups
            if (codeModel.MethodGroupModels.Any())
            {
                var methodGroupIndexTemplate = new MethodGroupIndexTemplate { Model = codeModel };
                await Write(methodGroupIndexTemplate, Path.Combine("operations", "index.js"));

                var methodGroupIndexTemplateTS = new MethodGroupIndexTemplateTS { Model = codeModel };
                await Write(methodGroupIndexTemplateTS, Path.Combine("operations", "index.d.ts"));
                
                foreach (var methodGroupModel in codeModel.MethodGroupModels)
                {
                    var methodGroupTemplate = new AzureMethodGroupTemplate { Model = methodGroupModel };
                    await Write(methodGroupTemplate, Path.Combine("operations", methodGroupModel.TypeName.ToCamelCase() + ".js"));
                }
            }

            if (generatorSettings.GenerateMetadata)
            {
                var packageJson = new PackageJson { Model = codeModel };
                await Write(packageJson, "package.json");

                var readme = new AzureReadmeTemplate { Model = codeModel };
                await Write(readme, "README.md");
            }
        }
    }
}
