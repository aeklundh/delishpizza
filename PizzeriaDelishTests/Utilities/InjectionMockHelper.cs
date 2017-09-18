using Microsoft.AspNetCore.Http;
using Moq;
using PizzeriaDelishTests.FakeClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzeriaDelishTests.Utilities
{
    public static class InjectionMockHelper
    {
        public static Mock<IHttpContextAccessor> CreateMockHttpAccessor()
        {
            Mock<IHttpContextAccessor> httpAccessor = new Mock<IHttpContextAccessor>();
            Mock<HttpContext> httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Session).Returns(new TestSession());
            httpAccessor.Setup(x => x.HttpContext).Returns(httpContext.Object);

            return httpAccessor;
        }
    }
}
