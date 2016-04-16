Shader "Custom/HoleGlow" {
 Properties {
        _MainTex ("Texture", 2D) = "white" {}  
        _Detail ("Texture", 2D) = "white" {}  
        _Speed ("Warp Amount", Range (0, 5.0)) = 1.0      
        
    }
    
    SubShader {
    //Tags { "RenderType" = "Transparent" }
    Tags { "Queue"="Transparent" "RenderType"="Additive" }
    
    CGPROGRAM
    
      #pragma surface surf SimpleLambert decal:add
		
      half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
          //half NdotL = dot (s.Normal, lightDir);
          half4 c = 1;
          c.rgb = s.Albedo;
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
    sampler2D _Detail;
    uniform float _Speed;
    // sampler2D _GrabTexture;
    
    
    void surf (Input IN, inout SurfaceOutput o) {
    	
    	float2 uv = IN.uv_MainTex - 0.5;
    
    	//rotate!
    	float theta = lerp( 0, 3.14159, _Time.y * _Speed );
		float cosine = cos( theta );
		float sine = sin( theta );		
		float U = cosine * uv.x - sine * uv.y;
		float V = sine * uv.x + cosine * uv.y;
		float2 samp = float2( U, V );
		samp += 0.5;
		
		float theta2 = lerp( 0, 3.14159, _Time.y * _Speed * 0.377 );
		float cosine2 = cos( theta2 );
		float sine2 = sin( theta2 );		
		float U2 = cosine2 * uv.x - sine2 * uv.y;
		float V2 = sine2 * uv.x + cosine2 * uv.y;
		float2 samp2 = float2( U2, V2 );
		samp2 += 0.5;
	
		float4 main = tex2D( _MainTex, samp);
		
		float4 main2 = tex2D( _Detail, samp2);
    
    	//float4 main = tex2D (_MainTex, IN.uv_MainTex);
    	
        	
    	   	
    	
    	
        o.Albedo = main.rgb + main2.rgb;        
        o.Alpha = main.a;        
    }
    ENDCG
    }
    Fallback "Diffuse"
}