using System;
using UniRx;

namespace Glidrive.Manager
{
    public enum SceneStateType
    {
        Title,          // タイトル
        Main,           // メイン
        Result          // リザルト
    }

    [Serializable]
    public class SceneStateReactiveProperty : ReactiveProperty<SceneStateType>
    {
        public SceneStateReactiveProperty()
        {
        }

        public SceneStateReactiveProperty(SceneStateType initialValue) : base(initialValue)
        {
        }
    }
}
