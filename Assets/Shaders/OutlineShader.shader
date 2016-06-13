Shader "Custom/OutlineShader" 
 {
     Properties 
     {
         _Color("Color", Color) = (1,0,0,1)
         _Thickness("Thickness", float) = 4
		 _MainTex("Texture", 2D) = "white" {}
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
 
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
				 // sample the texture
				 fixed4 col = tex2D(_MainTex, IN.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
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
				 modifiedMVP[0][0] *= _Thickness;  modifiedMVP[0][1] *= _Thickness;  modifiedMVP[0][2] *= _Thickness;
				 modifiedMVP[1][0] *= _Thickness;  modifiedMVP[1][1] *= _Thickness; modifiedMVP[1][2] *= _Thickness;
				 modifiedMVP[2][0] *= _Thickness;  modifiedMVP[2][1] *= _Thickness; modifiedMVP[2][2] *= _Thickness;
                 OUT.pos = mul(modifiedMVP, v.vertex);
                 
                 OUT.uv = v.texcoord;
                 OUT.normals = v.normal;
                 OUT.viewT = ObjSpaceViewDir(v.vertex);
                 
                 return OUT;
             }
             
             half4 frag(g2f IN) : COLOR
             {
                 _Color.a = 1;
                 return _Color;
             }
             
             ENDCG
 
         }
     }
     FallBack "Diffuse"
 }