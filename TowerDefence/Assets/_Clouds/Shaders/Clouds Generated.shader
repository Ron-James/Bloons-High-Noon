Shader "Clouds Generated"
{
    Properties
    {
        Vector4_484BA326("Rotate Projection", Vector) = (1, 0, 0, 0)
        Vector1_8F075DBC("Noise Scale", Float) = 10
        Vector1_8AAD665C("Noise Speed", Float) = 0.1
        Vector1_E43D3892("Noise Height", Float) = 1
        Vector4_1696EE66("Noise Remap", Vector) = (0, 1, -1, 1)
        Color_2CFB5E2A("Color Peak", Color) = (1, 1, 1, 0)
        Color_58195353("Color Valley", Color) = (0, 0, 0, 0)
        Vector1_FADC0395("Noise Edge 1", Float) = 0
        Vector1_E505CA4B("Noise Edge 2", Float) = 1
        Vector1_4CE104D9("Noise Power", Float) = 2
        Vector1_5536221E("Base Scale", Float) = 5
        Vector1_9058DE8C("Base Speed", Float) = 0.2
        Vector1_350643F1("Base Strength", Float) = 2
        Vector1_2A80C598("Emission Strength", Float) = 2
        Vector1_A664731F("Curvature Radius", Float) = 1
        Vector1_B4ED823D("Fresnel Power", Float) = 1
        Vector1_2BA361A8("Fresnel Opacity", Float) = 1
        Vector1_EED1A887("Fade Depth", Float) = 100
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent+0"
        }
        
        Pass
        {
            Name "Universal Forward"
            Tags 
            { 
                "LightMode" = "UniversalForward"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
			//!
            ZWrite On
			//!
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define FEATURES_GRAPH_VERTEX
            #pragma multi_compile_instancing
            #define SHADERPASS_FORWARD
            #define REQUIRE_DEPTH_TEXTURE
            
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 Vector4_484BA326;
            float Vector1_8F075DBC;
            float Vector1_8AAD665C;
            float Vector1_E43D3892;
            float4 Vector4_1696EE66;
            float4 Color_2CFB5E2A;
            float4 Color_58195353;
            float Vector1_FADC0395;
            float Vector1_E505CA4B;
            float Vector1_4CE104D9;
            float Vector1_5536221E;
            float Vector1_9058DE8C;
            float Vector1_350643F1;
            float Vector1_2A80C598;
            float Vector1_A664731F;
            float Vector1_B4ED823D;
            float Vector1_2BA361A8;
            float Vector1_EED1A887;
            CBUFFER_END
        
            // Graph Functions
            
            void Unity_Distance_float3(float3 A, float3 B, out float Out)
            {
                Out = distance(A, B);
            }
            
            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }
            
            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Rotate_About_Axis_Degrees_float(float3 In, float3 Axis, float Rotation, out float3 Out)
            {
                Rotation = radians(Rotation);
            
                float s = sin(Rotation);
                float c = cos(Rotation);
                float one_minus_c = 1.0 - c;
                
                Axis = normalize(Axis);
            
                float3x3 rot_mat = { one_minus_c * Axis.x * Axis.x + c,            one_minus_c * Axis.x * Axis.y - Axis.z * s,     one_minus_c * Axis.z * Axis.x + Axis.y * s,
                                          one_minus_c * Axis.x * Axis.y + Axis.z * s,   one_minus_c * Axis.y * Axis.y + c,              one_minus_c * Axis.y * Axis.z - Axis.x * s,
                                          one_minus_c * Axis.z * Axis.x - Axis.y * s,   one_minus_c * Axis.y * Axis.z + Axis.x * s,     one_minus_c * Axis.z * Axis.z + c
                                        };
            
                Out = mul(rot_mat,  In);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            
            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { 
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
            {
                Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
            }
            
            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float3 WorldSpacePosition;
                float3 TimeParameters;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Distance_D85DF7AD_Out_2;
                Unity_Distance_float3(SHADERGRAPH_OBJECT_POSITION, IN.WorldSpacePosition, _Distance_D85DF7AD_Out_2);
                float _Property_117D8453_Out_0 = Vector1_A664731F;
                float _Divide_6EF58E3_Out_2;
                Unity_Divide_float(_Distance_D85DF7AD_Out_2, _Property_117D8453_Out_0, _Divide_6EF58E3_Out_2);
                float _Power_33FF8791_Out_2;
                Unity_Power_float(_Divide_6EF58E3_Out_2, 3, _Power_33FF8791_Out_2);
                float3 _Multiply_E4027504_Out_2;
                Unity_Multiply_float(IN.WorldSpaceNormal, (_Power_33FF8791_Out_2.xxx), _Multiply_E4027504_Out_2);
                float _Property_34CB0090_Out_0 = Vector1_FADC0395;
                float _Property_AD125D68_Out_0 = Vector1_E505CA4B;
                float4 _Property_766D591A_Out_0 = Vector4_484BA326;
                float _Split_46EE164F_R_1 = _Property_766D591A_Out_0[0];
                float _Split_46EE164F_G_2 = _Property_766D591A_Out_0[1];
                float _Split_46EE164F_B_3 = _Property_766D591A_Out_0[2];
                float _Split_46EE164F_A_4 = _Property_766D591A_Out_0[3];
                float3 _RotateAboutAxis_A00D475E_Out_3;
                Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpacePosition, (_Property_766D591A_Out_0.xyz), _Split_46EE164F_A_4, _RotateAboutAxis_A00D475E_Out_3);
                float _Property_1195B7B7_Out_0 = Vector1_8AAD665C;
                float _Multiply_365B1AAE_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1195B7B7_Out_0, _Multiply_365B1AAE_Out_2);
                float2 _TilingAndOffset_94B76B59_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_365B1AAE_Out_2.xx), _TilingAndOffset_94B76B59_Out_3);
                float _Property_EEA5F82E_Out_0 = Vector1_8F075DBC;
                float _GradientNoise_4DF641FB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_94B76B59_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_4DF641FB_Out_2);
                float2 _TilingAndOffset_D3C6639A_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), float2 (0, 0), _TilingAndOffset_D3C6639A_Out_3);
                float _GradientNoise_ADD9B44D_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_D3C6639A_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_ADD9B44D_Out_2);
                float _Add_29BFF883_Out_2;
                Unity_Add_float(_GradientNoise_4DF641FB_Out_2, _GradientNoise_ADD9B44D_Out_2, _Add_29BFF883_Out_2);
                float _Divide_40E6AFDE_Out_2;
                Unity_Divide_float(_Add_29BFF883_Out_2, 2, _Divide_40E6AFDE_Out_2);
                float _Saturate_7F44B33_Out_1;
                Unity_Saturate_float(_Divide_40E6AFDE_Out_2, _Saturate_7F44B33_Out_1);
                float _Property_8444523D_Out_0 = Vector1_4CE104D9;
                float _Power_E1284C32_Out_2;
                Unity_Power_float(_Saturate_7F44B33_Out_1, _Property_8444523D_Out_0, _Power_E1284C32_Out_2);
                float4 _Property_9D282FC2_Out_0 = Vector4_1696EE66;
                float _Split_AC92423D_R_1 = _Property_9D282FC2_Out_0[0];
                float _Split_AC92423D_G_2 = _Property_9D282FC2_Out_0[1];
                float _Split_AC92423D_B_3 = _Property_9D282FC2_Out_0[2];
                float _Split_AC92423D_A_4 = _Property_9D282FC2_Out_0[3];
                float4 _Combine_548F329E_RGBA_4;
                float3 _Combine_548F329E_RGB_5;
                float2 _Combine_548F329E_RG_6;
                Unity_Combine_float(_Split_AC92423D_R_1, _Split_AC92423D_G_2, 0, 0, _Combine_548F329E_RGBA_4, _Combine_548F329E_RGB_5, _Combine_548F329E_RG_6);
                float4 _Combine_B40C924A_RGBA_4;
                float3 _Combine_B40C924A_RGB_5;
                float2 _Combine_B40C924A_RG_6;
                Unity_Combine_float(_Split_AC92423D_B_3, _Split_AC92423D_A_4, 0, 0, _Combine_B40C924A_RGBA_4, _Combine_B40C924A_RGB_5, _Combine_B40C924A_RG_6);
                float _Remap_F10A160D_Out_3;
                Unity_Remap_float(_Power_E1284C32_Out_2, _Combine_548F329E_RG_6, _Combine_B40C924A_RG_6, _Remap_F10A160D_Out_3);
                float _Absolute_4FC64134_Out_1;
                Unity_Absolute_float(_Remap_F10A160D_Out_3, _Absolute_4FC64134_Out_1);
                float _Smoothstep_56070F3_Out_3;
                Unity_Smoothstep_float(_Property_34CB0090_Out_0, _Property_AD125D68_Out_0, _Absolute_4FC64134_Out_1, _Smoothstep_56070F3_Out_3);
                float _Property_1671376A_Out_0 = Vector1_9058DE8C;
                float _Multiply_E8657CCC_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1671376A_Out_0, _Multiply_E8657CCC_Out_2);
                float2 _TilingAndOffset_B64A0F1F_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_E8657CCC_Out_2.xx), _TilingAndOffset_B64A0F1F_Out_3);
                float _Property_12404564_Out_0 = Vector1_5536221E;
                float _GradientNoise_3F907965_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B64A0F1F_Out_3, _Property_12404564_Out_0, _GradientNoise_3F907965_Out_2);
                float _Property_94C69BD5_Out_0 = Vector1_350643F1;
                float _Multiply_61FF3188_Out_2;
                Unity_Multiply_float(_GradientNoise_3F907965_Out_2, _Property_94C69BD5_Out_0, _Multiply_61FF3188_Out_2);
                float _Add_12AD4DE7_Out_2;
                Unity_Add_float(_Smoothstep_56070F3_Out_3, _Multiply_61FF3188_Out_2, _Add_12AD4DE7_Out_2);
                float _Add_B7625F11_Out_2;
                Unity_Add_float(1, _Property_94C69BD5_Out_0, _Add_B7625F11_Out_2);
                float _Divide_A999571A_Out_2;
                Unity_Divide_float(_Add_12AD4DE7_Out_2, _Add_B7625F11_Out_2, _Divide_A999571A_Out_2);
                float3 _Multiply_EF48E583_Out_2;
                Unity_Multiply_float(IN.ObjectSpaceNormal, (_Divide_A999571A_Out_2.xxx), _Multiply_EF48E583_Out_2);
                float _Property_347E548F_Out_0 = Vector1_E43D3892;
                float3 _Multiply_E6AAE955_Out_2;
                Unity_Multiply_float(_Multiply_EF48E583_Out_2, (_Property_347E548F_Out_0.xxx), _Multiply_E6AAE955_Out_2);
                float3 _Add_EDAC08D6_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_E6AAE955_Out_2, _Add_EDAC08D6_Out_2);
                float3 _Add_5924731B_Out_2;
                Unity_Add_float3(_Multiply_E4027504_Out_2, _Add_EDAC08D6_Out_2, _Add_5924731B_Out_2);
                description.VertexPosition = _Add_5924731B_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 WorldSpaceNormal;
                float3 TangentSpaceNormal;
                float3 WorldSpaceViewDirection;
                float3 WorldSpacePosition;
                float4 ScreenPosition;
                float3 TimeParameters;
            };
            
            struct SurfaceDescription
            {
                float3 Albedo;
                float3 Normal;
                float3 Emission;
                float Metallic;
                float Smoothness;
                float Occlusion;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _Property_25FBF6D9_Out_0 = Color_58195353;
                float4 _Property_1400C896_Out_0 = Color_2CFB5E2A;
                float _Property_34CB0090_Out_0 = Vector1_FADC0395;
                float _Property_AD125D68_Out_0 = Vector1_E505CA4B;
                float4 _Property_766D591A_Out_0 = Vector4_484BA326;
                float _Split_46EE164F_R_1 = _Property_766D591A_Out_0[0];
                float _Split_46EE164F_G_2 = _Property_766D591A_Out_0[1];
                float _Split_46EE164F_B_3 = _Property_766D591A_Out_0[2];
                float _Split_46EE164F_A_4 = _Property_766D591A_Out_0[3];
                float3 _RotateAboutAxis_A00D475E_Out_3;
                Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpacePosition, (_Property_766D591A_Out_0.xyz), _Split_46EE164F_A_4, _RotateAboutAxis_A00D475E_Out_3);
                float _Property_1195B7B7_Out_0 = Vector1_8AAD665C;
                float _Multiply_365B1AAE_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1195B7B7_Out_0, _Multiply_365B1AAE_Out_2);
                float2 _TilingAndOffset_94B76B59_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_365B1AAE_Out_2.xx), _TilingAndOffset_94B76B59_Out_3);
                float _Property_EEA5F82E_Out_0 = Vector1_8F075DBC;
                float _GradientNoise_4DF641FB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_94B76B59_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_4DF641FB_Out_2);
                float2 _TilingAndOffset_D3C6639A_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), float2 (0, 0), _TilingAndOffset_D3C6639A_Out_3);
                float _GradientNoise_ADD9B44D_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_D3C6639A_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_ADD9B44D_Out_2);
                float _Add_29BFF883_Out_2;
                Unity_Add_float(_GradientNoise_4DF641FB_Out_2, _GradientNoise_ADD9B44D_Out_2, _Add_29BFF883_Out_2);
                float _Divide_40E6AFDE_Out_2;
                Unity_Divide_float(_Add_29BFF883_Out_2, 2, _Divide_40E6AFDE_Out_2);
                float _Saturate_7F44B33_Out_1;
                Unity_Saturate_float(_Divide_40E6AFDE_Out_2, _Saturate_7F44B33_Out_1);
                float _Property_8444523D_Out_0 = Vector1_4CE104D9;
                float _Power_E1284C32_Out_2;
                Unity_Power_float(_Saturate_7F44B33_Out_1, _Property_8444523D_Out_0, _Power_E1284C32_Out_2);
                float4 _Property_9D282FC2_Out_0 = Vector4_1696EE66;
                float _Split_AC92423D_R_1 = _Property_9D282FC2_Out_0[0];
                float _Split_AC92423D_G_2 = _Property_9D282FC2_Out_0[1];
                float _Split_AC92423D_B_3 = _Property_9D282FC2_Out_0[2];
                float _Split_AC92423D_A_4 = _Property_9D282FC2_Out_0[3];
                float4 _Combine_548F329E_RGBA_4;
                float3 _Combine_548F329E_RGB_5;
                float2 _Combine_548F329E_RG_6;
                Unity_Combine_float(_Split_AC92423D_R_1, _Split_AC92423D_G_2, 0, 0, _Combine_548F329E_RGBA_4, _Combine_548F329E_RGB_5, _Combine_548F329E_RG_6);
                float4 _Combine_B40C924A_RGBA_4;
                float3 _Combine_B40C924A_RGB_5;
                float2 _Combine_B40C924A_RG_6;
                Unity_Combine_float(_Split_AC92423D_B_3, _Split_AC92423D_A_4, 0, 0, _Combine_B40C924A_RGBA_4, _Combine_B40C924A_RGB_5, _Combine_B40C924A_RG_6);
                float _Remap_F10A160D_Out_3;
                Unity_Remap_float(_Power_E1284C32_Out_2, _Combine_548F329E_RG_6, _Combine_B40C924A_RG_6, _Remap_F10A160D_Out_3);
                float _Absolute_4FC64134_Out_1;
                Unity_Absolute_float(_Remap_F10A160D_Out_3, _Absolute_4FC64134_Out_1);
                float _Smoothstep_56070F3_Out_3;
                Unity_Smoothstep_float(_Property_34CB0090_Out_0, _Property_AD125D68_Out_0, _Absolute_4FC64134_Out_1, _Smoothstep_56070F3_Out_3);
                float _Property_1671376A_Out_0 = Vector1_9058DE8C;
                float _Multiply_E8657CCC_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1671376A_Out_0, _Multiply_E8657CCC_Out_2);
                float2 _TilingAndOffset_B64A0F1F_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_E8657CCC_Out_2.xx), _TilingAndOffset_B64A0F1F_Out_3);
                float _Property_12404564_Out_0 = Vector1_5536221E;
                float _GradientNoise_3F907965_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B64A0F1F_Out_3, _Property_12404564_Out_0, _GradientNoise_3F907965_Out_2);
                float _Property_94C69BD5_Out_0 = Vector1_350643F1;
                float _Multiply_61FF3188_Out_2;
                Unity_Multiply_float(_GradientNoise_3F907965_Out_2, _Property_94C69BD5_Out_0, _Multiply_61FF3188_Out_2);
                float _Add_12AD4DE7_Out_2;
                Unity_Add_float(_Smoothstep_56070F3_Out_3, _Multiply_61FF3188_Out_2, _Add_12AD4DE7_Out_2);
                float _Add_B7625F11_Out_2;
                Unity_Add_float(1, _Property_94C69BD5_Out_0, _Add_B7625F11_Out_2);
                float _Divide_A999571A_Out_2;
                Unity_Divide_float(_Add_12AD4DE7_Out_2, _Add_B7625F11_Out_2, _Divide_A999571A_Out_2);
                float4 _Lerp_FA2A75DB_Out_3;
                Unity_Lerp_float4(_Property_25FBF6D9_Out_0, _Property_1400C896_Out_0, (_Divide_A999571A_Out_2.xxxx), _Lerp_FA2A75DB_Out_3);
                float _Property_1B76C741_Out_0 = Vector1_B4ED823D;
                float _FresnelEffect_B595F760_Out_3;
                Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_1B76C741_Out_0, _FresnelEffect_B595F760_Out_3);
                float _Multiply_244D326F_Out_2;
                Unity_Multiply_float(_Divide_A999571A_Out_2, _FresnelEffect_B595F760_Out_3, _Multiply_244D326F_Out_2);
                float _Property_5F13349D_Out_0 = Vector1_2BA361A8;
                float _Multiply_ACABB863_Out_2;
                Unity_Multiply_float(_Multiply_244D326F_Out_2, _Property_5F13349D_Out_0, _Multiply_ACABB863_Out_2);
                float4 _Add_B4DFAD13_Out_2;
                Unity_Add_float4(_Lerp_FA2A75DB_Out_3, (_Multiply_ACABB863_Out_2.xxxx), _Add_B4DFAD13_Out_2);
                float _Property_A1D4FE60_Out_0 = Vector1_2A80C598;
                float4 _Multiply_E393F590_Out_2;
                Unity_Multiply_float(_Add_B4DFAD13_Out_2, (_Property_A1D4FE60_Out_0.xxxx), _Multiply_E393F590_Out_2);
                float _SceneDepth_6FF4059F_Out_1;
                Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_6FF4059F_Out_1);
                float4 _ScreenPosition_874D7EFB_Out_0 = IN.ScreenPosition;
                float _Split_984E65AF_R_1 = _ScreenPosition_874D7EFB_Out_0[0];
                float _Split_984E65AF_G_2 = _ScreenPosition_874D7EFB_Out_0[1];
                float _Split_984E65AF_B_3 = _ScreenPosition_874D7EFB_Out_0[2];
                float _Split_984E65AF_A_4 = _ScreenPosition_874D7EFB_Out_0[3];
                float _Subtract_8113911_Out_2;
                Unity_Subtract_float(_Split_984E65AF_A_4, 1, _Subtract_8113911_Out_2);
                float _Subtract_6632FEEB_Out_2;
                Unity_Subtract_float(_SceneDepth_6FF4059F_Out_1, _Subtract_8113911_Out_2, _Subtract_6632FEEB_Out_2);
                float _Property_6F4C582_Out_0 = Vector1_EED1A887;
                float _Divide_98F0CC68_Out_2;
                Unity_Divide_float(_Subtract_6632FEEB_Out_2, _Property_6F4C582_Out_0, _Divide_98F0CC68_Out_2);
                float _Saturate_6FABCB9A_Out_1;
                Unity_Saturate_float(_Divide_98F0CC68_Out_2, _Saturate_6FABCB9A_Out_1);
                float _Smoothstep_3808517D_Out_3;
                Unity_Smoothstep_float(0, 1, _Saturate_6FABCB9A_Out_1, _Smoothstep_3808517D_Out_3);
                surface.Albedo = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                surface.Normal = IN.TangentSpaceNormal;
                surface.Emission = (_Multiply_E393F590_Out_2.xyz);
                surface.Metallic = 0;
                surface.Smoothness = 0.5;
                surface.Occlusion = 1;
                surface.Alpha = _Smoothstep_3808517D_Out_3;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv1 : TEXCOORD1;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float3 normalWS;
                float4 tangentWS;
                float3 viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                float2 lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                float3 sh;
                #endif
                float4 fogFactorAndVertexLight;
                float4 shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if defined(LIGHTMAP_ON)
                #endif
                #if !defined(LIGHTMAP_ON)
                #endif
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float3 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                float3 interp03 : TEXCOORD3;
                float2 interp04 : TEXCOORD4;
                float3 interp05 : TEXCOORD5;
                float4 interp06 : TEXCOORD6;
                float4 interp07 : TEXCOORD7;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyz = input.viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                output.interp04.xy = input.lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.interp05.xyz = input.sh;
                #endif
                output.interp06.xyzw = input.fogFactorAndVertexLight;
                output.interp07.xyzw = input.shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.viewDirectionWS = input.interp03.xyz;
                #if defined(LIGHTMAP_ON)
                output.lightmapUV = input.interp04.xy;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.sh = input.interp05.xyz;
                #endif
                output.fogFactorAndVertexLight = input.interp06.xyzw;
                output.shadowCoord = input.interp07.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.ObjectSpacePosition =         input.positionOS;
                output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                output.TimeParameters =              _TimeParameters.xyz;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            	float3 unnormalizedNormalWS = input.normalWS;
                const float renormFactor = 1.0 / length(unnormalizedNormalWS);
            
            
                output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
                output.WorldSpacePosition =          input.positionWS;
                output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define VARYINGS_NEED_POSITION_WS 
            #define FEATURES_GRAPH_VERTEX
            #pragma multi_compile_instancing
            #define SHADERPASS_SHADOWCASTER
            #define REQUIRE_DEPTH_TEXTURE
            
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 Vector4_484BA326;
            float Vector1_8F075DBC;
            float Vector1_8AAD665C;
            float Vector1_E43D3892;
            float4 Vector4_1696EE66;
            float4 Color_2CFB5E2A;
            float4 Color_58195353;
            float Vector1_FADC0395;
            float Vector1_E505CA4B;
            float Vector1_4CE104D9;
            float Vector1_5536221E;
            float Vector1_9058DE8C;
            float Vector1_350643F1;
            float Vector1_2A80C598;
            float Vector1_A664731F;
            float Vector1_B4ED823D;
            float Vector1_2BA361A8;
            float Vector1_EED1A887;
            CBUFFER_END
        
            // Graph Functions
            
            void Unity_Distance_float3(float3 A, float3 B, out float Out)
            {
                Out = distance(A, B);
            }
            
            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }
            
            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Rotate_About_Axis_Degrees_float(float3 In, float3 Axis, float Rotation, out float3 Out)
            {
                Rotation = radians(Rotation);
            
                float s = sin(Rotation);
                float c = cos(Rotation);
                float one_minus_c = 1.0 - c;
                
                Axis = normalize(Axis);
            
                float3x3 rot_mat = { one_minus_c * Axis.x * Axis.x + c,            one_minus_c * Axis.x * Axis.y - Axis.z * s,     one_minus_c * Axis.z * Axis.x + Axis.y * s,
                                          one_minus_c * Axis.x * Axis.y + Axis.z * s,   one_minus_c * Axis.y * Axis.y + c,              one_minus_c * Axis.y * Axis.z - Axis.x * s,
                                          one_minus_c * Axis.z * Axis.x - Axis.y * s,   one_minus_c * Axis.y * Axis.z + Axis.x * s,     one_minus_c * Axis.z * Axis.z + c
                                        };
            
                Out = mul(rot_mat,  In);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            
            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { 
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float3 WorldSpacePosition;
                float3 TimeParameters;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Distance_D85DF7AD_Out_2;
                Unity_Distance_float3(SHADERGRAPH_OBJECT_POSITION, IN.WorldSpacePosition, _Distance_D85DF7AD_Out_2);
                float _Property_117D8453_Out_0 = Vector1_A664731F;
                float _Divide_6EF58E3_Out_2;
                Unity_Divide_float(_Distance_D85DF7AD_Out_2, _Property_117D8453_Out_0, _Divide_6EF58E3_Out_2);
                float _Power_33FF8791_Out_2;
                Unity_Power_float(_Divide_6EF58E3_Out_2, 3, _Power_33FF8791_Out_2);
                float3 _Multiply_E4027504_Out_2;
                Unity_Multiply_float(IN.WorldSpaceNormal, (_Power_33FF8791_Out_2.xxx), _Multiply_E4027504_Out_2);
                float _Property_34CB0090_Out_0 = Vector1_FADC0395;
                float _Property_AD125D68_Out_0 = Vector1_E505CA4B;
                float4 _Property_766D591A_Out_0 = Vector4_484BA326;
                float _Split_46EE164F_R_1 = _Property_766D591A_Out_0[0];
                float _Split_46EE164F_G_2 = _Property_766D591A_Out_0[1];
                float _Split_46EE164F_B_3 = _Property_766D591A_Out_0[2];
                float _Split_46EE164F_A_4 = _Property_766D591A_Out_0[3];
                float3 _RotateAboutAxis_A00D475E_Out_3;
                Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpacePosition, (_Property_766D591A_Out_0.xyz), _Split_46EE164F_A_4, _RotateAboutAxis_A00D475E_Out_3);
                float _Property_1195B7B7_Out_0 = Vector1_8AAD665C;
                float _Multiply_365B1AAE_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1195B7B7_Out_0, _Multiply_365B1AAE_Out_2);
                float2 _TilingAndOffset_94B76B59_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_365B1AAE_Out_2.xx), _TilingAndOffset_94B76B59_Out_3);
                float _Property_EEA5F82E_Out_0 = Vector1_8F075DBC;
                float _GradientNoise_4DF641FB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_94B76B59_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_4DF641FB_Out_2);
                float2 _TilingAndOffset_D3C6639A_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), float2 (0, 0), _TilingAndOffset_D3C6639A_Out_3);
                float _GradientNoise_ADD9B44D_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_D3C6639A_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_ADD9B44D_Out_2);
                float _Add_29BFF883_Out_2;
                Unity_Add_float(_GradientNoise_4DF641FB_Out_2, _GradientNoise_ADD9B44D_Out_2, _Add_29BFF883_Out_2);
                float _Divide_40E6AFDE_Out_2;
                Unity_Divide_float(_Add_29BFF883_Out_2, 2, _Divide_40E6AFDE_Out_2);
                float _Saturate_7F44B33_Out_1;
                Unity_Saturate_float(_Divide_40E6AFDE_Out_2, _Saturate_7F44B33_Out_1);
                float _Property_8444523D_Out_0 = Vector1_4CE104D9;
                float _Power_E1284C32_Out_2;
                Unity_Power_float(_Saturate_7F44B33_Out_1, _Property_8444523D_Out_0, _Power_E1284C32_Out_2);
                float4 _Property_9D282FC2_Out_0 = Vector4_1696EE66;
                float _Split_AC92423D_R_1 = _Property_9D282FC2_Out_0[0];
                float _Split_AC92423D_G_2 = _Property_9D282FC2_Out_0[1];
                float _Split_AC92423D_B_3 = _Property_9D282FC2_Out_0[2];
                float _Split_AC92423D_A_4 = _Property_9D282FC2_Out_0[3];
                float4 _Combine_548F329E_RGBA_4;
                float3 _Combine_548F329E_RGB_5;
                float2 _Combine_548F329E_RG_6;
                Unity_Combine_float(_Split_AC92423D_R_1, _Split_AC92423D_G_2, 0, 0, _Combine_548F329E_RGBA_4, _Combine_548F329E_RGB_5, _Combine_548F329E_RG_6);
                float4 _Combine_B40C924A_RGBA_4;
                float3 _Combine_B40C924A_RGB_5;
                float2 _Combine_B40C924A_RG_6;
                Unity_Combine_float(_Split_AC92423D_B_3, _Split_AC92423D_A_4, 0, 0, _Combine_B40C924A_RGBA_4, _Combine_B40C924A_RGB_5, _Combine_B40C924A_RG_6);
                float _Remap_F10A160D_Out_3;
                Unity_Remap_float(_Power_E1284C32_Out_2, _Combine_548F329E_RG_6, _Combine_B40C924A_RG_6, _Remap_F10A160D_Out_3);
                float _Absolute_4FC64134_Out_1;
                Unity_Absolute_float(_Remap_F10A160D_Out_3, _Absolute_4FC64134_Out_1);
                float _Smoothstep_56070F3_Out_3;
                Unity_Smoothstep_float(_Property_34CB0090_Out_0, _Property_AD125D68_Out_0, _Absolute_4FC64134_Out_1, _Smoothstep_56070F3_Out_3);
                float _Property_1671376A_Out_0 = Vector1_9058DE8C;
                float _Multiply_E8657CCC_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1671376A_Out_0, _Multiply_E8657CCC_Out_2);
                float2 _TilingAndOffset_B64A0F1F_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_E8657CCC_Out_2.xx), _TilingAndOffset_B64A0F1F_Out_3);
                float _Property_12404564_Out_0 = Vector1_5536221E;
                float _GradientNoise_3F907965_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B64A0F1F_Out_3, _Property_12404564_Out_0, _GradientNoise_3F907965_Out_2);
                float _Property_94C69BD5_Out_0 = Vector1_350643F1;
                float _Multiply_61FF3188_Out_2;
                Unity_Multiply_float(_GradientNoise_3F907965_Out_2, _Property_94C69BD5_Out_0, _Multiply_61FF3188_Out_2);
                float _Add_12AD4DE7_Out_2;
                Unity_Add_float(_Smoothstep_56070F3_Out_3, _Multiply_61FF3188_Out_2, _Add_12AD4DE7_Out_2);
                float _Add_B7625F11_Out_2;
                Unity_Add_float(1, _Property_94C69BD5_Out_0, _Add_B7625F11_Out_2);
                float _Divide_A999571A_Out_2;
                Unity_Divide_float(_Add_12AD4DE7_Out_2, _Add_B7625F11_Out_2, _Divide_A999571A_Out_2);
                float3 _Multiply_EF48E583_Out_2;
                Unity_Multiply_float(IN.ObjectSpaceNormal, (_Divide_A999571A_Out_2.xxx), _Multiply_EF48E583_Out_2);
                float _Property_347E548F_Out_0 = Vector1_E43D3892;
                float3 _Multiply_E6AAE955_Out_2;
                Unity_Multiply_float(_Multiply_EF48E583_Out_2, (_Property_347E548F_Out_0.xxx), _Multiply_E6AAE955_Out_2);
                float3 _Add_EDAC08D6_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_E6AAE955_Out_2, _Add_EDAC08D6_Out_2);
                float3 _Add_5924731B_Out_2;
                Unity_Add_float3(_Multiply_E4027504_Out_2, _Add_EDAC08D6_Out_2, _Add_5924731B_Out_2);
                description.VertexPosition = _Add_5924731B_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float3 WorldSpacePosition;
                float4 ScreenPosition;
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _SceneDepth_6FF4059F_Out_1;
                Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_6FF4059F_Out_1);
                float4 _ScreenPosition_874D7EFB_Out_0 = IN.ScreenPosition;
                float _Split_984E65AF_R_1 = _ScreenPosition_874D7EFB_Out_0[0];
                float _Split_984E65AF_G_2 = _ScreenPosition_874D7EFB_Out_0[1];
                float _Split_984E65AF_B_3 = _ScreenPosition_874D7EFB_Out_0[2];
                float _Split_984E65AF_A_4 = _ScreenPosition_874D7EFB_Out_0[3];
                float _Subtract_8113911_Out_2;
                Unity_Subtract_float(_Split_984E65AF_A_4, 1, _Subtract_8113911_Out_2);
                float _Subtract_6632FEEB_Out_2;
                Unity_Subtract_float(_SceneDepth_6FF4059F_Out_1, _Subtract_8113911_Out_2, _Subtract_6632FEEB_Out_2);
                float _Property_6F4C582_Out_0 = Vector1_EED1A887;
                float _Divide_98F0CC68_Out_2;
                Unity_Divide_float(_Subtract_6632FEEB_Out_2, _Property_6F4C582_Out_0, _Divide_98F0CC68_Out_2);
                float _Saturate_6FABCB9A_Out_1;
                Unity_Saturate_float(_Divide_98F0CC68_Out_2, _Saturate_6FABCB9A_Out_1);
                float _Smoothstep_3808517D_Out_3;
                Unity_Smoothstep_float(0, 1, _Saturate_6FABCB9A_Out_1, _Smoothstep_3808517D_Out_3);
                surface.Alpha = _Smoothstep_3808517D_Out_3;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.ObjectSpacePosition =         input.positionOS;
                output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                output.TimeParameters =              _TimeParameters.xyz;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.WorldSpacePosition =          input.positionWS;
                output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define VARYINGS_NEED_POSITION_WS 
            #define FEATURES_GRAPH_VERTEX
            #pragma multi_compile_instancing
            #define SHADERPASS_DEPTHONLY
            #define REQUIRE_DEPTH_TEXTURE
            
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 Vector4_484BA326;
            float Vector1_8F075DBC;
            float Vector1_8AAD665C;
            float Vector1_E43D3892;
            float4 Vector4_1696EE66;
            float4 Color_2CFB5E2A;
            float4 Color_58195353;
            float Vector1_FADC0395;
            float Vector1_E505CA4B;
            float Vector1_4CE104D9;
            float Vector1_5536221E;
            float Vector1_9058DE8C;
            float Vector1_350643F1;
            float Vector1_2A80C598;
            float Vector1_A664731F;
            float Vector1_B4ED823D;
            float Vector1_2BA361A8;
            float Vector1_EED1A887;
            CBUFFER_END
        
            // Graph Functions
            
            void Unity_Distance_float3(float3 A, float3 B, out float Out)
            {
                Out = distance(A, B);
            }
            
            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }
            
            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Rotate_About_Axis_Degrees_float(float3 In, float3 Axis, float Rotation, out float3 Out)
            {
                Rotation = radians(Rotation);
            
                float s = sin(Rotation);
                float c = cos(Rotation);
                float one_minus_c = 1.0 - c;
                
                Axis = normalize(Axis);
            
                float3x3 rot_mat = { one_minus_c * Axis.x * Axis.x + c,            one_minus_c * Axis.x * Axis.y - Axis.z * s,     one_minus_c * Axis.z * Axis.x + Axis.y * s,
                                          one_minus_c * Axis.x * Axis.y + Axis.z * s,   one_minus_c * Axis.y * Axis.y + c,              one_minus_c * Axis.y * Axis.z - Axis.x * s,
                                          one_minus_c * Axis.z * Axis.x - Axis.y * s,   one_minus_c * Axis.y * Axis.z + Axis.x * s,     one_minus_c * Axis.z * Axis.z + c
                                        };
            
                Out = mul(rot_mat,  In);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            
            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { 
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float3 WorldSpacePosition;
                float3 TimeParameters;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Distance_D85DF7AD_Out_2;
                Unity_Distance_float3(SHADERGRAPH_OBJECT_POSITION, IN.WorldSpacePosition, _Distance_D85DF7AD_Out_2);
                float _Property_117D8453_Out_0 = Vector1_A664731F;
                float _Divide_6EF58E3_Out_2;
                Unity_Divide_float(_Distance_D85DF7AD_Out_2, _Property_117D8453_Out_0, _Divide_6EF58E3_Out_2);
                float _Power_33FF8791_Out_2;
                Unity_Power_float(_Divide_6EF58E3_Out_2, 3, _Power_33FF8791_Out_2);
                float3 _Multiply_E4027504_Out_2;
                Unity_Multiply_float(IN.WorldSpaceNormal, (_Power_33FF8791_Out_2.xxx), _Multiply_E4027504_Out_2);
                float _Property_34CB0090_Out_0 = Vector1_FADC0395;
                float _Property_AD125D68_Out_0 = Vector1_E505CA4B;
                float4 _Property_766D591A_Out_0 = Vector4_484BA326;
                float _Split_46EE164F_R_1 = _Property_766D591A_Out_0[0];
                float _Split_46EE164F_G_2 = _Property_766D591A_Out_0[1];
                float _Split_46EE164F_B_3 = _Property_766D591A_Out_0[2];
                float _Split_46EE164F_A_4 = _Property_766D591A_Out_0[3];
                float3 _RotateAboutAxis_A00D475E_Out_3;
                Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpacePosition, (_Property_766D591A_Out_0.xyz), _Split_46EE164F_A_4, _RotateAboutAxis_A00D475E_Out_3);
                float _Property_1195B7B7_Out_0 = Vector1_8AAD665C;
                float _Multiply_365B1AAE_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1195B7B7_Out_0, _Multiply_365B1AAE_Out_2);
                float2 _TilingAndOffset_94B76B59_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_365B1AAE_Out_2.xx), _TilingAndOffset_94B76B59_Out_3);
                float _Property_EEA5F82E_Out_0 = Vector1_8F075DBC;
                float _GradientNoise_4DF641FB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_94B76B59_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_4DF641FB_Out_2);
                float2 _TilingAndOffset_D3C6639A_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), float2 (0, 0), _TilingAndOffset_D3C6639A_Out_3);
                float _GradientNoise_ADD9B44D_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_D3C6639A_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_ADD9B44D_Out_2);
                float _Add_29BFF883_Out_2;
                Unity_Add_float(_GradientNoise_4DF641FB_Out_2, _GradientNoise_ADD9B44D_Out_2, _Add_29BFF883_Out_2);
                float _Divide_40E6AFDE_Out_2;
                Unity_Divide_float(_Add_29BFF883_Out_2, 2, _Divide_40E6AFDE_Out_2);
                float _Saturate_7F44B33_Out_1;
                Unity_Saturate_float(_Divide_40E6AFDE_Out_2, _Saturate_7F44B33_Out_1);
                float _Property_8444523D_Out_0 = Vector1_4CE104D9;
                float _Power_E1284C32_Out_2;
                Unity_Power_float(_Saturate_7F44B33_Out_1, _Property_8444523D_Out_0, _Power_E1284C32_Out_2);
                float4 _Property_9D282FC2_Out_0 = Vector4_1696EE66;
                float _Split_AC92423D_R_1 = _Property_9D282FC2_Out_0[0];
                float _Split_AC92423D_G_2 = _Property_9D282FC2_Out_0[1];
                float _Split_AC92423D_B_3 = _Property_9D282FC2_Out_0[2];
                float _Split_AC92423D_A_4 = _Property_9D282FC2_Out_0[3];
                float4 _Combine_548F329E_RGBA_4;
                float3 _Combine_548F329E_RGB_5;
                float2 _Combine_548F329E_RG_6;
                Unity_Combine_float(_Split_AC92423D_R_1, _Split_AC92423D_G_2, 0, 0, _Combine_548F329E_RGBA_4, _Combine_548F329E_RGB_5, _Combine_548F329E_RG_6);
                float4 _Combine_B40C924A_RGBA_4;
                float3 _Combine_B40C924A_RGB_5;
                float2 _Combine_B40C924A_RG_6;
                Unity_Combine_float(_Split_AC92423D_B_3, _Split_AC92423D_A_4, 0, 0, _Combine_B40C924A_RGBA_4, _Combine_B40C924A_RGB_5, _Combine_B40C924A_RG_6);
                float _Remap_F10A160D_Out_3;
                Unity_Remap_float(_Power_E1284C32_Out_2, _Combine_548F329E_RG_6, _Combine_B40C924A_RG_6, _Remap_F10A160D_Out_3);
                float _Absolute_4FC64134_Out_1;
                Unity_Absolute_float(_Remap_F10A160D_Out_3, _Absolute_4FC64134_Out_1);
                float _Smoothstep_56070F3_Out_3;
                Unity_Smoothstep_float(_Property_34CB0090_Out_0, _Property_AD125D68_Out_0, _Absolute_4FC64134_Out_1, _Smoothstep_56070F3_Out_3);
                float _Property_1671376A_Out_0 = Vector1_9058DE8C;
                float _Multiply_E8657CCC_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1671376A_Out_0, _Multiply_E8657CCC_Out_2);
                float2 _TilingAndOffset_B64A0F1F_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_E8657CCC_Out_2.xx), _TilingAndOffset_B64A0F1F_Out_3);
                float _Property_12404564_Out_0 = Vector1_5536221E;
                float _GradientNoise_3F907965_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B64A0F1F_Out_3, _Property_12404564_Out_0, _GradientNoise_3F907965_Out_2);
                float _Property_94C69BD5_Out_0 = Vector1_350643F1;
                float _Multiply_61FF3188_Out_2;
                Unity_Multiply_float(_GradientNoise_3F907965_Out_2, _Property_94C69BD5_Out_0, _Multiply_61FF3188_Out_2);
                float _Add_12AD4DE7_Out_2;
                Unity_Add_float(_Smoothstep_56070F3_Out_3, _Multiply_61FF3188_Out_2, _Add_12AD4DE7_Out_2);
                float _Add_B7625F11_Out_2;
                Unity_Add_float(1, _Property_94C69BD5_Out_0, _Add_B7625F11_Out_2);
                float _Divide_A999571A_Out_2;
                Unity_Divide_float(_Add_12AD4DE7_Out_2, _Add_B7625F11_Out_2, _Divide_A999571A_Out_2);
                float3 _Multiply_EF48E583_Out_2;
                Unity_Multiply_float(IN.ObjectSpaceNormal, (_Divide_A999571A_Out_2.xxx), _Multiply_EF48E583_Out_2);
                float _Property_347E548F_Out_0 = Vector1_E43D3892;
                float3 _Multiply_E6AAE955_Out_2;
                Unity_Multiply_float(_Multiply_EF48E583_Out_2, (_Property_347E548F_Out_0.xxx), _Multiply_E6AAE955_Out_2);
                float3 _Add_EDAC08D6_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_E6AAE955_Out_2, _Add_EDAC08D6_Out_2);
                float3 _Add_5924731B_Out_2;
                Unity_Add_float3(_Multiply_E4027504_Out_2, _Add_EDAC08D6_Out_2, _Add_5924731B_Out_2);
                description.VertexPosition = _Add_5924731B_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float3 WorldSpacePosition;
                float4 ScreenPosition;
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _SceneDepth_6FF4059F_Out_1;
                Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_6FF4059F_Out_1);
                float4 _ScreenPosition_874D7EFB_Out_0 = IN.ScreenPosition;
                float _Split_984E65AF_R_1 = _ScreenPosition_874D7EFB_Out_0[0];
                float _Split_984E65AF_G_2 = _ScreenPosition_874D7EFB_Out_0[1];
                float _Split_984E65AF_B_3 = _ScreenPosition_874D7EFB_Out_0[2];
                float _Split_984E65AF_A_4 = _ScreenPosition_874D7EFB_Out_0[3];
                float _Subtract_8113911_Out_2;
                Unity_Subtract_float(_Split_984E65AF_A_4, 1, _Subtract_8113911_Out_2);
                float _Subtract_6632FEEB_Out_2;
                Unity_Subtract_float(_SceneDepth_6FF4059F_Out_1, _Subtract_8113911_Out_2, _Subtract_6632FEEB_Out_2);
                float _Property_6F4C582_Out_0 = Vector1_EED1A887;
                float _Divide_98F0CC68_Out_2;
                Unity_Divide_float(_Subtract_6632FEEB_Out_2, _Property_6F4C582_Out_0, _Divide_98F0CC68_Out_2);
                float _Saturate_6FABCB9A_Out_1;
                Unity_Saturate_float(_Divide_98F0CC68_Out_2, _Saturate_6FABCB9A_Out_1);
                float _Smoothstep_3808517D_Out_3;
                Unity_Smoothstep_float(0, 1, _Saturate_6FABCB9A_Out_1, _Smoothstep_3808517D_Out_3);
                surface.Alpha = _Smoothstep_3808517D_Out_3;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.ObjectSpacePosition =         input.positionOS;
                output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                output.TimeParameters =              _TimeParameters.xyz;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.WorldSpacePosition =          input.positionWS;
                output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "Meta"
            Tags 
            { 
                "LightMode" = "Meta"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
        
            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define VARYINGS_NEED_POSITION_WS 
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define FEATURES_GRAPH_VERTEX
            #pragma multi_compile_instancing
            #define SHADERPASS_META
            #define REQUIRE_DEPTH_TEXTURE
            
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 Vector4_484BA326;
            float Vector1_8F075DBC;
            float Vector1_8AAD665C;
            float Vector1_E43D3892;
            float4 Vector4_1696EE66;
            float4 Color_2CFB5E2A;
            float4 Color_58195353;
            float Vector1_FADC0395;
            float Vector1_E505CA4B;
            float Vector1_4CE104D9;
            float Vector1_5536221E;
            float Vector1_9058DE8C;
            float Vector1_350643F1;
            float Vector1_2A80C598;
            float Vector1_A664731F;
            float Vector1_B4ED823D;
            float Vector1_2BA361A8;
            float Vector1_EED1A887;
            CBUFFER_END
        
            // Graph Functions
            
            void Unity_Distance_float3(float3 A, float3 B, out float Out)
            {
                Out = distance(A, B);
            }
            
            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }
            
            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Rotate_About_Axis_Degrees_float(float3 In, float3 Axis, float Rotation, out float3 Out)
            {
                Rotation = radians(Rotation);
            
                float s = sin(Rotation);
                float c = cos(Rotation);
                float one_minus_c = 1.0 - c;
                
                Axis = normalize(Axis);
            
                float3x3 rot_mat = { one_minus_c * Axis.x * Axis.x + c,            one_minus_c * Axis.x * Axis.y - Axis.z * s,     one_minus_c * Axis.z * Axis.x + Axis.y * s,
                                          one_minus_c * Axis.x * Axis.y + Axis.z * s,   one_minus_c * Axis.y * Axis.y + c,              one_minus_c * Axis.y * Axis.z - Axis.x * s,
                                          one_minus_c * Axis.z * Axis.x - Axis.y * s,   one_minus_c * Axis.y * Axis.z + Axis.x * s,     one_minus_c * Axis.z * Axis.z + c
                                        };
            
                Out = mul(rot_mat,  In);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            
            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { 
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
            {
                Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
            }
            
            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float3 WorldSpacePosition;
                float3 TimeParameters;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Distance_D85DF7AD_Out_2;
                Unity_Distance_float3(SHADERGRAPH_OBJECT_POSITION, IN.WorldSpacePosition, _Distance_D85DF7AD_Out_2);
                float _Property_117D8453_Out_0 = Vector1_A664731F;
                float _Divide_6EF58E3_Out_2;
                Unity_Divide_float(_Distance_D85DF7AD_Out_2, _Property_117D8453_Out_0, _Divide_6EF58E3_Out_2);
                float _Power_33FF8791_Out_2;
                Unity_Power_float(_Divide_6EF58E3_Out_2, 3, _Power_33FF8791_Out_2);
                float3 _Multiply_E4027504_Out_2;
                Unity_Multiply_float(IN.WorldSpaceNormal, (_Power_33FF8791_Out_2.xxx), _Multiply_E4027504_Out_2);
                float _Property_34CB0090_Out_0 = Vector1_FADC0395;
                float _Property_AD125D68_Out_0 = Vector1_E505CA4B;
                float4 _Property_766D591A_Out_0 = Vector4_484BA326;
                float _Split_46EE164F_R_1 = _Property_766D591A_Out_0[0];
                float _Split_46EE164F_G_2 = _Property_766D591A_Out_0[1];
                float _Split_46EE164F_B_3 = _Property_766D591A_Out_0[2];
                float _Split_46EE164F_A_4 = _Property_766D591A_Out_0[3];
                float3 _RotateAboutAxis_A00D475E_Out_3;
                Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpacePosition, (_Property_766D591A_Out_0.xyz), _Split_46EE164F_A_4, _RotateAboutAxis_A00D475E_Out_3);
                float _Property_1195B7B7_Out_0 = Vector1_8AAD665C;
                float _Multiply_365B1AAE_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1195B7B7_Out_0, _Multiply_365B1AAE_Out_2);
                float2 _TilingAndOffset_94B76B59_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_365B1AAE_Out_2.xx), _TilingAndOffset_94B76B59_Out_3);
                float _Property_EEA5F82E_Out_0 = Vector1_8F075DBC;
                float _GradientNoise_4DF641FB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_94B76B59_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_4DF641FB_Out_2);
                float2 _TilingAndOffset_D3C6639A_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), float2 (0, 0), _TilingAndOffset_D3C6639A_Out_3);
                float _GradientNoise_ADD9B44D_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_D3C6639A_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_ADD9B44D_Out_2);
                float _Add_29BFF883_Out_2;
                Unity_Add_float(_GradientNoise_4DF641FB_Out_2, _GradientNoise_ADD9B44D_Out_2, _Add_29BFF883_Out_2);
                float _Divide_40E6AFDE_Out_2;
                Unity_Divide_float(_Add_29BFF883_Out_2, 2, _Divide_40E6AFDE_Out_2);
                float _Saturate_7F44B33_Out_1;
                Unity_Saturate_float(_Divide_40E6AFDE_Out_2, _Saturate_7F44B33_Out_1);
                float _Property_8444523D_Out_0 = Vector1_4CE104D9;
                float _Power_E1284C32_Out_2;
                Unity_Power_float(_Saturate_7F44B33_Out_1, _Property_8444523D_Out_0, _Power_E1284C32_Out_2);
                float4 _Property_9D282FC2_Out_0 = Vector4_1696EE66;
                float _Split_AC92423D_R_1 = _Property_9D282FC2_Out_0[0];
                float _Split_AC92423D_G_2 = _Property_9D282FC2_Out_0[1];
                float _Split_AC92423D_B_3 = _Property_9D282FC2_Out_0[2];
                float _Split_AC92423D_A_4 = _Property_9D282FC2_Out_0[3];
                float4 _Combine_548F329E_RGBA_4;
                float3 _Combine_548F329E_RGB_5;
                float2 _Combine_548F329E_RG_6;
                Unity_Combine_float(_Split_AC92423D_R_1, _Split_AC92423D_G_2, 0, 0, _Combine_548F329E_RGBA_4, _Combine_548F329E_RGB_5, _Combine_548F329E_RG_6);
                float4 _Combine_B40C924A_RGBA_4;
                float3 _Combine_B40C924A_RGB_5;
                float2 _Combine_B40C924A_RG_6;
                Unity_Combine_float(_Split_AC92423D_B_3, _Split_AC92423D_A_4, 0, 0, _Combine_B40C924A_RGBA_4, _Combine_B40C924A_RGB_5, _Combine_B40C924A_RG_6);
                float _Remap_F10A160D_Out_3;
                Unity_Remap_float(_Power_E1284C32_Out_2, _Combine_548F329E_RG_6, _Combine_B40C924A_RG_6, _Remap_F10A160D_Out_3);
                float _Absolute_4FC64134_Out_1;
                Unity_Absolute_float(_Remap_F10A160D_Out_3, _Absolute_4FC64134_Out_1);
                float _Smoothstep_56070F3_Out_3;
                Unity_Smoothstep_float(_Property_34CB0090_Out_0, _Property_AD125D68_Out_0, _Absolute_4FC64134_Out_1, _Smoothstep_56070F3_Out_3);
                float _Property_1671376A_Out_0 = Vector1_9058DE8C;
                float _Multiply_E8657CCC_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1671376A_Out_0, _Multiply_E8657CCC_Out_2);
                float2 _TilingAndOffset_B64A0F1F_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_E8657CCC_Out_2.xx), _TilingAndOffset_B64A0F1F_Out_3);
                float _Property_12404564_Out_0 = Vector1_5536221E;
                float _GradientNoise_3F907965_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B64A0F1F_Out_3, _Property_12404564_Out_0, _GradientNoise_3F907965_Out_2);
                float _Property_94C69BD5_Out_0 = Vector1_350643F1;
                float _Multiply_61FF3188_Out_2;
                Unity_Multiply_float(_GradientNoise_3F907965_Out_2, _Property_94C69BD5_Out_0, _Multiply_61FF3188_Out_2);
                float _Add_12AD4DE7_Out_2;
                Unity_Add_float(_Smoothstep_56070F3_Out_3, _Multiply_61FF3188_Out_2, _Add_12AD4DE7_Out_2);
                float _Add_B7625F11_Out_2;
                Unity_Add_float(1, _Property_94C69BD5_Out_0, _Add_B7625F11_Out_2);
                float _Divide_A999571A_Out_2;
                Unity_Divide_float(_Add_12AD4DE7_Out_2, _Add_B7625F11_Out_2, _Divide_A999571A_Out_2);
                float3 _Multiply_EF48E583_Out_2;
                Unity_Multiply_float(IN.ObjectSpaceNormal, (_Divide_A999571A_Out_2.xxx), _Multiply_EF48E583_Out_2);
                float _Property_347E548F_Out_0 = Vector1_E43D3892;
                float3 _Multiply_E6AAE955_Out_2;
                Unity_Multiply_float(_Multiply_EF48E583_Out_2, (_Property_347E548F_Out_0.xxx), _Multiply_E6AAE955_Out_2);
                float3 _Add_EDAC08D6_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_E6AAE955_Out_2, _Add_EDAC08D6_Out_2);
                float3 _Add_5924731B_Out_2;
                Unity_Add_float3(_Multiply_E4027504_Out_2, _Add_EDAC08D6_Out_2, _Add_5924731B_Out_2);
                description.VertexPosition = _Add_5924731B_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 WorldSpaceNormal;
                float3 TangentSpaceNormal;
                float3 WorldSpaceViewDirection;
                float3 WorldSpacePosition;
                float4 ScreenPosition;
                float3 TimeParameters;
            };
            
            struct SurfaceDescription
            {
                float3 Albedo;
                float3 Emission;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _Property_25FBF6D9_Out_0 = Color_58195353;
                float4 _Property_1400C896_Out_0 = Color_2CFB5E2A;
                float _Property_34CB0090_Out_0 = Vector1_FADC0395;
                float _Property_AD125D68_Out_0 = Vector1_E505CA4B;
                float4 _Property_766D591A_Out_0 = Vector4_484BA326;
                float _Split_46EE164F_R_1 = _Property_766D591A_Out_0[0];
                float _Split_46EE164F_G_2 = _Property_766D591A_Out_0[1];
                float _Split_46EE164F_B_3 = _Property_766D591A_Out_0[2];
                float _Split_46EE164F_A_4 = _Property_766D591A_Out_0[3];
                float3 _RotateAboutAxis_A00D475E_Out_3;
                Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpacePosition, (_Property_766D591A_Out_0.xyz), _Split_46EE164F_A_4, _RotateAboutAxis_A00D475E_Out_3);
                float _Property_1195B7B7_Out_0 = Vector1_8AAD665C;
                float _Multiply_365B1AAE_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1195B7B7_Out_0, _Multiply_365B1AAE_Out_2);
                float2 _TilingAndOffset_94B76B59_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_365B1AAE_Out_2.xx), _TilingAndOffset_94B76B59_Out_3);
                float _Property_EEA5F82E_Out_0 = Vector1_8F075DBC;
                float _GradientNoise_4DF641FB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_94B76B59_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_4DF641FB_Out_2);
                float2 _TilingAndOffset_D3C6639A_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), float2 (0, 0), _TilingAndOffset_D3C6639A_Out_3);
                float _GradientNoise_ADD9B44D_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_D3C6639A_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_ADD9B44D_Out_2);
                float _Add_29BFF883_Out_2;
                Unity_Add_float(_GradientNoise_4DF641FB_Out_2, _GradientNoise_ADD9B44D_Out_2, _Add_29BFF883_Out_2);
                float _Divide_40E6AFDE_Out_2;
                Unity_Divide_float(_Add_29BFF883_Out_2, 2, _Divide_40E6AFDE_Out_2);
                float _Saturate_7F44B33_Out_1;
                Unity_Saturate_float(_Divide_40E6AFDE_Out_2, _Saturate_7F44B33_Out_1);
                float _Property_8444523D_Out_0 = Vector1_4CE104D9;
                float _Power_E1284C32_Out_2;
                Unity_Power_float(_Saturate_7F44B33_Out_1, _Property_8444523D_Out_0, _Power_E1284C32_Out_2);
                float4 _Property_9D282FC2_Out_0 = Vector4_1696EE66;
                float _Split_AC92423D_R_1 = _Property_9D282FC2_Out_0[0];
                float _Split_AC92423D_G_2 = _Property_9D282FC2_Out_0[1];
                float _Split_AC92423D_B_3 = _Property_9D282FC2_Out_0[2];
                float _Split_AC92423D_A_4 = _Property_9D282FC2_Out_0[3];
                float4 _Combine_548F329E_RGBA_4;
                float3 _Combine_548F329E_RGB_5;
                float2 _Combine_548F329E_RG_6;
                Unity_Combine_float(_Split_AC92423D_R_1, _Split_AC92423D_G_2, 0, 0, _Combine_548F329E_RGBA_4, _Combine_548F329E_RGB_5, _Combine_548F329E_RG_6);
                float4 _Combine_B40C924A_RGBA_4;
                float3 _Combine_B40C924A_RGB_5;
                float2 _Combine_B40C924A_RG_6;
                Unity_Combine_float(_Split_AC92423D_B_3, _Split_AC92423D_A_4, 0, 0, _Combine_B40C924A_RGBA_4, _Combine_B40C924A_RGB_5, _Combine_B40C924A_RG_6);
                float _Remap_F10A160D_Out_3;
                Unity_Remap_float(_Power_E1284C32_Out_2, _Combine_548F329E_RG_6, _Combine_B40C924A_RG_6, _Remap_F10A160D_Out_3);
                float _Absolute_4FC64134_Out_1;
                Unity_Absolute_float(_Remap_F10A160D_Out_3, _Absolute_4FC64134_Out_1);
                float _Smoothstep_56070F3_Out_3;
                Unity_Smoothstep_float(_Property_34CB0090_Out_0, _Property_AD125D68_Out_0, _Absolute_4FC64134_Out_1, _Smoothstep_56070F3_Out_3);
                float _Property_1671376A_Out_0 = Vector1_9058DE8C;
                float _Multiply_E8657CCC_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1671376A_Out_0, _Multiply_E8657CCC_Out_2);
                float2 _TilingAndOffset_B64A0F1F_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_E8657CCC_Out_2.xx), _TilingAndOffset_B64A0F1F_Out_3);
                float _Property_12404564_Out_0 = Vector1_5536221E;
                float _GradientNoise_3F907965_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B64A0F1F_Out_3, _Property_12404564_Out_0, _GradientNoise_3F907965_Out_2);
                float _Property_94C69BD5_Out_0 = Vector1_350643F1;
                float _Multiply_61FF3188_Out_2;
                Unity_Multiply_float(_GradientNoise_3F907965_Out_2, _Property_94C69BD5_Out_0, _Multiply_61FF3188_Out_2);
                float _Add_12AD4DE7_Out_2;
                Unity_Add_float(_Smoothstep_56070F3_Out_3, _Multiply_61FF3188_Out_2, _Add_12AD4DE7_Out_2);
                float _Add_B7625F11_Out_2;
                Unity_Add_float(1, _Property_94C69BD5_Out_0, _Add_B7625F11_Out_2);
                float _Divide_A999571A_Out_2;
                Unity_Divide_float(_Add_12AD4DE7_Out_2, _Add_B7625F11_Out_2, _Divide_A999571A_Out_2);
                float4 _Lerp_FA2A75DB_Out_3;
                Unity_Lerp_float4(_Property_25FBF6D9_Out_0, _Property_1400C896_Out_0, (_Divide_A999571A_Out_2.xxxx), _Lerp_FA2A75DB_Out_3);
                float _Property_1B76C741_Out_0 = Vector1_B4ED823D;
                float _FresnelEffect_B595F760_Out_3;
                Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, _Property_1B76C741_Out_0, _FresnelEffect_B595F760_Out_3);
                float _Multiply_244D326F_Out_2;
                Unity_Multiply_float(_Divide_A999571A_Out_2, _FresnelEffect_B595F760_Out_3, _Multiply_244D326F_Out_2);
                float _Property_5F13349D_Out_0 = Vector1_2BA361A8;
                float _Multiply_ACABB863_Out_2;
                Unity_Multiply_float(_Multiply_244D326F_Out_2, _Property_5F13349D_Out_0, _Multiply_ACABB863_Out_2);
                float4 _Add_B4DFAD13_Out_2;
                Unity_Add_float4(_Lerp_FA2A75DB_Out_3, (_Multiply_ACABB863_Out_2.xxxx), _Add_B4DFAD13_Out_2);
                float _Property_A1D4FE60_Out_0 = Vector1_2A80C598;
                float4 _Multiply_E393F590_Out_2;
                Unity_Multiply_float(_Add_B4DFAD13_Out_2, (_Property_A1D4FE60_Out_0.xxxx), _Multiply_E393F590_Out_2);
                float _SceneDepth_6FF4059F_Out_1;
                Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_6FF4059F_Out_1);
                float4 _ScreenPosition_874D7EFB_Out_0 = IN.ScreenPosition;
                float _Split_984E65AF_R_1 = _ScreenPosition_874D7EFB_Out_0[0];
                float _Split_984E65AF_G_2 = _ScreenPosition_874D7EFB_Out_0[1];
                float _Split_984E65AF_B_3 = _ScreenPosition_874D7EFB_Out_0[2];
                float _Split_984E65AF_A_4 = _ScreenPosition_874D7EFB_Out_0[3];
                float _Subtract_8113911_Out_2;
                Unity_Subtract_float(_Split_984E65AF_A_4, 1, _Subtract_8113911_Out_2);
                float _Subtract_6632FEEB_Out_2;
                Unity_Subtract_float(_SceneDepth_6FF4059F_Out_1, _Subtract_8113911_Out_2, _Subtract_6632FEEB_Out_2);
                float _Property_6F4C582_Out_0 = Vector1_EED1A887;
                float _Divide_98F0CC68_Out_2;
                Unity_Divide_float(_Subtract_6632FEEB_Out_2, _Property_6F4C582_Out_0, _Divide_98F0CC68_Out_2);
                float _Saturate_6FABCB9A_Out_1;
                Unity_Saturate_float(_Divide_98F0CC68_Out_2, _Saturate_6FABCB9A_Out_1);
                float _Smoothstep_3808517D_Out_3;
                Unity_Smoothstep_float(0, 1, _Saturate_6FABCB9A_Out_1, _Smoothstep_3808517D_Out_3);
                surface.Albedo = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                surface.Emission = (_Multiply_E393F590_Out_2.xyz);
                surface.Alpha = _Smoothstep_3808517D_Out_3;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                float3 normalWS;
                float3 viewDirectionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float3 interp01 : TEXCOORD1;
                float3 interp02 : TEXCOORD2;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyz = input.viewDirectionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.viewDirectionWS = input.interp02.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.ObjectSpacePosition =         input.positionOS;
                output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                output.TimeParameters =              _TimeParameters.xyz;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            	// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
            	float3 unnormalizedNormalWS = input.normalWS;
                const float renormFactor = 1.0 / length(unnormalizedNormalWS);
            
            
                output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.WorldSpaceViewDirection =     input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
                output.WorldSpacePosition =          input.positionWS;
                output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            // Name: <None>
            Tags 
            { 
                "LightMode" = "Universal2D"
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite Off
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define VARYINGS_NEED_POSITION_WS 
            #define FEATURES_GRAPH_VERTEX
            #pragma multi_compile_instancing
            #define SHADERPASS_2D
            #define REQUIRE_DEPTH_TEXTURE
            
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 Vector4_484BA326;
            float Vector1_8F075DBC;
            float Vector1_8AAD665C;
            float Vector1_E43D3892;
            float4 Vector4_1696EE66;
            float4 Color_2CFB5E2A;
            float4 Color_58195353;
            float Vector1_FADC0395;
            float Vector1_E505CA4B;
            float Vector1_4CE104D9;
            float Vector1_5536221E;
            float Vector1_9058DE8C;
            float Vector1_350643F1;
            float Vector1_2A80C598;
            float Vector1_A664731F;
            float Vector1_B4ED823D;
            float Vector1_2BA361A8;
            float Vector1_EED1A887;
            CBUFFER_END
        
            // Graph Functions
            
            void Unity_Distance_float3(float3 A, float3 B, out float Out)
            {
                Out = distance(A, B);
            }
            
            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }
            
            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
            
            void Unity_Rotate_About_Axis_Degrees_float(float3 In, float3 Axis, float Rotation, out float3 Out)
            {
                Rotation = radians(Rotation);
            
                float s = sin(Rotation);
                float c = cos(Rotation);
                float one_minus_c = 1.0 - c;
                
                Axis = normalize(Axis);
            
                float3x3 rot_mat = { one_minus_c * Axis.x * Axis.x + c,            one_minus_c * Axis.x * Axis.y - Axis.z * s,     one_minus_c * Axis.z * Axis.x + Axis.y * s,
                                          one_minus_c * Axis.x * Axis.y + Axis.z * s,   one_minus_c * Axis.y * Axis.y + c,              one_minus_c * Axis.y * Axis.z - Axis.x * s,
                                          one_minus_c * Axis.z * Axis.x - Axis.y * s,   one_minus_c * Axis.y * Axis.z + Axis.x * s,     one_minus_c * Axis.z * Axis.z + c
                                        };
            
                Out = mul(rot_mat,  In);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            
            float2 Unity_GradientNoise_Dir_float(float2 p)
            {
                // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }
            
            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            { 
                float2 p = UV * Scale;
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
                float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }
            
            void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
            {
                RGBA = float4(R, G, B, A);
                RGB = float3(R, G, B);
                RG = float2(R, G);
            }
            
            void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
            {
                Out = smoothstep(Edge1, Edge2, In);
            }
            
            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }
            
            void Unity_SceneDepth_Eye_float(float4 UV, out float Out)
            {
                Out = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV.xy), _ZBufferParams);
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
        
            // Graph Vertex
            struct VertexDescriptionInputs
            {
                float3 ObjectSpaceNormal;
                float3 WorldSpaceNormal;
                float3 ObjectSpaceTangent;
                float3 ObjectSpacePosition;
                float3 WorldSpacePosition;
                float3 TimeParameters;
            };
            
            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };
            
            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                float _Distance_D85DF7AD_Out_2;
                Unity_Distance_float3(SHADERGRAPH_OBJECT_POSITION, IN.WorldSpacePosition, _Distance_D85DF7AD_Out_2);
                float _Property_117D8453_Out_0 = Vector1_A664731F;
                float _Divide_6EF58E3_Out_2;
                Unity_Divide_float(_Distance_D85DF7AD_Out_2, _Property_117D8453_Out_0, _Divide_6EF58E3_Out_2);
                float _Power_33FF8791_Out_2;
                Unity_Power_float(_Divide_6EF58E3_Out_2, 3, _Power_33FF8791_Out_2);
                float3 _Multiply_E4027504_Out_2;
                Unity_Multiply_float(IN.WorldSpaceNormal, (_Power_33FF8791_Out_2.xxx), _Multiply_E4027504_Out_2);
                float _Property_34CB0090_Out_0 = Vector1_FADC0395;
                float _Property_AD125D68_Out_0 = Vector1_E505CA4B;
                float4 _Property_766D591A_Out_0 = Vector4_484BA326;
                float _Split_46EE164F_R_1 = _Property_766D591A_Out_0[0];
                float _Split_46EE164F_G_2 = _Property_766D591A_Out_0[1];
                float _Split_46EE164F_B_3 = _Property_766D591A_Out_0[2];
                float _Split_46EE164F_A_4 = _Property_766D591A_Out_0[3];
                float3 _RotateAboutAxis_A00D475E_Out_3;
                Unity_Rotate_About_Axis_Degrees_float(IN.WorldSpacePosition, (_Property_766D591A_Out_0.xyz), _Split_46EE164F_A_4, _RotateAboutAxis_A00D475E_Out_3);
                float _Property_1195B7B7_Out_0 = Vector1_8AAD665C;
                float _Multiply_365B1AAE_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1195B7B7_Out_0, _Multiply_365B1AAE_Out_2);
                float2 _TilingAndOffset_94B76B59_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_365B1AAE_Out_2.xx), _TilingAndOffset_94B76B59_Out_3);
                float _Property_EEA5F82E_Out_0 = Vector1_8F075DBC;
                float _GradientNoise_4DF641FB_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_94B76B59_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_4DF641FB_Out_2);
                float2 _TilingAndOffset_D3C6639A_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), float2 (0, 0), _TilingAndOffset_D3C6639A_Out_3);
                float _GradientNoise_ADD9B44D_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_D3C6639A_Out_3, _Property_EEA5F82E_Out_0, _GradientNoise_ADD9B44D_Out_2);
                float _Add_29BFF883_Out_2;
                Unity_Add_float(_GradientNoise_4DF641FB_Out_2, _GradientNoise_ADD9B44D_Out_2, _Add_29BFF883_Out_2);
                float _Divide_40E6AFDE_Out_2;
                Unity_Divide_float(_Add_29BFF883_Out_2, 2, _Divide_40E6AFDE_Out_2);
                float _Saturate_7F44B33_Out_1;
                Unity_Saturate_float(_Divide_40E6AFDE_Out_2, _Saturate_7F44B33_Out_1);
                float _Property_8444523D_Out_0 = Vector1_4CE104D9;
                float _Power_E1284C32_Out_2;
                Unity_Power_float(_Saturate_7F44B33_Out_1, _Property_8444523D_Out_0, _Power_E1284C32_Out_2);
                float4 _Property_9D282FC2_Out_0 = Vector4_1696EE66;
                float _Split_AC92423D_R_1 = _Property_9D282FC2_Out_0[0];
                float _Split_AC92423D_G_2 = _Property_9D282FC2_Out_0[1];
                float _Split_AC92423D_B_3 = _Property_9D282FC2_Out_0[2];
                float _Split_AC92423D_A_4 = _Property_9D282FC2_Out_0[3];
                float4 _Combine_548F329E_RGBA_4;
                float3 _Combine_548F329E_RGB_5;
                float2 _Combine_548F329E_RG_6;
                Unity_Combine_float(_Split_AC92423D_R_1, _Split_AC92423D_G_2, 0, 0, _Combine_548F329E_RGBA_4, _Combine_548F329E_RGB_5, _Combine_548F329E_RG_6);
                float4 _Combine_B40C924A_RGBA_4;
                float3 _Combine_B40C924A_RGB_5;
                float2 _Combine_B40C924A_RG_6;
                Unity_Combine_float(_Split_AC92423D_B_3, _Split_AC92423D_A_4, 0, 0, _Combine_B40C924A_RGBA_4, _Combine_B40C924A_RGB_5, _Combine_B40C924A_RG_6);
                float _Remap_F10A160D_Out_3;
                Unity_Remap_float(_Power_E1284C32_Out_2, _Combine_548F329E_RG_6, _Combine_B40C924A_RG_6, _Remap_F10A160D_Out_3);
                float _Absolute_4FC64134_Out_1;
                Unity_Absolute_float(_Remap_F10A160D_Out_3, _Absolute_4FC64134_Out_1);
                float _Smoothstep_56070F3_Out_3;
                Unity_Smoothstep_float(_Property_34CB0090_Out_0, _Property_AD125D68_Out_0, _Absolute_4FC64134_Out_1, _Smoothstep_56070F3_Out_3);
                float _Property_1671376A_Out_0 = Vector1_9058DE8C;
                float _Multiply_E8657CCC_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1671376A_Out_0, _Multiply_E8657CCC_Out_2);
                float2 _TilingAndOffset_B64A0F1F_Out_3;
                Unity_TilingAndOffset_float((_RotateAboutAxis_A00D475E_Out_3.xy), float2 (1, 1), (_Multiply_E8657CCC_Out_2.xx), _TilingAndOffset_B64A0F1F_Out_3);
                float _Property_12404564_Out_0 = Vector1_5536221E;
                float _GradientNoise_3F907965_Out_2;
                Unity_GradientNoise_float(_TilingAndOffset_B64A0F1F_Out_3, _Property_12404564_Out_0, _GradientNoise_3F907965_Out_2);
                float _Property_94C69BD5_Out_0 = Vector1_350643F1;
                float _Multiply_61FF3188_Out_2;
                Unity_Multiply_float(_GradientNoise_3F907965_Out_2, _Property_94C69BD5_Out_0, _Multiply_61FF3188_Out_2);
                float _Add_12AD4DE7_Out_2;
                Unity_Add_float(_Smoothstep_56070F3_Out_3, _Multiply_61FF3188_Out_2, _Add_12AD4DE7_Out_2);
                float _Add_B7625F11_Out_2;
                Unity_Add_float(1, _Property_94C69BD5_Out_0, _Add_B7625F11_Out_2);
                float _Divide_A999571A_Out_2;
                Unity_Divide_float(_Add_12AD4DE7_Out_2, _Add_B7625F11_Out_2, _Divide_A999571A_Out_2);
                float3 _Multiply_EF48E583_Out_2;
                Unity_Multiply_float(IN.ObjectSpaceNormal, (_Divide_A999571A_Out_2.xxx), _Multiply_EF48E583_Out_2);
                float _Property_347E548F_Out_0 = Vector1_E43D3892;
                float3 _Multiply_E6AAE955_Out_2;
                Unity_Multiply_float(_Multiply_EF48E583_Out_2, (_Property_347E548F_Out_0.xxx), _Multiply_E6AAE955_Out_2);
                float3 _Add_EDAC08D6_Out_2;
                Unity_Add_float3(IN.ObjectSpacePosition, _Multiply_E6AAE955_Out_2, _Add_EDAC08D6_Out_2);
                float3 _Add_5924731B_Out_2;
                Unity_Add_float3(_Multiply_E4027504_Out_2, _Add_EDAC08D6_Out_2, _Add_5924731B_Out_2);
                description.VertexPosition = _Add_5924731B_Out_2;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 TangentSpaceNormal;
                float3 WorldSpacePosition;
                float4 ScreenPosition;
            };
            
            struct SurfaceDescription
            {
                float3 Albedo;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float _SceneDepth_6FF4059F_Out_1;
                Unity_SceneDepth_Eye_float(float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _SceneDepth_6FF4059F_Out_1);
                float4 _ScreenPosition_874D7EFB_Out_0 = IN.ScreenPosition;
                float _Split_984E65AF_R_1 = _ScreenPosition_874D7EFB_Out_0[0];
                float _Split_984E65AF_G_2 = _ScreenPosition_874D7EFB_Out_0[1];
                float _Split_984E65AF_B_3 = _ScreenPosition_874D7EFB_Out_0[2];
                float _Split_984E65AF_A_4 = _ScreenPosition_874D7EFB_Out_0[3];
                float _Subtract_8113911_Out_2;
                Unity_Subtract_float(_Split_984E65AF_A_4, 1, _Subtract_8113911_Out_2);
                float _Subtract_6632FEEB_Out_2;
                Unity_Subtract_float(_SceneDepth_6FF4059F_Out_1, _Subtract_8113911_Out_2, _Subtract_6632FEEB_Out_2);
                float _Property_6F4C582_Out_0 = Vector1_EED1A887;
                float _Divide_98F0CC68_Out_2;
                Unity_Divide_float(_Subtract_6632FEEB_Out_2, _Property_6F4C582_Out_0, _Divide_98F0CC68_Out_2);
                float _Saturate_6FABCB9A_Out_1;
                Unity_Saturate_float(_Divide_98F0CC68_Out_2, _Saturate_6FABCB9A_Out_1);
                float _Smoothstep_3808517D_Out_3;
                Unity_Smoothstep_float(0, 1, _Saturate_6FABCB9A_Out_1, _Smoothstep_3808517D_Out_3);
                surface.Albedo = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
                surface.Alpha = _Smoothstep_3808517D_Out_3;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);
            
                output.ObjectSpaceNormal =           input.normalOS;
                output.WorldSpaceNormal =            TransformObjectToWorldNormal(input.normalOS);
                output.ObjectSpaceTangent =          input.tangentOS;
                output.ObjectSpacePosition =         input.positionOS;
                output.WorldSpacePosition =          TransformObjectToWorld(input.positionOS);
                output.TimeParameters =              _TimeParameters.xyz;
            
                return output;
            }
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
                output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
            
            
                output.WorldSpacePosition =          input.positionWS;
                output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
        
            ENDHLSL
        }
        
    }
    CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}
