Shader "Water/Simulation"
{

Properties
{
    _S2("PhaseVelocity^2", Range(0.0, 0.5)) = 0.2
    [PowerSlider(0.01)]
    _Atten("Attenuation", Range(0.0, 1.0)) = 0.999
    _DeltaUV("Delta UV", Float) = 3
    _addTextuer("Addtex", 2D) = "black" {}
    _gain ("Gain", Range(0.0, 2)) = 0
    _def ("def", Range(0.0, 1)) = 0
    _speed ("_speed", Range(1, 10)) = 1
    
}
SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            // インスペクタに表示したときにわかりやすいように名前を付けておく
            Name "Update"

            CGPROGRAM
            
            // UnityCustomRenderTexture.cgincをインクルードする
           #include "UnityCustomRenderTexture.cginc"

            // 頂点シェーダは決まったものを使う
           #pragma vertex CustomRenderTextureVertexShader
           #pragma fragment frag
           
           sampler2D _addTextuer;
           float4 _addTextuer_ST;
           float _gain;
           float _def;
           float _speed;
           
            // v2f構造体は決まったものを使う
            half4 frag(v2f_customrendertexture i) : SV_Target
            {
                float tw = 1 / _CustomRenderTextureWidth;

                // UVはこのように取得する
                float2 uv = i.globalTexcoord;
                
                float4 col = tex2D(_addTextuer, uv);
                // _SelfTexture2Dで前フレームの結果を取得する
                float4 colful = tex2D(_SelfTexture2D, uv + half2(0, -tw)*_speed);
                colful.rgb *= _def;
                colful.rgb =  lerp(col.rgb,colful.rgb,col.a);
                //colful.rgb *= _def;
                colful.rgb = colful.g;
                return colful;
            }

            ENDCG
        }
    }

}