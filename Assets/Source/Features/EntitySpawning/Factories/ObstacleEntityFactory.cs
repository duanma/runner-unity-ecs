﻿using Source.Entities.Components;
using Source.Features.EntitySpawning.Config;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace Source.Features.EntitySpawning.Factories
{
    public class ObstacleEntityFactory : AbstractEntityFactory, IEntityFactory
    {
        private readonly ObstacleEntityConfig _obstacleEntityConfig;
        private readonly EntityArchetype _entityArchetype;

        public ObstacleEntityFactory(ObstacleEntityConfig obstacleEntityConfig)
        {
            _obstacleEntityConfig = obstacleEntityConfig;
            _entityArchetype = EntityManager.CreateArchetype(
                typeof(Translation),
                typeof(LocalToWorld),
                typeof(Rotation),
                typeof(RenderMesh));
        }

        public Entity CreateEntityAt(float3 spawnPosition)
        {
            var entity = EntityManager.CreateEntity(_entityArchetype);

            EntityManager.SetComponentData(entity, new Translation
            {
                Value = spawnPosition
            });
            
            EntityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = _obstacleEntityConfig.EntityMesh,
                material = _obstacleEntityConfig.EntityMaterial
            });

            return entity;
        }
    }
}
