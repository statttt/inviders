// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "CustomMaterialGlowTransparent" {
   Properties {
      _ColorMain ("Diffuse Material Color", Color) = (1,1,1,1) 
      _Glow ("Glow", Float) = 1
      _GlobalLightIntensity("Global Light Intensity", Float) = 1
      customFogStart ("customFogStart", Float) = 1
      customFogEnd ("customFogEnd", Float) = 1
   }
   SubShader {
      Tags {"Queue"="AlphaTest"}

      Pass {    
        Tags { "LightMode" = "ForwardBase" } 
         Blend SrcAlpha OneMinusSrcAlpha
         Cull Back
         ZTest LEqual
         ZWrite Off
         CGPROGRAM


 
         #pragma vertex vert approxview
         #pragma fragment frag 
         #pragma multi_compile_fog
         #pragma multi_compile_fwdbase
         #pragma fragmentoption ARB_precision_hint_fastest
         #pragma fragmentoption ARB_fog_linear


         #include "AutoLight.cginc"
         #include "UnityCG.cginc"
         uniform half4 _LightColor0; 
         uniform half3 _ColorMain; 
         uniform half _Glow;
         uniform half _GlobalLightIntensity;
         uniform half customFogStart;
         uniform half customFogEnd;

         struct vertexOutput 
         {
            float4 pos : SV_POSITION;
            float fogColorFactor:FLOAT;
            half2 uv : TEXCOORD0;
            float4 color: COLOR;
            LIGHTING_COORDS(1,2)
         };

         struct appdata_t
         {
            float4 vertex   : POSITION;
            float3 normal   : NORMAL;
            float4 color    : COLOR;
            float2 texcoord : TEXCOORD0;
         };

         vertexOutput vert(appdata_t v) 
         {
            vertexOutput output;
            float4x4 modelMatrix = unity_ObjectToWorld;
            float4x4 modelMatrixInverse = unity_WorldToObject;
            float4 outpos = UnityObjectToClipPos(v.vertex);

            float pos = length(outpos.xyz);
            float diff = customFogEnd - customFogStart;
            float invDiff = 1.0f / diff;
            output.fogColorFactor = clamp ((customFogEnd - pos) * invDiff, 0.0, 1.0);

            output.uv = v.texcoord.xy;
            output.pos = outpos;
            output.color = v.color;
            TRANSFER_VERTEX_TO_FRAGMENT(output);


            return output;
         }
 
         half4 frag(vertexOutput input) : COLOR
         {
            float3 finalColor = lerp(unity_FogColor.rgb, _ColorMain * input.color.rgb * _Glow * _GlobalLightIntensity * _GlobalLightIntensity, input.fogColorFactor.x);
            half4 col = half4(finalColor, input.color.a);
            return col;
         }
 
         ENDCG
      }
    }
 
   Fallback "Mobile/VertexLit"
}