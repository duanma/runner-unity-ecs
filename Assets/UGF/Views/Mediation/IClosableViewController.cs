﻿using System;
using UniRx;

namespace UGF.Views.Mediation
{
    public interface IClosableViewController
    {
        void Open();
        void Close();

        bool IsOpen { get; }

        IObservable<Unit> OnViewOpen { get; }
        IObservable<Unit> OnViewOpenCompleted { get; }

        IObservable<Unit> OnViewClose { get; }
        IObservable<Unit> OnViewCloseCompleted { get; }
    }
}
