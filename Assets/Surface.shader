Shader "Water/Surface" 
{

Properties
{
    _Color("Color", color) = (1, 1, 1, 0)
    _DispTex("Disp Texture", 2D) = "gray" {}
    _Glossiness ("Smoothness", Range(0,1)) = 0.5
    _Metallic ("Metallic", Range(0,1)) = 0.0
}

SubShader
{

Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

CGPROGRAM

#pragma surface surf Standard alpha addshadow fullforwardshadows
#pragma target 5.0
#include "Tessellation.cginc"

sampler2D _DispTex;
float4 _DispTex_TexelSize;
fixed4 _Color;
half _Glossiness;
half _Metallic;

struct Input {
    float2 uv_DispTex;
};

void surf(Input IN, inout SurfaceOutputStandard o) 
{
    o.Albedo = _Color.rgb;
    o.Metallic = _Metallic;
    o.Smoothness = _Glossiness;
    o.Alpha = _Color.a * (0.5 + 0.5 * clamp(tex2D(_DispTex, IN.uv_DispTex).r, 0, 1));

    float3 duv = float3(_DispTex_TexelSize.xy, 0) * 10;
    half v1 = tex2D(_DispTex, IN.uv_DispTex - duv.xz).y;
    half v2 = tex2D(_DispTex, IN.uv_DispTex + duv.xz).y;
    half v3 = tex2D(_DispTex, IN.uv_DispTex - duv.zy).y;
    half v4 = tex2D(_DispTex, IN.uv_DispTex + duv.zy).y;
    o.Normal = normalize(float3(v1 - v2, v3 - v4, 0.3));
}

ENDCG

}

FallBack "Diffuse"

}