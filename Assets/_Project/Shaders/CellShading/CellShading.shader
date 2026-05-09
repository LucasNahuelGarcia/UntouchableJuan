Shader "Lucas/CellShading"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _StepShadingTex("StepShading", 2D) = "white"{}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #include <HLSLSupport.cginc>
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _StepShadingTex;
            float4 _MainTex_ST;

            v2f vert(appdata input)
            {
                v2f interpolators;
                interpolators.normalWS = TransformObjectToWorldNormal(input.normalOS);
                interpolators.vertex = TransformObjectToHClip(input.vertex);
                interpolators.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return interpolators;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const half3 lightDirection = GetMainLight().direction;
                const float lightIndex = (1 + dot(lightDirection, i.normalWS)) / 2;
                const half cellShadeIndex = tex2D(_StepShadingTex, lightIndex);
                fixed4 col  = tex2D(_MainTex, i.uv) * cellShadeIndex;
                
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}