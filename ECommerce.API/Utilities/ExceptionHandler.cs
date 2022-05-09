﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities.Helper;
using Entities.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Utilities
{
    public class ExceptionHandler : IActionFilter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(IWebHostEnvironment environment, ILogger<ExceptionHandler> logger, IConfiguration configuration)
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
                return;

            var exception = context.Exception.InnerException ?? context.Exception;

            string message = null;
            int status;

            context.ExceptionHandled = true;

            switch (exception)
            {
                case CustomException businessException:

                    status = businessException.Status;
                    message = businessException.Message;

                    context.Result = new OkObjectResult(new ApiResult()
                    {
                        Status = status,
                        Messages = new List<string> {message}
                    })
                    {
                        StatusCode = status,
                    };

                    break;

                default:
                    {
                        status = 500;

                        var result = new ApiResult()
                        {
                            Status = status
                        };

                        message = context.Exception.InnerException == null
                            ? context.Exception.Message
                            : context.Exception.InnerException.Message;

                        if (_environment.IsDevelopment())
                        {
                            result = new ApiResult()
                            {
                                Messages = new List<string> {message},
                                StackTrace = context.Exception.StackTrace,
                                Status = status
                            };
                        }

                        context.Result = new OkObjectResult(result)
                        {
                            StatusCode = status,
                        };

                        break;
                    }
            }


            var logModel = new LogModel()
            {
                Method = context.HttpContext.Request.Method,
                Messages = message,
                Status = status,
                Level = LogLevel.Error,
                Date = DateTime.Now,
                ApplicantId = context.HttpContext.User.Claims.Any() ? context.HttpContext.User?.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value : "",
                Route = context.HttpContext.Request.Path
            };

            _logger.LogError(JsonConvert.SerializeObject(logModel));
        }
    }
}