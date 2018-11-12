// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.NodeJS.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AutoRest.Core.Utilities.DependencyInjection;

namespace AutoRest.NodeJS.Model
{
    public class CompositeTypeJs : CompositeType
    {
        public CompositeTypeJs()
        {

        }

        public CompositeTypeJs(string name) : base(name)
        {
        }

        public bool ContainsTimeSpanPropertyWithValue()
        {
            return Properties.Any((Property property) =>
                !string.IsNullOrEmpty(property.DefaultValue) &&
                (property.ModelType.IsPrimaryType(KnownPrimaryType.TimeSpan) ||
                    (property.ModelType is SequenceType sequencePropertyType && sequencePropertyType.ElementType.IsPrimaryType(KnownPrimaryType.TimeSpan)) ||
                    (property.ModelType is DictionaryType dictionaryPropertyType && dictionaryPropertyType.ValueType.IsPrimaryType(KnownPrimaryType.TimeSpan))));
        }

        public override Property Add(Property item)
        {
            var result = base.Add(item);
            if (result != null)
            {
                AddPolymorphicPropertyIfNecessary();
            }
            return result;
        }

        public string NameAsFileName => Name.EqualsIgnoreCase("index") ? "IndexModelType" : (string)Name;

        public IModelType AdditionalProperties { get; set; }

        /// <summary>
        /// Gets or sets the discriminator property for polymorphic types.
        /// </summary>
        public override string PolymorphicDiscriminator
        {
            get { return base.PolymorphicDiscriminator; }
            set
            {
                base.PolymorphicDiscriminator = value;
                AddPolymorphicPropertyIfNecessary();
            }
        }

        public string AdditionalPropertiesTSType()
        {
            string result = "any";
            if (AdditionalProperties != null)
            {
                var type = AdditionalProperties.TSType(true);
                result = type != "any" ? $"{type} | any" : "any";
            }
            return result;
        }

        public string AdditionalPropertiesDocumentation()
        {
            string result = "Describes unknown properties. ";
            if (AdditionalProperties != null)
            {
                var type = AdditionalProperties.TSType(true);
                if (type != "any")
                {
                    result += $"The value of an unknown property MUST be of type \"{type}\". Due to valid TS constraints " +
                        $"we have modeled this as a union of `{type} | any`.";
                }
                else
                {
                    result += "The value of an unknown property can be of \"any\" type.";
                }
            }
            return result;
        }

        /// <summary>
        /// If PolymorphicDiscriminator is set, makes sure we have a PolymorphicDiscriminator property.
        /// </summary>
        private void AddPolymorphicPropertyIfNecessary()
        {
            if (!string.IsNullOrEmpty(PolymorphicDiscriminator) &&
                Properties.All(p => p.Name != PolymorphicDiscriminator))
            {
                base.Add(New<Core.Model.Property>(new
                {
                    IsRequired = true,
                    Name = this.PolymorphicDiscriminator,
                    SerializedName = this.PolymorphicDiscriminator,
                    Documentation = "Polymorphic Discriminator",
                    ModelType = New<PrimaryType>(KnownPrimaryType.String)
                }));
            }
        }

        private class PropertyWrapper
        {
            public Core.Model.Property Property { get; set; }
            public List<string> RecursiveTypes { get; set; }

            public PropertyWrapper() { RecursiveTypes = new List<string>(); }
        }

        public IEnumerable<Core.Model.Property> DocumentationPropertyList
        {
            get
            {
                var traversalStack = new Stack<PropertyWrapper>();
                var visitedHash = new Dictionary<string, PropertyWrapper>();
                var retValue = new Stack<Core.Model.Property>();

                foreach (var property in Properties.Where(p => !p.IsConstant))
                {
                    var tempWrapper = new PropertyWrapper()
                    {
                        Property = property,
                        RecursiveTypes = new List<string> () { Name }
                    };
                    traversalStack.Push(tempWrapper);
                }

                while (traversalStack.Count() != 0)
                {
                    var wrapper = traversalStack.Pop();
                    if (wrapper.Property.ModelType is CompositeType)
                    {
                        if (!visitedHash.ContainsKey(wrapper.Property.Name))
                        {
                            if (wrapper.RecursiveTypes.Contains(wrapper.Property.ModelType.Name))
                            {
                                retValue.Push(wrapper.Property);
                            }
                            else
                            {
                                traversalStack.Push(wrapper);
                                foreach (var subProperty in ((CompositeType)wrapper.Property.ModelType).Properties)
                                {
                                    if (subProperty.IsConstant)
                                    {
                                        continue;
                                    }
                                    var individualProperty = New<Core.Model.Property>();
                                    // used FixedValue to force the string
                                    individualProperty.Name.FixedValue = wrapper.Property.Name + "." + subProperty.Name;
                                    individualProperty.ModelType = subProperty.ModelType;
                                    individualProperty.Documentation = subProperty.Documentation;
                                    //Adding the parent type to recursive list
                                    var recursiveList = new List<string>() { wrapper.Property.ModelType.Name };
                                    if (subProperty.ModelType is CompositeType)
                                    {
                                        //Adding parent's recursive types to the list as well
                                        recursiveList.AddRange(wrapper.RecursiveTypes);
                                    }
                                    var subPropertyWrapper = new PropertyWrapper()
                                    {
                                        Property = individualProperty,
                                        RecursiveTypes = recursiveList
                                    };

                                    traversalStack.Push(subPropertyWrapper);
                                }
                            }

                            visitedHash.Add(wrapper.Property.Name, wrapper);
                        }
                        else
                        {
                            retValue.Push(wrapper.Property);
                        }
                    }
                    else
                    {
                        retValue.Push(wrapper.Property);
                    }
                }

                return retValue.ToList();
            }
        }

