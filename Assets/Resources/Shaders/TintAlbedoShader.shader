Shader "Custom/3D/OverrideTintShader"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _OverrideColor ("Override Color", Color) = (1,1,1,1)
        _OverrideAmount ("Override Amount", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;
        fixed4 _TintColor;
        fixed4 _OverrideColor;
        float _OverrideAmount;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            fixed3 tinted = tex.rgb * _TintColor.rgb;
            o.Albedo = lerp(tinted, _OverrideColor.rgb, _OverrideAmount);
            o.Alpha = tex.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
