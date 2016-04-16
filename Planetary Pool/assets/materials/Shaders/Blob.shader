Shader "Custom/Blob" {
 Properties {
        _MainTex ("Texture", 2D) = "white" {}  
        _Warp ("Warp Amount", Range (0, 5.0)) = 1.0      
        _LineWarp ("Warp Amount", Range (0, 5.0)) = 1.0      
    }
    SubShader {
    //Tags { "RenderType" = "Transparent" }
    Tags { "Queue"="Transparent" "RenderType"="Transparent" }
    CGPROGRAM
      #pragma surface surf SimpleLambert alpha

      half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
          //half NdotL = dot (s.Normal, lightDir);
          half4 c = 1;
          c.rgb = s.Albedo * 0.5;
          c.a = s.Alpha;
          //c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
    
          return c;
      }

    struct Input {
        float2 uv_MainTex;        
        float4 _Time;
        //float4 screenPos;
    };
    
    sampler2D _MainTex;    
    uniform float _Warp;
    uniform float _LineWarp;
   // sampler2D _GrabTexture;
    
    
    void surf (Input IN, inout SurfaceOutput o) {
    	float4 main = tex2D (_MainTex, IN.uv_MainTex);
    	
    	//float2 uvs = (IN.screenPos.xy/IN.screenPos.w).xy;
		//uvs.y = 1.0-uvs.y;
		//float3 scene=tex2D(_GrabTexture, uvs);
    	    	
    	main.rg -= 0.5;
    	main.rg *= _Warp;
    	main.rg += 0.5;
    	
    	main.b *= _LineWarp;
    	
        o.Albedo = main.rgb;        
        o.Alpha = main.a;        
    }
    ENDCG
    }
    Fallback "Diffuse"
}