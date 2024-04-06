using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace TransientFaultHandling.Controllers;

[ApiController]
[Route("[controller]")]
public class MockController : ControllerBase
{

    public enum EndpointState
    {
        Fail,
        Ok,
        Slow,
    }
    private readonly IHttpClientFactory _httpClientFactory;

    public MockController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

    }
    [Route("success")]
    [HttpGet]
    public Task<StatusCodeResult> OnGetSuccess()
    {
        return Task.FromResult(new StatusCodeResult(StatusCodes.Status200OK));
    }

    //   [HttpGet]
    //   public Task<StatusCodeResult> OnGet() {
    //     var rand = (EndpointState)new Random().Next(0,3);
    //     var result = StatusCodes.Status418ImATeapot;
    //     switch (rand) {
    //       case EndpointState.Fail:
    //         result = StatusCodes.Status500InternalServerError;
    //         break;
    //       case EndpointState.Ok:
    //         result = StatusCodes.Status200OK;
    //         break;
    //       case EndpointState.Slow:
    //         result = StatusCodes.Status408RequestTimeout;
    //       break;
    //     }
    //     return Task.FromResult(new StatusCodeResult(result));
    //   }



    [HttpGet]
public async Task<IActionResult> OnGet()
{
    try
    {
        var client = _httpClientFactory.CreateClient("PollyWaitAndRetry"); // Use the named HttpClient with Polly retry policy
        var response = await client.GetAsync("/"); // Targeting the GET route

        if (response.IsSuccessStatusCode)
        {
            return Ok(); // Return Ok if the request is successful
        }
        else
        {
            var rand = (EndpointState)new Random().Next(0, 3);
            var result = StatusCodes.Status418ImATeapot;

            switch (rand)
            {
                case EndpointState.Fail:
                    result = StatusCodes.Status500InternalServerError;
                    break;
                case EndpointState.Slow:
                    result = StatusCodes.Status408RequestTimeout;
                    break;
                case EndpointState.Ok:
                    result = StatusCodes.Status200OK;
                    break;
            }
            return StatusCode(result); // Return appropriate status code based on the random state
        }
    }
    catch (HttpRequestException)
    {
        return StatusCode((int)HttpStatusCode.ServiceUnavailable); // Handle if HttpClient request fails
    }
}

}