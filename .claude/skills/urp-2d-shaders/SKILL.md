---
name: urp-2d-shaders
description: Use when writing or converting shaders for this Unity 6 URP 2D project, including converting ShaderToy or Godot shaders to Unity, sprite effects (flash, outline, glow, dissolve) and the retro LCD/CRT screen effect for the console. Gives an HLSL sprite shader template, a GLSL to HLSL conversion table and common pitfalls.
---

# Shaders in URP 2D (Unity 6)

## Where things go

- Shader files: `Assets/Shaders/`. Materials: `Assets/Materials/`.
- Drive shader properties from a small script that listens to `GameEvents` (for example a hit flash
  when a frog is caught). Never put game logic in shader code.

## Sprite shader template (unlit, works with SpriteRenderer)

```hlsl
Shader "Catcher/SpriteFX"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Tags { "LightMode"="Universal2D" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes { float4 positionOS : POSITION; float4 color : COLOR; float2 uv : TEXCOORD0; };
            struct Varyings   { float4 positionCS : SV_POSITION; float4 color : COLOR; float2 uv : TEXCOORD0; };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                half4 _FlashColor;
                half _FlashAmount;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 c = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * IN.color;
                c.rgb = lerp(c.rgb, _FlashColor.rgb, _FlashAmount);
                return c;
            }
            ENDHLSL
        }
    }
}
```

Change properties from code with a `MaterialPropertyBlock` on the `SpriteRenderer`
(`SetFloat`, `SetColor`) so shared materials are not duplicated.

## Converting ShaderToy (GLSL) to Unity HLSL

| ShaderToy / GLSL | Unity HLSL |
|---|---|
| `void mainImage(out vec4 fragColor, in vec2 fragCoord)` | `half4 frag(Varyings IN) : SV_Target` and `return` the colour |
| `fragCoord / iResolution.xy` | `IN.uv` (already 0 to 1) |
| `iTime` | `_Time.y` |
| `iResolution` | `_ScreenParams.xy` or a custom property |
| `vec2 / vec3 / vec4` | `float2 / float3 / float4` |
| `mix(a, b, t)` | `lerp(a, b, t)` |
| `fract(x)` | `frac(x)` |
| `mod(x, y)` | `x - y * floor(x / y)` (do not use `fmod` for negative numbers) |
| `atan(y, x)` | `atan2(y, x)` |
| `texture(iChannel0, uv)` | `SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv)` |
| `mat2(a, b, c, d)` | `float2x2(a, c, b, d)` and multiply with `mul(m, v)` (order is swapped) |
| `iMouse` | remove, or expose as a property |

Also: fix the aspect ratio by multiplying `uv.x` by width/height. Loops need constant bounds
(`[unroll]`). Expose magic numbers as `Properties`.

## Pitfalls

- The sprite renders pink: shader compile error. Read the Console. Read the error line.
- The sprite is invisible: check `LightMode` is `Universal2D`, `Blend` is set, `_MainTex` is used.
- UI shaders (RawImage on a Canvas) are a different case: start from `Assets/Shaders/RetroScreen.shader`
  if it exists, use `ZTest Always` and `Cull Off`, and do not use `Universal2D`.
- Keep all properties inside `CBUFFER_START(UnityPerMaterial)` for SRP Batcher compatibility.
- Keep effects subtle: strong enough to feel, weak enough not to hurt readability.
