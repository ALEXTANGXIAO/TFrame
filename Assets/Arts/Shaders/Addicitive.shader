Shader "sfx/Additive" {
    Properties{
        _TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex("Particle Texture", 2D) = "white" {}
        [Toggle] _UseUvTrail("======UV条带======", Float) = 0
        [Toggle(_USEUVANI_ON)] _UseUvAni("======UV滚动======", Float) = 0
        _SpeedU("SpeedU", Float) = 0
        _SpeedV("SpeedV", Float) = 0
        [Toggle] _UseUvDistortion("======UV扰乱======", Float) = 0
        _UVNoiseTex("NoiseTex", 2D) = "bump" {}
        _Distortion("Distortion", Float) = 0
        _NoiseScroll("NoiseScroll", Vector) = (0,0,1,1)
        [Toggle] _UseMask("======Mask======", Float) = 0
        _Mask("Mask ( R Channel )", 2D) = "white" {}
        [Toggle] _UseDissolve("======溶解======", Float) = 0
        _DissolveTex("DissolveTex( R Channel )", 2D) = "white" {}
        _Dissolve("Dissolve", Range(0, 1.01)) = 0
        _DissolveWidth("DissolveWidth", Range(0, 1)) = 0
        [Toggle] _UseUIMaskClip("======UI裁切======", Float) = 0
        _ClipRect("Clip Rect", Vector) = (-32767, -32767, 32767, 32767)
    }

        Category{
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
            Blend SrcAlpha One
            ColorMask RGB
            Cull Off Lighting Off ZWrite Off

            SubShader {
                Pass {

                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    #pragma target 2.0

                    #pragma shader_feature _USEMASK_ON
                    #pragma shader_feature _USEUVTRAIL_ON
                    #pragma shader_feature _USEUVANI_ON
                    #pragma shader_feature _USEUVDISTORTION_ON
                    #pragma shader_feature _USEDISSOLVE_ON
                    #pragma shader_feature _USEUIMASKCLIP_ON

                    #include "UnityCG.cginc"
                    #include "UnityUI.cginc"

                    sampler2D _MainTex;
                    fixed4 _MainTex_ST;
                    fixed4 _TintColor;

                    #if _USEMASK_ON
                        sampler2D _Mask;
                        fixed4 _Mask_ST;
                    #endif

                        // #if _USEUVANI_ON
                            float _SpeedU;
                            float _SpeedV;
                            // #endif

                            #if _USEUVDISTORTION_ON
                                sampler2D _UVNoiseTex;
                                fixed4 _UVNoiseTex_ST;
                                float _Distortion;
                                float4 _NoiseScroll;
                            #endif

                            #if _USEDISSOLVE_ON
                                sampler2D _DissolveTex;
                                fixed4 _DissolveTex_ST;
                                fixed _Dissolve;
                                fixed _DissolveWidth;
                            #endif

                            #if _USEUIMASKCLIP_ON
                                fixed4 _ClipRect;
                            #endif

                            struct appdata_t {
                                float4 vertex : POSITION;
                                fixed4 color : COLOR;
                                float4 texcoord : TEXCOORD0;
                                fixed2 texcoord1 : TEXCOORD1;
                                UNITY_VERTEX_INPUT_INSTANCE_ID
                            };

                            struct v2f {
                                float4 vertex : SV_POSITION;
                                fixed4 color : COLOR;
                                float4 texcoord : TEXCOORD0;

                                fixed2 texcoord1 : TEXCOORD1;

                                #if _USEMASK_ON
                                    fixed2 texcoordMask : TEXCOORD2;
                                #endif

                                #if _USEUVDISTORTION_ON
                                    float2 texcoordNoise : TEXCOORD3;
                                #endif

                                #if _USEDISSOLVE_ON
                                    fixed2 texcoordDissolve : TEXCOORD4;
                                #endif

                                #if _USEUIMASKCLIP_ON
                                    fixed4 worldPosition : TEXCOORD5;
                                #endif

                                UNITY_VERTEX_OUTPUT_STEREO
                            };


                            v2f vert(appdata_t v)
                            {
                                v2f o;
                                UNITY_SETUP_INSTANCE_ID(v);
                                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                                o.vertex = UnityObjectToClipPos(v.vertex);

                                #if _USEUIMASKCLIP_ON
                                    o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                                    //o.worldPosition = v.vertex;
                                #endif

                                o.color = v.color * _TintColor;
                                o.texcoord.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
                                o.texcoord.zw = v.texcoord.zw;
                                o.texcoord1 = v.texcoord1;

                                #if _USEMASK_ON
                                    o.texcoordMask = TRANSFORM_TEX(v.texcoord,_Mask);
                                #endif

                                #if _USEUVDISTORTION_ON
                                    o.texcoordNoise = TRANSFORM_TEX(v.texcoord,_UVNoiseTex);
                                #endif

                                #if _USEDISSOLVE_ON
                                    o.texcoordDissolve = TRANSFORM_TEX(v.texcoord,_DissolveTex);
                                #endif

                                return o;
                            }


                            fixed4 frag(v2f i) : SV_Target
                            {
                                #if _USEUVTRAIL_ON
                                    i.texcoord.xy = saturate(i.texcoord.xy + i.texcoord1);
                                #else
                                // #if _USEUVANI_ON
                                    i.texcoord.xy += _Time.y * float2(_SpeedU, _SpeedV);
                            // #endif
                        #endif

                        #if _USEUVDISTORTION_ON
                            i.texcoordNoise += float2(_Time.g * _NoiseScroll.xy);
                            i.texcoord.xy += UnpackNormal(tex2D(_UVNoiseTex, i.texcoordNoise)).xy * _Distortion * _NoiseScroll.zw;
                        #endif

                        fixed4 col = 2.0f * i.color * tex2D(_MainTex, i.texcoord.xy);
                        col.a = saturate(col.a);

                        #if _USEMASK_ON
                            col.a = saturate(col.a * tex2D(_Mask, i.texcoordMask).r);
                        #endif

                        #if _USEDISSOLVE_ON
                            fixed DissolveWithParticle = (_Dissolve * (1 - i.texcoord.z) * (1 + _DissolveWidth) - _DissolveWidth);
                            fixed dissolveAlpha = saturate(smoothstep(DissolveWithParticle, (DissolveWithParticle + _DissolveWidth), tex2D(_DissolveTex, i.texcoordDissolve).r));
                            col.a *= dissolveAlpha;
                        #endif

                        #if _USEUIMASKCLIP_ON
                            col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                        #endif

                        return col;
                    }
                    ENDCG
                }
            }
        }
}