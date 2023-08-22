namespace FrontEnd.Extensions
{
    public static class SecurityHeadersDefinitions
    {
        public static HeaderPolicyCollection GetHeaderPolicyCollection(bool isDev)
        {
            HeaderPolicyCollection policy = new HeaderPolicyCollection()
                .AddFrameOptionsSameOrigin()
                .AddXssProtectionBlock()
                .AddContentTypeOptionsNoSniff()
                .AddReferrerPolicyOriginWhenCrossOrigin()
                //.AddCrossOriginOpenerPolicy(builder => builder.SameOriginAllowPopups())
                //.AddCrossOriginEmbedderPolicy(builder => builder.AddReport())
                //.AddCrossOriginResourcePolicy(builder => builder.AddReport())
                //.AddCustomHeader("Cross-Origin-Resource-Policy", "cross-origin")
                //.AddCustomHeader("Access-Control-Allow-Origin", "http://pcp-dbdev:88/")
                .AddCustomHeader("content-security-policy", "frame-ancestors 'self'; frame-src 'self'; manifest-src 'self'; ")
                .RemoveServerHeader()
                .AddPermissionsPolicy(builder =>
                {
                    //builder.AddAccelerometer().None();
                    //builder.AddAutoplay().None();
                    //builder.AddCamera().None();
                    //builder.AddEncryptedMedia().None();
                    _ = builder.AddFullscreen().All();
                    //builder.AddGeolocation().None();
                    //builder.AddGyroscope().None();
                    //builder.AddMagnetometer().None();
                    //builder.AddMicrophone().None();
                    //builder.AddMidi().None();
                    //builder.AddPayment().None();
                    //builder.AddPictureInPicture().None();
                    //builder.AddSyncXHR().None();
                    //builder.AddUsb().None();
                });

            AddCspHstsDefinitions(isDev, policy);

            return policy;
        }

        private static void AddCspHstsDefinitions(bool isDev, HeaderPolicyCollection policy)
        {
            if (!isDev)
            {
                _ = policy.AddContentSecurityPolicy(builder =>
                {
                    //_ = builder.AddObjectSrc().None();
                    //_ = builder.AddBlockAllMixedContent();
                    ////builder.AddImgSrc().None();
                    //builder.AddFormAction().None();
                    //builder.AddFontSrc().None();
                    //builder.AddStyleSrc().None();
                    //builder.AddScriptSrc().None();
                    _ = builder.AddBaseUri().Self();
                    _ = builder.AddFrameAncestors().None();
                    //builder.AddCustomDirective("require-trusted-types-for", "'script'");
                });
                // maxage = one year in seconds
                _ = policy.AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365);
            }
            else
            {
                // allow swagger UI for dev
                _ = policy.AddContentSecurityPolicy(builder =>
                {
                    //_ = builder.AddObjectSrc().None();
                    //_ = builder.AddBlockAllMixedContent();
                    //builder.AddImgSrc().Self().From("data:");
                    //builder.AddFormAction().Self();
                    //builder.AddFontSrc().Self();
                    //builder.AddStyleSrc().Self().UnsafeInline();
                    //builder.AddScriptSrc().Self().UnsafeInline(); //.WithNonce();
                    _ = builder.AddBaseUri().Self();
                    _ = builder.AddFrameAncestors().Self();
                });
            }
        }
    }
}
