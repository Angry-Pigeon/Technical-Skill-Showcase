Shader "Custom/HoleWriter" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "Queue"="Geometry-1" "RenderType"="Opaque" }
        Pass {
            // Write the stencil value (1) into the stencil buffer.
            Stencil {
                Ref 1
                Comp Always
                Pass Replace
            }
            // Don't output any color.
//            ColorMask 0
            // Disable depth writes so this object doesn't occlude others.
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            uniform fixed4 _Color;

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
