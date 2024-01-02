using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Data.DBRepository.RoleManagement;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.RoleManagement;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLifeAPI.Areas.Admin.Controllers;
using Xunit;

namespace SuperariLife.Tests.Controllers.Admin
{
    public class RoleManagementControllerTests
    {
        private readonly Mock<IRoleManagementService> _roleManagementServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private readonly RoleManagementController _roleManagementController;

        public RoleManagementControllerTests()
        {
            _roleManagementServiceMock = new Mock<IRoleManagementService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();

            _roleManagementController = new RoleManagementController(
                _roleManagementServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Fact]
        public async Task InsertUpdateRoleManagement_ReturnsSuccess()
        {
            // Arrange
            var roleManagementReqModel = new RoleManagementReqModel
            {
            };
            var expectedResult = new BaseApiResponse { Success = true };
            var jwtToken = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MTAwMTksXCJVc2VySWRcIjowLFwiUm9sZUlkXCI6MTAwMDcsXCJMYXN0TmFtZVwiOlwibmFuZGFcIixcIkZpcnN0TmFtZVwiOlwiTWFuYXZcIixcIkVtYWlsSWRcIjpcIm1hbmF2bmFuZGE5NzIzQGdtYWlsLmNvbVwiLFwiRnVsbE5hbWVcIjpcIk1hbmF2bmFuZGFcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMTM4MjMyLCJleHAiOjE3MDMzOTg2MzIsImlhdCI6MTcwMzEzODIzMn0._HNglwzA_8xTSpU12bv0GPFdHKGut9-NNw0Xw-LjLwkusdElcHTuyhv4L-m8LZvBblDarG8yvwqSGT9qwxdjfg";
            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(new StringValues($"Bearer {jwtToken}"));
            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(jwtToken))
                .Returns(new TokenModel { Id = 1 });

            _roleManagementServiceMock.Setup(x => x.InsertUpdateRoleManagement(It.IsAny<RoleManagementReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _roleManagementController.InsertUpdateRoleManagement(roleManagementReqModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetRoleListByAdmin_ReturnsRoleList()
        {
            // Arrange
            var expectedResult = new ApiResponse<RoleManagementResponseModel> { Success = true, Data = new List<RoleManagementResponseModel>() };
            _roleManagementServiceMock.Setup(x => x.GetRoleManagementList(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<RoleManagementResponseModel>());

            // Act
            var result = await _roleManagementController.GetRoleListByAdmin(new CommonPaginationModel());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact] 
        public async Task DeleteRoleManagement_ReturnsSuccess()
        {
            // Arrange
            var expectedResult = new ApiPostResponse<RoleManagementDeleteResponseModel> { Success = true, Data = new RoleManagementDeleteResponseModel() };
            _roleManagementServiceMock.Setup(x => x.DeleteRoleManagement(It.IsAny<long>()))
                .ReturnsAsync(new RoleManagementDeleteResponseModel { StatusOfRole = Status.Success });

            // Act
            var result = await _roleManagementController.DeleteRoleManagement(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Success, result.Success);
        }
    }
}
