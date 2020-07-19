using UnityEngine;

namespace Script.Texture.Texture
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
                rend = GetComponent<Renderer>();
                m_texture = Instantiate((Texture2D) rend.material.mainTexture);
        }

        // Update is called once per frame
        void Update()
        {
            SetTex();
        }

        void SetTex()
        {
            _colors = m_texture.GetPixels();
            var len = m_texture.texelSize.x;
            m_texture.filterMode = FilterMode.Point;
            for (int i = 0; i < spectrum.Length; i++)
            {
                _colors.SetValue(new Color(spectrum[i]*gain,spectrum[i]*gain,spectrum[i]*gain), i);
            }
            m_texture.SetPixels(_colors);
            m_texture.Apply();
            rend.material.mainTexture = m_texture;
        }
    }
}
