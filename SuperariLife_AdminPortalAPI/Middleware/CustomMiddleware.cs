using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using SuperariLife.Common.CommonMethod;
using SuperariLife.Common.EmailNotification;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.ReqResponse;
using SuperariLife.Model.Settings;
using SuperariLife.Model.Token;
using SuperariLife.Service.Account;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.Logger;
using System.Diagnostics;
using System.Net;
using System.Text;
using static SuperariLife.Common.EmailNotification.EmailNotification;

namespace SuperariLifeAPI.Middleware
{
    public class CustomMiddleware
    {
        private readonly ILoggerManager _logger;
        private readonly RequestDelegate _next;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IJWTAuthenticationService _jwtAuthenticationService;
        private readonly SMTPSettings _smtpSettings;
        private IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="memoryCache"></param>
        public CustomMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IJWTAuthenticationService jwtAuthenticationService,
            IConfiguration config, IOptions<SMTPSettings> smtpSettings,
            ILoggerManager logger)
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
            _jwtAuthenticationService = jwtAuthenticationService;
            _config = config;
            _smtpSettings = smtpSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Invoke on every request response
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settingService"></param>
        public async Task Invoke(HttpContext context, IAccountService _accountService)
        {
            var directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs");

            // Delete files from folder for logs of request and response older than 7 days.
            DeleteOldReqResLogFiles();

            string headervalue = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(headervalue))
            {
                string jwtToken = context.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    TokenModel userTokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);

                     var result = await _accountService.ValidateUserTokenData(userTokenModel.Id, jwtToken, userTokenModel.TokenValidTo,userTokenModel.IsSuperAdmin);

