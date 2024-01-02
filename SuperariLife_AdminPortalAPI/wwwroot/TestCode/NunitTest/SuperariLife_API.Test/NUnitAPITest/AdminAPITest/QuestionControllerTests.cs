using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;
using SuperariLife.Common.Enum;
using SuperariLife.Common.Helpers;
using SuperariLife.Model.CommonPagination;
using SuperariLife.Model.Question;
using SuperariLife.Model.Token;
using SuperariLife.Service.JWTAuthentication;
using SuperariLife.Service.Question;
using SuperariLifeAPI.Areas.Admin.Controllers;
using SuperariLifeAPI.Areas.CustomerPortal.Controllers;

namespace SuperariLife_API.Test.NUnitAPITest.AdminAPITest
{
    [TestFixture]
    public class QuestionControllerTests
    {
        private Mock<IQuestionService> _questionServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IJWTAuthenticationService> _jwtAuthenticationServiceMock;
        private QuestionController _questionController;

        [SetUp]
        public void Setup()
        {
            _questionServiceMock = new Mock<IQuestionService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _jwtAuthenticationServiceMock = new Mock<IJWTAuthenticationService>();
            _questionController = new QuestionController(
                _questionServiceMock.Object,
                _httpContextAccessorMock.Object,
                _jwtAuthenticationServiceMock.Object
            );
        }

        [Test]
        public async Task InsertUpdateQuestion_ReturnsSuccess()
        {
            // Arrange
            var questionReqModel = new QuestionReqModel();
            var expectedResult = new BaseApiResponse { Success = true };

            _httpContextAccessorMock.Setup(x => x.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns("Bearer YourTokenHere");

            _jwtAuthenticationServiceMock.Setup(x => x.GetTokenData(It.IsAny<string>()))
                .Returns(new TokenModel { Id = 1 });

            _questionServiceMock.Setup(x => x.InsertUpdateQuestion(It.IsAny<QuestionReqModel>()))
                .ReturnsAsync(StatusResult.Updated);

            // Act
            var result = await _questionController.InsertUpdateQuestion(questionReqModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task GetQuestionListByAdmin_ReturnsQuestionList()
        {
            // Arrange
            var commonPaginationModel = new CommonPaginationModel();
            var expectedResult = new ApiResponse<QuestionResponseModel> { Success = true, Data = new List<QuestionResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _questionController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _questionServiceMock.Setup(x => x.GetQuestionListByAdmin(It.IsAny<CommonPaginationModel>()))
                .ReturnsAsync(new List<QuestionResponseModel>());

            // Act
            var result = await _questionController.GetQuestionListByAdmin(commonPaginationModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetQuestionListForDropDownList_ReturnsQuestionList()
        {
            // Arrange
            var expectedResult = new ApiResponse<QuestionResponseModel> { Success = true, Data = new List<QuestionResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _questionController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _questionServiceMock.Setup(x => x.GetQuestionListForDropDownList())
                .ReturnsAsync(new List<QuestionResponseModel>());

            // Act
            var result = await _questionController.GetQuestionListForDropDownList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetQuestionTypeListByAdmin_ReturnsQuestionTypeList()
        {
            // Arrange
            var expectedResult = new ApiResponse<QuestionTypeResponseModel> { Success = true, Data = new List<QuestionTypeResponseModel>() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _questionController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _questionServiceMock.Setup(x => x.GetQuestionTypeListByAdmin())
                .ReturnsAsync(new List<QuestionTypeResponseModel>());

            // Act
            var result = await _questionController.GetQuestionTypeListByAdmin();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task GetQuestionById_ReturnsQuestionDetails()
        {
            // Arrange
            var expectedResult = new ApiPostResponse<QuestionResponseModel> { Success = true, Data = new QuestionResponseModel() };
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost:7061");

            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IntcIklkXCI6MixcIlVzZXJJZFwiOjAsXCJSb2xlSWRcIjozLFwiTGFzdE5hbWVcIjpcIllhZGF2XCIsXCJGaXJzdE5hbWVcIjpcIk5pbGVzaFwiLFwiRW1haWxJZFwiOlwibmlsZXNoLnlAc2hhbGlncmFtaW5mb3RlY2guY29tXCIsXCJGdWxsTmFtZVwiOlwiTmlsZXNoWWFkYXZcIixcIlRva2VuVmFsaWRUb1wiOlwiMDAwMS0wMS0wMVQwMDowMDowMFwiLFwiVXNlckltYWdlXCI6bnVsbCxcIklzU3VwZXJBZG1pblwiOnRydWV9IiwibmJmIjoxNzAzMDc0NTM0LCJleHAiOjE3MDMzMzQ5MzQsImlhdCI6MTcwMzA3NDUzNH0.QKu_gC4WLCfqu4h5PTJ4xdjHCnZKOrtaN2oxi8vl3UZjk2tycwB2LZb40w7VJpmev5-Oh7Kih2OGzMcD3NHKFw";
            _questionController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _questionServiceMock.Setup(x => x.GetQuestionById(It.IsAny<long>()))
                .ReturnsAsync(new QuestionResponseModel());

            // Act
            var result = await _questionController.GetQuestionById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.IsNotNull(result.Data);
        }

        [Test]
        public async Task DeleteQuestion_ReturnsSuccess()
        {
            // Arrange
            var expectedResult = new BaseApiResponse { Success = true };

            _questionServiceMock.Setup(x => x.DeleteQuestion(It.IsAny<long>()))
                .ReturnsAsync(Status.Success);

            // Act
            var result = await _questionController.DeleteQuestion(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
        }

        [Test]
        public async Task DeleteQuestion_ReturnsInUse()
        {
            // Arrange
            var expectedResult = new BaseApiResponse { Success = false, Message = ErrorMessages.QuestionInUse };

            _questionServiceMock.Setup(x => x.DeleteQuestion(It.IsAny<long>()))
                .ReturnsAsync(Status.InUse);

            // Act
            var result = await _questionController.DeleteQuestion(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }

        [Test]
        public async Task DeleteQuestion_ReturnsSomethingWentWrong()
        {
            // Arrange
            var expectedResult = new BaseApiResponse { Success = false, Message = ErrorMessages.SomethingWentWrong };

            _questionServiceMock.Setup(x => x.DeleteQuestion(It.IsAny<long>()))
                .ReturnsAsync(Status.Failed);

            // Act
            var result = await _questionController.DeleteQuestion(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Success, result.Success);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }
    }
}
