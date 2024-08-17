using Microsoft.AspNetCore.Identity;

namespace CaloryCalculation.Application.DTOs.Errors
{
    public class UserResults
    {
        public UserResults() { }


        public UserResults(IdentityResult result)
        {
            IdentityErrors = result.Errors;
            Success = result.Succeeded;
        }

        public UserResults(string id, IdentityResult result) : this(result)
        {
            UserId = id;
        }

        public UserResults(string code, string id, IdentityResult result) : this(id, result)
        {
            EmailSenderCode = code;
        }

        public IEnumerable<IdentityError> IdentityErrors { get; set; }

        public bool Success { get; set; }

        public string UserId {  get; set; }

        public string EmailSenderCode { get; set; }
    }
}
