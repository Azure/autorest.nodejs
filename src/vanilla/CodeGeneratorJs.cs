// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using AutoRest.Core;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.NodeJS.Model;
using AutoRest.NodeJS.vanilla.Templates;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AutoRest.Core.Utilities.DependencyInjection;

namespace AutoRest.NodeJS
{
    public class CodeGeneratorJs : CodeGenerator
    {
        private const string ClientRuntimePackage = "ms-rest version 2.5.0";

        public override string ImplementationFileExtension => ".js";

        public override string UsageInstructions => $"The {ClientRuntimePackage} or higher npm package is required to execute the generated code.";

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

            generatorSettings.UpdatePackageVersion();
            codeModel.PopulateFromSettings(generatorSettings);

            // Service client
            await GenerateServiceClientJs(() => new ServiceClientTemplate { Model = codeModel }, generatorSettings).ConfigureAwait(false);

            await GenerateServiceClientDts(() => new ServiceClientTemplateTS { Model = codeModel }, generatorSettings).ConfigureAwait(false);

            //Models
            if (codeModel.ModelTypes.Any())
            {
                await GenerateModelIndexJs(() => new ModelIndexTemplate { Model = codeModel }, generatorSettings).ConfigureAwait(false);

                await GenerateModelIndexDts(codeModel, generatorSettings).ConfigureAwait(false);

                foreach (CompositeTypeJs modelType in codeModel.ModelTemplateModels)
                {
                    await GenerateModelJs(modelType, generatorSettings).ConfigureAwait(false);
                }
            }

            //MethodGroups
            if (codeModel.MethodGroupModels.Any())
            {
                await GenerateMethodGroupIndexTemplateJs(codeModel, generatorSettings).ConfigureAwait(false);

                await GenerateMethodGroupIndexTemplateDts(codeModel, generatorSettings).ConfigureAwait(false);

                foreach (MethodGroupJs methodGroupModel in codeModel.MethodGroupModels)
                {
                    await GenerateMethodGroupJs(() => new MethodGroupTemplate { Model = methodGroupModel }, generatorSettings).ConfigureAwait(false);
                }
            }

            await GeneratePackageJson(codeModel, generatorSettings).ConfigureAwait(false);

            await GenerateReadmeMd(codeModel, generatorSettings).ConfigureAwait(false);

            await GenerateLicenseTxt(codeModel, generatorSettings).ConfigureAwait(false);

            await GeneratePostinstallScript(codeModel, generatorSettings).ConfigureAwait(false);
        }

        protected async Task GenerateServiceClientJs<T>(Func<Template<T>> serviceClientTemplateCreator, GeneratorSettingsJs generatorSettings) where T : CodeModelJs
        {
            Template<T> serviceClientTemplate = serviceClientTemplateCreator();
            await Write(serviceClientTemplate, GetSourceCodeFilePath(generatorSettings, serviceClientTemplate.Model.Name.ToCamelCase() + ".js"));
        }

        protected async Task GenerateServiceClientDts<T>(Func<Template<T>> serviceClientTemplateCreator, GeneratorSettingsJs generatorSettings) where T : CodeModelJs
        {
            Template<T> serviceClientTemplateTS = serviceClientTemplateCreator();
            await Write(serviceClientTemplateTS, GetSourceCodeFilePath(generatorSettings, serviceClientTemplateTS.Model.Name.ToCamelCase() + ".d.ts"));
        }

        protected async Task GenerateModelIndexJs<T>(Func<Template<T>> modelIndexTemplateCreator, GeneratorSettingsJs generatorSettings) where T : CodeModelJs
        {
            Template<T> modelIndexTemplate = modelIndexTemplateCreator();
            await Write(modelIndexTemplate, GetModelSourceCodeFilePath(generatorSettings, "index.js")).ConfigureAwait(false);
        }

        protected async Task GenerateModelIndexDts(CodeModelJs codeModel, GeneratorSettingsJs generatorSettings)
        {
            await Write(codeModel.GenerateModelIndexDTS(), GetModelSourceCodeFilePath(generatorSettings, "index.d.ts")).ConfigureAwait(false);
        }

