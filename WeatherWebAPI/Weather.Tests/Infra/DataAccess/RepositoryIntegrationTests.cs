namespace Weather.Tests.Infra.DataAccess;

public class RepositoryIntegrationTests
{
    private readonly DbConfig _intTestDbConfig = new DbConfig()
    {
        ConnectionString = TestConstants.ConnectionString,
    };

    private readonly DbConfig _invalidIntTestDbConfig = new DbConfig()
    {
        ConnectionString = "Host=invalidHost; Port=8082; Database=weatheritInvalid; Username=simha; Password=Postgres2019!;"
    };

    private readonly Mock<IOptions<DbConfig>> _dbConfigMock = new Mock<IOptions<DbConfig>>();

    [Fact]
    public async Task QueryAsync_Should_ReturnData_When_ValidQuery()
    {
        // Arrange
        _dbConfigMock.Setup(m => m.Value).Returns(_intTestDbConfig);
        var repository = new Repository(_dbConfigMock.Object);

        // Act
        var response = await repository.QueryAsync<int>("SELECT COUNT(*) FROM UserRegistration");

        // Assert
        response.IsSuccessful.Should().BeTrue();
        response.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task QueryAsync_Should_ReturnError_When_ConnectionError()
    {
        // Arrange
        _dbConfigMock.Setup(m => m.Value).Returns(_invalidIntTestDbConfig);
        var repository = new Repository(_dbConfigMock.Object);

        // Act
        var response = await repository.QueryAsync<int>("SELECT COUNT(*) FROM UserRegistration");

        // Assert
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Be("Error connecting to database");
    }

    [Fact]
    public async Task QueryAsync_Should_ReturnError_When_QueryError()
    {
        // Arrange
        _dbConfigMock.Setup(m => m.Value).Returns(_intTestDbConfig);
        var repository = new Repository(_dbConfigMock.Object);

        // Act
        var response = await repository.QueryAsync<UserEntity>("INVALID SQL");

        // Assert
        response.IsSuccessful.Should().BeFalse();
        response.ErrorMessage.Should().Contain("Query error for UserEntity");
    }
}