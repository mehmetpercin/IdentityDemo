using System.Text.Json.Serialization;

namespace IdentityDemo.Models
{
    public class Response<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        public List<string> Errors { get; private set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
            };
        }

        public static Response<T> Fail(string errorMessage, int statusCode)
        {
            var errors = new List<string> { errorMessage };

            return new Response<T> { Errors = errors, StatusCode = statusCode };
        }
    }
}
