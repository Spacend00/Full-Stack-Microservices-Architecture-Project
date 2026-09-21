using MediatR;
using UserService.Application.Features.Users.Commands.CreateUser;
using UserService.Application.Features.Users.Commands.DeleteUser;
using UserService.Application.Features.Users.Commands.LoginUser;
using UserService.Application.Features.Users.Commands.LogoutUser;
using UserService.Application.Features.Users.Commands.RefreshToken;
using UserService.Application.Features.Users.Commands.UpdatePassword;
using UserService.Application.Features.Users.Commands.UpdateUserProfile;
using UserService.Application.Features.Users.Queries.GetAllActiveUser;
using UserService.Application.Features.Users.Queries.GetAllUser;
using UserService.Application.Features.Users.Queries.GetUserByEmail;
using UserService.Application.Features.Users.Queries.GetUserById;

namespace UserService.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/users").WithTags("Users");

            group.MapGet("/get_all", async (ISender sender, CancellationToken ct) =>
            {
                var users = await sender.Send(new GetAllUserQuery(), ct);
                return users is not null ? Results.Ok(users) : Results.NotFound();
            });

            group.MapGet("/get_all_active", async (ISender sender, CancellationToken ct) =>
            {
                var users = await sender.Send(new GetAllActiveUserQuery(), ct);
                return users is not null ? Results.Ok(users) : Results.NotFound();
            });

            group.MapGet("/get_by_id/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var user = await sender.Send(new GetUserByIdQuery(id), ct);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });

            group.MapGet("/get_by_email", async (string email, ISender sender, CancellationToken ct) =>
            {
                var user = await sender.Send(new GetUserByEmailQuery(email), ct);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });

            group.MapPost("/register", async (CreateUserCommand command, ISender sender, CancellationToken ct) =>
            {
                var userId = await sender.Send(command, ct);
                return userId != Guid.Empty ? Results.Ok(userId) : Results.NotFound(); 
            });

            group.MapPost("/login", async (LoginUserCommand command, ISender sender, CancellationToken ct) =>
            {
                var token = await sender.Send(command, ct);
                return token is not null ? Results.Ok(token) : Results.NotFound();
            });

            group.MapPost("/logout", async (LogoutRequest request, HttpContext context, ISender sender) =>
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                var accessToken = authHeader?.Replace("Bearer ", "").Trim() ?? string.Empty;

                var expClaim = context.User.FindFirst("exp")?.Value;
                var expTime = expClaim != null ? DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).UtcDateTime : DateTime.UtcNow;

                var command = new LogoutUserCommand(request.RefreshToken, accessToken, expTime);
                var result = await sender.Send(command);

                return result ? Results.Ok(new { Message = "Başarıyla çıkış yapıldı." }) : Results.BadRequest();
            }).RequireAuthorization();

            group.MapPost("/refresh-token", async (RefreshTokenCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Ok(result);
            });

            group.MapPut("/update_profile", async (UpdateUserProfileCommand command, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(command, ct);
            });

            group.MapPut("/update-password", async (UpdatePasswordCommand command, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(command, ct);
            });

            group.MapDelete("/delete/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(new DeleteUserCommand(id), ct);
            });
        }
    }
}
