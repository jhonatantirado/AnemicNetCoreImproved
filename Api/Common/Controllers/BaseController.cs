using EnterprisePatterns.Api.Common.Application;
using Microsoft.AspNetCore.Mvc;

namespace EnterprisePatterns.Api.Common.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected new IActionResult Ok()
        {
            _unitOfWork.Commit(true);
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            _unitOfWork.Commit(true);
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }
    }
}
