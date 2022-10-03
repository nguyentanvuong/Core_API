using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using WebApi.Helpers.Utils;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;
using WebApi.Helpers.Common;
using System.Text;

namespace WebApi.Helpers.Services
{
    public class EncryptMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<EncryptMiddleware> _logger;


        public EncryptMiddleware(RequestDelegate next, ILogger<EncryptMiddleware> loggery)
        {
            _next = next;
            _logger = loggery;
        }

        /// <summary>
        /// 1：RSA giải mã dữ liệu trong phương pháp Body in the Post
        /// 2：Mã hóa dữ liệu trả về bằng RSA
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();
            if (!path.Contains("/coreapi/") && !path.Contains("/login") && !path.Contains("/favicon.ico") && !path.Contains("/swagger-ui") && !path.ToUpper().Contains("/DXXRDV"))
            {

                _logger.LogInformation($"Handling request: " + context.Request.Path);
                var api = new ApiRequestInputViewModel
                {
                    HttpType = context.Request.Method,
                    Query = context.Request.QueryString.Value,
                    RequestUrl = context.Request.Path,
                    RequestName = "",
                    RequestIP = context.Request.Host.Value
                };

                var request = context.Request.Body;
                var response = context.Response.Body;

                try
                {
                    using (var newRequest = new MemoryStream())
                    {
                        //thay thế luồn
                        //context.Request.Body = newRequest;

                            context.Request.EnableBuffering();
                            using (var reader = new StreamReader(
                            context.Request.Body,
                            encoding: Encoding.UTF8,
                            detectEncodingFromByteOrderMarks: false,
                            bufferSize: 1024,
                            leaveOpen: true))
                            {
                                api.Body = await reader.ReadToEndAsync();
                                context.Request.Body.Position = 0;

                                api.Body = RSAUtilEncrypt.Decrypt(api.Body, GlobalVariable.yourPrivateKey);
                            }

                            context.Request.Body = request;
                            await _next(context);

                            //thay thế luồn
                            context.Response.Body = response;


                            //response.Seek(0, SeekOrigin.Begin);
                            //api.ResponseBody = new StreamReader(response).ReadToEnd();
                            //response.Seek(0, SeekOrigin.Begin);
                          
                            using (var reader = new StreamReader(
                            context.Response.Body,
                            encoding: Encoding.UTF8,
                            detectEncodingFromByteOrderMarks: false,
                            bufferSize: 1024,
                            leaveOpen: true))
                            {
                                response.Position = 0;
                                api.ResponseBody = await reader.ReadToEndAsync();

                                if (!string.IsNullOrWhiteSpace(api.ResponseBody))
                                {
                                    api.ResponseBody = RSAUtilEncrypt.Encrypt(api.ResponseBody, GlobalVariable.myPublicKey);
                                }
                            }
                        
                           context.Response.Body = response;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($" Error: " + ex.ToString());
                }
                finally
                {
                    context.Request.Body = request;
                    context.Response.Body = response;
                }

                // lưu trữ trong bộ đệm khi phản hồi hoàn tất
                context.Response.OnCompleted(() =>
                {
                    _logger.LogDebug($"RequestLog:{DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next(0, 10000)}-{api.ElapsedTime}ms", $"{JsonConvert.SerializeObject(api)}");
                    return Task.CompletedTask;
                });
            }
            else
            {
                await _next(context);
            }
        }        
    }
}
