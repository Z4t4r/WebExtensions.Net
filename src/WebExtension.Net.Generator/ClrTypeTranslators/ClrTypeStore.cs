﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using WebExtension.Net.Generator.Extensions;
using WebExtension.Net.Generator.Models.ClrTypes;
using WebExtension.Net.Generator.Models.Entities;
using WebExtension.Net.Generator.Models.Schema;

namespace WebExtension.Net.Generator.ClrTypeTranslators
{
    public class ClrTypeStore
    {
        private readonly IDictionary<string, ClrTypeInfo> clrTypeStore;

        public ClrTypeStore()
        {
            clrTypeStore = new Dictionary<string, ClrTypeInfo>();
        }

        public void Reset()
        {
            clrTypeStore.Clear();
            CreateClrTypeFromSystemType(typeof(bool));
            CreateClrTypeFromSystemType(typeof(int));
            CreateClrTypeFromSystemType(typeof(double));
            CreateClrTypeFromSystemType(typeof(string));
            CreateClrTypeFromSystemType(typeof(object));
        }

        public void AddClrType(ClrTypeInfo clrTypeInfo)
        {
            clrTypeStore.Add(clrTypeInfo.Id, clrTypeInfo);
        }

        public ClrTypeInfo GetClrType(TypeReference? typeReference, NamespaceEntity namespaceEntity)
        {
            if (typeReference is null)
            {
                throw new ArgumentNullException(nameof(typeReference));
            }

            if (typeReference.Type == ObjectType.Array)
            {
                var arrayItemType = GetClrType(typeReference.ArrayItems, namespaceEntity);
                return arrayItemType.MakeEnumerableType();
            }

            if (typeReference.Type == ObjectType.Null && typeReference.Ref is null && typeReference.TypeChoices is null)
            {
                return GetClrType(typeof(void));
            }

            if (typeReference.Ref is null && typeReference.TypeChoices is not null)
            {
                return GetClrType(typeof(JsonElement));
            }

            if (typeReference.Type == ObjectType.Function)
            {
                return GetFunctionClrType(typeReference, namespaceEntity);
            }

            var typeId = GetTypeId(typeReference, namespaceEntity);

            if (!clrTypeStore.ContainsKey(typeId))
            {
                throw new InvalidOperationException($"Type id '{typeId}' is not defined in the CLR types store.");
            }

            return clrTypeStore[typeId];
        }

        private ClrTypeInfo GetClrType(Type type)
        {
            if (type.FullName is null)
            {
                throw new InvalidOperationException("Type full name must not be null.");
            }

            if (!clrTypeStore.ContainsKey(type.FullName))
            {
                CreateClrTypeFromSystemType(type);
            }

            return clrTypeStore[type.FullName];
        }

        private void CreateClrTypeFromSystemType(Type type)
        {
            if (type.FullName is null)
            {
                throw new InvalidOperationException("Type full name must not be null.");
            }
            var clrTypeInfo = GetClrTypeFromSystemType(type);
            clrTypeStore.Add(type.FullName, clrTypeInfo);
        }

