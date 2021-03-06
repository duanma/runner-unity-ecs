﻿using System;
using DG.Tweening;
using UnityEngine;

namespace UGF.Views.Closable.AnimationStrategies
{
    public class PopOutAnimationStrategy : IIClosableViewAnimationStrategy
    {
        private const float TransitionTimeSeconds = 0.2f;

        private readonly Transform _target;
        private readonly Vector3 _originalScale;

        public PopOutAnimationStrategy(Transform target)
        {
            _target = target;
            _originalScale = _target.localScale;
        }

        public void Open(Action onComplete)
        {
            _target.gameObject.SetActive(true);
            _target.localScale = Vector3.zero;

            DOTween.Sequence()
                .Append(_target
                    .DOScale(_originalScale, TransitionTimeSeconds)
                    .SetEase(Ease.InOutCubic))
                .AppendCallback(() => onComplete?.Invoke());
        }

        public void Close(Action onComplete)
        {
            DOTween.Sequence()
                .Append(_target
                    .DOScale(Vector3.zero, TransitionTimeSeconds)
                    .SetEase(Ease.InOutCubic))
                .AppendCallback(() => _target.gameObject.SetActive(false))
                .AppendCallback(() => onComplete?.Invoke());
        }
    }
}
