﻿using System;
using WebExtensions.Net.Generator.Extensions;
using WebExtensions.Net.Generator.Models;
using WebExtensions.Net.Generator.Models.ClrTypes;
using WebExtensions.Net.Generator.Models.Entities;
using WebExtensions.Net.Generator.Models.Schema;

namespace WebExtensions.Net.Generator.ClrTypeTranslators
{
    public class PropertyDefinitionTranslator
    {
        private readonly ClrTypeStore clrTypeStore;

        public PropertyDefinitionTranslator(ClrTypeStore clrTypeStore)
        {
            this.clrTypeStore = clrTypeStore;
        }

        public ClrPropertyInfo TranslatePropertyDefinition(string propertyName, PropertyDefinition propertyDefinition, NamespaceEntity namespaceEntity, ClrTypeInfo clrTypeInfo)
        {
            var propertyType = clrTypeStore.GetClrType(propertyDefinition, namespaceEntity);
            
            if (clrTypeInfo.Metadata.TryGetValue(Constants.TypeMetadata.ClassType, out var classType) &&
                (ClassType)classType == ClassType.CombinedCallbackParameterClass &&
                propertyType.FullName == typeof(object).FullName)
            {
                propertyType = propertyType.MakeJsonElement();
            }

            if (propertyDefinition.IsOptional && !propertyType.IsNullable)
            {
                propertyType = propertyType.MakeNullable();
            }

            clrTypeInfo.AddRequiredNamespaces(propertyType.ReferenceNamespaces);

            if (propertyName.Equals(clrTypeInfo.CSharpName, StringComparison.OrdinalIgnoreCase))
            {
                // Property name cannot be the same as declaring type, prefer to change the type name instead of the property name
                clrTypeInfo.CSharpName = $"{clrTypeInfo.CSharpName}Type";
            }

            return new ClrPropertyInfo()
            {
                Name = propertyName,
                PrivateName = propertyName.ToCamelCase(),
                PublicName = propertyName.ToCapitalCase(),
                Description = propertyDefinition.Description,
                DeclaringType = clrTypeInfo,
                PropertyType = propertyType,
                IsConstant = propertyDefinition.IsConstant,
                ConstantValue = propertyDefinition.ConstantValue,
                IsObsolete = propertyDefinition.IsDeprecated,
                ObsoleteMessage = propertyDefinition.Deprecated
            };
        }
    }
}