        public static string ConstructPropertyDocumentation(string propertyDocumentation)
        {
            var builder = new IndentedStringBuilder("  ");
            return builder.AppendLine(propertyDocumentation).ToString();
        }

        private bool ContainsCompositeType(IModelType type)
        {
            bool result = false;
            //base condition
            if (type is CompositeType ||
                type is Core.Model.SequenceType && (type as Core.Model.SequenceType).ElementType is CompositeType ||
                type is Core.Model.DictionaryType && (type as Core.Model.DictionaryType).ValueType is CompositeType)
            {
                result = true;
            }
            else if (type is Core.Model.SequenceType)
            {
                result = ContainsCompositeType((type as Core.Model.SequenceType).ElementType);
            }
            else if (type is Core.Model.DictionaryType)
            {
                result = ContainsCompositeType((type as Core.Model.DictionaryType).ValueType);
            }
            return result;
        }

        public virtual string ConstructModelMapper()
        {
            var modelMapper = this.ConstructMapper(SerializedName, null, false, true);
            var builder = new IndentedStringBuilder("  ");
            builder.AppendLine("return {{{0}}};", modelMapper);
            return builder.ToString();
        }

        /// <summary>
        /// Provides the property name in the correct jsdoc notation depending on
        /// whether it is required or optional
        /// </summary>
        /// <param name="property">Parameter to be documented</param>
        /// <returns>Parameter name in the correct jsdoc notation</returns>
        public static string GetPropertyDocumentationName(Core.Model.Property property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.IsRequired ? (string) property.Name : $"[{property.Name}]";
        }

        /// <summary>
        /// Provides the property documentation string along with default value if any.
        /// </summary>
        /// <param name="property">Parameter to be documented</param>
        /// <returns>Parameter documentation string along with default value if any
        /// in correct jsdoc notation</returns>
        public static string GetPropertyDocumentationString(Core.Model.Property property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return property.DefaultValue.IsNullOrEmpty() ?
                $"{property.Summary.EnsureEndsWith(".")} {property.Documentation}".Trim() :
                $"{property.Summary.EnsureEndsWith(".")} {property.Documentation.EnsureEndsWith(".")} Default value: {property.DefaultValue} .".Trim();
        }

        /// <summary>
        /// Provides the type of the property
        /// </summary>
        /// <param name="property">Parameter to be documented</param>
        /// <returns>Parameter name in the correct jsdoc notation</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static string GetPropertyDocumentationType(Core.Model.Property property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            string typeName = "object";
            if (property.ModelType is PrimaryTypeJs)
            {
                typeName = property.ModelType.Name;
            }
            else if (property.ModelType is Core.Model.SequenceType)
            {
                typeName = "array";
            }
            else if (property.ModelType is EnumType)
            {
                typeName = "string";
            }

            return typeName.ToLowerInvariant();
        }

        public string GenerateModelImports(string emptyLine)
        {
            StringBuilder builder = new StringBuilder();

            bool addEmptyLine = false;

            if (BaseModelType != null)
            {
                builder.AppendLine("const models = require('./index');");
                addEmptyLine = true;
            }

            if (ContainsTimeSpanPropertyWithValue())
            {
                builder.AppendLine("const moment = require('moment');");
                addEmptyLine = true;
            }

            if (addEmptyLine)
            {
                builder.AppendLine(emptyLine);
            }

            return builder.ToString();
        }

        public string GenerateModelDefinition()
        {
            TSBuilder builder = new TSBuilder();

            GenerateModelDefinition(builder);

            return builder.ToString();
        }

        public virtual void GenerateModelDefinition(TSBuilder builder)
        {
            builder.DocumentationComment(comment =>
            {
                comment.Summary(Summary);
                comment.Description(Documentation);
            });
            ISet<string> addedPropertyNames = new HashSet<string>();
            builder.ExportInterface(Name, BaseModelType?.Name?.ToString(), tsInterface =>
            {
                foreach (Property property in Properties.Where(p => !p.IsConstant))
                {
                    if (!addedPropertyNames.Contains(property.Name))
                    {
                        addedPropertyNames.Add(property.Name);

                        tsInterface.DocumentationComment(comment =>
                        {
                            comment.Summary(property.Summary);
                            comment.Description(property.Documentation);
                        });
                        string propertyType = property.ModelType.TSType(inModelsModule: true);
                        tsInterface.Property(property.Name, propertyType, isRequired: property.IsRequired, isReadonly: property.IsReadOnly);
                    }
                }

                if (AdditionalProperties != null)
                {
                    tsInterface.DocumentationComment(AdditionalPropertiesDocumentation());
                    tsInterface.Indexer("additionalPropertyName", AdditionalPropertiesTSType());
                }
            });
        }
    }
}