                    if (result == 1)
                    {
                        if (userTokenModel != null)
                        {
                            if (userTokenModel.TokenValidTo < DateTime.UtcNow.AddMinutes(1))
                            {
                                context.Response.ContentType = Constants.ContentTypeJson;
                                context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = Constants.AccessControlAllowOrigin;
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return;
                            }
                        }
                    }
                    else
                    {
                        context.Response.ContentType = Constants.ContentTypeJson;
                        context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = Constants.AccessControlAllowOrigin;
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }

                }
            }

            using (MemoryStream requestBodyStream = new MemoryStream())
            {
                using (MemoryStream responseBodyStream = new MemoryStream())
                {
                    Stream originalRequestBody = context.Request.Body;
                    //context.Request.EnableRewind();
                    Stream originalResponseBody = context.Response.Body;

                    try
                    {
                        await context.Request.Body.CopyToAsync(requestBodyStream);
                        requestBodyStream.Seek(0, SeekOrigin.Begin);

                        string requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                        requestBodyStream.Seek(0, SeekOrigin.Begin);
                        context.Request.Body = requestBodyStream;

                        string responseBody = "";

                        context.Response.Body = responseBodyStream;

                        Stopwatch watch = Stopwatch.StartNew();
                        await _next(context);
                        watch.Stop();

                        responseBodyStream.Seek(0, SeekOrigin.Begin);
                        responseBody = new StreamReader(responseBodyStream).ReadToEnd();

                        if (!context.Request.Path.Value.ToString().ToLower().Contains("swagger"))
                        {

                            AddRequestLogsToLoggerFile(context, requestBodyText, responseBody);

                        }
                        responseBodyStream.Seek(0, SeekOrigin.Begin);

                        await responseBodyStream.CopyToAsync(originalResponseBody);
                    }
                    catch (Exception ex)
                    {
                        await context.Request.Body.CopyToAsync(requestBodyStream);
                        requestBodyStream.Seek(0, SeekOrigin.Begin);

                        string requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

                        context.Response.ContentType = Constants.ContentTypeJson;
                        context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = Constants.AccessControlAllowOrigin;
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        byte[] data = System.Text.Encoding.UTF8.GetBytes(new BaseApiResponse()
                        {
                            Success = false,
                            Message = ex.Message
                        }.ToString());

                        originalResponseBody.WriteAsync(data, 0, data.Length);

                        AddExceptionLogsToLoggerFile(context, ex, requestBodyText);

                        SendExceptionEmail(ex, context);
                        return;
                    }
                    finally
                    {
                        context.Request.Body = originalRequestBody;
                        context.Response.Body = originalResponseBody;
                    }
                }
            }

        }

        private void AddRequestLogsToLoggerFile(HttpContext context, String requestBodyText, String responseBody)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                TokenModel userTokenData = null;
                ParamValue paramValues = CommonMethods.GetKeyValues(_httpContextAccessor.HttpContext);
                string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    userTokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                }
                string userFullName = userTokenData != null ? userTokenData.UserId + " ( " + userTokenData.FirstName + " )" : "";
                StringBuilder sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          Environment.NewLine + "--------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ---------------------------" +
                          Environment.NewLine + "User : " + userFullName +
                          Environment.NewLine + "Requested URL: " + context.Request.Path.Value +
                          Environment.NewLine + "Request Body: " + requestBodyText +
                          Environment.NewLine + "Query String Params: " + paramValues.QueryStringValue +
                          Environment.NewLine + "Response: " + responseBody +
                          Environment.NewLine + "Status Code: " + context.Response.StatusCode +
                          Environment.NewLine);
                logger.Info(sb.ToString());


            }
            catch (Exception ex)
            {
                logger.Error("Exception in AddRequestLogsToLoggerFile: ", ex.Message.ToString());
            }


        }

        #region AddExceptionLogsToLoggerFile
        /// <summary>
        /// Store exception in Logger Exception file
        /// </summary>
        private void AddExceptionLogsToLoggerFile(HttpContext context, Exception ex, string requestBody)
        {
            TokenModel userTokenData = null;
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            ParamValue paramValues = CommonMethods.GetKeyValues(_httpContextAccessor.HttpContext);
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                userTokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
            }

            string userFullName = userTokenData != null ? userTokenData.UserId + " ( " + userTokenData.FirstName + " )" : "";
            StringBuilder sb = new StringBuilder();


            sb.Append(Environment.NewLine +
                      Environment.NewLine + "--------------------- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ---------------------------" +
                      Environment.NewLine + "User : " + userFullName +
                      Environment.NewLine + "Requested URL: " + context.Request.Path.Value +
                      Environment.NewLine + "Request Params: " + requestBody +
                      Environment.NewLine + "Query String Params: " + paramValues.QueryStringValue +
                      Environment.NewLine + "Status Code: " + context.Response.StatusCode +
                      Environment.NewLine + "Exception: " + ex.Message +
                      Environment.NewLine + "Exception: " + ex.InnerException +
                      Environment.NewLine);
            logger.Error(sb.ToString());
        }
        #endregion

        ///// <summary>
        ///// Delete files from error logs folder which is older than 7 days.
        ///// </summary>
        public void DeleteOldReqResLogFiles()
        {

            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, Constants.LogsFilePathException);

            string[] files = Directory.GetFiles(filePath);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-7))
                {
                    fi.Delete();
                }
            }
            var directoryPath = Path.Combine(_hostingEnvironment.ContentRootPath, Constants.LogsFilePathRequest);

            string[] Dfiles = Directory.GetFiles(directoryPath);

            foreach (string file in Dfiles)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-7))
                {
                    fi.Delete();
                }
            }
        }



        /// <summary>
        /// Send Exception Email
        /// </summary>
        public async Task<bool> SendExceptionEmail(Exception ex, HttpContext context)
        {
            TokenModel userTokenData = null;

            ParamValue paramValues = CommonMethods.GetKeyValues(_httpContextAccessor.HttpContext);
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                userTokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            EmailNotification.EmailSetting setting = new EmailSetting
            {
                EmailEnableSsl = Convert.ToBoolean(_smtpSettings.EmailEnableSsl),
                EmailHostName = _smtpSettings.EmailHostName,
                EmailPassword = _smtpSettings.EmailPassword,
                EmailAppPassword = _smtpSettings.EmailAppPassword,
                EmailPort = Convert.ToInt32(_smtpSettings.EmailPort),
                FromEmail = _smtpSettings.FromEmail,
                FromName = _smtpSettings.FromName,
                EmailUsername = _smtpSettings.EmailUsername,
            };

            string emailBody = string.Empty;
            string BasePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.EmailTemplate);

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            bool isSuccess = false;

            using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.ExceptionReport)))
            {
                emailBody = reader.ReadToEnd();
            }
            string host = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/";
            string Client_URL = Convert.ToString(_config["Data:WebAppURL"]);
            emailBody = emailBody.Replace("##LogoURL##", host + Constants.ImagePath+ "/tom-logo.png");
            emailBody = emailBody.Replace("##DateTime##", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            emailBody = emailBody.Replace("##RequestedURL##", context.Request.GetDisplayUrl());
            emailBody = emailBody.Replace("##ExceptionMessage##", ex.Message);
            emailBody = emailBody.Replace("##RequestParams##", paramValues.HeaderValue);
            emailBody = emailBody.Replace("##QueryStringParams##", paramValues.QueryStringValue);
            emailBody = ex.InnerException != null ? emailBody.Replace("##InnerException##", ex.InnerException.ToString()) : emailBody.Replace("##InnerException##", string.Empty);
            emailBody = userTokenData != null && userTokenData.UserId != null ? emailBody = emailBody.Replace("##UserId##", userTokenData.UserId.ToString()) : emailBody.Replace("##UserId##", string.Empty);
            emailBody = userTokenData != null && userTokenData.EmailId != null ? emailBody = emailBody.Replace("##UserName##", userTokenData.EmailId.ToString()) : emailBody.Replace("##UserName##", string.Empty);
            isSuccess = SendMailMessage(_appSettings.ErrorSendToEmail, null, null, "Exception Log Alert !", emailBody, setting, null);
            if (isSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Handle Exception for middleware
        /// </summary>
        private Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            context.Response.ContentType = Constants.ContentTypeJson;
            context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = Constants.AccessControlAllowOrigin;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(new BaseApiResponse()
            {
                Success = false,
                Message = exception.Message
            }.ToString());
        }

        /// <summary>
        /// Store exception in logger file
        /// </summary>
        private void AddExceptionLogsToLoggerFile(HttpContext context, Exception exception)
        {
            ParamValue paramValues = CommonMethods.GetKeyValues(_httpContextAccessor.HttpContext);
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine + Constants.RequestParams + paramValues.HeaderValue +
                      Environment.NewLine + Constants.QueryStringParams + paramValues.QueryStringValue +
                      Environment.NewLine + Constants.RequestMessage + exception.Message);
            _logger.Error(sb.ToString());
        }
    }
}
