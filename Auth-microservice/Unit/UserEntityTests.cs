using Auth_microservice.Domain.Entities;
using Auth_microservice.Domain.Enums;

using Xunit;

using Assert = Xunit.Assert;


public class UserEntityTests
{
    [Fact]
    public void Should_Lock_User_After_5_Failed_Attempts()
    {
        var user = User.Create("test", "pwd", Role.Admin, "cab1");

        for (int i = 0; i < 5; i++)
            user.RecordFailedLogin();

        Assert.True(user.IsLocked());
    }

    [Fact]
    public void Should_Enable_2FA()
    {
        var user = User.Create("test", "pwd", Role.Admin, "cab1");

        user.Enable2FA("secret");

        Assert.True(user.Is2FAEnabled);
    }
}