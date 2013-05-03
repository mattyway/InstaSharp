﻿using InstaSharp.Models.Responses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstaSharp.Endpoints {
    public class InstagramAPI {

        public InstagramConfig InstagramConfig { get; private set; }
        public OAuthResponse OAuthResponse { get; private set; }
        public string Uri { get; set; }
        public RestSharp.RestClient Client { get; set; }

        public InstagramAPI(string endpoint, InstagramConfig instagramConfig, OAuthResponse oauthResponse = null) {
            InstagramConfig = instagramConfig;
            OAuthResponse = oauthResponse ?? null;
            Uri = InstagramConfig.APIURI + endpoint;
            Client = new RestSharp.RestClient(InstagramConfig.APIURI + "/" + endpoint);
        }

        internal Request Request(string fragment) {
            return AddAuth(new Request(fragment));
        }

        internal Request Request() {
            return AddAuth(new Request());
        }

        internal Request AddAuth(Request request) {
            if (OAuthResponse == null) {
                request.AddParameter("client_id", InstagramConfig.ClientId);
            } else {
                request.AddParameter("access_token", OAuthResponse.Access_Token);
            }

            return request;
        }

        internal StringBuilder FormatUri(string substitution = null) {
            var uri = new StringBuilder(Uri);
            
            if (substitution != null) {
                uri.Append(substitution);
            }

            string client_or_token = OAuthResponse == null ? "?client_id=" + InstagramConfig.ClientId : "?access_token=" + OAuthResponse.Access_Token;

            return uri.Append(client_or_token);
        }
    }
}
