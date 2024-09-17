using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace CaloryCalculation.API.Handlers
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var bearerResult = await Context.AuthenticateAsync(IdentityConstants.BearerScheme);

            if (!bearerResult.None)
            {
                return bearerResult;
            }

            return await Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // Устанавливаем статус код 401
            Response.StatusCode = 401;

            // Добавляем заголовок для информации о причине отказа (опционально)
            Response.Headers["WWW-Authenticate"] = "Bearer";

            // Вы можете добавить здесь дополнительную логику для обработки вызова

            return Task.CompletedTask;
        }
    }
}
