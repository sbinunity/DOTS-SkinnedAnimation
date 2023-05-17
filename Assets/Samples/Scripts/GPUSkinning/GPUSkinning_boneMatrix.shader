Shader "DOTS/BoneMatrix"
{
    Properties
    {
        _BaseColor(" Color ",color)=(1,1,1,1)
        _BufferIndexStart ("BufferIndexStart",int)=0
        _MainTex ("Texture", 2D) = "white" {}

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
           // #pragma multi_compile_instancing
            #pragma target 4.5


          #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;
            float4x4 _boneMatrix[112]; // 56
          //  StructuredBuffer<float4x4> _boneMatries:register(t1);
           
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float4 _BaseColor;
            int _BufferIndexStart;
            CBUFFER_END

#ifdef UNITY_DOTS_INSTANCING_ENABLED
                UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
                UNITY_DOTS_INSTANCED_PROP(float4, _MainTex_ST)
                UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
                UNITY_DOTS_INSTANCED_PROP(int, _BufferIndexStart)
                UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

              /*  UNITY_DOTS_INSTANCING_START(UserPropertyMetadata)
                   UNITY_DOTS_INSTANCED_PROP(float4x4[56], _boneMatrix)
                UNITY_DOTS_INSTANCING_END(UserPropertyMetadata)*/

    #define _MainTex_ST          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _MainTex_ST)
    #define _BaseColor           UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(float4 , _BaseColor)
    #define _BufferIndexStart          UNITY_ACCESS_DOTS_INSTANCED_PROP_WITH_DEFAULT(int , _BufferIndexStart)

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
                float4 color:TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };



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

                
               // float4 animationPos = mul(_boneMatrix[bone0], v.vertex) * w0 + mul(_boneMatrix[bone1], v.vertex) * w1;
                float4 animationPos = mul(_boneMatrix[_BufferIndexStart +bone0], v.vertex) * w0 + mul(_boneMatrix[_BufferIndexStart +bone1], v.vertex) * w1;

                o.vertex = TransformObjectToHClip(animationPos);
                //o.vertex = UnityObjectToClipPos(animationPos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
               
#ifdef DOTS_INSTANCING_ON
                float id = i.instanceID / 29.0 ;
#else
                float id = 1;
#endif
               
                half4 col = tex2D(_MainTex, i.uv) * _BaseColor;// *id;
                return col;
            }
                ENDHLSL
        }
    }
}
