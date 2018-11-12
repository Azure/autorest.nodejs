// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using System;
using AutoRest.NodeJS.Model;
using AutoRest.NodeJS.DSL;

namespace AutoRest.NodeJS.Azure.Model
{
    public class PageCompositeTypeJsa : CompositeTypeJs
    {
        public PageCompositeTypeJsa(string nextLinkName, string itemName) 
        {
            NextLinkName = nextLinkName;
            ItemName = itemName;
        }

        public string NextLinkName { get; private set; }

        public string ItemName { get; private set; }

        public override string ConstructModelMapper()
        {
            var modelMapper = this.ConstructMapper(SerializedName, null, true, true);
            var builder = new IndentedStringBuilder("  ");
            builder.AppendLine("return {{{0}}};", modelMapper);
            return builder.ToString();
        }

        public override void GenerateModelDefinition(TSBuilder builder)
        {
            builder.DocumentationComment(comment =>
            {
                comment.Summary(Summary);
                comment.Description(Documentation);
            });

            IModelType arrayType;
            Property arrayProperty = Properties.FirstOrDefault(p => p.ModelType is SequenceTypeJs);
            if (arrayProperty == null)
            {
                throw new Exception($"The Pageable model {Name} does not contain a single property that is an Array.");
            }
            else
            {
                arrayType = ((SequenceTypeJs)arrayProperty.ModelType).ElementType;
            }

            builder.ExportInterface(Name, $"Array<{ClientModelExtensions.TSType(arrayType, true)}>", tsInterface =>
            {
                Property nextLinkProperty = Properties.Where(p => p.Name.ToLowerInvariant().Contains("nextlink")).FirstOrDefault();
                if (nextLinkProperty != null)
                {
                    tsInterface.DocumentationComment(comment =>
                    {
                        comment.Summary(nextLinkProperty.Summary);
                        comment.Description(nextLinkProperty.Documentation);
                    });
                    string propertyType = nextLinkProperty.ModelType.TSType(inModelsModule: true);
                    tsInterface.Property(nextLinkProperty.Name, propertyType, isRequired: nextLinkProperty.IsRequired, isReadonly: nextLinkProperty.IsReadOnly);
                }
            });
        }
    }
}
