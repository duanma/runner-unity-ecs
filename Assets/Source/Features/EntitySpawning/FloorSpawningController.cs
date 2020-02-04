﻿using Source.Entities.Config;
using Source.Features.DataBridge;
using Source.Features.ScreenSize;
using System;
using UGF.Util.UniRx;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Source.Features.EntitySpawning
{
    public class FloorSpawningController : AbstractDisposable, IInitializable
    {
        private readonly IFloorSpawner _floorSpawner;
        private readonly ScreenSizeModel _screenSizeModel;
        private readonly ScreenSizeController _screenSizeController;

        private readonly Vector3 _floorTileSize;
        private float FloorTileHalfSizeX => _floorTileSize.x / 2f;
        private float FloorTileHalfSizeY => _floorTileSize.y / 2f;

        private float3 _lastFloorTileSpawnPosition;
        private float3 _lastRecordedPlayerPosition;

        public FloorSpawningController(
            IFloorSpawner floorSpawner,
            EntityConfig entityConfig,
            ScreenSizeModel screenSizeModel,
            ScreenSizeController screenSizeController)
        {
            _floorSpawner = floorSpawner;
            _screenSizeModel = screenSizeModel;
            _screenSizeController = screenSizeController;

            _floorTileSize = entityConfig.GetEntitySetting(EntityType.Floor)
                .EntityMesh
                .bounds
                .size;
        }

        public void Initialize()
        {
            FillFloor();

            Blackboard.TryGet(
                BlackboardEntryId.PlayerSpawnPosition,
                out _lastRecordedPlayerPosition);

            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            if (!Blackboard.TryGet(BlackboardEntryId.PlayerPosition, out float3 position))
            {
                return;
            }

            var distanceTraveled = Math.Abs(position.x - _lastRecordedPlayerPosition.x);
            if (distanceTraveled >= _floorTileSize.x)
            {
                SpawnNext();
                _lastRecordedPlayerPosition = position;
            }
        }

        private void FillFloor()
        {
            var startPosition = _screenSizeController.GetBottomLeftCorner(
                FloorTileHalfSizeX,
                FloorTileHalfSizeY);

            var minimumTileCount = Math.Ceiling(_screenSizeModel.WidthUnits / _floorTileSize.x);
            var paddedTileCount = (int)minimumTileCount + 1;

            UGF.Logger.Log($"Creating {paddedTileCount} floor tiles at start");

            for (var i = 0; i < paddedTileCount; i++)
            {
                var sizeOffset = i * _floorTileSize.x;

                _lastFloorTileSpawnPosition = new Vector3(
                    startPosition.x + sizeOffset,
                    startPosition.y,
                    0);

                _floorSpawner.SpawnFloorTileAt(_lastFloorTileSpawnPosition);
            }
        }

        private void SpawnNext()
        {
            var spawnPosition = new Vector3(
                _lastFloorTileSpawnPosition.x + _floorTileSize.x,
                _lastFloorTileSpawnPosition.y,
                0);

            _floorSpawner.SpawnFloorTileAt(spawnPosition);
            _lastFloorTileSpawnPosition = spawnPosition;
        }
    }
}
