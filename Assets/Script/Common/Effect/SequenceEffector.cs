using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading;
using UnityEngine.Playables;

namespace Script.Common.Effect
{
    //未だ加速できない
    //manualUpdate 再生できない
    //とりあえず再生と停止はOK
    public class SequenceEffector : MonoBehaviour
    {
        private PlayableDirector _director;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private bool _manualUpdate = false;
        private DirectorUpdateMode firstMode = default;

        private void Awake()
        {
            _director = GetComponent<PlayableDirector>();
            _manualUpdate = false;
            if (_director.enabled) _director.enabled = false;
            firstMode = _director.timeUpdateMode;
        }

        /// <summary>
        /// 終了時間指定で再生したい時、現在からの秒数
        /// </summary>
        /// <param name="sec"></param>
        public float GetBlendSpeedByTimer(float sec)
        {
            float deltaSpeed = 0f;
            
            return deltaSpeed;
        }
        
        /// <summary>
        /// Play PlayableDirector
        /// </summary>
        /// <param name="deltaSpeed"> 速度設定が必要な場合、相対速度を指定できます。初期値 1f</param>
        /// <returns></returns>
        public async UniTask Play(float deltaSpeed = 1f)
        {
            _director.enabled = true;
            _director.Play();
            InitTask();
            if (Math.Abs(deltaSpeed - 1f) > 0f) _manualUpdate = true;
            await InspectTimeline(_cancellationToken);
            Debug.Log("Finish");
        }

        /// <summary>
        /// Taskのキャンセル処理周りの初期化
        /// </summary>
        private void InitTask()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }
        private void DisposeTask()
        {
            _cancellationTokenSource?.Cancel();
            Debug.Log("dispose Task");
        }


        /// <summary>
        /// Stop PlayableDirector
        /// 現在状態を破棄、Timelineを終了状態で待機させます
        /// </summary>
        [ContextMenu("Stop")]
        public void Stop()
        {
            if (_director.extrapolationMode != DirectorWrapMode.Loop)
            {
                var endTime = _director.duration;
                if (_director.timeUpdateMode == DirectorUpdateMode.Manual)
                {
                    _director.timeUpdateMode = DirectorUpdateMode.Manual;
                    _director.playableGraph.Evaluate((float)endTime);
                    _director.timeUpdateMode = firstMode;
                }
                else
                {
                    _director.time = endTime; 
                }
                
            }
            _director.Pause();
            DisposeTask();
            //Reset();
        }

        /// <summary>
        /// PlayableDirector Playing Speed Up !
        /// 現在の速度を知りたい場合は GetSpeed()を使ってください
        /// </summary>
        /// <param name="deltaSpeed"> 加速するには加速度が必要です</param>
        public void SpeedUp(float deltaSpeed)
        {
            
        }

        /// <summary>
        /// Restart PlayableDirector
        /// シーケンスを破棄し、Taskを再構築します。
        /// </summary>
        /// <returns></returns>
        [ContextMenu("Reset")]
        public void Reset()
        {
            _manualUpdate = false;
            InitTask();
            _director.Stop();
        }
        
        /// <summary>
        /// Timelineの終了タイミングまで監視します
        /// </summary>
        /// <param name="cancellationToken">シーケンス破棄処理時のみ破棄</param>
        /// <returns></returns>
        private async UniTask InspectTimeline(CancellationToken cancellationToken)
        {
            var endTime = _director.duration - 0.01f;
                while (_director.time >= endTime)
            {
                await UniTask.Yield( PlayerLoopTiming.Update, cancellationToken);
            }

        }
        
        [ContextMenu("Play")]
        private void TestPlay()
        {
            Play().Forget();
        }

        [ContextMenu("SpeedUp x2")]
        private void TestSpeed()
        {
            SpeedUp(2f);
        }

    }
}
