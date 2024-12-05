#ifndef MYLIGHTNODE_INCLUDED
#define MYLIGHTNODE_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

#endif

void GetMainLightParams_float(float3 WorldPosition, out half3 Direction, out half3 Color, out float DistanceAttenuation,
                              out half ShadowAttenuation)
{
    #if defined(SHADERGRAPH_PREVIEW)
    Direction = float3(0.5, 0.5, 0);
    Color = 1;
    DistanceAttenuation = 1;
    ShadowAttenuation = 1;
    
    #else
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAttenuation = mainLight.distanceAttenuation;
    ShadowAttenuation = mainLight.shadowAttenuation;

    #endif
}

void GetAdditionalLight_float(float3 WorldPosition, float3 Normal, out half3 Color)
{
    #ifdef SHADERGRAPH_PREVIEW
    Color = half3(0.5, 0.5, 0.5);
    
    #else
    uint lightCount = GetAdditionalLightsCount();

    for (uint lightIndex = 0u; lightIndex < lightCount; ++lightIndex)
    {
        //ライトの取得
        Light light = GetAdditionalLight(lightIndex, WorldPosition);
        //ライティングの計算
        half3 lightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        Color += LightingLambert(lightColor, light.direction, Normal);
    }

    #endif
}

void GetReceiveShadow_float(float ShadowAlpha, float3 WorldPos, out half ShadowAttenuation)
{
    #ifdef SHADERGRAPH_PREVIEW
    ShadowAttenuation = 1.0;
    
    #else
    half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    Light mainLight = GetMainLight(shadowCoord);
    half shadow = mainLight.shadowAttenuation;
    int pixelLightCount = GetAdditionalLightsCount();

    for (int i = 0; i < pixelLightCount; i++)
    {
        Light AddLight0 = GetAdditionalLight(i, WorldPos, 1);
        half shadow0 = AddLight0.shadowAttenuation;
        shadow *= shadow0;
    }

    ShadowAttenuation = shadow * ShadowAlpha;

    #endif
}

#endif
