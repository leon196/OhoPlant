Shader "Custom/Ground" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Ground Color", Color) = (1,1,1,0)
        _ColorGrass ("Grass Color", Color) = (1,1,1,0)
        _Details ("Details", Range (1.0, 8.0)) = 1.0
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
        
        uniform float4 _Color;
        uniform float4 _ColorGrass;
        uniform float _Details;
        uniform float _Shades;
        
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
            
            // UVs
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
        	float details = pow(2, floor(_Details));
        	float2 uv = pixelize(IN.uv_MainTex.xy, details);
            uv.y = 1.0 - uv.y;

            // Color & Alpha init
        	float3 color = _Color.rgb;
        	float alpha = 1.0;

            // Time & Animation
        	float time = WorldTime * WorldSpeed;
        	MoonDirection.y = MoonDirection.y * -1.0;
        	MoonDirection.xy = pixelize(MoonDirection.xy, 4.0);
        	float2 anim = pixelize(MoonDirection.xy * time, details);

            // Random & Shades
            float random = rand(uv.xy + anim);
            float shade = min(1.0, random + _Shades);

        	// Grass
            float slice = 1.0 / details;
            float middle = max(0.0, min(1.0, 1.0 - screenUV.y - 0.5));
            float grass = color.g;
            alpha = alpha - step(middle, slice * 4.0) * step(random, 0.3);
        	grass = max(0.0, min(1.0, grass + step(middle, slice * 20.0) * step(1.0 - random, 0.3)));
        	grass = min(1.0, grass + step(middle, slice * 12.0));

            // Apply color
            color = _ColorGrass * grass + _Color * (1.0 - grass);

            // Horizon Cut out
            alpha = alpha - step(1.0 - screenUV.y, 0.5);

            // Return
            o.Emission = color * shade * WorldLight;
            o.Alpha = alpha;
        }

        ENDCG
    }
}

