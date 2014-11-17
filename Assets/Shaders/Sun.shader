Shader "Custom/Sun" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0)
        _Details ("Details", Range (1.0, 64.0)) = 32.0
        _InnerRadius ("Inner Radius", Range(0.0, 1.0)) = 0.5
        // _Step ("Step Radius", Range (0.0, 1.0)) = 0.5
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
            uniform float _InnerRadius;
            // uniform float _Step;
            
            uniform float WorldTime;
            uniform float WorldSpeed;

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

            float4 frag(v2f_img sp) : COLOR 
            {
            	float details = floor(_Details);
            	float2 uv = pixelize(sp.uv.xy, details);

            	float time = WorldTime * WorldSpeed;
            	float anim = cos(time * 100.0) * 0.1;

            	float alpha = min(1.0, 0.5 + (1.0 - floor(distance(uv, float2(0.5, 0.5)) * details) / details * 4.0));
            	//step(distance(uv, float2(0.5, 0.5)), _Step);

            	float shades = min(1.0, 1.0 - pixelize(min(1.0, distance(uv, float2(0.5, 0.5))), details) - _InnerRadius + anim);

            	float3 color = _Color.rgb + shades;

                return float4(color, alpha);
            }

            ENDCG
        }
    }
}

