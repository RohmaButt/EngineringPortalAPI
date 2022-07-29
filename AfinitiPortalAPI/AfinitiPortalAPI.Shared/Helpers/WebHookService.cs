//using AfinitiPortalAPI.Shared.DTOs;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Runtime.Serialization.Json;
//using System.Text;
//using System.Threading.Tasks;

//namespace AfinitiPortalAPI.Shared.Helpers
//{
//    public static class WebHookService
//    {
//        public static bool CreateAndSendCustomizedWebHook(string runningPaycomActivity, string additionalMessage = null)
//        {
//            string ToEmail = string.Empty, Ccemail = string.Empty;
//            //Enum.TryParse(runningPaycomActivity, out RunningPaycomActivity CurrentpaycomActivity);
//            //using (var context = new PaycomEngineContext())
//            //{
//            ToEmail = "";//EngineAPIConfigService.EngineAPIConfigs.FirstOrDefault(x => x.ConfigAppEvent == runningPaycomActivity).Email;
//                Ccemail = "";//EngineAPIConfigService.EngineAPIConfigs.FirstOrDefault(x => x.ConfigAppEvent == runningPaycomActivity).CcEmail;
//            if (string.IsNullOrEmpty(ToEmail) || string.IsNullOrWhiteSpace(ToEmail)) ToEmail = "rohma.butt@afiniti.com";
//            //}
//            PaycomEngineStatusUpdateTemplateModel templateModel = new PaycomEngineStatusUpdateTemplateModel
//            {
//            //    PaycomActivity = CurrentpaycomActivity,
//                ToEmail = ToEmail,
//                CcEmail = Ccemail
//            };
//            //switch (CurrentpaycomActivity)
//            //{
//            //    case RunningPaycomActivity.DataScrub:
//            //    case RunningPaycomActivity.PaycomErrorReport:
//            //    case RunningPaycomActivity.DataTransform:
//            //    case RunningPaycomActivity.PushInDB:
//            //        templateModel.EmailAdditionalMessage = additionalMessage;
//            //        break;
//            //    case RunningPaycomActivity.Exchange:
//            //    case RunningPaycomActivity.ExchangeAddition:
//            //    case RunningPaycomActivity.ExchangeTerminationDaily:
//            //    case RunningPaycomActivity.ExchangeTermination:
//            //    case RunningPaycomActivity.AD:
//            //    case RunningPaycomActivity.Certify:
//            //    case RunningPaycomActivity.SD:
//            //    case RunningPaycomActivity.Decibel:
//            //    case RunningPaycomActivity.Cornerstone:
//            //        templateModel.EmailAdditionalMessage = string.Concat($" Please check generated {runningPaycomActivity} downstream. ", additionalMessage);
//            //        break;
//            //    case RunningPaycomActivity.TK_Pull:
//            //    case RunningPaycomActivity.TK_Push:
//            //        templateModel.EmailAdditionalMessage = additionalMessage;
//            //        break;
//            //    default:
//            //        templateModel.EmailAdditionalMessage = additionalMessage;
//            //        break;
//            //}
        
//            CreateWebHookEmail(templateModel);
//            return true;
//        }
//        public static void CreateWebHookEmail(PaycomEngineStatusUpdateTemplateModel model)
//        {
//            string processDescription = "";// General.GetEnumDescription(model.PaycomActivity.ToString());
//            WebHookObject<GeneralNotification> nObject = new WebHookObject<GeneralNotification>
//            {
//                CallerApp = "PaycomSyncEngine",
//                Action = "DownstreamStatus",
//                UTC_Time = DateTime.Now.ToString("yyyy-MM-dd"),
//                Data = new GeneralNotification()
//                {
//                    emailUsers = model.ToEmail,
//                    emailTemplate = "PaycomSyncEngineStatus",
//                    emailSubject = $"Paycom SyncEngine notification - {processDescription}",
//                   // emailFrom = EngineAPIConfigService.WebHookFromEmail != "" ? EngineAPIConfigService.WebHookFromEmail : "",
//                    emailBcc = "",
//                    emailCc = model.CcEmail,
//                    emailTemplateValues = EmailBody(model)
//                }
//            };
//            SendWebHooks(nObject);
//        }
//        public static async void SendWebHooks<T>(WebHookObject<T> pObject) where T : class
//        {
//            try
//            {
//                string uri = "";// EngineAPIConfigService.WebHookServiceUri;
//                string cred = "";// EngineAPIConfigService.WebHookServiceCred;
//                if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(cred))
//                    return;
//                MemoryStream objectStream = new MemoryStream();
//                DataContractJsonSerializer objectSerialize = new DataContractJsonSerializer(typeof(WebHookObject<T>));
//                objectSerialize.WriteObject(objectStream, pObject);

//                objectStream.Position = 0;
//                StreamReader objectRead = new StreamReader(objectStream);
//                string objectJSON = objectRead.ReadToEnd();

//                var stringContent = new StringContent(objectJSON, Encoding.UTF8, "application/json");
//                using (var client = new HttpClient())
//                {
//                    string pCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(cred));
//                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", pCredentials);

//                    var messReturn = await client.PostAsync(uri, stringContent);
//                    if (!messReturn.IsSuccessStatusCode)
//                        throw new Exception(messReturn.StatusCode.ToString());
//                }
//            }
//            catch (Exception ex)
//            {
//             //   ExceptionHandling.LogException(ex, "SendWebHooks");
//            }
//        }
//        private static List<string> EmailBody(PaycomEngineStatusUpdateTemplateModel model)
//        {
//            List<string> pMessage = new List<string>(3)
//            {
//                GetEnumDescription(model.PaycomActivity.ToString()),
//                model.EmailAdditionalMessage,
//                model.ToEmail
//            };
//            return pMessage;
//        }
//        public static string GetEnumDescription(string inputStr)
//        {
//            //RunningPaycomActivity activity;
//            //Enum.TryParse(inputStr, true, out activity);
//            System.Reflection.MemberInfo[] memInfo = activity.GetType().GetMember(activity.ToString());
//          //  DescriptionAttribute attribute = CustomAttributeExtensions.GetCustomAttribute<DescriptionAttribute>(memInfo[0]);
//            return attribute.Description;
//        }
//    }
//}