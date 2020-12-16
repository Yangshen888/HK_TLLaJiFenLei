﻿using System;
using System.Net;

namespace HaikanRefuseClassification.Api.Extensions.CustomException
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        public HttpException(HttpStatusCode statusCode) { StatusCode = statusCode; }
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }
    }
}
