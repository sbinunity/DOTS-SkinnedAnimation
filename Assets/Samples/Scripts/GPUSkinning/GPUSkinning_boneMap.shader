Shader "DOTS/BoneMap"
{
    Properties
    {
        _BaseColor(" Color ",color)=(1,1,1,1)

        _MainTex ("Texture", 2D) = "white" {}

        _boneMap("BoneMap",2d) = "white"{}

        _Frame("Frame",int)=0
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM


            #pragma vertex vert
            #pragma fragment frag
           #pragma multi_compile _ DOTS_INSTANCING_ON
            #pragma target 4.5


          #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                    sampler2D _MainTex;
            sampler2D _boneMap;

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _boneMap_TexelSize;
            float4 _BaseColor;
             float _Frame;
            CBUFFER_END

#ifdef UNITY_DOTS_INSTANCING_ENABLED
                UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
                UNITY_DOTS_INSTANCED_PROP(float4, _MainTex_ST)
                UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
                UNITY_DOTS_INSTANCED_PROP(float4, _MainTex_TexelSize)
                UNITY_DOTS_INSTANCED_PROP(float4, _boneMap_TexelSize)
                UNITY_DOTS_INSTANCED_PROP(float, _Frame)
                UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

#define _MainTex_ST          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _MainTex_ST)
#define _BaseColor          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseColor)
#define _MainTex_TexelSize          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _MainTex_TexelSize)
#define _boneMap_TexelSize          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _boneMap_TexelSize)
#define _Frame          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float , _Frame)

#endif
          
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
           
                float4 color:COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;              
                float4 vertex : SV_POSITION;
            
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };



            float DecodeBoneMapFloatRGBA(float4 enc)
            {
                float4 kDecodeDot = float4(1.0, 1 / 255.0, 1 / 65025.0, 1 / 16581375.0);
                float v=  dot(enc, kDecodeDot);
                v = v * 100 - 50;
                return v;
            }


            float4x4 SampleBoneMap(int boneIndex)
            {

                int startIndex = boneIndex *4;
                float u = (startIndex + 0.5)* _boneMap_TexelSize.x ;
                float v = (_Frame*4+0.5)* _boneMap_TexelSize.y;


                float m00 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u, v, 0, 0)));
                float m01 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x, v, 0, 0)));
                float m02 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x * 2, v, 0, 0)));
                float m03 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x * 3, v, 0, 0)));


                float m10 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u , v+ _boneMap_TexelSize.y, 0, 0)));
                float m11 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x, v+_boneMap_TexelSize.y, 0, 0)));
                float m12 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x * 2, v+_boneMap_TexelSize.y, 0, 0)));
                float m13 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x * 3, v+_boneMap_TexelSize.y, 0, 0)));


                float m20 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u , v+ _boneMap_TexelSize.y * 2, 0, 0)));
                float m21 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x, v+_boneMap_TexelSize.y * 2, 0, 0)));
                float m22 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x * 2,v+ _boneMap_TexelSize.y * 2, 0, 0)));
                float m23 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x * 3, v + _boneMap_TexelSize.y * 2, 0, 0)));


                float m30 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u , v + _boneMap_TexelSize.y * 3, 0, 0)));
                float m31 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x, v + _boneMap_TexelSize.y * 3, 0, 0)));
                float m32 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x * 2, v + _boneMap_TexelSize.y * 3, 0, 0)));
                float m33 = DecodeBoneMapFloatRGBA(tex2Dlod(_boneMap, float4(u + _boneMap_TexelSize.x * 3, v + _boneMap_TexelSize.y * 3, 0, 0)));

                float4x4 m = float4x4(float4(m00, m01, m02, m03), float4(m10, m11, m12, m13), float4(m20, m21, m22, m23), float4(m30, m31, m32, m33));
           
                return  m;
            }

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                int bone0 = (int)(v.uv2.x /128);
                int bone1 = (int)(v.uv2.x %128);
               // int bone2 = (int)(v.uv2.y /128 );
               // int bone3 = (int)(v.uv2 .y%128 );


                float w0 = v.color.x;
                float w1 = v.color.y;
               // float w2 = v.color.z;
               // float w3 = v.color.w;

               
                


                float4x4 m0 = SampleBoneMap(bone0);
                float4x4 m1 = SampleBoneMap(bone1);
               // float4x4 m2 = SampleBoneMap(bone2);
               // float4x4 m3 = SampleBoneMap(bone3);

                
                float4 animationPos = mul(m0, v.vertex) * w0 + mul(m1, v.vertex) * w1;// +mul(m2, v.vertex) * w2 + mul(m3, v.vertex) * w3;
                o.vertex = TransformObjectToHClip(animationPos);
               
                
               // o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
           
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
              
                half4 col =  tex2D(_MainTex, i.uv)* _BaseColor;
               
           
                return col;
            }
                ENDHLSL
        }
    }
}
