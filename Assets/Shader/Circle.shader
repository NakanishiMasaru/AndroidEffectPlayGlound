Shader "Hidden/Circle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _x("x",float) = 0
        _y("y",float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define PI 3.14159265358979323844

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _x;
            float _y;

            half2 ConvertPolarCordinate(half2 uv, half x, half y) {
                const half PI2THETA = 1 / (3.1415926535 * 2);
                half2 res;

                // UV値を極座標系に変換
                uv = 2 * uv - 1;
                half r = 1 - sqrt(uv.x * uv.x + uv.y * uv.y);
                half theta = atan2(uv.y, uv.x) * PI2THETA; //　0.75はPhotoShopの変換に合わせた、回転の始軸の調整

                // スクロールのための処理
                res.y = r * y;
                res.x = theta * x;
                return res;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float2 uv = ConvertPolarCordinate(i.uv, _x, _y);

                fixed4 col = tex2D(_MainTex, uv);

                return col;
            }
            ENDCG
        }
    }
}
