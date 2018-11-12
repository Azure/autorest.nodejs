// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.Core;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.Extensions;
using AutoRest.NodeJS.DSL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AutoRest.NodeJS.Model
{
    public class CodeModelJs : CodeModel
    {
        private const string defaultGitHubRepositoryName = "azure-sdk-for-node";
        private const string defaultGitHubUrl = "https://github.com/azure/" + defaultGitHubRepositoryName;
        private const string searchStringSuffix = "/lib/services/";
        private const string outputFolderSearchString = "/" + defaultGitHubRepositoryName + searchStringSuffix;

        public CodeModelJs()
        {
        }

        public CodeModelJs(string packageName = "test-client", string packageVersion = "0.1.0")
        {
            PackageName = packageName;
            PackageVersion = packageVersion;
        }

        public bool IsCustomBaseUri => Extensions.ContainsKey(SwaggerExtensions.ParameterizedHostExtension);

        [JsonIgnore]
        public IEnumerable<MethodJs> MethodTemplateModels => Methods.Cast<MethodJs>().Where(each => each.MethodGroup.IsCodeModelMethodGroup);

        [JsonIgnore]
        public virtual IEnumerable<CompositeTypeJs> ModelTemplateModels => ModelTypes.Cast<CompositeTypeJs>();

        [JsonIgnore]
        public virtual IEnumerable<MethodGroupJs> MethodGroupModels => Operations.Cast<MethodGroupJs>().Where(each => !each.IsCodeModelMethodGroup);

        /// <summary>
        /// Provides an ordered ModelTemplateModel list such that the parent 
        /// type comes before in the list than its child. This helps when 
        /// requiring models in index.js
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<CompositeTypeJs> OrderedModelTemplateModels
        {
            get
            {
                List<CompositeTypeJs> orderedList = new List<CompositeTypeJs>();
                foreach (var model in ModelTemplateModels)
                {
                    constructOrderedList(model, orderedList);
                }
                return orderedList;
            }
        }

        public virtual string PackageName { get; private set; }

        public virtual string PackageVersion { get; private set; }

        public string OutputFolder { get; set; }

        public string HomePageUrl
        {
            get
            {
                string result = defaultGitHubUrl;
                if (!string.IsNullOrEmpty(OutputFolder))
                {
                    string outputFolder = OutputFolder.Replace('\\', '/');
                    int searchStringIndex = outputFolder.IndexOf(outputFolderSearchString, StringComparison.OrdinalIgnoreCase);
                    if (0 <= searchStringIndex)
                    {
                        result += "/tree/master" + searchStringSuffix + outputFolder.Substring(searchStringIndex + outputFolderSearchString.Length);
                    }
                }
                return result;
            }
        }

        public string RepositoryUrl => $"{defaultGitHubUrl}.git";

        public string BugsUrl => $"{defaultGitHubUrl}/issues";

        public virtual IEnumerable<string> PackageDependencies()
        {
            return new[]
            {
                "\"ms-rest\": \"^2.3.3\""
            };
        }

        public string PackageDependenciesString()
        {
            return string.Join(",\n", PackageDependencies());
        }

        public void PopulateFromSettings(GeneratorSettingsJs generatorSettings)
        {
            PackageName = generatorSettings.PackageName;
            PackageVersion = generatorSettings.PackageVersion;
            OutputFolder = generatorSettings.OutputFolder;
        }

        public string ClientPrefix
        {
            get
            {
                string clientPrefix = Name;

                const string clientSuffix = "Client";
                if (clientPrefix.EndsWith(clientSuffix))
                {
                    clientPrefix = clientPrefix.Substring(0, clientPrefix.Length - clientSuffix.Length);
                }

                return clientPrefix;
            }
        }

        public string ServiceModelsName => ClientPrefix + "Models";

        public string GetSampleClientImport()
        {
            return $"const {Name} = require(\"{PackageName}\");";
        }

        public string GetSampleSubscriptionVariable()
        {
            return "const subscriptionId = \"<Subscription_Id>\";";
        }

        public string GetSampleCatchBlock()
        {
            IndentedStringBuilder builder = new IndentedStringBuilder("  ");
            builder.AppendLine(".catch((err) => {");
            builder.Indent();
            builder.AppendLine("console.log('An error occurred:');");
            builder.AppendLine("console.dir(err, {depth: null, colors: true});");
            builder.Outdent();
            builder.AppendLine("});");
            return builder.ToString();
        }

        public virtual Method GetSampleMethod()
        {
            var getMethod = Methods.Where(m => m.HttpMethod == HttpMethod.Get).FirstOrDefault();
            return getMethod != null ? getMethod : Methods.FirstOrDefault();
        }

        public virtual string GetSampleMethodGroupName()
        {
            return GetSampleMethod()?.MethodGroup?.Name?.ToCamelCase();
        }

        public string GenerateSampleMethod(bool returnPromise, bool isBrowser = false)
        {
            Method method = GetSampleMethod();
            string methodGroup = GetSampleMethodGroupName();
            List<Parameter> requiredParameters = method.LogicalParameters.Where(
                p => p != null && !p.IsClientProperty && !string.IsNullOrWhiteSpace(p.Name) && !p.IsConstant).OrderBy(item => !item.IsRequired).ToList();
            var builder = new IndentedStringBuilder("  ");
            string paramInit = InitializeParametersForSampleMethod(requiredParameters, isBrowser);
            builder.AppendLine(paramInit);
            var declaration = new StringBuilder();
            bool first = true;
            foreach (var param in requiredParameters)
            {
                if (!first)
                    declaration.Append(", ");
                declaration.Append(param.Name);
                first = false;
            }
            string clientRef = "client.";
            if (!string.IsNullOrEmpty(methodGroup))
            {
                clientRef = $"client.{methodGroup}.";
            }
            builder.AppendLine($"{(returnPromise ? "return " : "")}{clientRef}{method.Name.ToCamelCase()}({declaration.ToString()}).then((result) => {{")
                   .Indent()
                   .AppendLine("console.log(\"The result is:\");")
                   .AppendLine("console.log(result);")
                   .Outdent();
            if (isBrowser)
            {
                builder.Append("})");
            }
            else
            {
                builder.AppendLine("});");
            }

            return builder.ToString();
        }

        public string InitializeParametersForSampleMethod(List<Parameter> requiredParameters, bool isBrowser = false)
        {
            var builder = new IndentedStringBuilder("  ");
            foreach (var param in requiredParameters)
            {
                var paramValue = "\"\"";
                paramValue = param.ModelType.InitializeType(param.Name, isBrowser);
                var paramDeclaration = $"const {param.Name}";
                if (param.ModelType is CompositeType && !isBrowser)
                {
                    paramDeclaration += $": {ServiceModelsName}.{param.ModelTypeName}";
                }
                paramDeclaration += $" = {paramValue};";
                builder.AppendLine(paramDeclaration);
            }
            return builder.ToString();
        }

        public bool ContainsDurationProperty()
        {
            Core.Model.Property prop = Properties.FirstOrDefault(p =>
                (p.ModelType is PrimaryTypeJs && (p.ModelType as PrimaryTypeJs).KnownPrimaryType == KnownPrimaryType.TimeSpan) ||
                (p.ModelType is Core.Model.SequenceType && (p.ModelType as Core.Model.SequenceType).ElementType.IsPrimaryType(KnownPrimaryType.TimeSpan)) ||
                (p.ModelType is Core.Model.DictionaryType && (p.ModelType as Core.Model.DictionaryType).ValueType.IsPrimaryType(KnownPrimaryType.TimeSpan)));
            return prop != null;
        }

        private void constructOrderedList(CompositeTypeJs model, List<CompositeTypeJs> orderedList)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // BaseResource and CloudError are specified in the ClientRuntime. 
            // They are required explicitly in a different way. Hence, they
            // are not included in the ordered list.
            if (model.BaseModelType == null ||
                (model.BaseModelType != null &&
                 (model.BaseModelType.Name == "BaseResource" ||
                  model.BaseModelType.Name == "CloudError")))
            {
                if (!orderedList.Contains(model))
                {
                    orderedList.Add(model);
                }
                return;
            }

            var baseModel = ModelTemplateModels.FirstOrDefault(m => m.Name == model.BaseModelType.Name);
            if (baseModel != null)
            {
                constructOrderedList(baseModel, orderedList);
            }
            // Add the child type after the parent type has been added.
            if (!orderedList.Contains(model))
            {
                orderedList.Add(model);
            }
        }

        public string PolymorphicDictionary
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                var polymorphicTypes = ModelTemplateModels.Where(m => m.BaseIsPolymorphic);

                for (int i = 0; i < polymorphicTypes.Count(); i++)
                {
                    string discriminatorField = polymorphicTypes.ElementAt(i).SerializedName;
                    var polymorphicType = polymorphicTypes.ElementAt(i) as CompositeType;
                    if (polymorphicType.BaseModelType != null)
                    {
                        while (polymorphicType.BaseModelType != null)
                        {
                            polymorphicType = polymorphicType.BaseModelType;
                        }
                        discriminatorField = string.Format(CultureInfo.InvariantCulture, "{0}.{1}",
                            polymorphicType.Name,
                            polymorphicTypes.ElementAt(i).SerializedName);
                        builder.Append(string.Format(CultureInfo.InvariantCulture,
                        "'{0}' : exports.{1}",
                            discriminatorField,
                            polymorphicTypes.ElementAt(i).Name));
                    }
                    else
                    {
                        builder.Append(string.Format(CultureInfo.InvariantCulture,
                        "'{0}' : exports.{1}",
                            discriminatorField,
                            polymorphicTypes.ElementAt(i).Name));
                    }


                    if (i == polymorphicTypes.Count() - 1)
                    {
                        builder.AppendLine();
                    }
                    else
                    {
                        builder.AppendLine(",");
                    }
                }

                return builder.ToString();
            }
        }

        public string RequiredConstructorParameters
        {
            get
            {
                var requireParams = new List<string>();
                this.Properties.Where(p => p.IsRequired && !p.IsConstant && string.IsNullOrEmpty(p.DefaultValue))
                    .ForEach(p => requireParams.Add(p.Name.ToCamelCase()));
                if (!IsCustomBaseUri)
                {
                    requireParams.Add("baseUri");
                }

                if (NullOrEmpty(requireParams))
                {
                    return string.Empty;
                }

                return string.Join(", ", requireParams);
            }
        }

        /// <summary>
        /// Return the service client constructor required parameters, in TypeScript syntax.
        /// </summary>
        public string RequiredConstructorParametersTS
        {
            get
            {
                StringBuilder requiredParams = new StringBuilder();

                bool first = true;
                foreach (var p in this.Properties)
                {
                    if (!p.IsRequired || p.IsConstant || (p.IsRequired && !string.IsNullOrEmpty(p.DefaultValue)))
                        continue;

                    if (!first)
                        requiredParams.Append(", ");

                    requiredParams.Append(p.Name);
                    requiredParams.Append(": ");
                    requiredParams.Append(p.ModelType.TSType(inModelsModule: false));

                    first = false;
                }

                if (!IsCustomBaseUri)
                {
                    if (!first)
                        requiredParams.Append(", ");

                    requiredParams.Append("baseUri?: string");
                }

                return requiredParams.ToString();
            }
        }

        public virtual string ConstructImportTS()
        {
            StringBuilder builder = new StringBuilder();
            if (!NullOrEmpty(MethodTemplateModels))
            {
                builder.Append("import { ServiceClient, ServiceClientOptions, ServiceCallback, HttpOperationResponse");
            }
            else
            {
                builder.Append("import { ServiceClient, ServiceClientOptions");
            }

            if (Properties.Any(p => p.Name.EqualsIgnoreCase("credentials")))
            {
                builder.Append(", ServiceClientCredentials");
            }

            builder.Append(" } from 'ms-rest';");
            return builder.ToString();
        }

        public bool ContainsTimeSpan
        {
            get
            {
                return this.Methods.FirstOrDefault(
                    m => m.Parameters.FirstOrDefault(p => p.ModelType.IsPrimaryType(KnownPrimaryType.TimeSpan)) != null) != null;
            }
        }

        public override IEnumerable<string> MyReservedNames => new[] { Name };

        public string ConstructServiceClientJSExports()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"module.exports = {Name};");
            builder.AppendLine($"module.exports['default'] = {Name};");
            builder.AppendLine($"module.exports.{Name} = {Name};");
            if (ModelTypes.Any())
            {
                builder.AppendLine($"module.exports.{ServiceModelsName} = models;");
            }
            return builder.ToString();
        }

        public string ConstructServiceClientDTSExports() =>
            ModelTypes.Any()
                ? $"export {{ {Name}, models as {ServiceModelsName} }};"
                : "";

        private static bool NullOrEmpty<T>(IEnumerable<T> values)
            => values == null || !values.Any();

        public string ConstructDocsHeader(string emptyLine)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"---");
            builder.AppendLine($"uid: {PackageName}");
            builder.AppendLine($"summary: *content");
            builder.AppendLine(emptyLine);
            builder.AppendLine($"---");

            return builder.ToString();
        }

        public string GenerateTypeScriptSDKMessage()
        {
            return $"**This SDK will be deprecated next year and will be replaced by a new TypeScript-based isomorphic SDK (found at https://github.com/Azure/azure-sdk-for-js) which works on Node.js and browsers.**";
        }

        public virtual string GenerateModelIndexDTS()
        {
            TSBuilder builder = new TSBuilder();

            builder.Comment(Settings.Instance.Header);
            builder.Line();
            builder.ImportAllAs("moment", "moment");
            foreach (CompositeTypeJs modelType in OrderedModelTemplateModels)
            {
                builder.Line();
                modelType.GenerateModelDefinition(builder);
            }

            return builder.ToString();
        }
    }
}