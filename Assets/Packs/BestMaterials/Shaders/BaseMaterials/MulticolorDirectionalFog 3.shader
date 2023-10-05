// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "CustomMaterialAnisotropicFogYTransparentNoCull" {
   Properties {
      _ColorMain ("Diffuse Material Color", Color) = (1,1,1,1) 
      _MidColor ("Semishadow Material Color", Color) = (1,1,1,1) 
      _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
      _Shininess ("Shininess", Float) = 1
      _AOFactor ("Ambient Occlusion Factor", Float) = 1
      _FogScaleZ ("Fog Scale Z", Float) = 10
      _FogPowerZ("Fog Power", Float) = 2
      _FogOffsetY("Fog Offset Y", Float) = 1
      _GlobalLightIntensity("Global Light Intensity", Float) = 1
      _Transparency ("Transparency", Float) = 1
   }
   SubShader {
      Tags {"Queue"="Transparent"}

      Pass {    
        Tags { "LightMode" = "ForwardBase" } 
         Blend SrcAlpha OneMinusSrcAlpha
         Cull Back
         ZTest LEqual
         ZWrite Off
         CGPROGRAM
        
         #pragma vertex vert  
         #pragma fragment frag 
         #pragma multi_compile_fog
         #pragma multi_compile_fwdbase
         #pragma fragmentoption ARB_precision_hint_fastest
         #pragma fragmentoption ARB_fog_linear
         #pragma glsl_no_auto_normalization


         #include "AutoLight.cginc"
         #include "UnityCG.cginc"
         uniform fixed4 _LightColor0; 

         uniform fixed3 _ColorMain; 
         uniform fixed3 _SpecColor; 
         uniform fixed3 _MidColor; 
         uniform half _Shininess;
         uniform half _AOFactor;
         uniform half _FogScaleZ;
         uniform half _FogPowerZ;
         uniform half _FogOffsetY;
         uniform half _GlobalLightIntensity;
         uniform fixed4 unity_FogStart;
         uniform fixed4 unity_FogEnd;
         uniform float _Transparency;
         struct vertexOutput 
         {
            half2 fogColorFactor:FLOAT;
            float4 pos : SV_POSITION;
            half2 uv : TEXCOORD0;
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

            //outpos.y -= outpos.z * (outpos.z + abs(outpos.x - 0.5) * abs(outpos.x - 0.5) * 0.03) * 0.03;

            half3 normalDirection = normalize(
               mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
            half3 viewDirection = normalize(_WorldSpaceCameraPos 
               - mul(modelMatrix, v.vertex).xyz);
            half3 lightDirection;
            half attenuation;
 
            attenuation = 1.0; // no attenuation
            lightDirection = normalize(_WorldSpaceLightPos0.xyz);
 


            half factor = max(0.0,dot(normalDirection, lightDirection) * 0.5 + 0.5 * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess));

            //FOG
            half height = mul(modelMatrix, v.vertex).y;
            half pos = (height - _FogOffsetY) / _FogScaleZ;//* lerp(_FogPowerZ,1, height / _FogScaleZ);
            half fogUpDown = clamp (pos, 0.0, 1.0f);

            half pos1 = length(outpos.xyz);
            half diff = unity_FogEnd.x - unity_FogStart.x;
            half invDiff = 1.0f / diff;
            half fogLinear = clamp ((unity_FogEnd.x - pos) * invDiff, 0.0, 1.0);

            output.fogColorFactor.x = fogUpDown;

            output.pos = outpos;
            output.fogColorFactor.y = lerp(factor, factor * (v.color.r - 0.5f) *2.0f, _AOFactor);

            TRANSFER_VERTEX_TO_FRAGMENT(output);


            return output;
         }
 
         half4 frag(vertexOutput input) : COLOR
         {
            half factor = input.fogColorFactor.y* LIGHT_ATTENUATION(input);

            half factorl = floor(factor  + 0.5);
            half3 tempcolshadowmid = lerp(_ColorMain,_MidColor,factor * 2.0);
            half3 tempcolmidlight = lerp(_MidColor,_SpecColor,(factor - 0.5 ) * 2.0);
            half3 tempcol = tempcolshadowmid * (1 - factorl) + tempcolmidlight * factorl;
           
            half3 finalColor = lerp(unity_FogColor.rgb, tempcol.rgb, input.fogColorFactor.x);
            half4 col = half4(finalColor, _Transparency) * _GlobalLightIntensity;
            return col;
         }
 
         ENDCG
      }
      }
 
   Fallback "VertexLit"
}