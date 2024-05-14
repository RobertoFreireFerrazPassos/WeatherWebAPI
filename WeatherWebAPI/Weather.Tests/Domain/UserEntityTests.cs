namespace Weather.Tests.Domain;

public class UserEntityTests
{
    [Fact]
    public void GenerateName_Should_Set_Username_Correctly()
    {
        // Arrange
        var user = new UserEntity
        {
            Firstname = "John",
            Lastname = "Doe"
        };

        // Act
        user.GenerateName();

        // Assert
        user.Username.Should().NotBeNullOrEmpty();
        user.Username.Should().HaveLength(9);
        user.Username.Should().StartWith("jodo");
    }

    [Theory]
    [InlineData("123", true)]
    [InlineData("456", false)]
    public void IsValidPhoneNumber_Should_Return_Correct_Result(string idd, bool expected)
    {
        // Arrange
        var user = new UserEntity
        {
            PhoneNumber = "1234567890"
        };

        // Act
        var result = user.IsValidPhoneNumber(idd);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GetLocation_Should_Return_LivingCountry()
    {
        // Arrange
        var user = new UserEntity
        {
            LivingCountry = "Canada"
        };

        // Act
        var location = user.GetLocation();

        // Assert
        location.Should().Be("Canada");
    }
}