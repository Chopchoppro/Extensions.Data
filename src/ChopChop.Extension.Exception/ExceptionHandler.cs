using System.Data;
using System.Net.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;   



namespace ChopChop.Extension.Exception;


public static class ExceptionHandler
{
    

    public static ErrorResponse GetErrorFromException(
     this   System.Exception exception
    )
    {
        var result = new ErrorResponse();

        switch (exception)
        {
            case NotFoundException notFound:
                result.MessageId = notFound.MessageId;
                result.Message = notFound.Message;
                result.Params = new Dictionary<string, object>
                {
                    {"EntityName", notFound.EntityName}
                };
                result.StatusCode = 404;
                break;
            case DataValidationException validation:
                result.MessageId = validation.MessageId;
                result.Message = validation.Message;
                result.StatusCode = 400;
                break;
            case System.ComponentModel.DataAnnotations.ValidationException validation:
                result.MessageId = validation.ValidationResult.ErrorMessage;
                result.Message = validation.Message;
                result.Params = validation.Data.OfType<KeyValuePair<string, object>>()
                    .ToDictionary(k => k.Key.ToString(), v => v.Value);
                result.StatusCode = 400;
                break;
            case DuplicateKeyException duplicateKey:
                result.MessageId = duplicateKey.MessageId;
                result.Message = duplicateKey.Message;
                result.Params = new Dictionary<string, object>
                {
                    {"EntityName", duplicateKey.EntityName}
                };
                result.StatusCode = 500;
                break;
            case ConcurrencyException concurrency:
                result.MessageId = concurrency.MessageId;
                result.Message = concurrency.Message;
                result.StatusCode = 500;
                break;
            case AppException appException:
                result.MessageId = appException.MessageId;
                result.Message = appException.Message;
                result.Params = new Dictionary<string, object>
                {
                    {"Params", appException.Params}
                };
                result.StatusCode = 500;
                break;
            case NotImplementedException _:
            case NotSupportedException _:
                result.MessageId = "NotFound";
                result.Message = exception.Message;
                result.StatusCode = 404;
                break;
            case ArgumentNullException _:
            case ArgumentOutOfRangeException _:
            case ArgumentException _:
                result.MessageId = "ValidationException";
                result.Message = exception.Message;
                result.StatusCode = 400;
                break;

            default:
                result.MessageId = "Exception";
                result.Message = exception.Message;
                result.StatusCode = 500;
                break;
        }

        return result;
    }
}
