// // Purpose: Contains the UserController class which is responsible for handling the requests to the User endpoint. 
// //The class contains two methods, one for handling a successful request and the other for handling 
// //a request that may fail, be slow, or return a successful response. 
// //The class uses the IHttpClientFactory to create an HttpClient instance with a named Polly retry policy. 
// //The class also contains an enum for the different states of the endpoint and a switch statement to return the appropriate status code based on the random state.

// //<<<<<<<<<<<<<<THIS IS EXERCISE 14.1>>>>>>>>> 

// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http;
// using System;
// using System.Net;

// namespace PollySetup.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class UserController : ControllerBase
// {

//     public enum EndpointState
//     {
//         Fail,
//         Ok,
//         Slow,
//     }
//     private readonly IHttpClientFactory _httpClientFactory;

//     public UserController(IHttpClientFactory httpClientFactory)
//     {
//         _httpClientFactory = httpClientFactory;

//     }

//     [Route("success")]
//     [HttpGet]
//     public Task<StatusCodeResult> OnGetSuccess()
//     {
//         return Task.FromResult(new StatusCodeResult(StatusCodes.Status200OK));
//     }


//     [HttpGet]
//     public async Task<IActionResult> OnGet()
//     {
//         try
//         {
//             var client = _httpClientFactory.CreateClient("PollyWaitAndRetry"); // Use the named HttpClient with Polly retry policy
//             var response = await client.GetAsync("/"); // Targeting the GET route

//             if (response.IsSuccessStatusCode)
//             {
//                 return Ok(); // Return Ok if the request is successful
//             }
//             else
//             {
//                 var rand = (EndpointState)new Random().Next(0, 3);
//                 var result = StatusCodes.Status418ImATeapot;

//                 switch (rand)
//                 {
//                     case EndpointState.Fail:
//                         result = StatusCodes.Status500InternalServerError;
//                         break;
//                     case EndpointState.Slow:
//                         result = StatusCodes.Status408RequestTimeout;
//                         break;
//                     case EndpointState.Ok:
//                         result = StatusCodes.Status200OK;
//                         break;
//                 }
//                 return StatusCode(result); // Return appropriate status code based on the random state
//             }
//         }
//         catch (HttpRequestException)
//         {
//             return StatusCode((int)HttpStatusCode.ServiceUnavailable); // Handle if HttpClient request fails
//         }
//     }

// }
