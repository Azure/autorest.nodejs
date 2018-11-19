// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.NodeJS.Model;
using System.Collections.Generic;

namespace AutoRest.NodeJS
{
    public static class Models
    {
        public static CodeModelJs CodeModel(IEnumerable<CompositeTypeJs> modelTypes = null)
        {
            CodeModelJs codeModel = DependencyInjection.New<CodeModelJs>();

            if (modelTypes != null)
            {
                foreach (CompositeTypeJs modelType in modelTypes)
                {
                    codeModel.Add(modelType);
                }
            }

            return codeModel;
        }

        public static MethodGroupJs MethodGroup(CodeModelJs codeModel = null, IEnumerable<MethodJs> methods = null)
        {
            if (codeModel == null)
            {
                codeModel = CodeModel();
            }

            MethodGroupJs methodGroup = DependencyInjection.New<MethodGroupJs>();
            codeModel.Add(methodGroup);

            if (methods != null)
            {
                foreach (MethodJs method in methods)
                {
                    methodGroup.Add(method);
                }
            }

            return methodGroup;
        }

        public static MethodJs Method(
            HttpMethod httpMethod = HttpMethod.Get,
            string requestContentType = null,
            CodeModelJs codeModel = null,
            MethodGroupJs methodGroup = null,
            Response defaultResponse = null,
            IEnumerable<ParameterJs> parameters = null,
            string deprecatedMessage = null)
        {
            if (codeModel == null)
            {
                codeModel = CodeModel();
            }

            MethodJs method = DependencyInjection.New<MethodJs>();
            if (methodGroup == null)
            {
                methodGroup = MethodGroup(codeModel);
            }
            method.MethodGroup = methodGroup;

            codeModel.Add(method);

            method.HttpMethod = httpMethod;
            method.RequestContentType = requestContentType;

            method.DefaultResponse = defaultResponse;

            if (parameters != null)
            {
                foreach (ParameterJs parameter in parameters)
                {
                    method.Add(parameter);
                }
            }

            method.DeprecationMessage = deprecatedMessage;

            return method;
        }

        public static ParameterJs Parameter(string name = null, ParameterLocation location = ParameterLocation.None)
        {
            ParameterJs parameter = DependencyInjection.New<ParameterJs>();

            parameter.Name = name;
            parameter.Location = location;

            return parameter;
        }

        public static Response Response(IModelType body = null)
        {
            Response response = new Response();

            response.Body = body;

            return response;
        }

        public static CompositeTypeJs CompositeType(string name = null, IEnumerable<PropertyJs> properties = null, string xmlPrefix = null, string xmlName = null)
        {
            CompositeTypeJs compositeType = new CompositeTypeJs(name);

            if (!string.IsNullOrEmpty(xmlPrefix))
            {
                compositeType.XmlProperties = new XmlProperties
                {
                    Name = xmlName,
                    Prefix = xmlPrefix,
                };
            }

            if (properties != null)
            {
                foreach (PropertyJs property in properties)
                {
                    compositeType.Add(property);
                }
            }

            return compositeType;
        }

        public static PropertyJs Property(string name = null, IModelType type = null, string xmlPrefix = null, string xmlName = null)
        {
            PropertyJs property = DependencyInjection.New<PropertyJs>();

            property.Name = name;
            property.ModelType = type;

            if (!string.IsNullOrEmpty(xmlPrefix) || !string.IsNullOrEmpty(xmlName))
            {
                property.XmlProperties = new XmlProperties
                {
                    Name = xmlName,
                    Prefix = xmlPrefix,
                };
            }

            return property;
        }
    }
}
