using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Common.Effect
{
    public class MaterialEffector : MonoBehaviour
    {
        [SerializeField] private Material mat;

        [SerializeField] private float alpha;

        private static readonly int Cutoff = Shader.PropertyToID("_Cutoff");

        // Start is called before the first frame update
        void Start()
        {
            mat = GetComponent<RawImage>().material;
            mat = Instantiate(mat);
            GetComponent<RawImage>().material = mat;
            SetMaterial(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask  SetMaterial(CancellationToken cancellationToken)
        {
            var oldAlpha = alpha;
            while (true)
            {
                await UniTask.Yield( PlayerLoopTiming.Update, cancellationToken );
                if (oldAlpha != alpha)
                {
                    oldAlpha = alpha;
                    mat.SetFloat(Cutoff , oldAlpha);
                }
            }
        }
    }
}
