using Moq;
using Xunit;
using Auth_microservice.Services;
using Auth_microservice.Domain.Interfaces;
using Auth_microservice.Domain.Entities;
using Auth_microservice.Exceptions;
using Assert = Xunit.Assert;


public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IJwtService> _jwt = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<ITotpService> _totp = new();

    private AuthService CreateService()
        => new AuthService(_uow.Object, _jwt.Object, _hasher.Object,  _totp.Object);

    [Fact]
    public async Task Login_Should_Throw_When_User_Not_Found()
    {
        _uow.Setup(x => x.Users.GetByLoginAsync(It.IsAny<string>(), default))
            .ReturnsAsync((User)null);

        var service = CreateService();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.LoginAsync("test", "pass"));
    }
}