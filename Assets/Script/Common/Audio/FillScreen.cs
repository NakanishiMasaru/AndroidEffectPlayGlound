using UnityEngine;

namespace Script.Common.Audio
{
    public class FillScreen : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (Camera.main != null)
            {
                var main = Camera.main;
                var worldScreenHeight = main.orthographicSize * 2f;
                var worldScreenWidth=worldScreenHeight/Screen.height*Screen.width;
                var tr = transform;
                var localScale = tr.localScale;
                var width  = localScale.x;
                var height = localScale.y;
                tr.localScale = new Vector3 (worldScreenWidth / width, worldScreenHeight / height);
                Vector3 camPos = main.transform.position;
                camPos.z = 0;
                tr.position = camPos;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
