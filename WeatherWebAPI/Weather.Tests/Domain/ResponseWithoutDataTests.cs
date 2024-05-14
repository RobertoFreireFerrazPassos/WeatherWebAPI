namespace Weather.Tests.Domain;

public class ResponseWithoutDataTests
{
    [Fact]
    public void ResponseWithoutData_IsSuccessful_True()
    {
        // Arrange
        bool isSuccessful = true;

        // Act
        var response = new ResponseWithoutData(isSuccessful);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeTrue();
        response.ErrorMessage.Should().BeNullOrEmpty();
    }

    [Fact]
    public void ResponseWithoutData_IsSuccessful_False_With_ErrorMessage()
    {
        // Arrange
        bool isSuccessful = false;
        string errorMessage = "Some error message";

        // Act
        var response = new ResponseWithoutData(isSuccessful, errorMessage);

        // Assert
        response.Should().NotBeNull();
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be(errorMessage);
    }
}
