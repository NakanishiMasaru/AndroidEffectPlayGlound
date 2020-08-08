Shader "ERROR"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Color("BaseColor",Color) = (1,1,1,1)
        _MaskColor("MaskColor",Color) = (1,1,1,1)
        _Cutoff("Cutoff",Range(0,1)) = 0
        _alpha("alpha",Range(0,1)) = 0
    }
    SubShader
    {
        Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Mask;
            float4 _Mask_ST;
            float4 _Color;
            float4 _MaskColor;
            float _Cutoff;
            float _alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 mcol = tex2D(_Mask,i.uv);

                _Cutoff = _Cutoff*1.1;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                //clip(col.a - (_Cutoff * 1.1));
                //if(col.a != 0)col.a = 0.9;
                col.a = smoothstep(_Cutoff,_Cutoff+0.05,col.r * 1.05)* col.a * _alpha;
                col.rgb = _Color.rgb ;
                col.rgb = col.rgb * mcol.a * _MaskColor + col.rgb*(1-mcol.a);
                return col;
            }
            ENDCG
        }
    }
}
