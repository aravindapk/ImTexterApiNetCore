﻿using ImTexterApi.Models;
using ImTexterApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImTexterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextAnalyzerController : ControllerBase
    {
        private readonly ITextAnalyzerService _textAnalyzerService;
        private readonly ILogger<TextAnalyzerController> _logger;
        public TextAnalyzerController(ITextAnalyzerService textAnalyzerService, ILogger<TextAnalyzerController> logger)
        { 
            _textAnalyzerService = textAnalyzerService;
            _logger = logger;
        }


        [HttpPost("AnalyzeTextFromHtmlDocument")]
        public async Task<ActionResult> AnalyzeTextAsync(TextAnalyzerRequest textAnalyzerRequest)
        {
            try
            {
                var result = await _textAnalyzerService.ProcessTextAsync(textAnalyzerRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Source, ex.Message);
                return BadRequest(ex.Message);
            }
            
        }
    }
}
