Shader "Custom/Plant" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,0)
        _Scale ("Scale", Range (1.0, 8.0)) = 1.0
        _Details ("Details", Range (1.0, 512.0)) = 1.0
        _Shades ("Shades", Range (0.0, 1.0)) = 0.2
    }
    SubShader {
	   	Tags { "Queue"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert

        #include "UnityCG.cginc"
        
        sampler2D _MainTex;
        float4 _Color;
        float _Scale;
        float _Details;
        float _Shades;
        
        uniform float WorldTime;
        uniform float WorldSpeed;
        uniform float WorldLight;
        uniform float4 MoonDirection;

        uniform float slider1;
        uniform float slider2;

        uniform float spiner1;
        uniform float spiner2;
        uniform float spiner3;

        float rand(float2 co){
            return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453);
        }

        float2 pixelize(float2 uv, float details) {
        	return floor(uv.xy * details) / details;
        }

        struct Input {
            float2 uv_MainTex;
            float4 screenPos;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
        	float details = floor(_Details);
            float2 uv = pixelize(IN.uv_MainTex.xy, details);

        	float time = WorldTime * WorldSpeed;

        	MoonDirection.xy = pixelize(MoonDirection.xy, 4.0);
        	float2 anim = pixelize(MoonDirection.xy * time, details);

        	float shade = min(1.0, rand(uv.xy + anim ) + _Shades);
        	//float slice = 1.0 / details;


            //float border = 1.0 - (1.0 + cos(IN.uv_MainTex.y * 6.28318530718)) * 0.5;
            //o.Emission = _ColorA.rgb * border + _ColorB.rgb * (1.0 - border);
            //o.Emission *= WorldLight;
            //border = step(1.0 - border, 0.5);

            uv.xy *= _Scale;
            uv.xy -= (_Scale - 1.0) * 0.5;
            float4 tex = tex2D(_MainTex, uv.xy);
            float lum = (tex.r + tex.g + tex.b) / 3.0;
            o.Emission = step(lum, 0.5) * _Color.rgb * shade;
            //o.Emission = _Color.rgb * shade * WorldLight - (1.0 - border) * 0.2;
            //o.Emission = (1.0 - border) * _Color.rgb + border * _Color.rgb * shade * WorldLight;
            float cutOutX = step(uv.x, 0.0) + (1.0 - step(uv.x, 1.0));
            float cutOutY = step(uv.y, 0.0) + (1.0 - step(uv.y, 1.0));
            o.Alpha = step(1.0-tex.b, 0.5) - cutOutX - cutOutY;
        }

        ENDCG
    }
}

