Shader "Custom/Cloud" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0)
        _Details ("Details", Range (2.0, 64.0)) = 32.0
        _Radius ("Radius", Range (0.0, 1.0)) = 0.5
        _Speed ("Speed", Range (0.01, 10.0)) = 0.1
    }
    SubShader {
	   	Tags { "Queue"="Transparent" "IgnoreProjector"="True" }
        Pass {
		    Blend SrcAlpha OneMinusSrcAlpha
		    Cull Off

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            
            uniform float4 _Color;
            uniform float _Details;
            uniform float _Radius;
            uniform float _Speed;
            
            uniform float WorldTime;
            uniform float WorldSpeed;
            uniform float WorldLight;

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

            float floorize(float value, float details) {
            	return floor(value * details) / details;
            }

            float4 frag(v2f_img sp) : COLOR 
            {
                float details = floor(_Details);
                float anim = floor(WorldTime * WorldSpeed * _Speed * details) / details;
                float2 uv = pixelize(sp.uv.xy, details);
                float shade = min(1.0, rand(uv.xy + float2(anim, 0.0)) * 0.1);
                float dist = distance(uv, float2(0.5, 0.5));
                float alpha = rand(uv.xy + float2(anim, 0.0)) * step(dist, _Radius) + step(dist, _Radius - 0.1);
                //float alpha = step(distance(uv, float2(0.25, 0.5)), _Radius * 0.75) + step(distance(uv, float2(0.4, 0.55)), _Radius) + step(distance(uv, float2(0.6, 0.55)), _Radius) + step(distance(uv, float2(0.75, 0.5)), _Radius * 0.75);
                float3 color = (_Color.rgb - shade) * WorldLight;
                return float4(color, alpha);
            }

            ENDCG
        }
    }
}

