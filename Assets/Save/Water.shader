Shader "Custom/Water" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,0)
        _Details ("Details", Range (1.0, 512.0)) = 1.0
        _Shades ("Shades", Range (0.0, 1.0)) = 0.2
    }
    SubShader {
	   	Tags { "Queue"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        CGPROGRAM
        #pragma surface surf Lambert

        #include "UnityCG.cginc"
        
        sampler2D _MainTex;
        float4 _Color;
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
            uv.y = 1.0 - uv.y;

        	//float time = WorldTime * WorldSpeed;

        	//float random = rand(uv.xy);

        	float4 tex = tex2D(_MainTex, uv.xy);
        	float seuil = (tex.r + tex.g + tex.b) / 3.0;
            float3 color = step(seuil, 0.5) * _Color.rgb;
            float2 anim = float2(floor(WorldTime * WorldSpeed * details) / details, 0.0);
            float alpha = step(1.0 - tex.b, 0.5) * max(0.0, min(1.0, rand(uv.xy + anim)));

            o.Emission = color * WorldLight;
            o.Alpha = alpha;
        }

        ENDCG
    }
}

