Shader "Custom/OutlineShader" 
 {
     Properties 
     {
         _Color("Color", Color) = (1,0,0,1)
         _Thickness("Thickness", float) = 4
     }
     SubShader 
     {
     
         Tags { "Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Transparent" }
         Blend SrcAlpha OneMinusSrcAlpha
         Cull Back
         ZTest always
         Pass
         {
             Stencil {
                 Ref 1
                 Comp always
                 Pass replace
             }
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma multi_compile_fog
             
             #include "UnityCG.cginc"
             
             struct v2g 
             {
                 float4  pos : SV_POSITION;
                 float2  uv : TEXCOORD0;
                 float3 viewT : TANGENT;
                 float3 normals : NORMAL;
             };
             
             struct g2f 
             {
                 float4  pos : SV_POSITION;
                 float2  uv : TEXCOORD0;
                 float3  viewT : TANGENT;
                 float3  normals : NORMAL;
             };
 
             v2g vert(appdata_base v)
             {
                 v2g OUT;
                 OUT.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                 OUT.uv = v.texcoord; 
                 OUT.normals = v.normal;
                 OUT.viewT = ObjSpaceViewDir(v.vertex);
                 
                 return OUT;
             }
             
             half4 frag(g2f IN) : COLOR
             {
                 //this renders nothing, if you want the base mesh and color
                 //fill this in with a standard fragment shader calculation
                 return float4(1.0, 1.0, 0.0, 1.0);
             }
             ENDCG
         }
         Pass 
         {
             Stencil {
                 Ref 0
                 Comp equal
             }
             CGPROGRAM
             #include "UnityCG.cginc"
             #pragma target 4.0
             #pragma vertex vert
             #pragma fragment frag
             
             
             half4 _Color;
             float _Thickness;
         
             struct v2g 
             {
                 float4 pos : SV_POSITION;
                 float2 uv : TEXCOORD0;
                 float3 viewT : TANGENT;
                 float3 normals : NORMAL;
             };
             
             struct g2f 
             {
                 float4 pos : SV_POSITION;
                 float2 uv : TEXCOORD0;
                 float3 viewT : TANGENT;
                 float3 normals : NORMAL;
             };
 
             v2g vert(appdata_base v)
             {
                 v2g OUT;

				 float4x4 modifiedMVP = UNITY_MATRIX_MVP;
				 modifiedMVP[0][0] *= 1.1;  modifiedMVP[0][1] *= 1.1;  modifiedMVP[0][2] *= 1.1;
				 modifiedMVP[1][0] *= 1.1;  modifiedMVP[1][1] *= 1.1; modifiedMVP[1][2] *= 1.1;
				 modifiedMVP[2][0] *= 1.1;  modifiedMVP[2][1] *= 1.1; modifiedMVP[2][2] *= 1.1;
                 OUT.pos = mul(modifiedMVP, v.vertex);
                 
                 OUT.uv = v.texcoord;
                 OUT.normals = v.normal;
                 OUT.viewT = ObjSpaceViewDir(v.vertex);
                 
                 return OUT;
             }
             
             half4 frag(g2f IN) : COLOR
             {
                 _Color.a = 1;
                 return float4(1.0, 1.0, 1.0, 1.0);
             }
             
             ENDCG
 
         }
     }
     FallBack "Diffuse"
 }