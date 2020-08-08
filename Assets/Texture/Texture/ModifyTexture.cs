using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Texture.Texture
{
    public class ModifyTexture : MonoBehaviour
    {
        [SerializeField] private Texture2D m_texture;
        private Color[] _colors;
        public float[] spectrum;
        private Renderer rend;
        [Range(0,500000)]
        [SerializeField] private float gain = 1f;
        
        // Start is called before the first frame update
        void Start()
        {    
                //rend = GetComponent<Renderer>();
                //m_texture = (Texture2D) rend.material.mainTexture;
                SetTex(this.GetCancellationTokenOnDestroy()).Forget();
        }


        async UniTask SetTex(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Yield( PlayerLoopTiming.Update, cancellationToken );  
                _colors = m_texture.GetPixels();
                var len = m_texture.texelSize.x;
                m_texture.filterMode = FilterMode.Point;
                for (int i = 0; i < spectrum.Length-2; i++)
                {
                    _colors.SetValue(new Color(spectrum[i]*gain,spectrum[i]*gain,spectrum[i]*gain,spectrum[i]*gain), i);
                }
                m_texture.SetPixels(_colors);
                m_texture.Apply();
            }
            
        }

    }
}
