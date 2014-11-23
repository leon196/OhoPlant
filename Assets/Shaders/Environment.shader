Shader "Custom/Environment" {
    Properties {
        _TextureBranches ("Branches", 2D) = "white" {}
        _ColorBranches ("Branches", Color) = (1,1,1,1)
        _TextureRoots ("Roots", 2D) = "white" {}
        _ColorRoots ("Roots", Color) = (1,1,1,1)
        _TextureWater ("Water", 2D) = "white" {}
        _ColorWater ("Water", Color) = (1,1,1,1)

        _ColorSunInner ("Sun Inner", Color) = (1,1,1,1)
        _ColorSunOutter ("Sun Outter", Color) = (1,1,1,1)
        _SunDistance ("Distance", Range (0.0, 0.5)) = 0.2
        _SunRadius ("Radius", Range (0.01, 0.1)) = 0.1

        _ColorMoonInner ("Moon Inner", Color) = (1,1,1,1)
        _ColorMoonOutter ("Moon Outter", Color) = (1,1,1,1)
        _MoonDistance ("Distance", Range (0.0, 0.5)) = 0.2
        _MoonRadius ("Radius", Range (0.01, 0.1)) = 0.1

        _ColorCloud ("Cloud", Color) = (1,1,1,1)
        _CloudDistance ("Distance", Range (0.0, 0.5)) = 0.2
        _CloudRadius ("Radius", Range (0.01, 0.1)) = 0.1

        _ColorSky ("Sky", Color) = (1,1,1,1)
        _ColorGround ("Ground", Color) = (1,1,1,1)
        _ColorGrass ("Grass", Color) = (1,1,1,1)

        _Shades ("Shades", Range (0.0, 1.0)) = 0.2
        _Details ("Details", Range (4.0, 8.0)) = 4.0
    }
    SubShader {
	   	Tags { "Queue"="Transparent" "IgnoreProjector"="True" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        CGPROGRAM
        #pragma surface surf Lambert

        #include "UnityCG.cginc"
        
        sampler2D _TextureBranches;
        float4 _ColorBranches;
        sampler2D _TextureRoots;
        float4 _ColorRoots;
        sampler2D _TextureWater;
        float4 _ColorWater;

        float4 _ColorSunInner;
        float4 _ColorSunOutter;
        float _SunDistance;
        float _SunRadius;

        float4 _ColorMoonInner;
        float4 _ColorMoonOutter;
        float _MoonDistance;
        float _MoonRadius;

        float4 _ColorCloud;
        float _CloudDistance;
        float _CloudRadius;

        float4 _ColorSky;
        float4 _ColorGround;
        float4 _ColorGrass;
        float _Details;
        float _Shades;
        
        uniform float WorldTime;
        uniform float WorldSpeed;
        uniform float WorldLight;

        uniform float4 SunDirection;
        uniform float4 MoonDirection;
        uniform float4 CloudDirection;

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
            screenUV.y *= _ScreenParams.y / _ScreenParams.x;
            screenUV.y += (1.0 - _ScreenParams.y / _ScreenParams.x) / 2.0;
            float details = pow(2, floor(_Details));
            float2 uv = pixelize(screenUV, details);

            // Time & Animation
        	float time = WorldTime * WorldSpeed;
            float2 sunDirection = pixelize(pixelize(SunDirection.xy, 4.0) * time, details);
            float2 moonDirection = pixelize(pixelize(MoonDirection.xy, 4.0) * time, details);

            // Random & Shades
            float random = rand(uv.xy);
            float shade = min(1.0, random + _Shades);
            float shadeGrass = min(1.0, rand(uv.xy - sunDirection) + _Shades);
            float shadeGround =  min(1.0, rand(uv.xy - moonDirection) + _Shades);

            // Ground position
            float slice = 1.0 / details;
            float currentY = 1.0 - screenUV.y - 0.5;
            currentY = clamp(currentY, 0.0, 1.0);

            // Sky
            float skyCloud = step(rand(uv.yy), 0.5) * rand(uv.yy + pixelize(float2(0.0, time), details));
            float3 sky = _ColorSky + skyCloud;

            // Sun
            float distFromSun = distance(uv, float2(0.5, 0.5) + SunDirection.xy * _SunDistance);
            float sunShades = pixelize(distFromSun * 1.0/_SunRadius, floor(_Details));
            float sunAlpha = 1.0 - step(_SunRadius, distFromSun);
            float3 sun = lerp(_ColorSunInner, _ColorSunOutter, clamp(sunShades, 0.0, 1.0));

            // Moon
            float distFromMoon = distance(uv, float2(0.5, 0.5) + MoonDirection.xy * _MoonDistance);
            float moonAlpha = 1.0 - step(_MoonRadius, distFromMoon);
            float moonShades = pixelize(distFromMoon * 1.0/_MoonRadius, floor(_Details));
            float3 moon = lerp(_ColorMoonInner, _ColorMoonOutter, clamp(moonShades, 0.0, 1.0));

            // Cloud
            float distFromCloud = distance(uv, float2(0.5, 0.5) + CloudDirection.xy * _CloudDistance);
            float cloudAlpha = 1.0 - step(_CloudRadius, distFromCloud);
            float cloudShades = pixelize(distFromCloud * 1.0/_CloudRadius, floor(_Details));
            float3 cloud =_ColorCloud * clamp(cloudShades, 0.0, 1.0);

        	// Grass
            float grass = 0.0;
        	grass += step(currentY, slice * 8.0) * step(1.0 - random, 0.3);
        	grass += step(currentY, slice * 6.0);
            grass = clamp(grass, 0.0, 1.0);
            
            // Ground
            float ground = 1.0;
            ground = step(currentY, 0.0) + step(currentY, slice * 2.0) * step(random, 0.3);
            ground = clamp(ground, 0.0, 1.0);

            // Zoom for Textures
            float textureDetails = pow(2.0, 8.0 - floor(_Details));
            float2 textureUV = screenUV;
            textureUV -= 0.5 + 0.5 * textureDetails;
            textureUV /= max(1.0, textureDetails);

            // Plant
            float4 branches = tex2D(_TextureBranches, textureUV);

            // Water
            float4 water = tex2D(_TextureWater, textureUV);

            // Apply layers
            float3 layerSky = lerp(lerp(lerp(sky, sun, sunAlpha), moon, moonAlpha), cloud, cloudAlpha);
            float3 layerGround = lerp(_ColorGround * shadeGround, _ColorGrass * shadeGrass, grass);
            float3 layerEnvironment = lerp(layerGround, layerSky, ground);
            float3 layerBranches = lerp(layerEnvironment, _ColorBranches * shade, branches.g);
            float3 layerWater = lerp(layerBranches, _ColorWater * shade, water.b);

            o.Emission = layerWater;
            o.Alpha = 1.0;
        }

        ENDCG
    }
}

