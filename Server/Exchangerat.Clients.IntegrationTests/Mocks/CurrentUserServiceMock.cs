namespace Exchangerat.Clients.IntegrationTests.Mocks
{
    using Exchangerat.Services.Identity;
    using Moq;
    using System;

    public class CurrentUserServiceMock
    {
        public static Mock<ICurrentUserService> New
            => GetMock();

        public static Mock<ICurrentUserService> ExistingUser
            => GetMock("KiroKirov");

        private static Mock<ICurrentUserService> GetMock(string userId = null)
        {
            var mock = new Mock<ICurrentUserService>();

            userId ??= Guid.NewGuid().ToString();

            mock.SetupGet(x => x.Id).Returns(userId);

            return mock;
        }
    }
}
