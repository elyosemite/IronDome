using Microsoft.AspNetCore.Mvc;

namespace CertificationAuthority.Presentation.Endpoints;

public static class GetGuidEndpoint
{
    public static Task<IResult> ExecuteAsync([FromServices] ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("GenericLogger");
        var guid = Guid.NewGuid();
        logger.LogInformation("Generating Guid = {Guid}", guid);
        return Task.FromResult(Results.Ok(guid));
    }
}
