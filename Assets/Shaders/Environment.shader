Shader "Custom/Environment" 
{
    Properties 
    {
        _Details ("Details", Range (0.0, 8.0)) = 8.0
        _DetailsUnit ("Details Range Unit", Range (0.0, 8.0)) = 4.0
        _DetailsScope ("Lens Scope", Range (0.0, 1.0)) = 0.0
        _DetailsDirection ("Lens Curve", Range (0.0, 1.0)) = 1.0
        _Shades ("Shades", Range (0.0, 1.0)) = 0.2

        _ColorBranches ("Branches Color", Color) = (1,1,1,1)
        _TextureBranches ("Branches", 2D) = "white" {}
        _ColorRoots ("Roots Color", Color) = (1,1,1,1)
        _TextureRoots ("Roots", 2D) = "white" {}
        _ColorWater ("Water Color", Color) = (1,1,1,1)
        _TextureWater ("Water", 2D) = "white" {}
        _ColorFood ("Food Color", Color) = (1,1,1,1)
        _TextureFood ("Food", 2D) = "white" {}

        _ColorSunInner ("Sun Inner", Color) = (1,1,1,1)
        _ColorSunOutter ("Sun Outter", Color) = (1,1,1,1)
        _SunDistance ("Sun Distance", Range (0.0, 0.5)) = 0.2
        _SunRadius ("Sun Radius", Range (0.01, 0.1)) = 0.1

        _ColorMoonInner ("Moon Inner", Color) = (1,1,1,1)
        _ColorMoonOutter ("Moon Outter", Color) = (1,1,1,1)
        _MoonDistance ("Moon Distance", Range (0.0, 0.5)) = 0.2
        _MoonRadius ("Moon Radius", Range (0.01, 0.1)) = 0.1

        _ColorCloud ("Cloud", Color) = (1,1,1,1)
        _CloudDistance ("Cloud Distance", Range (0.0, 0.5)) = 0.2
        _CloudRadius ("Cloud Radius", Range (0.01, 0.1)) = 0.1

        _ColorSky ("Sky", Color) = (1,1,1,1)
        _ColorGround ("Ground", Color) = (1,1,1,1)
        _ColorGrass ("Grass", Color) = (1,1,1,1)
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Lambert
        #include "UnityCG.cginc"

        /* Parameters from Unity Editor */

        // Textures
        sampler2D _TextureBranches, _TextureRoots, _TextureWater, _TextureFood;

        // Colors
        float4 _ColorSunInner, _ColorSunOutter, _ColorMoonInner, _ColorMoonOutter, _ColorCloud, _ColorSky, _ColorGround, _ColorGrass, _ColorBranches, _ColorRoots, _ColorWater, _ColorFood;

        // Floats
        float _SunDistance, _SunRadius, _MoonDistance, _MoonRadius, _CloudDistance, _CloudRadius, _Details, _Shades, _DetailsMin, _DetailsMax, _DetailsUnit, _DetailsScope, _DetailsDirection;

        /* Uniforms from Shaders.cs */

        // Uniforms World
        uniform float WorldTime, WorldSpeed, WorldLight;

        uniform float _ToggleTexture;

        // Uniforms Directions
        uniform float4 SunDirection, MoonDirection, CloudDirection;


        /* Helpers */

        // Dat random function for glsl 
        float rand(float2 co){ return frac(sin(dot(co.xy, float2(12.9898,78.233))) * 43758.5453); }

        // Pixelize coordinates
        float2 pixelize(float2 uv, float details) { return floor(uv.xy * details) / details; }

        /* Surface Shader */

        struct Input 
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            
            // UVs
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
            float screenRatio = _ScreenParams.y / _ScreenParams.x;
            screenUV.y *= screenRatio;
            screenUV.y += (1.0 - screenRatio) / 2.0;

            // Pixel Fake QuadTree
            float dist = distance(pixelize(screenUV, pow(2.0, floor(_DetailsUnit))), float2(0.5, 0.5));
            dist = abs(_DetailsDirection - dist);
            dist = clamp(dist * sqrt(dist) + _DetailsScope, 0.0, 1.0);
            float details = pow(2.0, floor(dist * _Details));
            //float details = pow(2.0, floor(_Details));
            float2 uv = pixelize(screenUV, details);

            // Time & Animation
        	float time = WorldTime * WorldSpeed * 0.1;
            float oscillo = (1.0 + cos(time)) / 2.0;
            float2 sunDirection = pixelize(pixelize(SunDirection.xy, 4.0) * time, details);
            float2 moonDirection = pixelize(pixelize(MoonDirection.xy, 4.0) * time, details);

            // Random & Shades
            float random = rand(uv.xy);
            float shade = min(1.0, random + _Shades);
            float shadeGrass = min(1.0, rand(uv.xy - sunDirection) + _Shades);
            //float couche = cos((uv.y + cos((uv.x + uv.y) * 10.0) * 0.01) * 40.0);
            //float gr = step(0.0, couche);
            
            //
            float shadeGround = min(1.0, rand(uv.xy + moonDirection) + _Shades);//clamp(gr + _Shades, 0.0, 1.0);

            // Ground position
            float slice = 1.0 / details;
            float currentY = 1.0 - screenUV.y - 0.5 + slice * 6.0;
            currentY = clamp(currentY, 0.0, 1.0);

            // Sky
            float skyCloud = step(0.0, cos((uv.y - time + cos((uv.x + uv.y) * 10.0) * 0.01) * 40.0)); //step(rand(uv.yy), 0.5) * rand(uv.yy + pixelize(float2(0.0, time), details));
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
            //textureUV = pixelize(textureUV, details);

            // Plant
            float4 branches = tex2D(_TextureBranches, textureUV);
            float4 roots = tex2D(_TextureRoots, textureUV);
            float3 colorRoots = lerp(_ColorRoots.rgb, _ColorBranches.rgb, ground);

            // Water
            float shadeWater = rand(uv.xx) * 0.4;
            float4 water = tex2D(_TextureWater, pixelize(textureUV, details));

            // Food
            float4 food = tex2D(_TextureFood, pixelize(textureUV, details));

            // Apply layers
            float3 layerSky = lerp(lerp(lerp(sky, sun, sunAlpha), moon, moonAlpha), cloud, cloudAlpha);
            float3 layerGround = lerp(_ColorGround * shadeGround, _ColorGrass * shadeGrass, grass);
            float3 layerEnvironment = lerp(lerp(layerGround, layerSky, ground), _ColorFood, food.r);
            float3 layerPlant = lerp(lerp(layerEnvironment, _ColorBranches * shade, branches.g), colorRoots * shade, roots.g);
            float3 layerWater = lerp(layerPlant, _ColorWater + shadeWater, water.b);

            o.Emission = layerWater;
            o.Alpha = 1.0;
        }

        ENDCG
    }
}

