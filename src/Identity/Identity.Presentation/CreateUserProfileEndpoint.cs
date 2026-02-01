using FastEndpoints;
using Identity.Domain;

namespace Identity.Presentation;

public record CreateUserProfileRequest(string Name);
public record CreateUserProfileResponse(Guid Id, string Name, DateTime CreatedAt);

public class CreateUserProfileEndpoint : Endpoint<CreateUserProfileRequest, CreateUserProfileResponse>
{
    public override void Configure()
    {
        Post("/api/userprofile");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserProfileRequest req, CancellationToken ct)
    {
        var user = UserProfile.Create(req.Name);

        var response = new CreateUserProfileResponse(user.Id, user.Name, DateTime.Now);
        await Send.ResultAsync(TypedResults.Ok(response));
    }
}