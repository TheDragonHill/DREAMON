// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "CustomShader/WaveBlobShader" 
{
    Properties 
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Frequency ("Frequency", Range(0,100)) = 10
        _Amplitude ("Amplitude", Range(0,1)) = 0.1
        _Speed ("Speed", Range(0, 10)) = 2
        _Axis ("Axis", Vector) = (1,1,1,1)
        
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 because it uses wrong array syntax (type[size] name)
#pragma exclude_renderers d3d11
        #pragma surface surf  Standard fullforwardshadows addshadow vertex:vertex_shader

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input 
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half _Frequency;
        half _Amplitude;
        float _Speed;
        fixed4 _Axis;
        float[4] array;

      
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void vertex_shader (inout appdata_base v) 
        {
            _Axis.xyz *= v.normal * sin(v.vertex.y * _Frequency + _Time.y * _Speed) * _Amplitude;
            v.vertex.xyz += _Axis.xyz;
        }

        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}