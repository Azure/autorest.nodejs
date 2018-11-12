// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
//

using System;

namespace AutoRest.NodeJS.DSL
{
    public class TSInterface
    {
        private readonly TSBuilder builder;

        public TSInterface(TSBuilder builder)
        {
            this.builder = builder;
        }

        public void DocumentationComment(Action<TSDocumentationComment> action)
        {
            builder.DocumentationComment(action);
        }

        public void DocumentationComment(params string[] lines)
        {
            builder.DocumentationComment(lines);
        }

        public void Property(string propertyName, string propertyType, bool isRequired = true, bool isReadonly = false)
        {
            builder.Property(propertyName, propertyType, required: isRequired, accessModifier: isReadonly ? "readonly" : "");
        }

        public void Indexer(string keyName, string valueType)
        {
            builder.Indexer(keyName, valueType);
        }
    }
}
