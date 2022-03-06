Shader "Zelda/ShaderEye" {
    Properties{
        _speed("speed", Range(0, 5)) = 0.4
        _MeltTex("MeltTex", 2D) = "black" {}
        _Intensity("Intensity", Range(0, 1)) = 0.3601617
        _MainTex("MainTex", 2D) = "white" {}
        [HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
        _Color("Color Tint", Color) = (1, 1, 1, 1)
        _ReflectColor("Reflection Color", Color) = (1, 1, 1, 1)
        _ReflectAmount("Reflect Amount", Range(0, 1)) = 1
        _Cubemap("Reflection Cubemap", Cube) = "_Skybox" {}

        //rgb to hsv
        _HueOffset("Hue Offset", Range(0,1)) = 0
        _SaturationOffset("Hue Offset", Range(-1,1)) = 0.3
        _ValueOffset("Hue Offset", Range(-1,1)) = 0.15

        _FresnelScale("FresnelScale",Range(0.1,10)) = 1
        [HDR]_FresnelColor("FresnelColor",color) = (1,1,1,1)
    }
        SubShader{
            Tags {
                "Queue" = "AlphaTest"
                "RenderType" = "TransparentCutout"
            }
            Pass {
                Name "FORWARD"
                Tags {
                    "LightMode" = "ForwardBase"
                }


                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #define UNITY_PASS_FORWARDBASE
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"
                #pragma multi_compile_fwdbase_fullshadows
                #pragma multi_compile_fog
                #pragma only_renderers d3d9 d3d11 glcore gles 
                #pragma target 3.0
                uniform float4 _LightColor0;
                uniform float _speed;
                uniform sampler2D _MeltTex; uniform float4 _MeltTex_ST;
                uniform float _Intensity;
                uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
                fixed4 _Color;
                fixed4 _ReflectColor;
                fixed _ReflectAmount;
                samplerCUBE _Cubemap;

                float _FresnelScale;
                fixed4 _FresnelColor;

                float _HueOffset;
                float _SaturationOffset;
                float _ValueOffset;
                struct VertexInput {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord0 : TEXCOORD0;
                    float2 uv : TEXCOORD1;
                };
                struct VertexOutput {
                    float4 pos : SV_POSITION;
                    float2 uv0 : TEXCOORD0;
                    float4 posWorld : TEXCOORD1;
                    float3 normalDir : TEXCOORD2;
                    fixed3 worldViewDir : TEXCOORD3;
                    fixed3 worldRefl : TEXCOORD4;
                    float2 uvFoam : TEXCOORD5;
                    SHADOW_COORDS(4)
                    LIGHTING_COORDS(3,4)
                    UNITY_FOG_COORDS(5)
                };
                VertexOutput vert(VertexInput v) {
                    VertexOutput o = (VertexOutput)0;
                    o.uv0 = v.texcoord0;
                    o.normalDir = UnityObjectToWorldNormal(v.normal);
                    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                    o.worldViewDir = UnityWorldSpaceViewDir(o.posWorld);
                    o.worldRefl = reflect(-o.worldViewDir, o.normalDir);
                    o.uvFoam = TRANSFORM_TEX(v.uv,_MainTex);
                    TRANSFER_SHADOW(o);
                    float3 lightColor = _LightColor0.rgb;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    UNITY_TRANSFER_FOG(o,o.pos);
                    TRANSFER_VERTEX_TO_FRAGMENT(o)
                    return o;
                }
                float4 frag(VertexOutput i) : COLOR {
                    i.normalDir = normalize(i.normalDir);
                    fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.posWorld));
                    fixed3 worldViewDir = normalize(i.worldViewDir);
                    float3 normalDirection = i.normalDir;
                    float4 time = _Time;
                    float2 Tex = (i.uv0 + (_speed * time.g) * float2(0.1,0.1));
                    float4 _MeltTex_var = tex2D(_MeltTex,TRANSFORM_TEX(Tex, _MeltTex));
                    float2 Tex2 = lerp(i.uv0,(float3(i.uv0,0.0) * _MeltTex_var.rgb * 2.0).rg,_Intensity);
                    float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(Tex2, _MainTex));
                    clip(_MainTex_var.a - 0.5);
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    float3 lightColor = _LightColor0.rgb;

                    float attenuation = LIGHT_ATTENUATION(i);
                    float3 attenColor = attenuation * _LightColor0.xyz;

                    float NdotL = max(0.0,dot(normalDirection, lightDirection));
                    float3 directDiffuse = max(0.0, NdotL) * attenColor;
                    float3 indirectDiffuse = float3(0,0,0);
                    indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                    float3 diffuseColor = _MainTex_var.rgb;
                    float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;

                    float3 finalColor = diffuse;
                    fixed4 finalRGBA = fixed4(finalColor,1);
                    UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;
                    fixed3 _diffuse = _LightColor0.rgb * _Color.rgb * max(0, dot(normalDirection, worldLightDir));
                    fixed3 reflection = texCUBE(_Cubemap, i.worldRefl).rgb * _ReflectColor.rgb;
                    UNITY_LIGHT_ATTENUATION(atten, i, i.posWorld);
                    fixed3 Fresnel = pow(1.0 - max(0,dot(worldLightDir, worldViewDir)),_FresnelScale) * _FresnelColor.rgb;
                    fixed4 finalFresnel = fixed4(Fresnel,1);
                    // Mix the diffuse color with the reflected color
                    fixed3 color = ambient + lerp(_diffuse, reflection, _ReflectAmount) * atten;
                    fixed4 col = fixed4(reflection, 1.0) + finalRGBA + _MainTex_var;
                    float3 rgb = col.rgb;
                    float4 k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                    float4 p = lerp(float4(rgb.bg, k.wz), float4(rgb.gb, k.xy), step(rgb.b, rgb.g));
                    // 比较r和max(b,g)
                    float4 q = lerp(float4(p.xyw, rgb.r), float4(rgb.r, p.yzx), step(p.x, rgb.r));
                    float d = q.x - min(q.w, q.y);
                    float e = 1.0e-10;
                    float3 hsv = float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);

                    hsv.x = hsv.x + _HueOffset;
                    hsv.y = hsv.y + _SaturationOffset;
                    hsv.z = hsv.z + _ValueOffset;
                    rgb = saturate(3.0 * abs(1.0 - 2.0 * frac(hsv.x + float3(0.0,-1.0 / 3.0,1.0 / 3.0))) - 1); //明度和饱和度为1时的颜色
                    rgb = (lerp(float3(1,1,1),rgb,hsv.y) * hsv.z); // hsv

                    col.rgb = rgb;
                    col.a = 1;
                    return col + finalFresnel;
                }
                ENDCG
            }
            Pass {
                Name "FORWARD_DELTA"
                Tags {
                    "LightMode" = "ForwardAdd"
                }
                Blend One One


                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #define UNITY_PASS_FORWARDADD
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"
                #pragma multi_compile_fwdadd_fullshadows
                #pragma multi_compile_fog
                #pragma only_renderers d3d9 d3d11 glcore gles 
                #pragma target 3.0
                uniform float4 _LightColor0;
                uniform float _speed;
                uniform sampler2D _MeltTex; uniform float4 _MeltTex_ST;
                uniform float _Intensity;
                uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
                struct VertexInput {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord0 : TEXCOORD0;
                };
                struct VertexOutput {
                    float4 pos : SV_POSITION;
                    float2 uv0 : TEXCOORD0;
                    float4 posWorld : TEXCOORD1;
                    float3 normalDir : TEXCOORD2;
                    LIGHTING_COORDS(3,4)
                    UNITY_FOG_COORDS(5)
                };
                VertexOutput vert(VertexInput v) {
                    VertexOutput o = (VertexOutput)0;
                    o.uv0 = v.texcoord0;
                    o.normalDir = UnityObjectToWorldNormal(v.normal);
                    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                    float3 lightColor = _LightColor0.rgb;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    UNITY_TRANSFER_FOG(o,o.pos);
                    TRANSFER_VERTEX_TO_FRAGMENT(o)
                    return o;
                }
                float4 frag(VertexOutput i) : COLOR {
                    i.normalDir = normalize(i.normalDir);
                    float3 normalDirection = i.normalDir;
                    float4 time = _Time;
                    float2 Tex = (i.uv0 + (_speed * time.g) * float2(0.1,0.1));
                    float4 _MeltTex_var = tex2D(_MeltTex,TRANSFORM_TEX(Tex, _MeltTex));
                    float2 Tex2 = lerp(i.uv0,(float3(i.uv0,0.0) * _MeltTex_var.rgb * 2.0).rg,_Intensity);
                    float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(Tex2, _MainTex));
                    clip(_MainTex_var.a - 0.5);
                    float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                    float3 lightColor = _LightColor0.rgb;

                    float attenuation = LIGHT_ATTENUATION(i);
                    float3 attenColor = attenuation * _LightColor0.xyz;

                    float NdotL = max(0.0,dot(normalDirection, lightDirection));
                    float3 directDiffuse = max(0.0, NdotL) * attenColor;
                    float3 diffuseColor = _MainTex_var.rgb;
                    float3 diffuse = directDiffuse * diffuseColor;

                    float3 finalColor = diffuse;
                    fixed4 finalRGBA = fixed4(finalColor * 1,0);
                    UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                    return finalRGBA;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}


