namespace Weather.Tests.Domain;

public class ResponseTests
{
    [Fact]
    public void Response_IsSuccessful_True()
    {
        // Arrange
        var data = "Some data";

        // Act
        var response = new Response<string>(true, data: data);

        // Assert
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeEmpty();
        response.Data.Should().Be(data);
    }

    [Fact]
    public void Response_IsSuccessful_False_With_ErrorMessage()
    {
        // Arrange
        var errorMessage = "An error occurred";

        // Act
        var response = new Response<string>(false, errorMessage);

        // Assert
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be(errorMessage);
        response.Data.Should().BeNull();
    }

    [Fact]
    public void Response_IsSuccessful_False_Without_ErrorMessage()
    {
        // Act
        var response = new Response<string>(false);

        // Assert
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().BeEmpty();
        response.Data.Should().BeNull();
    }
}
