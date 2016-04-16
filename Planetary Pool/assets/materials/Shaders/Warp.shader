Shader "Custom/Warp" {
 Properties {
        _MainTex ("Capture", 2D) = "white" {}
        _Detail ("Lines", 2D) = "gray" {}
        _Detail2 ("BG", 2D) = "gray" {}  
        _Detail3 ("Stars", 2D) = "gray" {}  
        _LineColor ("Line Color", Color) = (1,1,1,1)     
        
    }
    SubShader {
    Tags { "RenderType" = "Opaque" }
    CGPROGRAM
      #include "UnityCG.cginc"
      #pragma surface surf SimpleLambert

      half4 LightingSimpleLambert (SurfaceOutput s, half3 lightDir, half atten) {
          //half NdotL = dot (s.Normal, lightDir);
          half4 c = 1;
          c.rgb = s.Albedo;
          //c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
    
          return c;
      }

    struct Input {
        float2 uv_MainTex;        
        float4 _Time;
        float4 screenPos;
    };
    
    sampler2D _MainTex;
    sampler2D _Detail;
    sampler2D _Detail2;
    sampler2D _Detail3;
    uniform float4 _LineColor;    
    
    
    void surf (Input IN, inout SurfaceOutput o) {
    	float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
          screenUV.x -= 0.5;
          screenUV *= float2((_ScreenParams.x / _ScreenParams.y) * 0.501,0.99);
          screenUV.xy += float2(0.498, 0.013);          
    	
    	
    	
    	float3 capture = tex2D (_MainTex, screenUV).rgb;
    	
    	float2 nrm = normalize((capture.rg - 0.25) * 4);
    	
    	float4 lines = 1;
    	lines.rgb = tex2D (_Detail, float2(capture.b + 0.5, 0.5)).rgb; //* capture.b;
    	lines.rgb *= _LineColor;
    	//lines.rg = lines.rg / lines.b;
    	
    	//float4 c = 1;
    	//c.rgb = (tex2D (_Detail2, float2(capture * 0.2 - _Time.g + 0.4, 0.5)).rgb * 0.3 + lines)  * capture;
    	//capture.rg = pow(capture.rg, 0.454545);
    	capture.rg = (capture.rg - 0.5) * 0.5;
    	//capture.rg = pow(capture.rg, 2.2);
    	
    	float4 c = 1;
    	c.rgb = tex2D (_Detail2, IN.uv_MainTex + capture.rg + _Time.x * 0.06).rgb;
    	
    	float4 stars = 1;
    	stars.rgb = tex2D (_Detail3, IN.uv_MainTex + capture.rg + _Time.x * 0.03).rgb;
        
        o.Albedo =  (lines * c.rgb) + c.rgb * (capture.b * capture.b * 4.8 + 0.1) + stars;
        
        //o.Albedo *=  saturate(abs(dot(float2(cos(_Time.g), sin(_Time.g)), nrm)));
        //o.Albedo =  float3(nrm, 0);
        
        //o.Albedo =  lines;
        //o.Albedo = c.rgb;
    }
    ENDCG
    }
    Fallback "Diffuse"
}