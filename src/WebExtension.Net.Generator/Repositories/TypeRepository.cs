﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebExtension.Net.Generator.Extensions;
using WebExtension.Net.Generator.Models.Entities;
using WebExtension.Net.Generator.Models.Schema;

namespace WebExtension.Net.Generator.Repositories
{
    public class TypeRepository : BaseRepository<TypeEntity>
    {
        private readonly IDictionary<string, IList<TypeDefinition>> typeExtensions;

        public TypeRepository()
        {
            typeExtensions = new Dictionary<string, IList<TypeDefinition>>();
        }

        public void RegisterType(string typeId, TypeDefinition typeDefinition, NamespaceEntity namespaceEntity)
        {
            var namespaceQualifiedId = namespaceEntity.GetNamespaceQualifiedId(typeId);
            if (Entities.Any(e => e.NamespaceQualifiedId.Equals(namespaceQualifiedId)))
            {
                throw new InvalidOperationException($"Type entity with id '{namespaceQualifiedId}' already exists.");
            }

            var entity = new TypeEntity(typeId, namespaceQualifiedId, namespaceEntity, typeDefinition);

            if (typeExtensions.ContainsKey(namespaceQualifiedId) && typeExtensions.Remove(namespaceQualifiedId, out var extensions))
            {
                // register all the type extensions that was registered before this type
                foreach (var extension in extensions)
                {
                    entity.Extensions.Add(extension);
                }
            }

            Entities.Add(entity);
        }

        public void RegisterTypeExtension(string typeId, TypeDefinition typeDefinition, NamespaceEntity namespaceEntity)
        {
            var namespaceQualifiedId = namespaceEntity.GetNamespaceQualifiedId(typeId);
            var entity = Entities.SingleOrDefault(e => e.NamespaceQualifiedId.Equals(namespaceQualifiedId));
            if (entity is null)
            {
                // The type to be extended is not yet registered, store the definition in a dictionary first
                if (typeExtensions.ContainsKey(namespaceQualifiedId))
                {
                    typeExtensions[namespaceQualifiedId].Add(typeDefinition);
                }
                else
                {
                    typeExtensions.Add(namespaceQualifiedId, new List<TypeDefinition>() { typeDefinition });
                }
            }
            else
            {
                entity.Extensions.Add(typeDefinition);
            }
        }

        public IEnumerable<TypeEntity> GetAllTypes()
        {
            return Entities;
        }
    }
}
