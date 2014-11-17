Shader "Custom/Moon" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,0)
        _Details ("Details", Range (2.0, 128.0)) = 32.0
        _Step ("Radius", Range (0.0, 1.0)) = 0.5
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
            uniform float _Step;
            uniform float _Speed;
            
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

            float floorize(float value, float details) {
            	return floor(value * details) / details;
            }

            float4 frag(v2f_img sp) : COLOR 
            {
                float details = floor(_Details);

                // Disco ball
                float anim = floor(WorldTime * WorldSpeed * _Speed * details) / details;
                float2 uv = pixelize(sp.uv.xy, details);
                float shade = min(1.0, rand(uv.xy + float2(anim, 0.0)) + 0.2);
                float alpha = step(distance(uv, float2(0.5, 0.5)), _Step);

                //
                //float2 uv2 = sp.uv.xy;

                //float alpha2 = min(1.0, 0.5 + (1.0 - floor(distance(uv, float2(0.5, 0.5)) * details) / details * 4.0));
                //step(distance(uv, float2(0.5, 0.5)), _Step);
                
                //shade = min(1.0, 1.0 - pixelize(min(1.0, distance(uv, float2(0.5, 0.5))), details) - _Step);

                //float3 color = _Color.rgb + shades;


                return float4(_Color.rgb * shade, alpha);// + alpha2);
            }

            ENDCG
        }
    }
}

