using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public interface IGimmickNumEventProvider
{
    IReadOnlyReactiveProperty<int> GimmickNum { get; }
}
