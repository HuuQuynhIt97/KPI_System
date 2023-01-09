using A4KPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace A4KPI.Helpers
{
    public static class ErrorUtil
    {
        public static OperationResult GetMessageError(this Exception ex)
        {
            string message = ex.Message;
            if (ex.InnerException != null)
            {
                message += " \n " + ex.InnerException.Message;
            }

            return new OperationResult { StatusCode = HttpStatusCode.InternalServerError, Message = message, Success = false };

        }
    }
}
