using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RazorLight;
using SendGrid;
using SendGrid.Helpers.Mail;
using SmartStore.Web.Portal.Models;

namespace SmartStore.Web.Portal.Utility
{
    public class EmailSender
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        private ILogger<EmailSender> _logger;
        private SendGridClient _client;
        private RazorLightEngine _engine;
        private int _maxEmailAttempts;
        private string _fromName;
        private string _fromEmail;

        public EmailSender(IConfiguration configuration,
            ILogger<EmailSender> logger,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
            _client = new SendGridClient(_configuration["SENDGRID_API_KEY"]);

            if (_configuration.GetSection("Email") == null)
                throw new Exception("Configuration for Email not found");

            _maxEmailAttempts = Convert.ToInt32(_configuration["Email:MaxAttempts"] ?? "3");
            _fromEmail = _configuration["Email:FromEmail"];
            _fromName = _configuration["Email:FromName"];

            string templatePath = $@"{hostEnvironment.WebRootPath}\EmailTemplates";
            _engine = new RazorLightEngineBuilder()
                          .UseFilesystemProject(templatePath)
                          .UseMemoryCachingProvider()
                          .Build();
        }

        public async Task<bool> SendRegisterConfirmationEmailAsync(string email, string firstName, string lastName, string confirmationToken)
        {
            try
            {
                bool result = false;
                int attempts = 0;
                IUrlHelper urlHelper = (IUrlHelper)_httpContextAccessor.HttpContext.Items[BaseController.URLHELPER];
                string toName = $"{firstName} {lastName}";
                string subject = "Account Email Confirmation";

                var model = new ConfirmEmailModel
                {
                    Name = firstName,
                    ConfirmationUrl = urlHelper.Link("ConfirmEmail", new { token = confirmationToken })
                };

                string htmlContent = await _engine.CompileRenderAsync("ConfirmationEmail.cshtml", model);
                string plainTextContent = htmlContent;

                while (!result && attempts < _maxEmailAttempts)
                {
                    result = await SendEmailAsync(email, toName, subject, plainTextContent, htmlContent);
                    attempts++;
                }

                if (!result)
                    throw new Exception("Maxium number of attempts reached");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return false;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string toName, string subject, string plainTextContent, string htmlContent)
        {
            try
            {
                var from = new EmailAddress(_fromEmail, _fromName);
                var to = new EmailAddress(toEmail, toName);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await _client.SendEmailAsync(msg);
                if (((int)response.StatusCode) >= 200 && ((int)response.StatusCode) < 300)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            return false;
        }
    }
}
