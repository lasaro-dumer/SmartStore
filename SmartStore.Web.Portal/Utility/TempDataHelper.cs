using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SmartStore.Web.Portal.Utility
{
    public static class TempDataHelper
    {
        public const string INFORMATION_MESSAGES = "InformationMessages";
        public const string ERROR_MESSAGES = "ErrorMessages";
        public const string WARNING_MESSAGES = "WarningMessages";

        public static List<string> GetValidList(object listCandidate)
        {
            if (listCandidate != null)
            {
                if (listCandidate is string[])
                    return ((string[])listCandidate).ToList();
                if (listCandidate is List<string>)
                    return (List<string>)listCandidate;
            }

            return new List<string>();
        }

        public static void AddInformationMessage(this Controller controller, string message)
        {
            List<string> informationMessages = GetValidList(controller.TempData[INFORMATION_MESSAGES]);
            informationMessages.Add(message);
            controller.TempData[INFORMATION_MESSAGES] = informationMessages;
        }

        public static void AddErrorMessage(this Controller controller, string message)
        {
            List<string> errorMessages = GetValidList(controller.TempData[ERROR_MESSAGES]);
            errorMessages.Add(message);
            controller.TempData[ERROR_MESSAGES] = errorMessages;
        }

        public static void AddWarningMessage(this Controller controller, string message)
        {
            List<string> warningMessages = GetValidList(controller.TempData[WARNING_MESSAGES]);
            warningMessages.Add(message);
            controller.TempData[WARNING_MESSAGES] = warningMessages;
        }
    }
}
