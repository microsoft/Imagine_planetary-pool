 Shader "Custom/BlackHole" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
       Tags { "Queue"="Transparent" "RenderType"="Transparent" }
      CGPROGRAM
      #pragma surface surf SimpleLambert alpha
      
      half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
          //half NdotL = saturate(dot (s.Normal, lightDir));
          
          half falloff = saturate(dot (s.Normal, float3 (0, 1, 0)) * 2);
          
          half4 c = 1;
          
          c.rgb = 0;
          c.a = falloff;          
    
          return c;
      }
      struct Input {
          float2 uv_MainTex;
          float4 _Time;
      };
      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
      
     	 // float3 c = tex2D (_MainTex, float2(IN.uv_MainTex.r + _Time.r, IN.uv_MainTex.g)).rgb ;
     	  
          o.Albedo = 0;
          o.Alpha = 1;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }