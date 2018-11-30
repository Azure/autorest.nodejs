// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoRest.Core.Model;
using AutoRest.Extensions.Azure;
using AutoRest.NodeJS.Model;
using Newtonsoft.Json;
using AutoRest.Core.Utilities;
using AutoRest.NodeJS.DSL;
using AutoRest.Core;

namespace AutoRest.NodeJS.Azure.Model
{
    
    public class CodeModelJsa : CodeModelJs
    {
        public CodeModelJsa()
            : base()
        {
        }
        [JsonIgnore]
        public override bool IsAzure => true;


        [JsonIgnore]
        public override IEnumerable<CompositeTypeJs> ModelTemplateModels => ModelTypes.Cast<CompositeTypeJs>().Concat(PageTemplateModels).Where(each => !PageTemplateModels.Any(ptm => ptm.Name.EqualsIgnoreCase(each.Name)));



        public override CompositeType Add(CompositeType item)
        {
            // Removing all models that contain the extension "x-ms-external", as they will be
            // generated in nodejs client runtime for azure - "ms-rest-azure".
            if (item.Extensions.ContainsKey(AzureExtensions.PageableExtension) ||
                item.Extensions.ContainsKey(AzureExtensions.ExternalExtension))
            {
                return null;
            }

            return base.Add(item);
        }

        public override IEnumerable<string> PackageDependencies()
        {
            return base.PackageDependencies().Concat(new[]
            {
                "\"ms-rest-azure\": \"^2.5.5\""
            });
        }

        public IList<PageCompositeTypeJsa> PageTemplateModels { get; set; } = new List<PageCompositeTypeJsa>();

        public bool shouldOptionsInterfaceBeDeclared
        {
            get
            {
                List<string> predefinedOptionalParameters = new List<string>() { "apiVersion", "acceptLanguage", "longRunningOperationRetryTimeout", "generateClientRequestId", "rpRegistrationRetryTimeout" };
                var optionalParameters = this.Properties.Where(
                    p => (!p.IsRequired || p.IsRequired && !string.IsNullOrEmpty(p.DefaultValue)) 
                    && !p.IsConstant && !predefinedOptionalParameters.Contains(p.Name));
                return optionalParameters.Count() > 0;
            }
        }

        public override string ConstructImportTS() =>
            MethodTemplateModels.Any()
                ? "import { ServiceClient, ServiceClientOptions, ServiceCallback, HttpOperationResponse, ServiceClientCredentials } from 'ms-rest';"
                : "import { ServiceClientCredentials } from 'ms-rest';";

        public string ConstructImportTSAzure() =>
            "import { AzureServiceClient, AzureServiceClientOptions } from 'ms-rest-azure';";



        public override string GenerateModelIndexDTS()
        {
            TSBuilder builder = new TSBuilder();

            builder.Comment(Settings.Instance.Header);
            builder.Line();
            builder.Import(new[] { "BaseResource", "CloudError" }, "ms-rest-azure");
            builder.ImportAllAs("moment", "moment");
            builder.Line();
            builder.Export(export =>
            {
                export.Export("BaseResource");
                export.Export("CloudError");
            });
            foreach (CompositeTypeJs modelType in OrderedModelTemplateModels)
            {
                builder.Line();
                modelType.GenerateModelDefinition(builder);
            }
            foreach (PageCompositeTypeJsa pageModelType in PageTemplateModels)
            {
                builder.Line();
                pageModelType.GenerateModelDefinition(builder);
            }

            return builder.ToString();
        }

        public override string GenerateReadmeMd()
        {
            MarkdownBuilder builder = new MarkdownBuilder();
            ConstructDocsHeader(builder);
            GenerateTypeScriptSDKMessage(builder);

            builder.IncreaseCurrentHeaderLevel();
            builder.Section($"Microsoft Azure SDK for Node.js - {Name}", () =>
            {
                builder.Line("This project provides a Node.js package for accessing Azure. Right now it supports:");
                builder.List("**Node.js version 6.x.x or higher**");
                builder.Line();
                builder.Section("Features");
                builder.Line();
                builder.Line();
                builder.Section("How to Install", () =>
                {
                    builder.Console($"npm install {PackageName}");
                });
                builder.Line();
                builder.Section("How to use", () =>
                {
                    builder.Section($"Authentication, client creation, and {GetSampleMethod()?.Name} {GetSampleMethodGroupName()} as an example.", () =>
                    {
                        builder.JavaScript(jsBuilder =>
                        {
                            jsBuilder.ConstVariable("msRestAzure", "require(\"ms-rest-azure\")");
                            jsBuilder.ConstVariable(Name, $"require(\"{PackageName}\")");
                            jsBuilder.Line($"msRestAzure.interactiveLogin().then((creds) => {{");
                            jsBuilder.Indent(() =>
                            {
                                jsBuilder.ConstQuotedStringVariable("subscriptionId", "<Subscription_Id>");
                                jsBuilder.ConstVariable("client", $"new {Name}(creds, subscriptionId)");
                                jsBuilder.Line($"{GenerateSampleMethod(true, true)};");
                            });
                            jsBuilder.Text($"}}){GetSampleCatchBlock()}");
                        });
                    });
                });
                builder.Section("Related projects", () =>
                {
                    builder.List("[Microsoft Azure SDK for Node.js](https://github.com/Azure/azure-sdk-for-node)");
                });
            });

            return builder.ToString();
        }
    }
}