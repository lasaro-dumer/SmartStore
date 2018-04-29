using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SmartStore.Web.Portal.Helpers
{
    public static class TempDataHelper
    {
        public const string INFORMATION_MESSAGES = "InformationMessages";
        public const string ERROR_MESSAGES = "ErrorMessages";
        public const string WARNING_MESSAGES = "WarningMessages";

        public static void AddInformationMessage(this Controller controller, string message)
        {
            List<string> informationMessages = (List<string>)controller.TempData[INFORMATION_MESSAGES] ?? new List<string>();
            informationMessages.Add(message);
            controller.TempData[INFORMATION_MESSAGES] = informationMessages;
        }

        public static void AddErrorMessage(this Controller controller, string message)
        {
            List<string> errorMessages = (List<string>)controller.TempData[ERROR_MESSAGES] ?? new List<string>();
            errorMessages.Add(message);
            controller.TempData[ERROR_MESSAGES] = errorMessages;
        }

        public static void AddWarningMessage(this Controller controller, string message)
        {
            List<string> warningMessages = (List<string>)controller.TempData[WARNING_MESSAGES] ?? new List<string>();
            warningMessages.Add(message);
            controller.TempData[WARNING_MESSAGES] = warningMessages;
        }
    }
}
