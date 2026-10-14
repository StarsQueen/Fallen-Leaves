Shader "UI/RetroScreen"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _LcdTint ("LCD Tint", Color) = (0.92, 1, 0.85, 1)
        _ScanlineCount ("Scanline Count", Float) = 200
        _ScanlineStrength ("Scanline Strength", Range(0,1)) = 0.2
        _Curvature ("Curvature", Range(0,0.3)) = 0.06
        _Vignette ("Vignette", Range(0,1)) = 0.5
        _Fringe ("Color Fringe", Range(0,0.01)) = 0.0015
        _Flicker ("Flicker", Range(0,0.1)) = 0.015
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv    : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _LcdTint;
            float _ScanlineCount;
            float _ScanlineStrength;
            float _Curvature;
            float _Vignette;
            float _Fringe;
            float _Flicker;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Slight CRT bulge
                float2 p = i.uv * 2.0 - 1.0;
                p *= 1.0 + _Curvature * dot(p, p);
                float2 uv = p * 0.5 + 0.5;

                // Colour fringing (tiny RGB offset)
                fixed r = tex2D(_MainTex, uv + float2(_Fringe, 0)).r;
                fixed g = tex2D(_MainTex, uv).g;
                fixed b = tex2D(_MainTex, uv - float2(_Fringe, 0)).b;
                fixed3 col = fixed3(r, g, b);

                // Scanlines
                float scan = sin(uv.y * _ScanlineCount * 6.28318) * 0.5 + 0.5;
                col *= 1.0 - _ScanlineStrength * scan;

                // Vignette
                col *= saturate(1.0 - dot(p, p) * _Vignette * 0.5);

                // Flicker
                col *= 1.0 + sin(_Time.y * 60.0) * _Flicker;

                // LCD tint
                col *= _LcdTint.rgb;

                // Black outside the bulged image
                float2 inside = step(0.0, uv) * step(uv, 1.0);
                col *= inside.x * inside.y;

                return fixed4(col, 1) * i.color;
            }
            ENDCG
        }
    }
}
