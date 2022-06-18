﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Quizard.Interfaces;


namespace Quizard.Controllers
{
    public class QuizController : Controller
    {
        readonly IStreamFileUploadService _streamFileUploadService;

        public QuizController(IStreamFileUploadService streamFileUploadService)
        {
            _streamFileUploadService = streamFileUploadService;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [ActionName("Upload")]
        [HttpPost]
        public async Task<IActionResult> SaveFileToPhysicalFolder()
        {
            var boundary = HeaderUtilities.RemoveQuotes(
             MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
            ).Value;

            var reader = new MultipartReader(boundary, Request.Body);

            var section = await reader.ReadNextSectionAsync();

            string response = string.Empty;
            try
            {
                if (await _streamFileUploadService.UploadFile(reader, section))
                {
                    ViewBag.Message = "File Upload Successful";
                }
                else
                {
                    ViewBag.Message = "File Upload Failed";
                }
            }
            catch (Exception ex)
            {
                //Log ex
                ViewBag.Message = "File Upload Failed";
            }
            return View();
        }
    }
}