        private ClrTypeInfo GetClrTypeFromSystemType(Type type)
        {
#pragma warning disable CS8601, CS8604 // Type should not have these properties as null
            var clrTypeInfo = new ClrTypeInfo()
            {
                Id = type.FullName,
                Namespace = type.Namespace,
                Name = type.Name,
                FullName = type.FullName,
                CSharpName = type.Name,
                IsEnum = type.IsEnum,
                EnumValues = null,
                IsNullable = true,
                IsInterface = type.IsInterface,
                IsNullType = type == typeof(void),
                IsGenericType = type.IsGenericType,
                GenericTypeArguments = type.GenericTypeArguments.Select(GetClrTypeFromSystemType).ToArray(),
                IsObsolete = false,
                ObsoleteMessage = null,
                IsGenerated = false,
                IsPartial = false,
                GeneratedNamespace = null,
                RequiredNamespaces = new HashSet<string>(),
                ReferenceNamespaces = new HashSet<string>() { type.Namespace },
                Interfaces = new HashSet<string>(),
                BaseTypeName = null,
                Description = null,
                Metadata = new Dictionary<string, object>(),
                Methods = Enumerable.Empty<ClrMethodInfo>(),
                Properties = Enumerable.Empty<ClrPropertyInfo>(),
                TypeChoices = null
            };
#pragma warning restore CS8601, CS8604

            if (type.IsGenericType)
            {
                clrTypeInfo.CSharpName = clrTypeInfo.CSharpName[..clrTypeInfo.CSharpName.IndexOf('`')];
            }

            switch (type.FullName)
            {
                case "System.Boolean":
                    clrTypeInfo.CSharpName = "bool";
                    clrTypeInfo.ReferenceNamespaces.Remove(type.Namespace);
                    clrTypeInfo.IsNullable = false;
                    break;
                case "System.Int32":
                    clrTypeInfo.CSharpName = "int";
                    clrTypeInfo.ReferenceNamespaces.Remove(type.Namespace);
                    clrTypeInfo.IsNullable = false;
                    break;
                case "System.Double":
                    clrTypeInfo.CSharpName = "double";
                    clrTypeInfo.ReferenceNamespaces.Remove(type.Namespace);
                    clrTypeInfo.IsNullable = false;
                    break;
                case "System.String":
                    clrTypeInfo.CSharpName = "string";
                    clrTypeInfo.ReferenceNamespaces.Remove(type.Namespace);
                    break;
                case "System.Object":
                    clrTypeInfo.CSharpName = "object";
                    clrTypeInfo.ReferenceNamespaces.Remove(type.Namespace);
                    break;
                case "System.Void":
                    clrTypeInfo.CSharpName = "void";
                    clrTypeInfo.ReferenceNamespaces.Remove(type.Namespace);
                    break;
            }

            return clrTypeInfo;
        }

        private ClrTypeInfo GetFunctionClrType(TypeReference typeReference, NamespaceEntity namespaceEntity)
        {
            if ((typeReference.FunctionParameters is null || !typeReference.FunctionParameters.Any()) && typeReference.FunctionReturns is null)
            {
                return GetClrType(typeof(Action));
            }

            var functionType = typeof(Action<>);
            var genericTypeArguments = new List<ClrTypeInfo>();

            if (typeReference.FunctionParameters is not null)
            {
                var parameterTypes = typeReference.FunctionParameters.Select(parameterDefinition =>
                {
                    var parameterType = GetClrType(parameterDefinition, namespaceEntity);
                    if (parameterDefinition.IsOptional && !parameterType.IsNullable)
                    {
                        parameterType = parameterType.MakeNullable();
                    }

                    return parameterType;
                });
                genericTypeArguments.AddRange(parameterTypes);
            }

            if (typeReference.FunctionReturns is not null)
            {
                var functionReturnClrType = GetClrType(typeReference.FunctionReturns, namespaceEntity);
                if (functionReturnClrType.FullName != typeof(void).FullName)
                {
                    functionType = typeof(Func<>);
                    genericTypeArguments.Add(functionReturnClrType);
                }
            }

            var clrType = GetClrTypeFromSystemType(functionType);
            clrType.GenericTypeArguments = genericTypeArguments;
            clrType.CSharpName = $"{clrType.CSharpName}<{string.Join(", ", genericTypeArguments.Select(genericTypeArgument => genericTypeArgument.CSharpName))}>";
            return clrType;
        }

        private string GetTypeId(TypeReference? typeReference, NamespaceEntity namespaceEntity)
        {
            if (typeReference is null)
            {
                throw new ArgumentNullException(nameof(typeReference));
            }

            if (typeReference.Ref is not null)
            {
                if (typeReference.Ref.Contains("."))
                {
                    return $"{Constants.RelativeNamespaceToken}.{typeReference.Ref.ToCapitalCase()}";
                }
                return $"{Constants.RelativeNamespaceToken}.{namespaceEntity.FormattedName}.{typeReference.Ref.ToCapitalCase()}";
            }

            return typeReference.Type switch
            {
                ObjectType.Array => GetTypeId(typeReference.ArrayItems, namespaceEntity) + "Array",
                ObjectType.Boolean => "System.Boolean",
                ObjectType.Function => throw new InvalidOperationException($"Functions should be handled by the method '{nameof(GetFunctionClrType)}'."),
                ObjectType.Integer => "System.Int32",
                ObjectType.Number => "System.Double",
                ObjectType.String => "System.String",
                _ => "System.Object"
            };
        }
    }
}
