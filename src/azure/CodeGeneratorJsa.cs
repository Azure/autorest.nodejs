// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.NodeJS.azure.Templates;
using AutoRest.NodeJS.Azure.Model;
using System;
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

            generatorSettings.UpdatePackageVersion();
            codeModel.PopulateFromSettings(generatorSettings);

            // Service client
            await GenerateServiceClientJs(() => new AzureServiceClientTemplate { Model = codeModel }, generatorSettings).ConfigureAwait(false);

            await GenerateServiceClientDts(() => new AzureServiceClientTemplateTS { Model = codeModel }, generatorSettings).ConfigureAwait(false);

            //Models
            if (codeModel.ModelTypes.Any())
            {
                await GenerateModelIndexJs(() => new AzureModelIndexTemplate { Model = codeModel }, generatorSettings).ConfigureAwait(false);

                await GenerateModelIndexDts(codeModel, generatorSettings).ConfigureAwait(false);

                // Paged Models
                foreach (var pageModel in codeModel.PageTemplateModels)
                {
                    var pageTemplate = new PageModelTemplate { Model = pageModel };
                    await Write(pageTemplate, GetModelSourceCodeFilePath(generatorSettings, pageModel.Name.ToCamelCase() + ".js")).ConfigureAwait(false);
                }
                
                foreach (var modelType in codeModel.ModelTemplateModels)
                {
                    await GenerateModelJs(modelType, generatorSettings).ConfigureAwait(false);
                }
            }

            //MethodGroups
            if (codeModel.MethodGroupModels.Any())
            {
                await GenerateMethodGroupIndexTemplateJs(codeModel, generatorSettings).ConfigureAwait(false);

                await GenerateMethodGroupIndexTemplateDts(codeModel, generatorSettings).ConfigureAwait(false);

                foreach (var methodGroupModel in codeModel.MethodGroupModels)
                {
                    await GenerateMethodGroupJs(() => new AzureMethodGroupTemplate { Model = methodGroupModel }, generatorSettings).ConfigureAwait(false);
                }
            }

            await GeneratePackageJson(codeModel, generatorSettings).ConfigureAwait(false);

            await GenerateReadmeMd(codeModel, generatorSettings).ConfigureAwait(false);

            await GenerateLicenseTxt(codeModel, generatorSettings).ConfigureAwait(false);
        }
    }
}