        protected async Task GenerateModelJs(CompositeTypeJs model, GeneratorSettingsJs generatorSettings)
        {
            var modelTemplate = new ModelTemplate { Model = model };
            await Write(modelTemplate, GetModelSourceCodeFilePath(generatorSettings, model.NameAsFileName.ToCamelCase() + ".js")).ConfigureAwait(false);
        }

        protected async Task GenerateMethodGroupIndexTemplateJs(CodeModelJs codeModel, GeneratorSettingsJs generatorSettings)
        {
            var methodGroupIndexTemplate = new MethodGroupIndexTemplate { Model = codeModel };
            await Write(methodGroupIndexTemplate, GetOperationSourceCodeFilePath(generatorSettings, "index.js")).ConfigureAwait(false);
        }

        protected async Task GenerateMethodGroupIndexTemplateDts(CodeModelJs codeModel, GeneratorSettingsJs generatorSettings)
        {
            var methodGroupIndexTemplateTS = new MethodGroupIndexTemplateTS { Model = codeModel };
            await Write(methodGroupIndexTemplateTS, GetOperationSourceCodeFilePath(generatorSettings, "index.d.ts")).ConfigureAwait(false);
        }

        protected async Task GenerateMethodGroupJs<T>(Func<Template<T>> methodGroupTemplateCreator, GeneratorSettingsJs generatorSettings) where T : MethodGroupJs
        {
            Template<T> methodGroupTemplate = methodGroupTemplateCreator();
            await Write(methodGroupTemplate, GetOperationSourceCodeFilePath(generatorSettings, methodGroupTemplate.Model.TypeName.ToCamelCase() + ".js")).ConfigureAwait(false);
        }

        protected async Task GeneratePackageJson(CodeModelJs codeModel, GeneratorSettingsJs generatorSettings)
        {
            if (generatorSettings.GeneratePackageJson)
            {
                var packageJson = new PackageJson { Model = codeModel };
                await Write(packageJson, "package.json").ConfigureAwait(false);
            }
        }

        protected async Task GenerateReadmeMd(CodeModelJs codeModel, GeneratorSettingsJs generatorSettings)
        {
            if (generatorSettings.GenerateReadmeMd)
            {
                await Write(codeModel.GenerateReadmeMd(), "README.md").ConfigureAwait(false);
            }
        }

        protected async Task GenerateLicenseTxt(CodeModelJs codeModel, GeneratorSettingsJs generatorSettings)
        {
            if (generatorSettings.GenerateLicenseTxt)
            {
                LicenseTemplate license = new LicenseTemplate { Model = codeModel };
                await Write(license, "LICENSE.txt").ConfigureAwait(false);
            }
        }

        protected async Task GeneratePostinstallScript(CodeModelJs codeModel, GeneratorSettingsJs generatorSettings)
        {
            if (generatorSettings.GeneratePostinstallScript)
            {
                PostinstallScript postinstallScript = new PostinstallScript { Model = codeModel };
                await Write(postinstallScript, ".scripts/postinstall.js").ConfigureAwait(false);
            }
        }
        protected string GetModelSourceCodeFilePath(GeneratorSettingsJs generatorSettings, string modelFileName)
            => GetSourceCodeFilePath(generatorSettings, "models", modelFileName);

        protected string GetOperationSourceCodeFilePath(GeneratorSettingsJs generatorSettings, string operationFileName)
            => GetSourceCodeFilePath(generatorSettings, "operations", operationFileName);

        protected string GetSourceCodeFilePath(GeneratorSettingsJs generatorSettings, params string[] pathSegments)
        {
            string[] totalPathSegments = new string[pathSegments.Length + 1];
            totalPathSegments[0] = generatorSettings.SourceCodeFolderPath;
            for (int i = 0; i < pathSegments.Length; i++)
            {
                totalPathSegments[1 + i] = pathSegments[i];
            }
            return Path.Combine(totalPathSegments).Replace('\\', '/');
        }
    }
}
