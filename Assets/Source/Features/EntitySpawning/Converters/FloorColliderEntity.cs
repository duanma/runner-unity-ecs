﻿using Source.Entities.Components;
using Unity.Entities;
using UnityEngine;

namespace Source.Features.EntitySpawning.Converters
{
    public class FloorColliderEntity : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
        {
            entityManager.AddComponent<FollowEntity>(entity);
        }
    }
}
