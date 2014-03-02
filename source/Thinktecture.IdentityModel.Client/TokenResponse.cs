﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using Newtonsoft.Json.Linq;
using System.Net;

namespace Thinktecture.IdentityModel.Client
{
    public class TokenResponse
    {
        public string Raw { get; protected set; }
        public JObject Json { get; protected set; }

        private bool _isHttpError;
        private HttpStatusCode _statusCode;
        private string _reason;

        public TokenResponse(string raw)
        {
            Raw = raw;
            Json = JObject.Parse(raw);
        }

        public TokenResponse(HttpStatusCode statusCode, string reason)
        {
            _isHttpError = true;
            _statusCode = statusCode;
            _reason = reason;
        }

        public bool IsHttpError
        {
            get
            {
                return _isHttpError;
            }
        }

        public HttpStatusCode ErrorStatusCode
        {
            get
            {
                return _statusCode;
            }
        }

        public string ErrorReason
        {
            get
            {
                return _reason;
            }
        }

        public string AccessToken
        {
            get
            {
                return GetStringOrNull(OAuth2Constants.AccessToken);
            }
        }

        public string IdentityToken
        {
            get
            {
                return GetStringOrNull(OAuth2Constants.IdentityToken);
            }
        }

        public string Error
        {
            get
            {
                return GetStringOrNull(OAuth2Constants.Error);
            }
        }

        public bool IsError
        {
            get
            {
                return (!string.IsNullOrWhiteSpace(GetStringOrNull(OAuth2Constants.Error)) ||
                        IsHttpError);
            }
        }

        public long ExpiresIn
        {
            get
            {
                return GetLongOrNull(OAuth2Constants.ExpiresIn);
            }
        }

        public string TokenType
        {
            get
            {
                return GetStringOrNull(OAuth2Constants.TokenType);
            }
        }

        public string RefreshToken
        {
            get
            {
                return GetStringOrNull(OAuth2Constants.RefreshToken);
            }
        }

        protected virtual string GetStringOrNull(string name)
        {
            JToken value;
            if (Json.TryGetValue(name, out value))
            {
                return value.ToString();
            }

            return null;
        }

        protected virtual long GetLongOrNull(string name)
        {
            JToken value;
            if (Json.TryGetValue(name, out value))
            {
                long longValue = 0;
                if (long.TryParse(value.ToString(), out longValue))
                {
                    return longValue;
                }
            }

            return 0;
        }
    }
}