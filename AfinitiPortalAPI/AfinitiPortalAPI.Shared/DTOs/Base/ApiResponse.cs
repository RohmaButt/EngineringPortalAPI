using AfinitiPortalAPI.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class ApiResponse
    {
        #region Ctor
        public ApiResponse()
        {
            this.Success();
        }

        public ApiResponse(ApiResponseCode code, string explanation)
        {
            this.ResponseCode = code;
            this.Explanation = explanation;
        }
        #endregion

        [JsonProperty("responseCode")]
        public ApiResponseCode ResponseCode { get; set; }

        [JsonProperty("explanation")]
        public string Explanation { get; set; }

        #region Hot Funcs
        public ApiResponse Success(ApiResponseCode responseCode = ApiResponseCode.Success, string explanation = "Success!")
        {
            this.ResponseCode = responseCode;
            this.Explanation = explanation;
            return this;
        }

        public ApiResponse NotFound(ApiResponseCode responseCode = ApiResponseCode.NotFound, string explanation = "Not Found.")
        {
            this.ResponseCode = responseCode;
            this.Explanation = explanation;
            return this;
        }

        public ApiResponse Failure(ApiResponseCode responseCode = ApiResponseCode.Failure, string explanation = "Sorry for unexpected error.")
        {
            this.ResponseCode = responseCode;
            this.Explanation = explanation;
            return this;
        }

        public ApiResponse NotAuthorized(ApiResponseCode responseCode = ApiResponseCode.NotAuthorized, string explanation = "This user is not authorized for Engineering Portal.")
        {
            this.ResponseCode = responseCode;
            this.Explanation = explanation;
            return this;
        }
        #endregion

        #region Static
        public static ApiResponse Success()
        {
            return new ApiResponse();
        }

        public static ApiResponse NotFound()
        {
            return new ApiResponse().NotFound();
        }

        public static ApiResponse Failure()
        {
            return new ApiResponse().Failure();
        }

        public static ApiResponse NotAuthorized()
        {
            return new ApiResponse().NotAuthorized();
        }
        #endregion
    }
}
